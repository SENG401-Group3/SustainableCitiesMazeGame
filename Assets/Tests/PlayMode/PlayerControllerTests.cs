using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// Verifies initial player state on startup
public class PlayerControllerTests
{
    private GameObject playerObj;
    private Rigidbody2D rb;
    private PlayerController playerController;

    [SetUp]
    public void SetUp()
    {
        playerObj = new GameObject("Player");
        rb = playerObj.AddComponent<Rigidbody2D>();
        var itemController = playerObj.AddComponent<PlayerItemController>();
        playerController = playerObj.AddComponent<PlayerController>();

        var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
        var type = typeof(PlayerController);
        type.GetField("itemController", flags).SetValue(playerController, itemController);
        type.GetField("timeslice",     flags).SetValue(playerController, 0.02f);
        type.GetField("maxVel",        flags).SetValue(playerController, 5f);
        type.GetField("minVel",        flags).SetValue(playerController, 1f);
        type.GetField("acceleration",  flags).SetValue(playerController, 10f);
        type.GetField("velDecay",      flags).SetValue(playerController, 1.1f);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(playerObj);
    }

    /// Verifies that the player begins the game stationary
    [UnityTest]
    public IEnumerator VelocityStartsAtZero()
    {
        yield return null;
        
        Assert.AreEqual(Vector2.zero, playerController.GetCurrentVelocity());
    }
}