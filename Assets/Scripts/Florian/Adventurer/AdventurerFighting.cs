using UnityEngine;
using Adventurer;
using System;

public class AdventurerFighting : MonoBehaviour
{
    //References
    private AdventurerController _adventurer;
    private Animator _animator;

    public event Action onAttacking;

    [Header("Animator Parameters")]
    [SerializeField, Tooltip("Nmae of the Attack Parameter in the Animator")]
    private string ATTACK_PARAM = "Attack";

    private float _attackCooldown;
    private float _dodgeCooldown;

    private void Awake()
    {
        InitMethod();
    }

    private void OnEnable()
    {
        onAttacking += Attack;
    }

    private void OnDisable()
    {
        onAttacking -= Attack;
    }

    private void InitMethod()
    {
        if (transform.GetChild(0).TryGetComponent(out Animator animator))
            _animator = animator;

        _adventurer = GetComponent<AdventurerController>();
    }

    public void Attack()
    {
        if (!_adventurer.CanAttack)
            return;

        //Blocking controls
        _adventurer.CanAttack = false;
        _adventurer.CanMove = false;
        _adventurer.CanDodge = false;

        //Set Animation
        _animator.SetTrigger(ATTACK_PARAM);
    }
}
