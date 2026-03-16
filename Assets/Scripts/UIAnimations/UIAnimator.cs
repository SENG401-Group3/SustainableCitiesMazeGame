using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class UIAnimator : MonoBehaviour
{
    public static UIAnimator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    // Animate a panel to fade in with scale (entrance animation)
    public void AnimateEntrance(VisualElement element, float duration = 0.5f)
    {
        if (element == null) return;
        element.style.opacity = 0;
        element.style.scale = new StyleScale(new Scale(Vector2.one * 0.8f));
        StartCoroutine(EntranceCoroutine(element, duration));
    }

    private IEnumerator EntranceCoroutine(VisualElement element, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float scaleT = Mathf.Sin(t * Mathf.PI * 0.5f); // ease out
            element.style.opacity = t;
            element.style.scale = new StyleScale(new Scale(Vector2.one * (0.8f + 0.2f * scaleT)));
            yield return null;
        }
        element.style.opacity = 1;
        element.style.scale = new StyleScale(new Scale(Vector2.one));
    }

    // Pulse a VisualElement's scale
    public void PulseElement(VisualElement element, float duration = 1f, float frequency = 8f, float amplitude = 0.05f)
    {
        if (element == null) return;
        StartCoroutine(PulseCoroutine(element, duration, frequency, amplitude));
    }

    private IEnumerator PulseCoroutine(VisualElement element, float duration, float frequency, float amplitude)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float pulse = 1 + Mathf.Sin(elapsed * frequency) * amplitude;
            element.style.scale = new StyleScale(new Scale(new Vector2(pulse, pulse)));
            yield return null;
        }
        element.style.scale = new StyleScale(new Scale(Vector2.one));
    }

    // Fade in a spinner or element
    public void FadeInElement(VisualElement element, float duration = 0.2f)
    {
        if (element == null) return;
        element.style.opacity = 0;
        element.style.display = DisplayStyle.Flex;
        StartCoroutine(FadeCoroutine(element, duration, fadeIn: true));
    }

    // Fade out a spinner or element
    public void FadeOutElement(VisualElement element, float duration = 0.2f)
    {
        if (element == null) return;
        StartCoroutine(FadeCoroutine(element, duration, fadeIn: false));
    }

    private IEnumerator FadeCoroutine(VisualElement element, float duration, bool fadeIn)
    {
        float elapsed = 0;
        float startOpacity = fadeIn ? 0 : element.resolvedStyle.opacity;
        float targetOpacity = fadeIn ? 1 : 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            element.style.opacity = Mathf.Lerp(startOpacity, targetOpacity, t);
            yield return null;
        }

        element.style.opacity = targetOpacity;
        if (!fadeIn)
            element.style.display = DisplayStyle.None;
    }

    // Rotate an element continuously (e.g., spinner)
    public void RotateElement(VisualElement element, float speed = 360f)
    {
        if (element == null) return;
        StartCoroutine(RotateCoroutine(element, speed));
    }

    private IEnumerator RotateCoroutine(VisualElement element, float speed)
    {
        float rotation = 0;
        while (element != null && element.style.display != DisplayStyle.None)
        {
            rotation += speed * Time.deltaTime;
            element.style.rotate = new StyleRotate(new Rotate(Angle.Degrees(rotation % 360)));
            yield return null;
        }
    }

    // Shake a UI element (useful for errors)
    public void ShakeElement(VisualElement element, float duration = 0.5f, float strengthX = 5f, float strengthY = 2f)
    {
        if (element == null) return;
        StartCoroutine(ShakeCoroutine(element, duration, strengthX, strengthY));
    }

    private IEnumerator ShakeCoroutine(VisualElement element, float duration, float strengthX, float strengthY)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float offsetX = UnityEngine.Random.Range(-strengthX, strengthX);
            float offsetY = UnityEngine.Random.Range(-strengthY, strengthY);

            element.style.translate = new StyleTranslate(new Translate(offsetX, offsetY));

            yield return null;
        }

        // Reset position
        element.style.translate = new StyleTranslate(new Translate(0, 0));
    }

    public void SlideInElement(VisualElement element, Vector2 startOffset, float duration = 0.3f)
    {
        if (element == null) return;

        element.style.opacity = 0;
        element.style.translate = new StyleTranslate(new Translate(startOffset.x, startOffset.y));

        StartCoroutine(SlideInCoroutine(element, startOffset, duration));
    }

    private IEnumerator SlideInCoroutine(VisualElement element, Vector2 startOffset, float duration)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            element.style.opacity = t;

            element.style.translate = new StyleTranslate(new Translate(
                Mathf.Lerp(startOffset.x, 0, t),
                Mathf.Lerp(startOffset.y, 0, t)
            ));

            yield return null;
        }

        element.style.opacity = 1;
        element.style.translate = new StyleTranslate(new Translate(0, 0));
    }
}