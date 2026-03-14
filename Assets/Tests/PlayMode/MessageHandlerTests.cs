using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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

    [UnityTest]
    public IEnumerator MessageCount_StartsAtZero()
    {
        yield return null;

        Assert.AreEqual(0, messageHandler.getMessageCount());
    }

    [UnityTest]
    public IEnumerator AddMessage_IncreasesCount()
    {
        yield return null;
        messageHandler.addMessage("Hello");

        Assert.AreEqual(1, messageHandler.getMessageCount());
    }

    [UnityTest]
    public IEnumerator AddMultipleMessages_CountIsCorrect()
    {
        yield return null;
        messageHandler.addMessage("Message 1");
        messageHandler.addMessage("Message 2");
        messageHandler.addMessage("Message 3");

        Assert.AreEqual(3, messageHandler.getMessageCount());
    }

    [UnityTest]
    public IEnumerator MessageIsRemovedAfterLifetime()
    {
        yield return null;
        messageHandler.addMessage("Hello");
        yield return new WaitForSeconds(3f);
        
        Assert.AreEqual(0, messageHandler.getMessageCount());
    }
}