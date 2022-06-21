using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimatorOverrider
{
    public Animator _animator;
    public AnimatorOverrideController _overrideController;
    public int firstAbilityOverrideIndex;
    public int secondAbilityOverrideIndex;

    private List<KeyValuePair<AnimationClip, AnimationClip>> _overrides;

    public void GetOverrideClips()
    {
        _overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(_overrideController.overridesCount);
        _overrideController.GetOverrides(_overrides);
    }

    public void OverrideAnimations(AnimationClip firstAbility,AnimationClip secondAbility)
    {
        OverrideClip(firstAbilityOverrideIndex, firstAbility);
        OverrideClip(secondAbilityOverrideIndex, secondAbility);

        _overrideController.ApplyOverrides(_overrides);

        for (int i = 0; i < _overrides.Count; i++)
        {
            Debug.Log(_overrides[i].Key + " / " + _overrides[i].Value);
        }

        _animator.runtimeAnimatorController = _overrideController;
    }

    private void OverrideClip(int index, AnimationClip newClip)
    {
        if (index >= _overrides.Count || index < 0)
            return;

        _overrides[index] = SetNewAimation(_overrides[index].Key, newClip);
    }

    private KeyValuePair<AnimationClip, AnimationClip> SetNewAimation(AnimationClip keyValue, AnimationClip clipValue)
    {
        return new KeyValuePair<AnimationClip, AnimationClip>(keyValue, clipValue);
    }
}
