using UnityEngine;

public class TestingAbilities : MonoBehaviour
{
    public Animator animator;
    public AnimatorOverrider animOverrider;

    public AnimationClip[] abilities;

    public void SetMagicAttacks()
    {
        animOverrider.GetOverrideClips();
        animOverrider.OverrideAnimations(abilities[0], abilities[1]);
    }
    public void SetMeleeAttack()
    {
        animOverrider.GetOverrideClips();
        animOverrider.OverrideAnimations(abilities[2], abilities[3]);
    }

    public void SetSwordAttacks()
    {
        animOverrider.GetOverrideClips();
        animOverrider.OverrideAnimations(abilities[4], abilities[5]);
    }

    public void TriggerFA()
    {
        animator.SetTrigger("FA");
    }

    public void TriggerSA()
    {
        animator.SetTrigger("SA");
    }
}
