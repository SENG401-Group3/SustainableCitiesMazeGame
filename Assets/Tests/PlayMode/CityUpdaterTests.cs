using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Reflection;

/// Verifies singleton behaviour and city progression logic
public class CityUpdaterTests
{
    private GameObject obj;
    private CityUpdater cityUpdater;

    [SetUp]
    public void SetUp()
    {
        // Note: Direct assignment to CityUpdater.Instance was removed because the property has a private setter
        // The singleton's Awake() method handles setting Instance automatically when we add the component
        obj = new GameObject("CityUpdater");
        cityUpdater = obj.AddComponent<CityUpdater>(); // Awake() runs here and sets Instance

        PlayerPrefs.DeleteAll();
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the GameObject
        if (obj != null)
        {
            Object.DestroyImmediate(obj);
        }

        // Use reflection to reset the static instance for test isolation
        // This is necessary because Instance has a private setter and we can't assign to it directly
        var field = typeof(CityUpdater).GetField("Instance",
            BindingFlags.Static | BindingFlags.NonPublic);
        field?.SetValue(null, null);

        PlayerPrefs.DeleteAll();
    }

    /// Verifies that only one CityUpdater instance exists at a time
    /// A second instance should be destroyed leaving the first as the singleton
    [Test]
    public void Singleton_OnlyOneInstanceExists()
    {
        // Create a second instance - Awake() will run and see Instance is already set
        var obj2 = new GameObject("CityUpdater2");
        var secondUpdater = obj2.AddComponent<CityUpdater>();

        // Instance should still be the first one since second should be rejected by singleton pattern
        Assert.AreEqual(cityUpdater, CityUpdater.Instance);

        // The second object should have been destroyed in Awake()
        // In EditMode tests, we need to check if the component was destroyed
        Assert.IsTrue(secondUpdater == null || secondUpdater.Equals(null));

        // Clean up
        if (obj2 != null)
            Object.DestroyImmediate(obj2);
    }
}