using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

/// Verifies item collection, removal, and observer notification behaviour
public class PlayerItemControllerTests
{
    private GameObject obj;
    private PlayerItemController controller;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("PlayerItemController");
        controller = obj.AddComponent<PlayerItemController>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(obj);
    }

    /// Verifies that the item inventory is empty on startup
    [UnityTest]
    public IEnumerator HasItem_ReturnsFalse_WhenEmpty()
    {
        yield return null;
        
        Assert.IsFalse(controller.hasItem(HelperItem.itemName.SpeedBoost));
    }

    /// Verifies that collecting an item adds it to the inventory
    [UnityTest]
    public IEnumerator CollectItem_AddsItem()
    {
        yield return null;
        controller.collectItem(HelperItem.itemName.SpeedBoost);

        Assert.IsTrue(controller.hasItem(HelperItem.itemName.SpeedBoost));
    }

    /// Verifies that using an item removes it from the inventory
    [UnityTest]
    public IEnumerator UseItem_RemovesItem()
    {
        yield return null;
        controller.collectItem(HelperItem.itemName.SpeedBoost);
        controller.useItem(HelperItem.itemName.SpeedBoost);

        Assert.IsFalse(controller.hasItem(HelperItem.itemName.SpeedBoost));
    }

    /// Verifies that collecting an item notifies all registered observers, which is how 
    /// PlayerController receives the SpeedBoost effect
    [UnityTest]
    public IEnumerator CollectItem_NotifiesObservers()
    {
        yield return null;

        // using PlayerController as a dummy observer since it implements Observer
        bool notified = false;
        var testObserver = new TestObserver(() => notified = true);
        controller.addObserver(testObserver);
        controller.collectItem(HelperItem.itemName.SpeedBoost);

        Assert.IsTrue(notified);
    }
}

/// Observer stub used for testing observer notifications
public class TestObserver : Patterns.Observer
{
    private System.Action onNotify;
    public TestObserver(System.Action onNotify) { this.onNotify = onNotify; }
    public void notify() { onNotify(); }
}
