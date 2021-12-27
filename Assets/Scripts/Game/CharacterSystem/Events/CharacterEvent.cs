using System;
using Utils;

namespace Game.CharacterSystem.Events
{
    public enum CharacterEventType
    {
        ON_START,
        ON_FINISH,
        ON_DEATH,
        ON_RESTART
    }
    
    public class CharacterEvent
    {
        private CharacterEventType _characterEventType;
        private Action _onCharacterEvent;

        public CharacterEvent(CharacterEventType characterEventType)
        {
            _characterEventType = characterEventType;
        }

        public void SubscribeToEvent(Action action)
        {
            _onCharacterEvent += action;
        }

        public void InvokeEvent()
        {
            _onCharacterEvent.SafeInvoke();
        }

        public CharacterEventType GetEventType()
        {
            return _characterEventType;
        }
    }
}
