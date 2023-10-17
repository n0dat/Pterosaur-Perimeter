using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateCurrency : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyAmount;

    public void updateCurrencyAmount(int newAmount)
    {
        if (currencyAmount && newAmount >= 0)
            currencyAmount.SetText(newAmount.ToString());
    }
    
}
