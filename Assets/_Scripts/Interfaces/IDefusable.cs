using UnityEngine;
using System.Collections;

namespace _Scripts.Interfaces
{
    public interface IDefusable
    {
        public float DefuseDuration { get; set; }
        public bool IsDisabled { get; set; }

        public void DefuseTrap();
    }
}