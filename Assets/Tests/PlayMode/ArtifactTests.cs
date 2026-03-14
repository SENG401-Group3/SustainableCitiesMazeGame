using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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

    [UnityTest]
    public IEnumerator HandleCollision_WithPlayer_DisablesCollider()
    {
        yield return null;

        var playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        var col = playerObj.AddComponent<CircleCollider2D>();
        artifact.handleCollision(col);

        Assert.IsFalse(obj.GetComponent<Collider2D>().enabled);

        Object.Destroy(playerObj);
    }

    [UnityTest]
    public IEnumerator HandleCollision_WithoutPlayerTag_DoesNotDisableCollider()
    {
        yield return null;

        var otherObj = new GameObject("Enemy");
        var col = otherObj.AddComponent<CircleCollider2D>();
        artifact.handleCollision(col);

        Assert.IsTrue(obj.GetComponent<Collider2D>().enabled);

        Object.Destroy(otherObj);
    }

    [UnityTest]
    public IEnumerator HandleCollision_CannotCollectTwice()
    {
        yield return null;

        var playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        var col = playerObj.AddComponent<CircleCollider2D>();
        artifact.handleCollision(col); // first collection
        artifact.handleCollision(col); // second should be ignored

        // collider should still be disabled
        Assert.IsFalse(obj.GetComponent<Collider2D>().enabled);

        Object.Destroy(playerObj);
    }
}