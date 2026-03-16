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
        PlayerPrefs.DeleteAll();
    }

    [TearDown]
    public void TearDown()
    {
        CityUpdater.Instance = null;
        Object.Destroy(obj);
        PlayerPrefs.DeleteAll();
    }

    /// Verifies that completing a city increments CurrentCity in PlayerPrefs by 1
    [UnityTest]
    public IEnumerator CompleteCity_AdvancesToNextCity()
    {
        yield return null;
        PlayerPrefs.SetInt("CurrentCity", 1);
        cityUpdater.CompleteCity();

        Assert.AreEqual(2, PlayerPrefs.GetInt("CurrentCity", 1));
    }
}