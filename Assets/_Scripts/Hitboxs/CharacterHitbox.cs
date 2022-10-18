using UnityEngine;

public class CharacterHitbox : Hitbox
{
    protected override void TriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamageable player))
            return;

        player.Damage(_character.CharacterDatas.GetAttackDamages());
    }
}
