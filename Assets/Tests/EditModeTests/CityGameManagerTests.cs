using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

/// Verifies score tracking and progress reset behaviour
public class CityGameManagerTests
{
    private GameObject obj;
    private CityGameManager manager;

    [SetUp]
    public void SetUp()
    {
        // Instead of trying to set Instance to null, we let Awake handle it
        // Just create a new GameObject and let the singleton pattern work
        obj = new GameObject("CityGameManager");
        manager = obj.AddComponent<CityGameManager>();

        // Wait one frame for Awake to run
        // Note: In EditMode tests, Awake runs immediately on AddComponent

        PlayerPrefs.DeleteAll();
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up
        if (obj != null)
        {
            Object.DestroyImmediate(obj);
        }

        // Reset the static instance
        var field = typeof(CityGameManager).GetField("Instance",
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.NonPublic);
        field.SetValue(null, null);

        PlayerPrefs.DeleteAll();
    }

    /// Verifies that adding score correctly increases the session score
    [Test]
    public void AddScore_IncreasesScore()
    {
        manager.AddScore(10);
        Assert.AreEqual(10, manager.GetPlayerScore());
    }

    /// Verifies that multiple score additions add up correctly
    [Test]
    public void AddScore_AccumulatesCorrectly()
    {
        manager.AddScore(10);
        manager.AddScore(5);
        Assert.AreEqual(15, manager.GetPlayerScore());
    }

    /// Verifies that ClearTempProgress resets score back to zero
    [Test]
    public void ClearTempProgress_ResetsScore()
    {
        manager.AddScore(10);
        manager.ClearTempProgress();
        Assert.AreEqual(0, manager.GetPlayerScore());
    }
}