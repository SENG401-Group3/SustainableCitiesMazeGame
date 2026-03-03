using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MessageHandler : MonoBehaviour
{
  private Queue<string> messageQ;
  private TextMeshProUGUI textComponent;
  private Transform parentCanvas;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    parentCanvas = gameObject.GetComponent<Transform>();

    // create a text object
    CreateTextObject("Hint Scroll Collected!");

    // create a message queue
    messageQ = new Queue<string>();

  }

  public TextMeshProUGUI CreateTextObject(string textString)
  {
    GameObject textGO = new GameObject("DynamicText");
    textGO.transform.SetParent(parentCanvas, false); // 'false' keeps local orientation/scale correct

    textComponent = textGO.AddComponent<TextMeshProUGUI>();
    textComponent.text = textString;
    textComponent.fontSize = 16;
    textComponent.color = Color.white;

    // Optional: Adjust RectTransform properties
    RectTransform trans = textGO.GetComponent<RectTransform>();
    trans.anchoredPosition = new Vector2(-250, -225);

    return textComponent;
  }

  // Update is called once per frame
  void Update()
  {

    // create something to go through messages in the queue and update the text object every n seconds
  }
}
