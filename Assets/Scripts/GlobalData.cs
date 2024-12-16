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
    public static string _CurrentActiveCard = "_CurrentActiveCard";
    public static string _PurchasedCardTime = "_PurchasedCardTime";
    public static string _DiscountPromo = "_DiscountPromo";
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
    public static void SetCardPurchased(bool status, int cardIndex)
    {
        if (status == true)
            PlayerPrefs.SetInt(_IsCardPurchased + cardIndex, 1);
        else
            PlayerPrefs.SetInt(_IsCardPurchased + cardIndex, 0);
    }
    public static bool GetCardPurchase(int cardIndex)
    {
        if (PlayerPrefs.GetInt(_IsCardPurchased + cardIndex) == 1)
            return true;
        else
            return false;
    }
    public static void SetCardActive(bool status, int cardIndex)
    {
        if (status == true)
            PlayerPrefs.SetInt(_IsCardActive + cardIndex, 1);
        else
            PlayerPrefs.SetInt(_IsCardActive + cardIndex, 0);
    }
    public static bool GetCardActive(int cardIndex)
    {
        if (PlayerPrefs.GetInt(_IsCardActive + cardIndex) == 1)
            return true;
        else
            return false;
    }
    public static void SetPurchasedCardTime(string dateTime, int cardIndex)
    {
        PlayerPrefs.SetString(_PurchasedCardTime + cardIndex, dateTime);
    }
    public static string GetPurchasedCardTime(int cardIndex)
    {
        return PlayerPrefs.GetString(_PurchasedCardTime + cardIndex);
    }
    public static void SetCardExpire(bool status, int cardIndex)
    {
        if (status == true)
            PlayerPrefs.SetInt(_IsCardExpire + cardIndex, 1);
        else
            PlayerPrefs.SetInt(_IsCardExpire + cardIndex, 0);
    }
    public static bool GetCardExpire(int cardIndex)
    {
        if (PlayerPrefs.GetInt(_IsCardExpire + cardIndex) == 1)
            return true;
        else
            return false;
    }
    public static void SetCardDiscard(bool status, int cardIndex)
    {
        if (status == true)
            PlayerPrefs.SetInt(_IsCardDiscard + cardIndex, 1);
        else
            PlayerPrefs.SetInt(_IsCardDiscard + cardIndex, 0);
    }
    public static bool GetCardDiscard(int cardIndex)
    {
        if (PlayerPrefs.GetInt(_IsCardDiscard + cardIndex) == 1)
            return true;
        else
            return false;
    }
    public static void SetDiscountPromo(int promo, int cardIndex)
    {
        PlayerPrefs.SetInt(_DiscountPromo + cardIndex, promo);
    }
    public static int GetSetDiscountPromo(int cardIndex)
    {    
        return PlayerPrefs.GetInt(_DiscountPromo + cardIndex);   
    }
}
