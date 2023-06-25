using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGridPosition
{
    public Vector3 worldPosition { get; set; }
    public bool isMoving { get; set; }

    private GemSO gemSO;

    public GemGridPosition(GemSO gemSO, Vector3 worldPosition)
    {
        this.gemSO = gemSO;
        this.worldPosition = worldPosition;
    }

    public GemSO GetGemSO()
    {
        return gemSO;
    }
}
