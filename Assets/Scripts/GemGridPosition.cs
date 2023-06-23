using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGridPosition
{
    private GemSO gemSO;
    private int x;
    private int y;

    public GemGridPosition(GemSO gemSO, int x, int y)
    {
        this.gemSO = gemSO;
        this.x = x;
        this.y = y;
    }

    public GemSO GetGemSO()
    {
        return gemSO;
    }
}
