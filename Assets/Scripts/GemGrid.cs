using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGrid
{
    public int width { get; }
    public int height { get; }
    public float cellSize { get; }
    public float cellPadding { get; }
    private List<GemSO> gemSos;

    private GemGridPosition[,] gridArray;

    public GemGrid(int width, int height, float cellSize, float cellPadding, List<GemSO> gemSOs)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.cellPadding = cellPadding;
        this.gemSos = gemSOs;

        gridArray = new GemGridPosition[width, height];

        Setup();
    }

    private void Setup()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                List<GemSO> availableGemSOs = new List<GemSO>(gemSos);

                GemSO leftGem = gridArray[Mathf.Max(0, x - 1), y]?.GetGemSO();
                GemSO secondLeftGem = gridArray[Mathf.Max(0, x - 2), y]?.GetGemSO();

                GemSO bottomGem = gridArray[x, Mathf.Max(0, y - 1)]?.GetGemSO();
                GemSO secondBottomGem = gridArray[x, Mathf.Max(0, y - 2)]?.GetGemSO();

                if (leftGem == secondLeftGem)
                {
                    availableGemSOs.Remove(leftGem);
                }
                if (bottomGem == secondBottomGem)
                {
                    availableGemSOs.Remove(bottomGem);
                }

                GemSO newGemSO = availableGemSOs[Random.Range(0, availableGemSOs.Count)];

                gridArray[x, y] = new GemGridPosition(newGemSO, x, y);
            }
        }

    }

    public Vector3 GetCellWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public GemGridPosition GetGem(int x, int y)
    {
        return gridArray[x, y];
    }
}
