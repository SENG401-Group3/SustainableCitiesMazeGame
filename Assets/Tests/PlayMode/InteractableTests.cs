using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestInteractable : Interactable
{
    public bool collisionHandled = false;

    public override void handleCollision(Collider2D other)
    {
        collisionHandled = true;
    }
}

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
        Object.Destroy(obj);
    }

    [Test]
    public void MessageHandler_IsNullByDefault()
    {
        Assert.IsNull(interactable.messageHandler);
    }

    [Test]
    public void SetMessageHandler_AssignsCorrectly()
    {
        var handlerObj = new GameObject().AddComponent<MessageHandler>();
        interactable.setMessageHandler(handlerObj);
        
        Assert.AreEqual(handlerObj, interactable.messageHandler);
    }
}