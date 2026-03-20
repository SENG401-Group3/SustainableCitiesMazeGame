using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
  [SerializeField]
  private int fontSize;

  private TextMeshProUGUI textComponent;
  RectTransform trans;
  private Transform parentCanvas;
  private Vector2 baseAnchorPosition;

  private float time;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    parentCanvas = gameObject.GetComponent<Transform>();

    // specify where the text will sit
    baseAnchorPosition = new Vector2(250, 150);

    // create a text object
    CreateTextObject("");

    time = 0;
  }

  public TextMeshProUGUI CreateTextObject(string textString)
  {
    GameObject textGO = new GameObject("DynamicText");
    textGO.transform.SetParent(parentCanvas, false); // 'false' keeps local orientation/scale correct

    textComponent = textGO.AddComponent<TextMeshProUGUI>();
    textComponent.text = textString;
    textComponent.fontSize = fontSize;
    textComponent.color = Color.white;
    textComponent.textWrappingMode = TextWrappingModes.NoWrap;
    textComponent.alignment = TextAlignmentOptions.TopRight;

    // Optional: Adjust RectTransform properties
    // TODO: keep text in bottom corner irrespective of screen size or message length
    // message length doesnt matter because its left justified. Need to know message height to move up when new messages
    trans = textGO.GetComponent<RectTransform>();
    trans.anchoredPosition = baseAnchorPosition;

    return textComponent;
  }

  void OnDestroy()
  {
    PlayerPrefs.SetInt("Time", (int)time);
  }

  // Update is called once per frame
  void Update()
  {
    time = Math.Min(time + Time.deltaTime, 999.0f);
    textComponent.text = "Time: " + (int)time;
  }
}
