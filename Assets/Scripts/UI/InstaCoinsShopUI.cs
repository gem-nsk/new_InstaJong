using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct _PackData
{
    public int _RefreshCount;
    public int _TipsCount;
    public int _AddTimeCount;

    public int _Price;
}

[System.Serializable]
public struct _ShopUnit
{
    public string IAP_Id;
    public int CoinsCount;
    public Text PriceText;

    public _PackData _pack;

    public type _Type;
    public enum type
    {
        real,
        instacoins
    }

    public _ShopUnit GetUnit()
    {
        return this;
    }
}

public class InstaCoinsShopUI : ui_basement
{
    public _ShopUnit[] Units;
    public GameObject BuyObject;
    public Text _InstaCoins;

    public override void Activate()
    {
        base.Activate();

        PlayerStats.instance._addInstaCoins += UpdateInstaCoins;

        PlayerStats.instance.AddInstaCoins(0);


        for (int i = 0; i < Units.Length; i++)
        {
            if(Units[i]._Type == _ShopUnit.type.real)
            {
                Units[i].PriceText.text = PurchaseManager.instance.C_PRODUCTS[i].Price;
            }
            else
            {
                Units[i].PriceText.text =   Units[i]._pack._Price + " Instacoins";
            }
        }
    }

    public void UpdateInstaCoins(int coins)
    {
        _InstaCoins.text = coins.ToString();
    }

    private void BuyAnimation(UnityEngine.Purchasing.PurchaseEventArgs args)
    {
        BuyObject.SetActive(true);
        PurchaseManager.OnPurchaseConsumable -= BuyAnimation;
    }

    public void CloseWindow()
    {
        BuyObject.SetActive(false);
    }

    public void BuyButton(int id)
    {
        switch (Units[id]._Type)
        {
            case _ShopUnit.type.instacoins:

                AddPack(Units[id]._pack);

                break;
            case _ShopUnit.type.real:

                PurchaseManager.OnPurchaseConsumable += BuyAnimation;
                PurchaseManager.instance.BuyConsumable(id);



                break;
        }
        
        //Adding to Player stats
        //_ShopUnit _unit = Units[id].GetUnit();

        //PlayerStats.instance.AddInstaCoins(_unit.CoinsCount);

        //IAP
    }

    public void AddPack(_PackData data)
    {
        if(PlayerStats.instance.InstaCoins >= data._Price)
        {
            PlayerStats.instance.AddPack(data._TipsCount, data._AddTimeCount, data._RefreshCount);
            BuyObject.SetActive(true);

            PlayerStats.instance.AddInstaCoins(-data._Price);
        }
    }


    public void Close()
    {
        Debug.Log("Canvas Closed");
        CanvasController.instance.CloseCanvas();
        PlayerStats.instance._addInstaCoins -= UpdateInstaCoins;
    }
}
