using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// Verifies collision handling and collection behaviour
public class ArtifactTests
{
    private GameObject obj;
    private Artifact artifact;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("Artifact");
        artifact = obj.AddComponent<Artifact>();
        obj.AddComponent<CircleCollider2D>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(obj);
    }

    /// Verifies that when a player collides with the artifact,
    /// the collider is disabled to prevent further collection
    [UnityTest]
    public IEnumerator HandleCollision_WithPlayer_DisablesCollider()
    {
        yield return null;

        // prevent scene load so the test environment is not destroyed
        // before the assertion runs
        artifact.disableSceneLoadForTesting = true;

        var playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        var col = playerObj.AddComponent<CircleCollider2D>();

        // get collider reference before collision since object is destroyed after
        var collider = obj.GetComponent<Collider2D>();
        artifact.handleCollision(col);

        Assert.IsFalse(collider.enabled);

        Object.Destroy(playerObj);
    }

    /// Verifies that the artifact cannot be collected more than once
    /// The second collision should be ignored and the collider stays disabled
    [UnityTest]
    public IEnumerator HandleCollision_CannotCollectTwice()
    {
        yield return null;

        // prevent scene load so the test environment is not destroyed
        // before the assertion runs
        artifact.disableSceneLoadForTesting = true;

        var playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        var col = playerObj.AddComponent<CircleCollider2D>();

        var collider = obj.GetComponent<Collider2D>();
        artifact.handleCollision(col); // first collection
        artifact.handleCollision(col); // second should be ignored

        Assert.IsFalse(collider.enabled);

        Object.Destroy(playerObj);
    }
}