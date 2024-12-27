using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData
{
    private static string _FirstRun = "_FirstRun";
    private static string _Coins = "_Coins";
    private static string _IsCardPurchased = "_isCardPurchased";
    private static string _IsCardActive = "_isCardActive";
    private static string _IsCardExpire = "_isCardExpire";
    private static string _IsCardDiscard = "_isCardDiscard";
    private static string _CurrentActiveCard = "_CurrentActiveCard";
    private static string _PurchasedCardTime = "_PurchasedCardTime";
    private static string _DiscountPromo = "_DiscountPromo";
    private static string _CardInventory = "_CardInventory";
    // For Mini Game of Discount Tyhcoon
    public static int CurrentCardSelected;
    public static int CurrentMainCardSelected;
    public static int CurrentInventorySelected;
    public static int FirstRun
    {
        get { return PlayerPrefs.GetInt(_FirstRun); }
        set { PlayerPrefs.SetInt(_FirstRun, value); }
    }
    public static float Coins   
    {
        get { return PlayerPrefs.GetFloat(_Coins); }   
        set { PlayerPrefs.SetFloat(_Coins, value); }  
    }
    public static int CurrentActiveCardCount
    {
        get { return PlayerPrefs.GetInt(_CurrentActiveCard); }
        set { PlayerPrefs.SetInt(_CurrentActiveCard, value); }
    }
    public static void SetCardInventoryPurchased(int cardIndex, int inventoryIndex, bool status)
    {
        if (status == true)
            PlayerPrefs.SetInt(_IsCardPurchased + cardIndex + inventoryIndex, 1);
        else
            PlayerPrefs.SetInt(_IsCardPurchased + cardIndex + inventoryIndex, 0);
    }
    public static bool GetCardInventoryPurchase(int cardIndex, int inventoryIndex)
    {
        if (PlayerPrefs.GetInt(_IsCardPurchased + cardIndex + inventoryIndex) == 1)
            return true;
        else
            return false;
    }
    //public static void SetCardActive(bool status, int cardIndex)
    //{
    //    if (status == true)
    //        PlayerPrefs.SetInt(_IsCardActive + cardIndex, 1);
    //    else
    //        PlayerPrefs.SetInt(_IsCardActive + cardIndex, 0);
    //}
    //public static bool GetCardActive(int cardIndex)
    //{
    //    if (PlayerPrefs.GetInt(_IsCardActive + cardIndex) == 1)
    //        return true;
    //    else
    //        return false;
    //}
    public static void SetPurchasedCardInventoryTime(int cardIndex, int inventoryIndex, string dateTime)
    {
        PlayerPrefs.SetString(_PurchasedCardTime + cardIndex + inventoryIndex, dateTime);
    }
    public static string GetPurchasedCardTime(int cardIndex, int inventoryIndex)
    {
        return PlayerPrefs.GetString(_PurchasedCardTime + cardIndex + inventoryIndex);
    }
    //public static void SetCardExpire(bool status, int cardIndex, int inventoryIndex)
    //{
    //    if (status == true)
    //        PlayerPrefs.SetInt(_IsCardExpire + cardIndex + inventoryIndex, 1);
    //    else
    //        PlayerPrefs.SetInt(_IsCardExpire + cardIndex + inventoryIndex, 0);
    //}
    //public static bool GetCardExpire(int cardIndex)
    //{
    //    if (PlayerPrefs.GetInt(_IsCardExpire + cardIndex) == 1)
    //        return true;
    //    else
    //        return false;
    //}
    //public static void SetCardDiscard(bool status, int cardIndex)
    //{
    //    if (status == true)
    //        PlayerPrefs.SetInt(_IsCardDiscard + cardIndex, 1);
    //    else
    //        PlayerPrefs.SetInt(_IsCardDiscard + cardIndex, 0);
    //}
    //public static bool GetCardDiscard(int cardIndex)
    //{
    //    if (PlayerPrefs.GetInt(_IsCardDiscard + cardIndex) == 1)
    //        return true;
    //    else
    //        return false;
    //}
    public static void SetDiscountPromo(int cardIndex, int inventoryIndex, int promo)
    {
        PlayerPrefs.SetInt(_DiscountPromo + cardIndex + inventoryIndex, promo);
    }
    public static int GetSetDiscountPromo(int cardIndex, int inventoryIndex)
    {    
        return PlayerPrefs.GetInt(_DiscountPromo + cardIndex + inventoryIndex);   
    }
    public static void SetCardInventory(int cardIndex, int value)
    {
        PlayerPrefs.SetInt(_CardInventory + cardIndex, value);
    }
    public static int GetCardInventory(int cardIndex)
    {
        return PlayerPrefs.GetInt(_CardInventory + cardIndex);
    }

    public static void SetCardInventoryActive(int cardIndex, int inventoryNumber, bool value)
    {
        if (value == true)
        {
            PlayerPrefs.SetInt(_IsCardActive + cardIndex + inventoryNumber, 1);
        }
        else
        {
            PlayerPrefs.SetInt(_IsCardActive + cardIndex + inventoryNumber, 0);
        }
    }
    public static bool GetCardInventoryActive(int cardIndex, int inventoryNumber)
    {
        if (PlayerPrefs.GetInt(_IsCardActive + cardIndex + inventoryNumber) == 1)
            return true;
        else
            return false;
    }
    public static void SetCardInventoryDiscard(int cardIndex, int inventoryNumber, bool value)
    {
        if (value == true)
        {
            PlayerPrefs.SetInt(_IsCardDiscard + cardIndex + inventoryNumber, 1);
        }
        else
        {
            PlayerPrefs.SetInt(_IsCardDiscard + cardIndex + inventoryNumber, 0);
        }
    }
    public static bool GetCardInventoryDiscard(int cardIndex, int inventoryNumber)
    {
        if (PlayerPrefs.GetInt(_IsCardDiscard + cardIndex + inventoryNumber) == 1)
            return true;
        else
            return false;
    }
    public static void SetCardInventoryExpire(int cardIndex, int inventoryNumber, bool value)
    {
        if (value == true)
        {
            PlayerPrefs.SetInt(_IsCardExpire + cardIndex + inventoryNumber, 1);
        }
        else
        {
            PlayerPrefs.SetInt(_IsCardExpire + cardIndex + inventoryNumber, 0);
        }
    }
    public static bool GetCardInventoryExpire(int cardIndex, int inventoryNumber)
    {
        if (PlayerPrefs.GetInt(_IsCardExpire + cardIndex + inventoryNumber) == 1)
            return true;
        else
            return false;
    }
}
