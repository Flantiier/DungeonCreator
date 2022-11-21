using System.Collections;
using UnityEngine;

public class testCoroutine : MonoBehaviour
{
    Coroutine _lastCoroutine;
    private void Update()
    {
        if (_lastCoroutine != null)
            return;

        Debug.Log("Null");
    }

    [ContextMenu("Start")]
    private void StartCoroutine()
    {
        if(_lastCoroutine != null)
            StopCoroutine(_lastCoroutine);

        _lastCoroutine = StartCoroutine(MyCoroutine());
    }

    private IEnumerator MyCoroutine()
    {
        Debug.Log("start");

        yield return new WaitForSecondsRealtime(3f);

        Debug.Log("end");
        _lastCoroutine = null;
    }
}
