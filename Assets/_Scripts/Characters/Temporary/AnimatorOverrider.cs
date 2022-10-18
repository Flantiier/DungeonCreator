using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimatorOverrider
{
    #region Overrider Variables
    /// <summary>
    /// Animator to override animations
    /// </summary>
    public Animator _animator;
    /// <summary>
    /// OverrideController
    /// </summary>
    public AnimatorOverrideController _overrideController;
    /// <summary>
    /// First Ability animation index
    /// </summary>
    public int firstAbilityOverrideIndex;
    /// <summary>
    /// Second Ability animation index
    /// </summary>
    public int secondAbilityOverrideIndex;
    /// <summary>
    /// Show animation List or not
    /// </summary>
    public bool showAnimationsList;

    /// <summary>
    /// List of animations
    /// </summary>
    private List<KeyValuePair<AnimationClip, AnimationClip>> _overrides;
    #endregion

    #region Overrider Methods
    /// <summary>
    /// Get overrideController clips
    /// </summary>
    public void GetOverrideClips()
    {
        //Create a new instance of the overrideController
        _overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        //Create a new List of animations
        _overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(_overrideController.overridesCount);
        //Copying animation clips list in the local list
        _overrideController.GetOverrides(_overrides);
    }

    /// <summary>
    /// Overriding animations
    /// </summary>
    /// <param name="firstAbility">Animation clip of the first Ability</param>
    /// <param name="secondAbility">Animation clip of the second Ability</param>
    public void OverrideAnimations(AnimationClip firstAbility,AnimationClip secondAbility)
    {
        //Override the first clip
        OverrideClip(firstAbilityOverrideIndex, firstAbility);
        //Override the sconde clip
        OverrideClip(secondAbilityOverrideIndex, secondAbility);

        //Applying overridedList on the overrideController
        _overrideController.ApplyOverrides(_overrides);

        if (showAnimationsList)
            ShowAnimationsList();

        //Setting the runtimeAnimator
        _animator.runtimeAnimatorController = _overrideController;
    }

    /// <summary>
    /// Override a clip at a certain index
    /// </summary>
    /// <param name="index">Animation clip index</param>
    /// <param name="newClip">New animation clip</param>
    private void OverrideClip(int index, AnimationClip newClip)
    {
        //Check if the index is out of the bounds
        if (index >= _overrides.Count || index < 0)
            return;

        //Set the new Animation clip
        _overrides[index] = SetNewAimation(_overrides[index].Key, newClip);
    }

    /// <summary>
    /// Create a new KeyValuePair or 2 Animations clips
    /// </summary>
    /// <param name="keyValue">OverrideController clip</param>
    /// <param name="clipValue">Overriding animation clip</param>
    private KeyValuePair<AnimationClip, AnimationClip> SetNewAimation(AnimationClip keyValue, AnimationClip clipValue)
    {
        return new KeyValuePair<AnimationClip, AnimationClip>(keyValue, clipValue);
    }

    public void ShowAnimationsList()
    {
        for (int i = 0; i < _overrides.Count; i++)
        {
            Debug.Log(_overrides[i].Key + " / " + _overrides[i].Value);
        }
    }
    #endregion
}
