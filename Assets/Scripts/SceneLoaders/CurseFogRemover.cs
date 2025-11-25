using UnityEngine;
using System.Collections;
public class CurseFogRemover : MonoBehaviour
{
    public float fadeSpeed = 1f;          // Higher = faster fade
    public float targetAlphaOnExit = 0f;  // Fully opaque

    private SpriteRenderer sr;
    private Coroutine fadeRoutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

   /* private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        StartFade(targetAlphaOnEnter);
        StartFade();
    }*/

   /* private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        StartFade(targetAlphaOnExit);
    }*/

    public void StartFade()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(targetAlphaOnExit));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        yield return new WaitForSeconds(1.5f);
        Color c = sr.color;

        while (!Mathf.Approximately(c.a, targetAlpha))
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
            sr.color = c;
            yield return null;
        }
    }
}
