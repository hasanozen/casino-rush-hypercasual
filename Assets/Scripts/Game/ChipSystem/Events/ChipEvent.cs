using System;
using UnityEngine;
using Utils;

namespace Game.ChipSystem.Events
{
    public enum ChipEventType
    {
        ON_VALUE_CHANGE,
        ON_STACKED,
        ON_DESTACKED
    }
    
    public class ChipEvent
    {
        private ChipEventType _chipEventType;
        private Action _onChipEvent;

        public ChipEvent(ChipEventType chipEventType)
        {
            _chipEventType = chipEventType;
        }

        public void SubscribeToEvent(Action action)
        {
            _onChipEvent += action;
        }

        public void InvokeEvent()
        {
            _onChipEvent.SafeInvoke();
        }

        public ChipEventType GetEventType()
        {
            return _chipEventType;
        }
    }
}
