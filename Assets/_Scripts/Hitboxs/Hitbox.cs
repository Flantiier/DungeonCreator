using UnityEngine;
using _Scripts.Characters;

public class Hitbox : MonoBehaviour
{
    #region Variables
    protected Character _character;
    public Collider Collider { get; private set; }
    #endregion

    #region Builts_In
    public virtual void Awake()
    {
        _character = GetComponentInParent<Character>();
        Collider = GetComponent<Collider>();
        SetColliderState(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!_character.PViewIsMine)
            return;

        TriggerEnter(other);   
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!_character.PViewIsMine)
            return;

        ColliderEnter(collision.collider);
    }
    #endregion

    #region Inherited methods
    /// <summary>
    /// Set the collider state
    /// </summary>
    public void SetColliderState(bool state)
    {
        Collider.enabled = state;
    }

    /// <summary>
    /// Method called during OnCollisionEnter
    /// </summary>
    protected virtual void ColliderEnter(Collider other) { }
    /// <summary>
    /// Method called during OnTriggerEnter
    /// </summary>
    protected virtual void TriggerEnter(Collider other) { }
    #endregion
}
