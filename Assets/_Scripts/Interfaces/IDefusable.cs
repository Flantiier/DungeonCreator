using UnityEngine;

namespace _Scripts.Interfaces
{

    public interface IDefusable
    {
        public float DefuseDuration { get; }
        public bool IsDisabled { get; set; }

        public void IsDefused();
    }
}