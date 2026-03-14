using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MazeGridTests
{
    private GameObject obj;
    private MazeGrid mazeGrid;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("MazeGrid");
        mazeGrid = obj.AddComponent<MazeGrid>();
        mazeGrid.mazeDimsX = 5;
        mazeGrid.mazeDimsY = 5;
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(obj);
    }

    [Test]
    public void GenerateMaze_ReturnsGrid()
    {
        var grid = mazeGrid.generateMaze();

        Assert.IsNotNull(grid);
    }

    [Test]
    public void GenerateMaze_GridHasCorrectDimensions()
    {
        var grid = mazeGrid.generateMaze();

        Assert.AreEqual(5, grid.GetLength(0));
        Assert.AreEqual(5, grid.GetLength(1));
    }

    [Test]
    public void GenerateMaze_ThrowsIfDimsAreZero()
    {
        mazeGrid.mazeDimsX = 0;
        mazeGrid.mazeDimsY = 0;
        
        Assert.Throws<System.Exception>(() => mazeGrid.generateMaze());
    }

    [Test]
    public void GetMaxDistance_IsGreaterThanZero()
    {
        mazeGrid.generateMaze();

        Assert.Greater(mazeGrid.getMaxDistance(), 0);
    }

    [Test]
    public void GetSpawnableTiles_ReturnsNonEmptyList()
    {
        mazeGrid.generateMaze();
        int max = mazeGrid.getMaxDistance();
        var tiles = mazeGrid.getSpawnableTiles(new Vector2Int(0, max));

        Assert.Greater(tiles.Count, 0);
    }

    [Test]
    public void GetSpawnableTiles_ReturnsEmptyForImpossibleRange()
    {
        mazeGrid.generateMaze();
        var tiles = mazeGrid.getSpawnableTiles(new Vector2Int(99999, 99999));

        Assert.AreEqual(0, tiles.Count);
    }
}