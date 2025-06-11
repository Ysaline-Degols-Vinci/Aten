using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    int money = 0;
    [SerializeField] private TextMeshProUGUI moneyText;
    public DialogueManager dialogueManager;

    private void Awake()
    {
        moneyText.text = money.ToString();
    }

    public void ChangeMoney(int amount)
    {
        money += amount;
        dialogueManager.updateInk(money);
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = money.ToString();
    }

    public int GetMoney()
    {
        return money;
    }

 




}
