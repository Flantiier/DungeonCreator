using UnityEngine;
using System.Collections;
using _Scripts.Interfaces;

public class TestTrapInvisible : MonoBehaviour, IDetectable
{
    [SerializeField] private GameObject canvasFeedback;
    [SerializeField] private float feedbackDuration = 4f;

    public void GetDetected(float duration)
    {
        if (!canvasFeedback)
            return;

        StartCoroutine("DetectFeedback");
    }

    private IEnumerator DetectFeedback()
    {
        canvasFeedback.SetActive(true);

        yield return new WaitForSecondsRealtime(feedbackDuration);

        canvasFeedback.SetActive(false);
    }
}
