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
        foreach(_ShopUnit unit in Units)
        {
            unit.PriceText.text = PurchaseManager.GetProductPrice(unit.IAP_Id);
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
