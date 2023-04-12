using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.GameplayFeatures
{
    public class WeaponRagdoll : DestructibleRagdollPart
    {
        [TitleGroup("Weapon")]
        [SerializeField] private Transform weaponReference;

        public override void SetRagdollToBone()
        {
            transform.position = weaponReference.position;
            transform.rotation = weaponReference.rotation;
        }
    }
}
