using UnityEngine;

public class PlayerManager : MonoBehaviour {
    
    //Required references
    private MainManager mainManager;
    [SerializeField] private UpdateCurrency currencyTextUIScript;
    
    [SerializeField] private int skulls; //This is our currency
    
    

    void Start() {
        DontDestroyOnLoad(this);
        //mainManager = GameObject.Find("MainManager").gameObject.GetComponent<MainManager>();
        
        //Initially set our skulls amount to whatever was set in the unity editor.
        int skullsTemp = skulls;
        skulls = 0;
        skullsCredit(skullsTemp);
    }

    public void skullsCredit(int amount)
    {
        if (!hasAllReferences())
            return;
        
        if (amount <= 0)
            return;
        
        skulls += amount;
        currencyTextUIScript.updateCurrencyAmount(skulls);
    }

    public int getSkulls()
    {
        return skulls;
    }

    
    //Function will subtract the amount specified only if the credit is actually available.
    //If successfully subtracted, true will be returned. Otherwise, a value of false will be returned instead.
    public bool skullsCost(int amount)
    {
        if (!hasAllReferences())
            return false;
            
        if (amount <= 0)
            return false;

        int newSkullsAmount = skulls - amount;

        if (newSkullsAmount < 0)
        {
            currencyTextUIScript.flashCurrency();
            return false;
        }
        
        skulls = newSkullsAmount;
        currencyTextUIScript.updateCurrencyAmount(skulls);
        return true;
    }

    private bool hasAllReferences()
    {
        if (!currencyTextUIScript)
        {
            Debug.Log("Missing required references in PlayerManager script.");
            return false;
        }

        return true;
    }
}