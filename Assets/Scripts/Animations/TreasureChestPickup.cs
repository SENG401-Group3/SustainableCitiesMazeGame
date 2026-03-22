using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureChestPickup : BasePickup
{
    protected override void OnPickupCollected(Collider2D player)
    {
        // Load question scene after short delay to allow animation
        Invoke("LoadQuestionScene", 0.3f);
    }

    private void LoadQuestionScene()
    {
        SceneManager.LoadScene("QuestionScene");
    }
}