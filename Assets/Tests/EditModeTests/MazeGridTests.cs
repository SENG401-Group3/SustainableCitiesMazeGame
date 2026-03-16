using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// Verifies maze generation, dimensions, distance tracking, and spawnable tile logic
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
        Object.DestroyImmediate(obj);
    }

    /// Verifies that generateMaze returns a non-null grid
    [Test]
    public void GenerateMaze_ReturnsGrid()
    {
        var grid = mazeGrid.generateMaze();

        Assert.IsNotNull(grid);
    }

    /// Verifies that the generated grid matches the set dimensions
    [Test]
    public void GenerateMaze_GridHasCorrectDimensions()
    {
        var grid = mazeGrid.generateMaze();

        Assert.AreEqual(5, grid.GetLength(0));
        Assert.AreEqual(5, grid.GetLength(1));
    }
    
    /// Verifies that generateMaze throws an exception if dimensions are zero
    [Test]
    public void GenerateMaze_ThrowsIfDimsAreZero()
    {
        mazeGrid.mazeDimsX = 0;
        mazeGrid.mazeDimsY = 0;
        
        Assert.Throws<System.Exception>(() => mazeGrid.generateMaze());
    }
    
    /// Verifies that the maze has a valid max distance greater than zero
    [Test]
    public void GetMaxDistance_IsGreaterThanZero()
    {
        mazeGrid.generateMaze();

        Assert.Greater(mazeGrid.getMaxDistance(), 0);
    }
    
    /// Verifies that getSpawnableTiles returns at least one tile for a valid distance range
    [Test]
    public void GetSpawnableTiles_ReturnsNonEmptyList()
    {
        mazeGrid.generateMaze();
        int max = mazeGrid.getMaxDistance();
        var tiles = mazeGrid.getSpawnableTiles(new Vector2Int(0, max));

        Assert.Greater(tiles.Count, 0);
    }

    /// Verifies that getSpawnableTiles returns an empty list when the distance range is invalid
    [Test]
    public void GetSpawnableTiles_ReturnsEmptyForImpossibleRange()
    {
        mazeGrid.generateMaze();
        var tiles = mazeGrid.getSpawnableTiles(new Vector2Int(99999, 99999));

        Assert.AreEqual(0, tiles.Count);
    }
}