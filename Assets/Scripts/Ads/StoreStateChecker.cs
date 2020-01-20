using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class StoreStateChecker : MonoBehaviour
{
    public static bool isStoreAuthed;

    private void Start()
    {
        UnityEngine.Purchasing.CodelessIAPStoreListener.OnStoreInitHandler += checkAds;
    }

    public void checkAds(IStoreController controller)
    {
        isStoreAuthed = true;

        if (CheckBuyState(controller))
        {
            AdsController.instance.Init(true);
            Debug.Log("Ads inited with no ads");
        }
        else
        {
            AdsController.instance.Init(false);
            Debug.Log("Ads inited");
        }
    }

    public bool CheckBuyState(IStoreController controller)
    {
        Product product = controller.products.WithID("no_ads1");
        if (product.hasReceipt) { return true; }
        else { return false; }
    }

}
