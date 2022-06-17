using UnityEngine;
using Adventurer;

public class AdventurerFighting : MonoBehaviour
{
    //References
    private AdventurerController _adventurer;

    [SerializeField] private float maxHoldingTime = 3f;
    public float HoldAttack { get; private set; }
    private bool _isAttacking;

    private void Awake()
    {
        _adventurer = GetComponent<AdventurerController>();
    }

    private void Update()
    {
        AttackHolding();
    }

    private void AttackHolding()
    {
        if (!_adventurer.CanAttack || !_adventurer.IsGrounded)
            return;

        _isAttacking = _adventurer.Input.isAttacking;

        if (_isAttacking && HoldAttack < maxHoldingTime)
            HoldAttack += Time.deltaTime;
        else
            HoldAttack = 0f;
    }
}
