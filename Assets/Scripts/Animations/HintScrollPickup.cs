using UnityEngine;

public class HintScrollPickup : BasePickup
{
    protected override void OnPickupCollected(Collider2D player)
    {
        // Set PlayerPrefs flag for hint availability
        PlayerPrefs.SetInt("HintScrollCollected", 1);
        PlayerPrefs.Save();
        Debug.Log("📜 Hint scroll collected - hint will be available in next question");
    }
}