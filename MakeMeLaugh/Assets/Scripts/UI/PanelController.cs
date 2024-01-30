using System.Collections;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public float duration = 0.50f; // duration of the animation
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private float originHeight = -50;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        yield return StartCoroutine(AnimatePanel(true));
        yield return StartCoroutine(Shake()); // add the shake at the end
    }

    public void Hide()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        StartCoroutine(AnimatePanel(false));
    }

    IEnumerator AnimatePanel(bool showing)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            // compute the time ratio elapsed/total
            float t = (Time.time - startTime) / duration;

            // move the panel up or down
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, Mathf.Lerp(showing ? -rectTransform.sizeDelta.y : originHeight, showing ? originHeight : -rectTransform.sizeDelta.y, t));

            // fade the panel in or out
            canvasGroup.alpha = showing ? t : 1 - t;

            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, showing ? originHeight : -rectTransform.sizeDelta.y);
        canvasGroup.alpha = showing ? 1 : 0;
    }

    IEnumerator Shake()
    {
        Vector3 originalPos = rectTransform.anchoredPosition;

        float elapsed = 0.0f;
        float duration = 0.1f; // duration of the shake
        float intensity = 5.0f; // intensity of the shake

        while (elapsed < duration)
        {
            float x = originalPos.x;
            float y = originalPos.y + Random.Range(-1f, 1f) * intensity;

            rectTransform.anchoredPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
    }
}