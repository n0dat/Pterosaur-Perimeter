using UnityEngine;

public class PlayerManager : MonoBehaviour {
    
    //Required references
    [SerializeField] private PlayerHealthUIHandler m_playerHealthUIHandler;
    private int m_playerHealth = 3;
    
    [SerializeField] private UpdateCurrency currencyTextUIScript;
    [SerializeField] private int skulls; //This is our currency

    
    

    void Start() {
        //Initially set our skulls amount to whatever was set in the unity editor.
        int skullsTemp = skulls;
        skulls = 0;
        skullsCredit(skullsTemp);
        
        setPlayerHealth(2);
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

    public void setPlayerHealth(int val)
    {
        if (val < 0 || val > 3 || !hasAllReferences())
            return;
        m_playerHealth = val;
        m_playerHealthUIHandler.setHealthLevel(m_playerHealth);
    }

    public void hit()
    {
        if ( !hasAllReferences() || m_playerHealth <= 0)
            return;
        m_playerHealth--;
        m_playerHealthUIHandler.setHealthLevel(m_playerHealth);
    }
    
    public void heal()
    {
        if ( !hasAllReferences() || m_playerHealth >= 3)
            return;
        m_playerHealth++;
        m_playerHealthUIHandler.setHealthLevel(m_playerHealth);
    }
}