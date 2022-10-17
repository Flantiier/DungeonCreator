using UnityEngine;
public class TrapHitbox : MonoBehaviour
{
    [SerializeField] private TrapClass trapInfo;

    public void OnTriggerEnter(Collider other)
    {
        StartCoroutine(trapInfo.StartCooldown());
    }
}
