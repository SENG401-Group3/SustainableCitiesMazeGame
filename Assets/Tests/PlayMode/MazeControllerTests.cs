using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MazeControllerTests
{
    private GameObject obj;
    private MazeController mazeController;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("MazeController");
        mazeController = obj.AddComponent<MazeController>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(obj);
    }

    [Test]
    public void MazeController_ComponentExists()
    {
        Assert.IsNotNull(mazeController);
    }

    [Test]
    public void MazeDims_DefaultToZero()
    {
        var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
        int x = (int)typeof(MazeController).GetField("mazeDimsX", flags).GetValue(mazeController);
        int y = (int)typeof(MazeController).GetField("mazeDimsY", flags).GetValue(mazeController);

        Assert.AreEqual(0, x);
        Assert.AreEqual(0, y);
    }
}