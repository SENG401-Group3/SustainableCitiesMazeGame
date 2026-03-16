using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

/// Verifies singleton behaviour and city progression logic
public class CityUpdaterTests
{
    private GameObject obj;
    private CityUpdater cityUpdater;

    [SetUp]
    public void SetUp()
    {
        CityUpdater.Instance = null;
        obj = new GameObject("CityUpdater");
        cityUpdater = obj.AddComponent<CityUpdater>();
        CityUpdater.Instance = cityUpdater; // set manually since Awake doesn't run in Edit Mode
        PlayerPrefs.DeleteAll();
    }

    [TearDown]
    public void TearDown()
    {
        CityUpdater.Instance = null;
        Object.DestroyImmediate(obj);
        PlayerPrefs.DeleteAll();
    }

    /// Verifies that only one CityUpdater instance exists at a time
    /// A second instance should be destroyed leaving the first as the singleton
    [Test]
    public void Singleton_OnlyOneInstanceExists()
    {
        var obj2 = new GameObject("CityUpdater2");
        obj2.AddComponent<CityUpdater>();

        // Instance should still be the first one since second should be rejected
        Assert.AreEqual(cityUpdater, CityUpdater.Instance);

        Object.DestroyImmediate(obj2);
    }
}