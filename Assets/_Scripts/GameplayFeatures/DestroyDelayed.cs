using UnityEngine;

public class DestroyDelayed : MonoBehaviour
{
    [SerializeField] private float delay = 1f;

    private void Start()
    {
        Destroy(gameObject, delay);
    }

}