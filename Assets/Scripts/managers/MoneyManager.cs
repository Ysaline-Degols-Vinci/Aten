using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    int money = 0;
    [SerializeField] private TextMeshProUGUI moneyText;
    public DialogueManager dialogueManager;
    public delegate void OnMoneyChanged();
    public event OnMoneyChanged onMoneyChanged;
    public static MoneyManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("MoneyManager instance already exists.");
        }
        instance = this;
        moneyText.text = money.ToString();
    }

    public void ChangeMoney(int amount)
    {
        money += amount;
        dialogueManager.updateInk(money);
        onMoneyChanged?.Invoke();
        GameEventsManager.Instance.MoneyChanged(money); // ou appelle DialogueManager directement

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
