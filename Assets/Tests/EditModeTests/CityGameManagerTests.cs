using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

/// Verifies score tracking, artifact counting, and progress reset behaviour
public class CityGameManagerTests
{
    private GameObject obj;
    private CityGameManager manager;

    [SetUp]
    public void SetUp()
    {
        CityGameManager.Instance = null;

        obj = new GameObject("CityGameManager");
        manager = obj.AddComponent<CityGameManager>();

        PlayerPrefs.DeleteAll();
    }

    [TearDown]
    public void TearDown()
    {
        CityGameManager.Instance = null;
        Object.DestroyImmediate(obj);
        PlayerPrefs.DeleteAll();
    }

    /// Verifies that adding score correctly increases the session score
    [Test]
    public void AddScoreAndArtifacts_IncreasesScore()
    {
        manager.AddScoreAndArtifacts(10);

        Assert.AreEqual(10, manager.GetPlayerScore());
    }

    /// Verifies that collecting an artifact increases the artifact count by 1
    [Test]
    public void AddScoreAndArtifacts_IncreasesArtifactCount()
    {
        manager.AddScoreAndArtifacts(10);

        Assert.AreEqual(1, manager.GetArtifactsCollected());
    }
    
    /// Verifies that multiple score additions add up correctly for both score and artifact count
    [Test]
    public void AddScoreAndArtifacts_AccumulatesCorrectly()
    {
        manager.AddScoreAndArtifacts(10);
        manager.AddScoreAndArtifacts(5);

        Assert.AreEqual(15, manager.GetPlayerScore());
        Assert.AreEqual(2, manager.GetArtifactsCollected());
    }

    /// Verifies that ClearTempProgress resets both score and artifact count back to zero
    [Test]
    public void ClearTempProgress_ResetsScoreAndArtifacts()
    {
        manager.AddScoreAndArtifacts(10);
        manager.ClearTempProgress();

        Assert.AreEqual(0, manager.GetPlayerScore());
        Assert.AreEqual(0, manager.GetArtifactsCollected());
    }
}