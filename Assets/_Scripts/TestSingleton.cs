using _Scripts.Managers;
using UnityEngine;

public class TestSingleton : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.HelloWorld();
    }
}
