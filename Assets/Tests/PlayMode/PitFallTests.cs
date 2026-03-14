using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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

    [Test]
    public void MessageHandler_IsSetCorrectly()
    {
        Assert.AreEqual(messageHandler, pitfall.getMessageHandler());
    }

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