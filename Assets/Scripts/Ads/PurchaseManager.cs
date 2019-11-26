using System;
using UnityEngine;
using UnityEngine.Purchasing;

[System.Serializable]
public class C_Settings
{
    public string id;
    public int CoinsCount;
    public string Price;
    public string _eventData;
}

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    private int currentProductIndex;

    [Tooltip("Не многоразовые товары. Больше подходит для отключения рекламы и т.п.")]
    public string[] NC_PRODUCTS;
    [Tooltip("Многоразовые товары. Больше подходит для покупки игровой валюты и т.п.")]
    public C_Settings[] C_PRODUCTS;

    /// <summary>
    /// Событие, которое запускается при удачной покупке многоразового товара.
    /// </summary>
    public static event OnSuccessConsumable OnPurchaseConsumable;
    /// <summary>
    /// Событие, которое запускается при удачной покупке не многоразового товара.
    /// </summary>
    public static event OnSuccessNonConsumable OnPurchaseNonConsumable;
    /// <summary>
    /// Событие, которое запускается при неудачной покупке какого-либо товара.
    /// </summary>
    public static event OnFailedPurchase PurchaseFailed;

    public static PurchaseManager instance;

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        InitializePurchasing();
    }
    /// <summary>
    /// Проверить, куплен ли товар.
    /// </summary>
    /// <param name="id">Индекс товара в списке.</param>
    /// <returns></returns>
    public static bool CheckBuyState(string id)
    {
        Product product = m_StoreController.products.WithID(id);
        if (product.hasReceipt) { return true; }
        else { return false; }
    }

    public static string GetProductPrice(string id)
    {
        Product product = m_StoreController.products.WithID(id);
            Debug.Log(product.metadata.isoCurrencyCode);
            return product.metadata.localizedPriceString;
    }

    public void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (C_Settings s in C_PRODUCTS) builder.AddProduct(s.id, ProductType.Consumable);
        foreach (string s in NC_PRODUCTS) builder.AddProduct(s, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyConsumable(int index)
    {
        currentProductIndex = index;
        BuyProductID(C_PRODUCTS[index].id);
    }

    public void BuyNonConsumable(int index)
    {
        currentProductIndex = index;
        BuyProductID(NC_PRODUCTS[index]);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                OnPurchaseFailed(product, PurchaseFailureReason.ProductUnavailable);
            }
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        for (int i = 0; i < C_PRODUCTS.Length; i++)
        {
            C_PRODUCTS[i].Price = GetProductPrice(C_PRODUCTS[i].id);
        }

        if(CheckBuyState(NC_PRODUCTS[0]))
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

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (C_PRODUCTS.Length > 0 && String.Equals(args.purchasedProduct.definition.id, C_PRODUCTS[currentProductIndex].id, StringComparison.Ordinal))
            OnSuccessC(args);
        else if (NC_PRODUCTS.Length > 0 && String.Equals(args.purchasedProduct.definition.id, NC_PRODUCTS[currentProductIndex], StringComparison.Ordinal))
            OnSuccessNC(args);
        else
        {
            Debug.Log("Origin product: " + C_PRODUCTS[currentProductIndex].id);
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    public delegate void OnSuccessConsumable(PurchaseEventArgs args);
    protected virtual void OnSuccessC(PurchaseEventArgs args)
    {
        if (OnPurchaseConsumable != null) OnPurchaseConsumable(args);
        Debug.Log(C_PRODUCTS[currentProductIndex] + " Buyed!");

        AnalyticsEventsController.LogEvent(C_PRODUCTS[currentProductIndex]._eventData);
        PlayerStats.instance.AddInstaCoins(C_PRODUCTS[currentProductIndex].CoinsCount);
    }
    public delegate void OnSuccessNonConsumable(PurchaseEventArgs args);
    protected virtual void OnSuccessNC(PurchaseEventArgs args)
    {
        if (OnPurchaseNonConsumable != null) OnPurchaseNonConsumable(args);
        AnalyticsEventsController.LogEvent("No_ads_buyed");
        Debug.Log(NC_PRODUCTS[currentProductIndex] + " Buyed!");
    }
    public delegate void OnFailedPurchase(Product product, PurchaseFailureReason failureReason);

    protected virtual void OnFailedP(Product product, PurchaseFailureReason failureReason)
    {
        PurchaseFailed?.Invoke(product, failureReason);
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        OnFailedP(product, failureReason);
    }
}