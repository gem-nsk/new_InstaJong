using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct _ShopUnit
{
    public string IAP_Id;
    public int CoinsCount;
    public Text PriceText;

    public _ShopUnit GetUnit()
    {
        return this;
    }
}

public class InstaCoinsShopUI : ui_basement
{
    public _ShopUnit[] Units;

    public override void Activate()
    {
        base.Activate();

        for (int i = 0; i < Units.Length; i++)
        {
            Units[i].PriceText.text = PurchaseManager.instance.C_PRODUCTS[i].Price;
        }
    }

    public void BuyButton(int id)
    {

        PurchaseManager.instance.BuyConsumable(id);
        //Adding to Player stats
        //_ShopUnit _unit = Units[id].GetUnit();

        //PlayerStats.instance.AddInstaCoins(_unit.CoinsCount);

        //IAP
    }
    public void Close()
    {
        Debug.Log("Canvas Closed");
        CanvasController.instance.CloseCanvas();
    }
}
