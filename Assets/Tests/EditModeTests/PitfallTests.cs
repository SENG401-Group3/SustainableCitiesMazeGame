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
        Object.DestroyImmediate(pitfallObj);
        Object.DestroyImmediate(messageObj);
    }

    /// Verifies that the message handler is correctly assigned
    [Test]
    public void MessageHandler_IsSetCorrectly()
    {
        Assert.AreEqual(messageHandler, pitfall.getMessageHandler());
    }
}