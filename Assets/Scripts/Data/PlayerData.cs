using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    private static PlayerData _instance;
    
    private int _balance;
    public int Balance { get => _balance; }
    
    public int LastLevelIndex { get; private set; }
    public int CurrentMultiplier { get; private set; }
    

    private string json;
    
    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerData();
            return _instance;
        }
    }
    
    private PlayerData()
    {
        json = PlayerPrefs.GetString("PlayerData");
        JsonUtility.FromJsonOverwrite(json, this);
    }

    private void CreateData()
    {
        json = JsonUtility.ToJson(this);
        Debug.Log(json);

        PlayerPrefs.SetString("PlayerData", json);
    }

    public void AddBalance(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Amount of chips to be added can not be 0 or less.");
            return;
        }
        
        _balance += amount;
        Debug.Log(amount + " coin added. Current coin: " + _balance);
        CreateData();
    }

    public void ReduceBalance(int amount)
    {
        if (amount <= 0 || amount > _balance)
        {
            Debug.LogWarning("Amount of chips to be extracted can not be 0 or less and can not be greater than current coin amount.");
            return;
        }
        
        _balance -= amount;
        Debug.Log(amount + " coin reduced. Current coin: " + _balance);
        CreateData();
    }

    public void SetLevelIndex(int index)
    {
        LastLevelIndex = index;
        
        CreateData();
    }

    public void SetMultiplier(int value)
    {
        CurrentMultiplier = value;
        
        CreateData();
    }

    public void Reset()
    {
        _balance = 0;
        
        CreateData();
    }
    
}
