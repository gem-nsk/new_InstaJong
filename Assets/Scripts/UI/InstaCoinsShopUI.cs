using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct _ShopUnit
{
    public string IAP_Id;
    public int CoinsCount;

    public _ShopUnit GetUnit()
    {
        return this;
    }
}

public class InstaCoinsShopUI : ui_basement
{
    public _ShopUnit[] Units;

    public void BuyButton(int id)
    {

        //Adding to Player stats
        _ShopUnit _unit = Units[id].GetUnit();

        PlayerStats.instance.AddInstaCoins(_unit.CoinsCount);

        //IAP
    }
    public void Close()
    {
        Debug.Log("Canvas Closed");
        CanvasController.instance.CloseCanvas();
    }
}
