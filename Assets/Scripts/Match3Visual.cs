using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Visual : MonoBehaviour
{
    [SerializeField] private GameObject gemGridVisualPrefab;

    private GemGrid gemGrid;
    private Dictionary<GemGridPosition, GameObject> gemGridDictionary = new Dictionary<GemGridPosition, GameObject>();

    private void OnEnable()
    {
        Match3Logic.OnGridCreated += Setup;
    }

    private void OnDisable()
    {
        Match3Logic.OnGridCreated -= Setup;
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
                Vector3 position = gemGrid.GetGemGridPositionWorldPosition(x, y);

                GameObject gemGridGameObject = Instantiate(gemGridVisualPrefab, position, Quaternion.identity, gemGridEmpty.transform);
                gemGridGameObject.transform.localScale = new Vector3(gemGrid.cellSize - gemGrid.cellPadding, gemGrid.cellSize - gemGrid.cellPadding);
                gemGridGameObject.GetComponent<SpriteRenderer>().sprite = gemGridPosition.gemSO.sprite;

                gemGridGameObject.GetComponent<GemGridVisual>().Setup(gemGridPosition);

                gemGridDictionary[gemGridPosition] = gemGridGameObject;
            }
        }
    }
}
