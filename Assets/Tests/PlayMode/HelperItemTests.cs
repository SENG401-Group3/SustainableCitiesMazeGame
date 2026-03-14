using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HelperItemTests
{
    private GameObject obj;
    private HelperItem helperItem;
    private MessageHandler messageHandler;
    private PlayerItemController itemController;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("HelperItem");
        helperItem = obj.AddComponent<HelperItem>();

        var messageObj = new GameObject("MessageHandler");
        messageHandler = messageObj.AddComponent<MessageHandler>();
        var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
        typeof(MessageHandler).GetField("messageLifetimeSeconds", flags).SetValue(messageHandler, 2f);
        typeof(MessageHandler).GetField("fontSize", flags).SetValue(messageHandler, 24);
        helperItem.setMessageHandler(messageHandler);

        var controllerObj = new GameObject("ItemController");
        itemController = controllerObj.AddComponent<PlayerItemController>();
        helperItem.itemController = itemController;
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(obj);
    }

    [UnityTest]
    public IEnumerator HandleCollision_AddsItemToController()
    {
        yield return null;
        helperItem.setId(0);
        helperItem.handleCollision(null);

        Assert.IsTrue(itemController.hasItem(HelperItem.itemName.SpeedBoost));
    }

    [UnityTest]
    public IEnumerator HandleCollision_AddsMessage()
    {
        yield return null;
        helperItem.setId(0);
        helperItem.handleCollision(null);

        Assert.AreEqual(1, messageHandler.getMessageCount());
    }

    [UnityTest]
    public IEnumerator SetId_InvalidId_DefaultsToSpeedBoost()
    {
        yield return null;
        helperItem.setId(999);
        helperItem.handleCollision(null);

        Assert.IsTrue(itemController.hasItem(HelperItem.itemName.SpeedBoost));
    }
}