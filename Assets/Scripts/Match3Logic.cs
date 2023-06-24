using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Logic : MonoBehaviour
{
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float cellSize = .8f;
    [SerializeField] private float cellPadding = .1f;
    [SerializeField] private List<GemSO> gemSOs;

    private Match3Visual match3Visual;
    private GemGrid grid;

    private void Awake()
    {
        match3Visual = GetComponent<Match3Visual>();
    }

    private void Start()
    {
        grid = new GemGrid(gridWidth, gridHeight, cellSize, cellPadding, gemSOs);
        match3Visual.Setup(grid);
    }

    public void SwapGemsPositions(Vector2Int gemOneXY, Vector2Int gemTwoXY)
    {
        GemGridPosition gemOneGridPosition = grid.GetGemGridPosition(gemOneXY);
        GemGridPosition gemTwoGridPosition = grid.GetGemGridPosition(gemTwoXY);

        grid.SetGemGridPosition(gemOneXY, gemTwoGridPosition);
        grid.SetGemGridPosition(gemTwoXY, gemOneGridPosition);

        match3Visual.SwapGemsVisualPositions(gemOneXY, gemTwoXY);

        CheckForLink(gemOneXY);
        CheckForLink(gemTwoXY);
    }

    private void CheckForLink(Vector2Int gemPosition)
    {

    }
}
