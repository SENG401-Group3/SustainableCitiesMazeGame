using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// Verifies PlayerPref updates, message display, and destruction on collection
public class HintScrollTests
{
    private GameObject obj;
    private HintScroll hintScroll;
    private MessageHandler messageHandler;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("HintScroll");
        hintScroll = obj.AddComponent<HintScroll>();

        var messageObj = new GameObject("MessageHandler");
        messageHandler = messageObj.AddComponent<MessageHandler>();

        var type = typeof(MessageHandler);
        var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
        type.GetField("messageLifetimeSeconds", flags).SetValue(messageHandler, 2f);
        type.GetField("fontSize", flags).SetValue(messageHandler, 24);

        hintScroll.setMessageHandler(messageHandler);

        // reset PlayerPrefs before each test
        PlayerPrefs.DeleteKey("HintScrollCollected");
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(obj);
    }

    /// Verifies that collecting the hint scroll sets the HintScrollCollected PlayerPref to 1 
    /// so the question screen can display the hint
    [UnityTest]
    public IEnumerator HandleCollision_SetsPlayerPref()
    {
        yield return null;
        var otherObj = new GameObject("Player");
        var col = otherObj.AddComponent<CircleCollider2D>();
        hintScroll.handleCollision(col);

        Assert.AreEqual(1, PlayerPrefs.GetInt("HintScrollCollected"));

        Object.Destroy(otherObj);
    }

    /// Verifies that collecting the hint scroll displays a message in the MessageHandler
    [UnityTest]
    public IEnumerator HandleCollision_AddsMessage()
    {
        yield return null;
        var otherObj = new GameObject("Player");
        var col = otherObj.AddComponent<CircleCollider2D>();
        hintScroll.handleCollision(col);

        Assert.AreEqual(1, messageHandler.getMessageCount());

        Object.Destroy(otherObj);
    }

    /// Verifies that the hint scroll object is destroyed after collection so it cannot be picked up again
    [UnityTest]
    public IEnumerator HandleCollision_DestroysGameObject()
    {
        yield return null;
        var otherObj = new GameObject("Player");
        var col = otherObj.AddComponent<CircleCollider2D>();
        hintScroll.handleCollision(col);

        yield return null;

        Assert.IsTrue(obj == null);

        Object.Destroy(otherObj);
    }
}