using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateCurrency : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyAmount;
    [SerializeField] private Animator m_animatior;

    //Interface function for updating the currency.
    public void updateCurrencyAmount(int newAmount)
    {
        if (currencyAmount && newAmount >= 0)
            currencyAmount.SetText(newAmount.ToString());
    }

    //Flashes red for spending money.
    public void flashCurrency()
    {
        if (!hasAllReferences())
            return;
        
        m_animatior.SetTrigger("FlashRed");
    }

    // make sure we have all the references to avoid null exceptions
    private bool hasAllReferences()
    {
        if (!m_animatior || !currencyAmount)
        {
            Debug.Log("Missing required references in PlayerManager script.");
            return false;
        }
        return true;
    }
}
