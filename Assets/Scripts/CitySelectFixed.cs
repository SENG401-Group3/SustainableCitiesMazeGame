using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // ADD THIS

public class CitySelect : MonoBehaviour
{
    void Start()
    {
        FixCamera();
    }

    void FixCamera()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.orthographic = true;
            cam.orthographicSize = 10f;
            cam.transform.position = new Vector3(0, 0, -10);
        }
    }

    void OnGUI()
    {
        // Black background
        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;

        // Title - SMALLER
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 30;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(0, 20, Screen.width, 50), "SELECT A CITY", titleStyle);

        // Instructions - SMALLER
        GUIStyle instructionStyle = new GUIStyle(GUI.skin.label);
        instructionStyle.fontSize = 15;
        instructionStyle.alignment = TextAnchor.UpperCenter;
        instructionStyle.normal.textColor = Color.yellow;

        GUI.Label(new Rect(0, 70, Screen.width, 30), "PRESS 1-5 OR CLICK", instructionStyle);

        // Buttons - SMALLER
        string[] cities = {
            "CITY 1",
            "CITY 2",
            "CITY 3",
            "CITY 4",
            "CITY 5"
        };

        float buttonWidth = 180;
        float buttonHeight = 35;
        float startY = 110;

        for (int i = 0; i < 5; i++)
        {
            float x = (Screen.width - buttonWidth) / 2;
            float y = startY + (i * 45);

            if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), cities[i]))
            {
                LoadCity(i + 1);
            }
        }
    }

    void LoadCity(int cityNumber)
    {
        if (cityNumber == 1)
            SceneManager.LoadScene("City1_LeastSustainable");
        else if (cityNumber == 2)
            SceneManager.LoadScene("City2_SomewhatSustainable");
        else if (cityNumber == 3)
            SceneManager.LoadScene("City3_ModeratelySustainable");
        else if (cityNumber == 4)
            SceneManager.LoadScene("City4_VerySustainable");
        else if (cityNumber == 5)
            SceneManager.LoadScene("City5_MostSustainable");
    }

    void Update()
    {
        // Using the new Input System
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return; // Safety check

        // Check for number keys (both top row and keypad)
        if (keyboard.digit1Key.wasPressedThisFrame || keyboard.numpad1Key.wasPressedThisFrame)
            LoadCity(1);
        if (keyboard.digit2Key.wasPressedThisFrame || keyboard.numpad2Key.wasPressedThisFrame)
            LoadCity(2);
        if (keyboard.digit3Key.wasPressedThisFrame || keyboard.numpad3Key.wasPressedThisFrame)
            LoadCity(3);
        if (keyboard.digit4Key.wasPressedThisFrame || keyboard.numpad4Key.wasPressedThisFrame)
            LoadCity(4);
        if (keyboard.digit5Key.wasPressedThisFrame || keyboard.numpad5Key.wasPressedThisFrame)
            LoadCity(5);
    }
}