using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MessageHandler : MonoBehaviour
{
  [SerializeField]
  private float messageLifetimeSeconds;
  [SerializeField]
  private int fontSize;

  private Queue<string> messageQ;
  private TextMeshProUGUI textComponent;
  RectTransform trans;
  private Transform parentCanvas;
  private bool updating;
  private Vector2 baseAnchorPosition;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    parentCanvas = gameObject.GetComponent<Transform>();

    // specify where the text will sit
    baseAnchorPosition = new Vector2(-250, -150);

    // create a text object
    CreateTextObject("");

    // create a message queue
    messageQ = new Queue<string>();

    // dont update until we have messages
    updating = false;
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

    // Optional: Adjust RectTransform properties
    // TODO: keep text in bottom corner irrespective of screen size or message length
    // message length doesnt matter because its left justified. Need to know message height to move up when new messages
    trans = textGO.GetComponent<RectTransform>();
    trans.anchoredPosition = baseAnchorPosition;

    return textComponent;
  }

  public void addMessage(string textString)
  {
    // add a message to the queue and reconstruct the text display
    messageQ.Enqueue(textString);
    constructTextDisplay();
  }

  public int getMessageCount() => messageQ.Count;

  private void constructTextDisplay(){
    // create a copy of the queue and an empty message
    // no copy needed because foreach wont modify
    string message = "";
    
    // pop each message off and add to text message with a \n
    foreach(string msg in messageQ){
      message += "\n" + msg;
    }
  
    textComponent.text = message;
    trans.anchoredPosition = baseAnchorPosition + new Vector2(0, (fontSize + 3) * messageQ.Count);
  }

  private void updateTextDisplay(){
    // dequeue a message
    messageQ.Dequeue();
    // update display
    constructTextDisplay();
  }

  // Update is called once per frame
  void Update()
  {
    // start updating when we have messages, and stop if we dont
    if(updating && messageQ.Count == 0){
      CancelInvoke(nameof(updateTextDisplay));
      updating = false;
    }else if(!updating && messageQ.Count > 0){
      InvokeRepeating(nameof(updateTextDisplay), messageLifetimeSeconds, messageLifetimeSeconds);
      updating = true;
    }
  }
}
