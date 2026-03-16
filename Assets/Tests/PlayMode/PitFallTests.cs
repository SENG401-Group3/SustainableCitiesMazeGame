using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// Verifies message handler assignment and collision message behaviour
public class PitfallTests
{
    private GameObject pitfallObj;
    private GameObject messageObj;
    private Pitfall pitfall;
    private MessageHandler messageHandler;

    [SetUp]
    public void SetUp()
    {
        pitfallObj = new GameObject("Pitfall");
        pitfall = pitfallObj.AddComponent<Pitfall>();

        messageObj = new GameObject("MessageHandler");
        messageHandler = messageObj.AddComponent<MessageHandler>();
        
        pitfall.setMessageHandler(messageHandler);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(pitfallObj);
        Object.Destroy(messageObj);
    }

    /// Verifies that colliding with a pitfall adds a message to the MessageHandler queue to notify the player
    [UnityTest]
    public IEnumerator HandleCollision_AddsMessageToQueue()
    {
        yield return null;

        var otherObj = new GameObject("Player");
        var col = otherObj.AddComponent<CircleCollider2D>();

        pitfall.handleCollision(col);

        Assert.AreEqual(1, messageHandler.getMessageCount());

        Object.Destroy(otherObj);
    }
}