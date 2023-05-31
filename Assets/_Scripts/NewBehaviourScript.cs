using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("SubScene_Spawn", LoadSceneMode.Additive);
    }
}
