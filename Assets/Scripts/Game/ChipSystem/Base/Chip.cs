using UnityEngine;

namespace Game.ChipSystem.Base
{
    public class Chip : ChipBase
    {
        public override void Init()
        {
            base.Init();
            MeshRenderer = transform.GetComponent<MeshRenderer>();
        }
    }
}
