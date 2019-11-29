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
    public string _EventData;
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
    public GameObject SuccessfulBuy;
    public GameObject ErrorBuy;
    public Text _InstaCoins;

    public GameObject MainShop;
    public GameObject ShopLoading;

    //TUTORIAL
    public GameObject SPAWN;
    public GameObject SPAWN1;
    public GameObject SPAWN2;
    public GameObject SPAWN3;
    public GameObject Tutorialmenu_ui;


    public override void Activate()
    {
        base.Activate();

        StartCoroutine(WaitForAuth());

        if (GameControllerScr.instance != null)
        {
            GameControllerScr.instance._Timer.SetPaused("shop", true);
        }
    }

    private void auth()
    {
        MainShop.SetActive(true);
        ShopLoading.SetActive(false);

        StartTutourial();

        PlayerStats.instance._addInstaCoins += UpdateInstaCoins;

        PlayerStats.instance.AddInstaCoins(0);


        for (int i = 0; i < Units.Length; i++)
        {
            if (Units[i]._Type == _ShopUnit.type.real)
            {
                Units[i].PriceText.text = PurchaseManager.instance.C_PRODUCTS[i].Price;
            }
            else
            {
                Units[i].PriceText.text = Units[i]._pack._Price.ToString();
            }
        }
    }

    public void StartTutourial()
    {
        #region tutorial
        if (!PlayerPrefs.HasKey("_tut4"))
        {
            List<RectTransform> RTs = new List<RectTransform>();
            RTs.Add((RectTransform)SPAWN.transform);
            RTs.Add((RectTransform)SPAWN1.transform);
            RTs.Add((RectTransform)SPAWN2.transform);//rev
            RTs.Add((RectTransform)SPAWN3.transform);//rev

            string[] messages = {
            //"Справа вы можете купить InstaCoin-ы. На ваш выбор представлены несколько наборов всегда по выгодным ценам!",
            //"На InstaCoin-ы вы можете купить карты, которые в себе содержат определенный набор подсказок.",
            //"Вверху показан ваш текущий баланс InstaCoin-ов",
            //"Рядом с вашим балансом есть кнопка отключения рекламы, чтобы она не мешала вам играть. Опция платная.",
            "_t_tut4_1",
            "_t_tut4_2",
            "_t_tut4_3",
            "_t_tut4_4"
            };


            //"А сейчас мы сыграем в аккаунт дня. Нажмите на него прямо сейчас!"

            CanvasController.instance.OpenCanvas(Tutorialmenu_ui, false);
            TutorialMenu_ui.instance.Init(RTs, 4, messages, false, 3, (RectTransform)SPAWN2.transform, (RectTransform)SPAWN2.transform);

            PlayerPrefs.SetInt("_tut4", 1);

        }
        #endregion
    }

    public void UpdateInstaCoins(int coins)
    {
        if(_InstaCoins)
        _InstaCoins.text = coins.ToString();
    }

    public IEnumerator WaitForAuth()
    {
        while(StoreStateChecker.isStoreAuthed)
        {
                auth();
            yield break;
        }
        
        yield return null;

    }

    public void BuyNoAds()
    {
        AnalyticsEventsController.LogEvent("No_ads_buyed");
        Debug.Log("No ads buyed!");
    }

    private void BuyAnimation(UnityEngine.Purchasing.PurchaseEventArgs args)
    {
        SuccessfulBuy.SetActive(true);
        PurchaseManager.OnPurchaseConsumable -= BuyAnimation;
        PurchaseManager.PurchaseFailed -= FailedPurchase;
    }

    public void CloseWindow()
    {
        SuccessfulBuy.SetActive(false);
        ErrorBuy.SetActive(false);
    }

    public void BuyButton(int id)
    {
        switch (Units[id]._Type)
        {
            case _ShopUnit.type.instacoins:

                AddPack(Units[id]._pack);

                break;
        }
    }

    public void BuyInstaCoins(int count)
    {
        PlayerStats.instance.AddInstaCoins(count);
    }

    private void FailedPurchase(UnityEngine.Purchasing.Product product, UnityEngine.Purchasing.PurchaseFailureReason failureReason)
    {
        ErrorBuy.SetActive(true);
    }

    public void AddPack(_PackData data)
    {
        if(PlayerStats.instance.InstaCoins >= data._Price)
        {
            PlayerStats.instance.AddPack(data._TipsCount, data._AddTimeCount, data._RefreshCount);
            SuccessfulBuy.SetActive(true);

            PlayerStats.instance.AddInstaCoins(-data._Price);

            AnalyticsEventsController.LogEvent(data._EventData);
        }
    }

    public override void CanvasControllerClose()
    {
        Debug.Log("Canvas Closed");
        if (GameControllerScr.instance != null)
        {
            GameControllerScr.instance._Timer.SetPaused("shop", false);
        }
        //CanvasController.instance.CloseCanvas();
        PlayerStats.instance._addInstaCoins -= UpdateInstaCoins;
        PurchaseManager.OnPurchaseConsumable -= BuyAnimation;
        PurchaseManager.PurchaseFailed -= FailedPurchase;

        base.CanvasControllerClose();
    }
}
