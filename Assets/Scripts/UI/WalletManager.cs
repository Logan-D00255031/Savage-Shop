using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    public static WalletManager instance;

    public float balance;

    public TMP_Text walletText;

    private void Start()
    {
        instance = this;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        // Do not display decimal point if it's a whole number
        if (Mathf.Approximately(balance % 1, 0))
        {
            walletText.text = balance.ToString();
        }
        else
        {
            walletText.text = balance.ToString("F2");
        }
    }

    public void AddToWallet(float amount)
    {
        balance += amount;
        UpdateDisplay();
    }

    public void RemoveFromWallet(float amount)
    {
        balance -= amount;
        if (balance < 0)
        {
            balance = 0;
        }
        UpdateDisplay();
    }
}
