using System.Collections;
using UnityEngine;
using _Scripts.Interfaces;

public class TestTrapInvisible : MonoBehaviour
{
    [SerializeField] private GameObject canvasFeedback;
    [SerializeField] private float feedbackDuration = 4f;

    public void GotDetected()
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
