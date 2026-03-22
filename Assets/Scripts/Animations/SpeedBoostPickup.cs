using UnityEngine;

public class SpeedBoostPickup : BasePickup
{
    protected override void OnPickupCollected(Collider2D player)
    {
        // Speed boost effect applied via PlayerController's notify system
        Debug.Log($"✅ Speed boost effect applied to player");
        // The actual speed boost is handled by PlayerController's notify() method
    }
}