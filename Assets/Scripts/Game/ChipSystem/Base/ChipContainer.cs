using System.Collections.Generic;
using System.Linq;
using Config;
using Game.ChipSystem.Events;
using Game.PoolingSystem;
using UnityEngine;
using Zenject;

namespace Game.ChipSystem.Base
{
    public class ChipContainer : MonoBehaviour
    {
        private ObjectPooler _objectPooler;

        #region Positioning Variables

        private int _row = 3;
        private int _column = 3;

        private int _currentRow = 0;
        private int _currentColumn = 0;

        private Vector3 _startPos;
        private Vector3 _currentPos;

        private float _increaseAmount;

        #endregion


        private bool _active;
        private List<GameObject> _chips;

        private int zAxisMax;
        private Vector3 miniChipPos;

        public void Init(ObjectPooler objectPooler)
        {
            _objectPooler = objectPooler;
            _chips = new List<GameObject>();

            zAxisMax = GameConfig.CHIP_STACK_MAX_Z;
            
            SetPositioningVariables();
        }

        private void SetPositioningVariables()
        {
            _startPos = _currentPos = new Vector3(
                -(transform.localScale.x / 2 - 0.17f),
                transform.position.y + .08f,
                (transform.localScale.z / _column) - transform.localScale.z);

            _increaseAmount = transform.localScale.x / _row;
        }

        public void DisconnectFromStack(int amount)
        {
            for (int i = _chips.Count; i > _chips.Count - amount; i--)
            {
                UnsubscribeMember(_chips[i]);
            }
        }

        public void SubscribeMember(GameObject chip)
        {
            _chips.Add(chip);
            ReplaceChip();
        }

        public void SubscribeMembers(List<GameObject> chips)
        {
            _chips.AddRange(chips);
            ReplaceChips(_chips.Count);
        }

        public void UnsubscribeMember(GameObject chip)
        {
            chip.GetComponent<ChipMini>().GetEventManager().InvokeEvent(ChipEventType.ON_DESTACKED);
            _chips.Remove(chip);
        }

        public void ActivateChips(int amount)
        {
            int index = _chips.IndexOf(_chips.FirstOrDefault(x => x.GetComponent<ChipBase>().Active == false));

            for (int i = index; i < index + amount; i++)
            {
                _chips[i].GetComponent<ChipBase>().ActivateChip();
                _chips[i].GetComponent<ChipBase>().GetEventManager().InvokeEvent(ChipEventType.ON_STACKED);
                
            }
        }

        public void DeactivateChips(int amount)
        {
            int index = _chips.IndexOf(_chips.FirstOrDefault(x => x.GetComponent<ChipBase>().Active == false)) - 1;

            for (int i = index; i > index - amount; i--)
            {
                _chips[i].GetComponent<ChipBase>().GetEventManager().InvokeEvent(ChipEventType.ON_DESTACKED);
                
            }
        }

        public void ReplaceChip()
        {
            var tmpObject = _objectPooler.SpawnFromPool(NameFields.DEFAULT_MINI_CHIP_POOL_TAG, _currentPos);
            tmpObject.GetComponent<ChipMini>().stackLocation = _currentPos;
            _currentPos = new Vector3(_currentPos.x + _increaseAmount, _currentPos.y, _currentPos.z);
            _currentRow++;

            if (_currentRow == _row)
            {
                _currentRow = 0;
                _currentPos = new Vector3(_startPos.x, _currentPos.y, _currentPos.z - _increaseAmount);
                _currentColumn++;
            }

            if (_currentColumn == _column)
            {
                _currentColumn = 0;
                _currentPos = new Vector3(_startPos.x, _currentPos.y + .05f, _startPos.z);
            }
        }

        public void ReplaceChips(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var tmpObject = _objectPooler.SpawnFromPool(NameFields.DEFAULT_MINI_CHIP_POOL_TAG, _currentPos);
                tmpObject.GetComponent<ChipMini>().stackLocation = _currentPos;
                _currentPos = new Vector3(_currentPos.x + _increaseAmount, _currentPos.y, _currentPos.z);
                _currentRow++;

                if (_currentRow == _row)
                {
                    _currentRow = 0;
                    _currentPos = new Vector3(_startPos.x, _currentPos.y, _currentPos.z - _increaseAmount);
                    _currentColumn++;
                }

                if (_currentColumn == 3)
                {
                    _currentColumn = 0;
                    _currentPos = new Vector3(_startPos.x, _currentPos.y + .05f, _startPos.z);
                }
            }
        }

        public void ActivateContainer()
        {
            _active = true;
        }

        public void DeactivateContainer()
        {
            _active = false;
        }

        public int GetRow()
        {
            return _row;
        }

        public void SetRow(int row)
        {
            _row = row;
        }
    }
}