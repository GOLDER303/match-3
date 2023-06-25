using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Logic : MonoBehaviour
{
    public static event Action<GemGrid> OnGridCreated;

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
        OnGridCreated?.Invoke(grid);
    }

    public IEnumerator HandleGemMove(Vector2Int gemOneXY, Vector2Int gemTwoXY)
    {
        SwapGemsPositions(gemOneXY, gemTwoXY);

        if (HasMatch3Link(gemOneXY) || HasMatch3Link(gemTwoXY))
        {
            //TODO
        }
        else
        {
            yield return null;
            yield return new WaitWhile(() => grid.GetGemGridPosition(gemOneXY).isMoving || grid.GetGemGridPosition(gemTwoXY).isMoving);
            yield return null;
            SwapGemsPositions(gemOneXY, gemTwoXY);
        }
    }

    private void SwapGemsPositions(Vector2Int gemOneXY, Vector2Int gemTwoXY)
    {
        GemGridPosition gemOneGridPosition = grid.GetGemGridPosition(gemOneXY);
        GemGridPosition gemTwoGridPosition = grid.GetGemGridPosition(gemTwoXY);

        gemOneGridPosition.worldPosition = grid.GetGemGridPositionWorldPosition(gemTwoXY);
        gemTwoGridPosition.worldPosition = grid.GetGemGridPositionWorldPosition(gemOneXY);

        grid.SetGemGridPosition(gemOneXY, gemTwoGridPosition);
        grid.SetGemGridPosition(gemTwoXY, gemOneGridPosition);
    }

    private bool HasMatch3Link(Vector2Int gemPosition)
    {
        //TODO
        return false;
    }
}
