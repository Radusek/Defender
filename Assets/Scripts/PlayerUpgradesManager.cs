﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradesManager : MonoBehaviour
{
    public enum Upgrade { DoubleMissile, TripleMissile, Perpendicular, MissileRange, MissileSize, Dash, Mine, Immortal, TimeSlow, Reflection, ScoreBoost1, ScoreBoost2, Lives, UpgradesCount };

    int[] upgradePrice = new int[(int)Upgrade.UpgradesCount];
    public bool[] upgradeBought = new bool[(int)Upgrade.UpgradesCount];


    public Button tripleShotButton;
    public Button sideShotsButton;

    public Button reflectButton;
    public Button timeSlowButton;

    public Button scoreBoost2Button;
    public Button livesButton;

    public Button[] allButtons;


    private ColorBlock xorButtonColor;
    private ColorBlock normalColor;


    public GameObject dashUI;
    public Slider dashSlider;

    public GameObject mineUI;
    public Slider mineSlider;

    public GameObject immortalUI;
    public Slider immortalSlider;

    public GameObject reflectUI;
    public Slider reflectSlider;

    public GameObject timeSlowUI;
    public Slider timeSlowSlider;


    public GameObject shootingSliderObject;
    public Slider shootingSlider;


    public static PlayerUpgradesManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        shootingSlider.value = 1f;
    }

    private void Start()
    {
        upgradePrice[(int)Upgrade.DoubleMissile] = 5;
        upgradePrice[(int)Upgrade.TripleMissile] = 6;
        upgradePrice[(int)Upgrade.Perpendicular] = 9;
        upgradePrice[(int)Upgrade.MissileRange] = 1;
        upgradePrice[(int)Upgrade.MissileSize] = 1;
        upgradePrice[(int)Upgrade.Dash] = 3;
        upgradePrice[(int)Upgrade.Mine] = 2;
        upgradePrice[(int)Upgrade.Immortal] = 4;
        upgradePrice[(int)Upgrade.TimeSlow] = 5;
        upgradePrice[(int)Upgrade.Reflection] = 5;
        upgradePrice[(int)Upgrade.ScoreBoost1] = 6;
        upgradePrice[(int)Upgrade.ScoreBoost2] = 9;
        upgradePrice[(int)Upgrade.Lives] = 10;

        normalColor = tripleShotButton.colors;

        xorButtonColor = normalColor;
        xorButtonColor.disabledColor = Color.black;
    }

    private void Update()
    {
        ShowShootingSlider(shootingSlider.value < 1f);
    }

    bool CanAfford(int upgrade)
    {
        if (GameManager.Instance.shopCredits < upgradePrice[upgrade])
            return false;
        return true;
    }

    public bool TryToBuyUpgrade(Button button, int upgrade)
    {
        if (CanAfford(upgrade) == false)
            return false;

        BuyUpgrade(button, upgrade);
        return true;
    }

    void BuyUpgrade(Button button, int upgrade)
    {
        GameManager.Instance.shopCredits -= upgradePrice[upgrade];
        GameManager.Instance.shopCreditsText.text = "Credits: " + GameManager.Instance.shopCredits.ToString();

        upgradeBought[upgrade] = true;

        button.interactable = false;
    }

    public void DoubleMissiles(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.DoubleMissile);

    }

    public void TripleMissiles(Button button)
    {
        if (upgradeBought[(int)Upgrade.DoubleMissile] && upgradeBought[(int)Upgrade.Perpendicular] == false)
        {
            if (TryToBuyUpgrade(button, (int)Upgrade.TripleMissile))
            {
                sideShotsButton.colors = xorButtonColor;
                sideShotsButton.interactable = false;
            }
        }
    }

    public void Perpendicular(Button button)
    {
        if (upgradeBought[(int)Upgrade.DoubleMissile] && upgradeBought[(int)Upgrade.TripleMissile] == false)
        {
            if (TryToBuyUpgrade(button, (int)Upgrade.Perpendicular))
            {
                tripleShotButton.colors = xorButtonColor;
                tripleShotButton.interactable = false;
            }
        }
    }

    public void MissileRange(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.MissileRange);
    }

    public void MissileSize(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.MissileSize);
    }

    public void Dash(Button button)
    {
        if (TryToBuyUpgrade(button, (int)Upgrade.Dash))
            dashUI.SetActive(true);
    }

    public void Mine(Button button)
    {
        if (TryToBuyUpgrade(button, (int)Upgrade.Mine))
            mineUI.SetActive(true);
    }

    public void Immortal(Button button)
    {
        if (TryToBuyUpgrade(button, (int)Upgrade.Immortal))
            immortalUI.SetActive(true);
    }

    public void TimeSlow(Button button)
    {
        if (upgradeBought[(int)Upgrade.Immortal] && upgradeBought[(int)Upgrade.Reflection] == false)
        {
            if (TryToBuyUpgrade(button, (int)Upgrade.TimeSlow))
            {
                reflectButton.colors = xorButtonColor;
                reflectButton.interactable = false;

                timeSlowUI.SetActive(true);
            }      
        }
    }

    public void Reflection(Button button)
    {
        if (upgradeBought[(int)Upgrade.Immortal] && upgradeBought[(int)Upgrade.TimeSlow] == false)
        {
            if (TryToBuyUpgrade(button, (int)Upgrade.Reflection))
            {
                timeSlowButton.colors = xorButtonColor;
                timeSlowButton.interactable = false;

                reflectUI.SetActive(true);
            }
        }
    }

    public void ScoreBoost1(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.ScoreBoost1);
    }

    public void ScoreBoost2(Button button)
    {
        if (upgradeBought[(int)Upgrade.ScoreBoost1] && upgradeBought[(int)Upgrade.Lives] == false)
        {
            if (TryToBuyUpgrade(button, (int)Upgrade.ScoreBoost2))
            {
                livesButton.colors = xorButtonColor;
                livesButton.interactable = false;
            }
        }
    }

    public void Lives(Button button)
    {
        if (upgradeBought[(int)Upgrade.ScoreBoost2] == false)
        {
            if (TryToBuyUpgrade(button, (int)Upgrade.Lives))
            {
                scoreBoost2Button.colors = xorButtonColor;
                scoreBoost2Button.interactable = false;
            }
        }
    }

    public void SetCooldownSlider(float value, int ability)
    {
        switch (ability)
        {
            case (int)Upgrade.Dash:
                dashSlider.value = value;
                break;
            case (int)Upgrade.Mine:
                mineSlider.value = value;
                break;
            case (int)Upgrade.Immortal:
                immortalSlider.value = value;
                break;
            case (int)Upgrade.Reflection:
                reflectSlider.value = value;
                break;
            case (int)Upgrade.TimeSlow:
                timeSlowSlider.value = value;
                break;
        }
    }


    public void Reset()
    {
        for (int i = 0; i < upgradeBought.Length; i++)
            upgradeBought[i] = false;

        foreach (var button in allButtons)
        {
            button.colors = normalColor;
            button.interactable = true;
        }

        dashUI.SetActive(false);
        mineUI.SetActive(false);
        immortalUI.SetActive(false);
        reflectUI.SetActive(false);
        timeSlowUI.SetActive(false);
    }

    public void setShootingSliderValue(float val)
    {
        shootingSlider.value = val;
    }

    public void ShowShootingSlider(bool b)
    {
        shootingSliderObject.SetActive(b);
    }
}
