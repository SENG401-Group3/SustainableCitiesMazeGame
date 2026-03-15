using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// Implementation of Interactable used for testing since Interactable is abstract 
/// it cannot be instantiated directly
public class TestInteractable : Interactable
{
    public bool collisionHandled = false;

    public override void handleCollision(Collider2D other)
    {
        collisionHandled = true;
    }
}

/// Verifies message handler assignment which all interactables are dependent on
public class InteractableTests
{
    private GameObject obj;
    private TestInteractable interactable;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("Interactable");
        interactable = obj.AddComponent<TestInteractable>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(obj);
    }

    /// Verifies that messageHandler is null by default before the setMessageHandler is called
    [Test]
    public void MessageHandler_IsNullByDefault()
    {
        Assert.IsNull(interactable.messageHandler);
    }

    /// Verifies that setMessageHandler correctly assigns the message handler reference
    [Test]
    public void SetMessageHandler_AssignsCorrectly()
    {
        var handlerObj = new GameObject().AddComponent<MessageHandler>();
        interactable.setMessageHandler(handlerObj);
        
        Assert.AreEqual(handlerObj, interactable.messageHandler);
    }
}