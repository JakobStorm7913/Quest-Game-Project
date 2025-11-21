using UnityEngine;
using System.Collections;

public class SpriteFadeOnTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    public float fadeSpeed = 5f;          // Higher = faster fade
    public float targetAlphaOnEnter = 0f; // Fully transparent
    public float targetAlphaOnExit = 1f;  // Fully opaque

    private SpriteRenderer sr;
    private Coroutine fadeRoutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        StartFade(targetAlphaOnEnter);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        StartFade(targetAlphaOnExit);
    }

    void StartFade(float targetAlpha)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(targetAlpha));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        Color c = sr.color;

        while (!Mathf.Approximately(c.a, targetAlpha))
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
            sr.color = c;
            yield return null;
        }
    }
}
