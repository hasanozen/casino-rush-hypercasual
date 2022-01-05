using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.CharacterSystem.Events
{
    public class CharacterEventManager
    {
        private List<CharacterEvent> _characterEvents;

        public CharacterEventManager()
        {
            _characterEvents = new List<CharacterEvent>
            {
                new CharacterEvent(CharacterEventType.ON_DEATH),
                new CharacterEvent(CharacterEventType.ON_START),
                new CharacterEvent(CharacterEventType.ON_FINISH),
                new CharacterEvent(CharacterEventType.ON_RESTART),
                new CharacterEvent(CharacterEventType.ON_END_GAME)
            };
        }

        public void SubscribeEvent(CharacterEventType characterEventType, Action action)
        {
            var specifiedEvent = _characterEvents?.FirstOrDefault(
                x => x.GetEventType() == characterEventType);

            if (specifiedEvent != null) specifiedEvent.SubscribeToEvent(action);
        }

        public void InvokeEvent(CharacterEventType characterEventType)
        {
            var specifiedEvent = _characterEvents?.FirstOrDefault(
                x => x.GetEventType() == characterEventType);
            
            if (specifiedEvent != null) specifiedEvent.InvokeEvent();
        }
    }
}