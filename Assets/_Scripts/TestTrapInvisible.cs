using UnityEngine;
using _Scripts.Interfaces;

public class TestTrapInvisible : MonoBehaviour, IDetectable
{
    public void GetDetected()
    {
        Debug.Log($"Detected Trap : {name}");
    }
}
