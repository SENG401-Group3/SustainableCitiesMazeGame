using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// Verifies message queuing, count tracking, and automatic message expiry
public class MessageHandlerTests
{
    private GameObject obj;
    private MessageHandler messageHandler;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("MessageHandler");
        messageHandler = obj.AddComponent<MessageHandler>();

        var type = typeof(MessageHandler);
        var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
        type.GetField("messageLifetimeSeconds", flags).SetValue(messageHandler, 2f);
        type.GetField("fontSize", flags).SetValue(messageHandler, 24);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(obj);
    }

    /// Verifies that the message queue is empty on startup
    [UnityTest]
    public IEnumerator MessageCount_StartsAtZero()
    {
        yield return null;

        Assert.AreEqual(0, messageHandler.getMessageCount());
    }

    /// Verifies that adding a message increases the queue count by 1
    [UnityTest]
    public IEnumerator AddMessage_IncreasesCount()
    {
        yield return null;
        messageHandler.addMessage("Hello");

        Assert.AreEqual(1, messageHandler.getMessageCount());
    }

    /// Verifies that adding multiple messages correctly adds up to the total count in the queue
    [UnityTest]
    public IEnumerator AddMultipleMessages_CountIsCorrect()
    {
        yield return null;
        messageHandler.addMessage("Message 1");
        messageHandler.addMessage("Message 2");
        messageHandler.addMessage("Message 3");

        Assert.AreEqual(3, messageHandler.getMessageCount());
    }

    /// Verifies that messages are automatically removed from the queue after their lifetime expires (set to 2 seconds in this test)
    [UnityTest]
    public IEnumerator MessageIsRemovedAfterLifetime()
    {
        yield return null;
        messageHandler.addMessage("Hello");
        yield return new WaitForSeconds(3f);
        
        Assert.AreEqual(0, messageHandler.getMessageCount());
    }
}