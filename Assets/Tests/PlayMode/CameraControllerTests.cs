using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CameraControllerTests
{
    private GameObject obj;
    private CameraController cameraController;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("CameraController");
        cameraController = obj.AddComponent<CameraController>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(obj);
    }

    [Test]
    public void SetCameraSize_UpdatesOrthographicSize()
    {
        cameraController.SetCameraSize(10f);

        Assert.AreEqual(10f, Camera.main.orthographicSize);
    }

    [Test]
    public void SetCameraPosition_UpdatesCameraPosition()
    {
        cameraController.SetCameraPosition(new Vector2(3f, 4f));
        
        Assert.AreEqual(3f, Camera.main.transform.position.x);
        Assert.AreEqual(4f, Camera.main.transform.position.y);
        Assert.AreEqual(-100f, Camera.main.transform.position.z);
    }
}