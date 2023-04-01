using UnityEngine;

namespace _Scripts.GameplayFeatures
{
    public class WeaponRagdoll : DestructibleRagdollPart
    {
        [SerializeField] private Transform weaponReference;

        public override void SetRagdollToBone()
        {
            transform.position = weaponReference.position;
            transform.rotation = weaponReference.rotation;
        }
    }
}
