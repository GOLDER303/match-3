using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Visual : MonoBehaviour
{
    [SerializeField] private GameObject gemGridVisualPrefab;

    private Match3Logic match3Logic;
    private GemGrid gemGrid;
    private Dictionary<GemGridPosition, GameObject> gemGridDictionary = new Dictionary<GemGridPosition, GameObject>();
    private Vector2Int startDragGemXY = new Vector2Int(-1, -1);
    private Vector2Int endDragGemXY = new Vector2Int(-1, -1);

    private void Awake()
    {
        match3Logic = GetComponent<Match3Logic>();
    }

    public void Setup(GemGrid gemGrid)
    {
        this.gemGrid = gemGrid;

        GameObject gemGridEmpty = new GameObject("GemGrid");

        for (int x = 0; x < gemGrid.width; x++)
        {
            for (int y = 0; y < gemGrid.height; y++)
            {
                GemGridPosition gemGridPosition = gemGrid.GetGemGridPosition(x, y);
                Vector3 position = gemGrid.GetGemGridPositionWorldPosition(x, y) + new Vector3(gemGrid.cellSize / 2, gemGrid.cellSize / 2);

                GameObject gemGridGameObject = Instantiate(gemGridVisualPrefab, position, Quaternion.identity, gemGridEmpty.transform);
                gemGridGameObject.transform.localScale = new Vector3(gemGrid.cellSize - gemGrid.cellPadding, gemGrid.cellSize - gemGrid.cellPadding);
                gemGridGameObject.GetComponent<SpriteRenderer>().sprite = gemGridPosition.GetGemSO().sprite;

                gemGridDictionary[gemGridPosition] = gemGridGameObject;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            startDragGemXY = gemGrid.GetGemGridPositionXY(mouseWorldPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            endDragGemXY = gemGrid.GetGemGridPositionXY(mouseWorldPosition);

            if (startDragGemXY.x == endDragGemXY.x && startDragGemXY.y == endDragGemXY.y)
            {
                return;
            }

            if (startDragGemXY.x != endDragGemXY.x)
            {
                if (endDragGemXY.x < startDragGemXY.x)
                {
                    endDragGemXY.x = startDragGemXY.x - 1;
                }
                else
                {
                    endDragGemXY.x = startDragGemXY.x + 1;
                }
            }
            else
            {
                if (endDragGemXY.y < startDragGemXY.y)
                {
                    endDragGemXY.y = startDragGemXY.y - 1;
                }
                else
                {
                    endDragGemXY.y = startDragGemXY.y + 1;
                }
            }

            match3Logic.SwapGemsPositions(startDragGemXY, endDragGemXY);
        }
    }

    public void SwapGemsVisualPositions(Vector2Int gemOnePosition, Vector2Int gemTwoPosition)
    {
        GemGridPosition gemOneGridPosition = gemGrid.GetGemGridPosition(gemOnePosition);
        GemGridPosition gemTwoGridPosition = gemGrid.GetGemGridPosition(gemTwoPosition);

        GameObject gemOneGameObject = gemGridDictionary[gemOneGridPosition];
        GameObject gemTwoGameObject = gemGridDictionary[gemTwoGridPosition];

        Vector3 gemTwoTransformPosition = gemTwoGameObject.transform.position;
        gemTwoGameObject.transform.position = gemOneGameObject.transform.position;
        gemOneGameObject.transform.position = gemTwoTransformPosition;
    }


    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return mouseWorldPosition;
    }
}
