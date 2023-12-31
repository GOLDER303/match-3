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
    [SerializeField] private ScoreManager scoreManager;

    private Match3Visual match3Visual;
    private GemGrid grid;

    private void Awake()
    {
        match3Visual = GetComponent<Match3Visual>();
    }

    private void Start()
    {
        grid = new GemGrid(gridWidth, gridHeight, cellSize, cellPadding, gemSOs, transform.position);
        OnGridCreated?.Invoke(grid);
    }

    public IEnumerator HandleGemMove(Vector2Int gemOneXY, Vector2Int gemTwoXY)
    {
        if (!grid.IsValidGridPosition(gemTwoXY) || !grid.IsValidGridPosition(gemTwoXY))
        {
            yield break;
        }

        SwapGemsPositions(gemOneXY, gemTwoXY);

        yield return null;
        yield return new WaitWhile(() => grid.GetGemGridPosition(gemOneXY).isMoving || grid.GetGemGridPosition(gemTwoXY).isMoving);
        yield return null;

        if (HasMatch3Link(gemOneXY.x, gemOneXY.y) || HasMatch3Link(gemTwoXY.x, gemTwoXY.y))
        {
            while (DestroyAllMatches())
            {
                FallGemGridPositionsIntoDestroyedPositions();
                yield return new WaitForSeconds(.5f);
                SpawnMissingGemGridPositions();
                yield return new WaitForSeconds(.5f);
            }
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
        if (!grid.IsValidGridPosition(gemTwoXY) || !grid.IsValidGridPosition(gemTwoXY))
        {
            return;
        }

        GemGridPosition gemOneGridPosition = grid.GetGemGridPosition(gemOneXY);
        GemGridPosition gemTwoGridPosition = grid.GetGemGridPosition(gemTwoXY);

        gemOneGridPosition.worldPosition = grid.GetGemGridPositionWorldPosition(gemTwoXY);
        gemTwoGridPosition.worldPosition = grid.GetGemGridPositionWorldPosition(gemOneXY);

        grid.SetGemGridPosition(gemOneXY, gemTwoGridPosition);
        grid.SetGemGridPosition(gemTwoXY, gemOneGridPosition);
    }


    private bool DestroyAllMatches()
    {
        HashSet<Vector2Int> allLinkedGemGridPositions = GetAllLinkedGemGridPosition();

        foreach (Vector2Int gemGridPositionXY in allLinkedGemGridPositions)
        {
            scoreManager.AddScore(100);
            DestroyGemGridPosition(gemGridPositionXY);
        }

        return allLinkedGemGridPositions.Count >= 1;
    }

    private HashSet<Vector2Int> GetAllLinkedGemGridPosition()
    {
        HashSet<Vector2Int> allLinkedGemGridPositions = new HashSet<Vector2Int>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (HasMatch3Link(x, y) && !IsGemGridPositionDestroyed(x, y))
                {
                    allLinkedGemGridPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        return allLinkedGemGridPositions;
    }

    private bool HasMatch3Link(int x, int y)
    {
        GemGridPosition gemGridPosition = grid.GetGemGridPosition(x, y);

        if (gemGridPosition.isDestroyed)
        {
            return false;
        }

        GemSO gemSO = gemGridPosition.gemSO;

        int rightLinkCount = CountMatchingGems(gemSO, x, y, 1, 0);
        int leftLinkCount = CountMatchingGems(gemSO, x, y, -1, 0);
        int topLinkCount = CountMatchingGems(gemSO, x, y, 0, 1);
        int bottomLinkCount = CountMatchingGems(gemSO, x, y, 0, -1);

        if (rightLinkCount + 1 + leftLinkCount >= 3)
        {
            return true;
        }

        if (topLinkCount + 1 + bottomLinkCount >= 3)
        {
            return true;
        }

        return false;
    }

    private int CountMatchingGems(GemSO gemSO, int startX, int startY, int offsetX, int offsetY)
    {
        int linkCount = 0;
        int i = 1;

        while (grid.IsValidGridPosition(startX + (i * offsetX), startY + (i * offsetY)) &&
               !IsGemGridPositionDestroyed(startX + (i * offsetX), startY + (i * offsetY)))
        {
            GemSO nextGemSO = grid.GetGemGridPosition(startX + (i * offsetX), startY + (i * offsetY)).gemSO;
            if (nextGemSO == gemSO)
            {
                linkCount++;
            }
            else
            {
                break;
            }

            i++;
        }

        return linkCount;
    }

    private bool IsGemGridPositionDestroyed(int x, int y)
    {
        return grid.GetGemGridPosition(x, y).isDestroyed;
    }

    private void DestroyGemGridPosition(Vector2Int gemGridPositionXY)
    {
        GemGridPosition gemGridPositionToDestroy = grid.GetGemGridPosition(gemGridPositionXY);
        gemGridPositionToDestroy.Destroy();
    }

    private void FallGemGridPositionsIntoDestroyedPositions()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GemGridPosition gemGridPosition = grid.GetGemGridPosition(x, y);

                if (!gemGridPosition.isDestroyed)
                {
                    int lowestDestroyedGemGridPositionY = y;

                    while (lowestDestroyedGemGridPositionY >= 1 && IsGemGridPositionDestroyed(x, lowestDestroyedGemGridPositionY - 1))
                    {
                        lowestDestroyedGemGridPositionY--;
                    }

                    if (lowestDestroyedGemGridPositionY < y)
                    {
                        SwapGemsPositions(new Vector2Int(x, y), new Vector2Int(x, lowestDestroyedGemGridPositionY));
                    }
                }
            }
        }
    }
    private void SpawnMissingGemGridPositions()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GemGridPosition gemGridPosition = grid.GetGemGridPosition(x, y);

                if (gemGridPosition.isDestroyed)
                {
                    gemGridPosition.Renew(gemSOs[UnityEngine.Random.Range(0, gemSOs.Count)], grid.GetGemGridPositionWorldPosition(x, y));
                    match3Visual.SpawnGem(gemGridPosition);
                }
            }
        }
    }
}
