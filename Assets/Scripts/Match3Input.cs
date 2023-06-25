using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Input : MonoBehaviour
{
    private Match3Logic match3Logic;
    private Match3Visual match3Visual;
    private GemGrid grid;

    private Vector2Int startDragGemXY = new Vector2Int(-1, -1);
    private Vector2Int endDragGemXY = new Vector2Int(-1, -1);

    private void Awake()
    {
        match3Logic = GetComponent<Match3Logic>();
        match3Visual = GetComponent<Match3Visual>();
    }

    private void OnEnable()
    {
        Match3Logic.OnGridCreated += Setup;
    }

    private void OnDisable()
    {
        Match3Logic.OnGridCreated -= Setup;
    }

    private void Setup(GemGrid grid)
    {
        this.grid = grid;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            startDragGemXY = grid.GetGemGridPositionXY(mouseWorldPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            endDragGemXY = grid.GetGemGridPositionXY(mouseWorldPosition);

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

            StartCoroutine(match3Logic.HandleGemMove(startDragGemXY, endDragGemXY));
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return mouseWorldPosition;
    }
}
