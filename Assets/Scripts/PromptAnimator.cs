using System.Collections;
using UnityEngine;

public class PromptAnimator : MonoBehaviour
{
    [SerializeField] private float speed = 0.8f;
    [SerializeField] private float floatAmplitude = 0.1f;
    [SerializeField] private float floatFrequency = 2f;

    private Vector3 startPosition;
    private float floatTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private bool showing = false;
    private bool isAnimatingIn = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.localPosition;
        SetAlpha(0f);
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        showing = true;
        floatTimer = 0f;
        StopAllCoroutines();
        StartCoroutine(AnimateIn());
    }

    public void Hide()
    {
        showing = false;
        StopAllCoroutines();
        StartCoroutine(AnimateOut());
    }

    void Update()
    {
        // On flotte seulement si on est en affichage et pas en cours d'animation d'entrée
        if (showing && !isAnimatingIn)
        {
            floatTimer += Time.deltaTime * floatFrequency;
            float offsetY = Mathf.Sin(floatTimer) * floatAmplitude;
            transform.localPosition = startPosition + new Vector3(0, offsetY, 0);
        }
    }

    private IEnumerator AnimateIn()
    {
        isAnimatingIn = true;
        floatTimer = 0f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            float eased = EaseOut(t);
            SetAlpha(Mathf.Lerp(0f, 1f, eased));
            yield return null;
        }

        SetAlpha(1f);
        isAnimatingIn = false;
    }


    private IEnumerator AnimateOut()
    {
        float t = 0f;
        float currentAlpha = spriteRenderer.color.a;

        // On désactive le flottement immédiatement
        isAnimatingIn = true;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            float eased = EaseIn(t);
            SetAlpha(Mathf.Lerp(currentAlpha, 0, eased));
            yield return null;
        }

        SetAlpha(0f);
        transform.localPosition = startPosition;
        gameObject.SetActive(false);
        isAnimatingIn = false;
    }

    private void SetAlpha(float a)
    {
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = a;
            spriteRenderer.color = c;
        }
    }

    private float EaseOut(float t) => 1 - Mathf.Pow(1 - t, 3);
    private float EaseIn(float t) => t * t * t;
}
