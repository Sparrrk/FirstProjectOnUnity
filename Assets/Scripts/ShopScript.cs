using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] TextMeshProUGUI[] buttonTexts;
    [SerializeField] TextMeshProUGUI[] buttonDescriptions;
    [SerializeField] GameObject panel;
    [SerializeField] BulletScript bullet;
    [SerializeField] int[] prices;


    private bool isActive = false;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        SetDescription();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!PlayerPrefs.HasKey("Button" + i.ToString()))
            {
                PlayerPrefs.SetInt("Button" + i.ToString(), 0);
            }
            else if (PlayerPrefs.GetInt("Button" + i.ToString()) == 1)
            {
                buttons[i].interactable = false;
                buttonTexts[i].text = "Purchased";
            }
        }
    }

    private void Update()
    {
        CheckPrices();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive; 
            panel.SetActive(isActive);

            if (panel.activeInHierarchy)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.visible = false;
            }

        }
    }

    /// <summary>
    /// Проверяет, соответствует ли текущая сумма денег у игрока цене улучшения.
    /// </summary>
    private void CheckPrices()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Button" + i.ToString()) == 1) continue;

            if (PlayerScript.instance.coinAmount < prices[i])
            {
                buttons[i].interactable = false;
                buttonTexts[i].text = "Not enough money";
            }
            else
            {
                buttons[i].interactable = true;
                buttonTexts[i].text = "Buy";
            }
        }
    }

    public void Buy(int buttonIndex)
    {
        buttons[buttonIndex].interactable = false;
        buttonTexts[buttonIndex].text = "Purchased";
        PlayerScript.instance.coinAmount -= prices[buttonIndex];
        PlayerPrefs.SetInt("Button" + buttonIndex.ToString(), 1);
        GiveBonus(buttonIndex);
    }

    private void GiveBonus(int index)
    {
        switch (index)
        {
            case 0:
                bullet.IncreaseDamage();
                break;
            case 1:
                break;
            case 2:
                PlayerScript.instance.IncreaseAtkSpeed();
                break;
            case 3:
                break;
        }
    } 

    /// <summary>
    /// добавляет цену в описание кнопок
    /// </summary>
    private void SetDescription()
    {
        for (int i = 0; i < buttonDescriptions.Length; i++) 
        {
            buttonDescriptions[i].text = prices[i].ToString() + "coins";
        }
    }
}
