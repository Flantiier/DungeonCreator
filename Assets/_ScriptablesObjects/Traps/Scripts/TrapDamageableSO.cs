using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.TrapSystem.Datas
{
    [CreateAssetMenu(fileName = "New Trap Damageable", menuName = "Scriptables/TrapDamageable")]
    public class TrapDamageableSO : TrapSO
    {
        [Header("Trap damageable infos")]
        public float health;
    }
}