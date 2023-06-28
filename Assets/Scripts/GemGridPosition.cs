using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGridPosition
{
    public event Action OnGemGridPositionDestroyed;

    public Vector3 worldPosition { get; set; }
    public bool isMoving { get; set; }
    public bool isDestroyed { get; private set; }
    public GemSO gemSO { get; private set; }

    public GemGridPosition(GemSO gemSO, Vector3 worldPosition)
    {
        this.gemSO = gemSO;
        this.worldPosition = worldPosition;

        isDestroyed = false;
    }

    public void Destroy()
    {
        isDestroyed = true;
        OnGemGridPositionDestroyed?.Invoke();
    }

    public void Renew(GemSO gemSO, Vector3 worldPosition)
    {
        this.gemSO = gemSO;
        this.worldPosition = worldPosition;

        isDestroyed = false;
    }
}
