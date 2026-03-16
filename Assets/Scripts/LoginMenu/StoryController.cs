using UnityEngine;
using UnityEngine.UIElements;
using System;

public class StoryController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TypewriterText typewriter;
    private VisualElement root;
    private Label storyLabel;
    private Button nextButton;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        nextButton = root.Q<Button>("NextButton");
        nextButton.clicked += OnNextClicked;
        nextButton.style.display = DisplayStyle.None;
        //nextButton.style.visibility = Visibility.Hidden;

        storyLabel = root.Q<Label>("StoryLabel");

        Debug.Log(root);
        Debug.Log(storyLabel);
        Debug.Log(typewriter);

        string story = "Good day mayor! We have been appointed to assist " + 
        "you in putting your knowledge of sustainable cities to the test. It " +
        "is highly important that you get familiar with running a city, as " +
        "the good people of Alberta need a new place to call home. You will " +
        "get to know the importance of navigation and solve real-world problems " +
        "that could be faced in your city. Goodluck mayor, and may the odds be ever "+
        "in your favour \n\n" +
        "Click Next to get started..."; // add something to point towards sustainability goals about the city failing

        typewriter.StartTyping(storyLabel, story, () =>
        {
            Debug.Log("Typing finished!");
            UIAnimator.Instance.FadeInElement(nextButton, 0.5f);
            //nextButton.style.visibility = Visibility.Visible; // show button after typing
        });
    }

    private void OnNextClicked()
    {
        Debug.Log("next button clicked");
        uiManager.ShowWelcome();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
