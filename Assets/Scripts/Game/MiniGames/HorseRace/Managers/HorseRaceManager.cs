using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.BetSystem.Base;
using Game.LevelSystem.Base;
using Game.MiniGames.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.MiniGames.HorseRace.Managers
{
    public class HorseRaceManager : LevelGame
    {
        public enum RaceState
        {
            RUN,
            PHOTO_FINISH,
            AFTER_PHOTO_FINISH,
            COMPLETE
        }

        public override LevelGameType levelGameType => LevelGameType.HORSE_RACE;
    
        public delegate void OnWinnerChanged(int index);
        public delegate void OnRaceStateChanged(RaceState state);
        public event OnRaceStateChanged OnRaceStateChangedEvent;
        public event OnWinnerChanged OnWinnerChangedEvent;
    
        [SerializeField] private Bidder[] horses;
    
        public int CurrentWinnerHorse { get; private set; }
    
        private RaceState state;
        public RaceState State
        {
            get { return state; }
            private set
            {
                bool isChanged = state != value;
                state = value;
                if (OnRaceStateChangedEvent != null && isChanged)
                    OnRaceStateChangedEvent(state);
            }
        }
    
        [SerializeField] private Transform startPointTransform, endPointTransform, outTrackTransform;

        private List<GameObject> horseObjs = new List<GameObject>();
        private int horseCount, winnerHorseId;
        private Sequence horseMovement;
        private float lastTimeAfterUpdate, raceStartTime;
        private List<AnimationCurve> curves;
        [SerializeField] private float raceTime, totalRaceTime, photoFinishSecond;

        private float test;

        private void Awake()
        {
            horseMovement = DOTween.Sequence();
            horseCount = horses.Length;

            foreach ( Bidder horse in horses)
                horseObjs.Add(horse.gameObject);
        }

        public override void Init(int winnerID)
        {
            Debug.Log("Initializing Horse Race");
            
            this.winnerHorseId = winnerID;
            //SetHorseBetInfos();
            LoadRace(CalculatePaths(3), this.winnerHorseId, raceTime);
            //OpenDoors();
            StartRace();
            //GetWinnerBets();
        }

        private void OpenDoors()
        {
            Transform door = transform.Find("Doors");
            door.DOLocalMoveY(-1, .3f);
        }

        public void GetWinnerBets()
        {
            // BetInfo[] betInfos = FindObjectsOfType<BetInfo>();
            // BetInfo betInfo = betInfos.FirstOrDefault(x => x.id == winnerHorseId);
            //
            // Bidder bidder = Dealer.Instance.bidders.FirstOrDefault(x => x.id == betInfo.id);
            //
            // MenuManager.Instance.SetHorseScreenValues(betInfo.color, (int)(bidder.odd * bidder.givenBid));
        }

        private void SetHorseBetInfos()
        {
            // BetInfo[] betInfos = FindObjectsOfType<BetInfo>();
            //
            // foreach (var bidder in Dealer.Instance.bidders)
            // {
            //     BetInfo info = betInfos.FirstOrDefault(x => x.id == bidder.id);
            //
            //     if (!(info is null))
            //     {
            //         info.SetValues(bidder.odd, bidder.givenBid);
            //         info.Apply();
            //     }
            //     else 
            //         Debug.Log("There is no info with id " + bidder.id);
            // }
            //
            // MenuManager.Instance.HorseBetMenuIn();
        }

        private List<float[]> CalculatePaths(int sequence)
        {
            List<float[]> paths = new List<float[]>();
            float gap = 1f / sequence;
        
            Debug.Log("Horse Count: " + horseCount);
            
            for (int i = 0; i < horseCount; i++)
            {
                List<float> values = new List<float>();
                float remaining;
                for (int j = 0; j < sequence; j++)
                {
                    float value = Random.Range(gap * .75f, gap);
                    remaining = gap - value;

                    if (values.Count == 0)
                        values.Add(value);
                    else if (values.Count > 0)
                        values.Add(values[j - 1] + value );

                    if (j == sequence - 1)
                    {
                        values.RemoveAt(j);
                        values.Add(i == winnerHorseId ? 1f : Random.Range(.85f, .95f));
                    }

                }
                paths.Add(values.ToArray());
            }

            for (int i = 0; i < paths.Count; i++)
            {
                string pathString = "Horse " + (i + 1) + " Path = ";
                for (int j = 0; j < paths[i].Length; j++)
                {
                    pathString += paths[i][j] + " | ";
                }
            
                Debug.Log(pathString);
            }
        
            return paths;
        }

        private void Update()
        {
            if (curves == null)
                return;
        
            List<float> valuesAtCurrentTime = new List<float>();
            float raceCurrentTimeNormalized = (Time.realtimeSinceStartup - raceStartTime) / totalRaceTime;
            curves.ForEach(x => valuesAtCurrentTime.Add(x.Evaluate(raceCurrentTimeNormalized)));
        
            if (State == RaceState.RUN && lastTimeAfterUpdate < Time.realtimeSinceStartup)
            {
                lastTimeAfterUpdate = Time.realtimeSinceStartup;
                int lastWinnerHorse = CurrentWinnerHorse;
                CurrentWinnerHorse = valuesAtCurrentTime.IndexOf(valuesAtCurrentTime.Max());
                ChangeWinnerHorse();
            }
        }
    
        void ChangeWinnerHorse()
        {
            // var betInfos = MenuManager.Instance.elements[11].transform.GetComponentsInChildren<BetInfo>();
            // var winnerHorseMenu = betInfos?.FirstOrDefault(x => x.id == CurrentWinnerHorse);
            // var winnerHorseMenuObject = winnerHorseMenu.transform.Find("WinnerHorseBG");
            //
            // foreach (BetInfo betInfo in betInfos)
            // {
            //     betInfo.transform.Find("WinnerHorseBG").gameObject.SetActive(false);
            // }
            //
            // winnerHorseMenuObject.gameObject.SetActive(true);
        
            if (OnWinnerChangedEvent != null)
                OnWinnerChangedEvent(CurrentWinnerHorse);
        }

        public void LoadRace(List<float[]> runPaths, int winnerHorseId, float raceTimeSeconds)
        {
            SetCurves(runPaths);
            totalRaceTime = raceTimeSeconds;
            this.winnerHorseId = winnerHorseId;
            CurrentWinnerHorse = 0;

            state = RaceState.COMPLETE;
        }

        public void StartRace()
        {
            if (State == RaceState.RUN)
            {
                Debug.LogError("Race already started.");
                return;
            }

            KillSequences();
            State = RaceState.RUN;
            raceStartTime = Time.realtimeSinceStartup - (totalRaceTime - raceTime);
            // Run 
            StartCoroutine(CompleteHorseRun(raceTime));

            for (int i = 0; i < horseObjs.Count; i++)
            {
                horseMovement.Join(
                    horseObjs[i].transform.DOMoveZ(endPointTransform.position.z, raceTime)
                        .SetEase(curves[i]));
            }
        }
    
        IEnumerator CompleteHorseRun(float delay)
        {
            if (delay > 0)
                yield return new WaitForSecondsRealtime(delay);
            KillSequences();
            CurrentWinnerHorse = winnerHorseId;
            ChangeWinnerHorse();
            State = RaceState.COMPLETE;
        
            Debug.Log("Winner ID: " + CurrentWinnerHorse);
        
            StartCoroutine("AfterPhotoFinish");
        
            // End current level
            // LevelController.Instance.EndLevel();
            // AudioManager.Instance.Stop("HorseRace");
        }
    
        IEnumerator AfterPhotoFinish()
        {
            float runDuration = .5f;
            yield return new WaitForSeconds(photoFinishSecond);
            State = RaceState.AFTER_PHOTO_FINISH;
            foreach (var horseObj in horseObjs)
            {
                horseMovement.Join(horseObj.transform.DOMoveZ(outTrackTransform.position.z, runDuration)
                    .SetEase(Ease.Linear));
            }
            yield return new WaitForSecondsRealtime(runDuration);
            State = RaceState.COMPLETE;
        }
    
        public void KillSequences()
        {
            horseMovement.Kill();
            horseMovement = DOTween.Sequence();
            foreach (var horseObj in horseObjs)
                horseObj.transform.DOKill();
        }
    
        void SetCurves(List<float[]> paths)
        {
            Debug.Log("Paths is null: " + paths.Count);
            
            List<float> listOfMaxValues = new List<float>();
            paths.ForEach(x => listOfMaxValues.Add(x.Max()));
            float maxVal = listOfMaxValues.Max();
            curves = new List<AnimationCurve>();
            foreach (var path in paths)
            {
                AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0, 0);
                for (int j = 0; j < path.Length; j++)
                {
                    float normalizedValue = path[j] / maxVal;
                    curve.AddKey((j + 1f) / path.Length, normalizedValue);
                }

                curves.Add(curve);
            }
        }
    }
}
