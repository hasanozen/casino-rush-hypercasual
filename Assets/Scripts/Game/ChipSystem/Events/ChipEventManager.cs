using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.ChipSystem.Events
{
    public class ChipEventManager
    {
        private List<ChipEvent> _chipEvents;

        public ChipEventManager()
        {
            _chipEvents = new List<ChipEvent>
            {
                new ChipEvent(ChipEventType.ON_VALUE_CHANGE),
                new ChipEvent(ChipEventType.ON_STACKED),
                new ChipEvent(ChipEventType.ON_DESTACKED)
            };
        }

        public void SubscribeEvent(ChipEventType chipEventType, Action action)
        {
            var specifiedEvent = _chipEvents?.FirstOrDefault(
                x => x.GetEventType() == chipEventType);

            if (specifiedEvent != null) specifiedEvent.SubscribeToEvent(action);
        }
        
        public void InvokeEvent(ChipEventType chipEventType)
        {
            var specifiedEvent = _chipEvents?.FirstOrDefault(
                x => x.GetEventType() == chipEventType);

            if (specifiedEvent != null) specifiedEvent.InvokeEvent();
        }
    }
}
