using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class TypewriterText : MonoBehaviour
{
    public static TypewriterText Instance { get; private set; }

    private float typingSpeed = 0.03f;

    /*private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }*/

    // Add an optional callback that will run when typing finishes
    public void StartTyping(Label label, string text, Action onComplete = null)
    {
        StartCoroutine(TypeRoutine(label, text, onComplete));
    }

    private IEnumerator TypeRoutine(Label label, string text, Action onComplete)
    {
        label.text = "";
        foreach (char c in text)
        {
            label.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Typing finished, call the callback if assigned
        onComplete?.Invoke();
    }
}