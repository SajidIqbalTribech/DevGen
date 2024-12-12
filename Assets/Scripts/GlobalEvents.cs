using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEvents
{
    #region Coins Event

    public static Action<float> OnUpdateCurrencyText;
    public static Action<float> OnCurrencyReduction;
    public static Action<float> OnCurrencyAddition;

    public static void InvokeUpdateCurrencyText(float amount)
    {
        OnUpdateCurrencyText?.Invoke(amount);
    }

    public static void InvokeCoinReduction(float amount)
    {
        OnCurrencyReduction?.Invoke(amount);
    }

    public static void InvokeCoinsAddition(float amount)
    {
        OnCurrencyAddition?.Invoke(amount);
    }
    #endregion
}
