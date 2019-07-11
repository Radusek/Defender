using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradesManager : MonoBehaviour
{
    public enum Upgrade { DoubleMissile, TripleMissile, Perpendicular, MissileRange, MissileSize, Dash, Mine, Immortal, TimeSlow, Reflection, ScoreBoost1, ScoreBoost2, Lives, UpgradesCount };

    int[] upgradePrice = new int[(int)Upgrade.UpgradesCount];
    public bool[] upgradeBought = new bool[(int)Upgrade.UpgradesCount];


    public static PlayerUpgradesManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        upgradePrice[(int)Upgrade.DoubleMissile] = 5;
        upgradePrice[(int)Upgrade.TripleMissile] = 8;
        upgradePrice[(int)Upgrade.Perpendicular] = 6;
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
    }

    bool CanAfford(int upgrade)
    {
        if (GameManager.Instance.shopCredits < upgradePrice[upgrade])
            return false;
        return true;
    }

    public void TryToBuyUpgrade(Button button, int upgrade)
    {
        if (CanAfford(upgrade) == false)
            return;

        BuyUpgrade(button, upgrade);
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
            TryToBuyUpgrade(button, (int)Upgrade.TripleMissile);
    }

    public void Perpendicular(Button button)
    {
        if (upgradeBought[(int)Upgrade.DoubleMissile] && upgradeBought[(int)Upgrade.TripleMissile] == false)
            TryToBuyUpgrade(button, (int)Upgrade.Perpendicular);
    }

    public void MissileRange(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.MissileRange);

        //rob cos
    }

    public void MissileSize(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.MissileSize);

        //rob cos
    }

    public void Dash(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.Dash);

        //rob cos
    }

    public void Mine(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.Mine);

        //rob cos
    }

    public void Immortal(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.Immortal);

        //rob cos
    }

    public void TimeSlow(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.TimeSlow);

        //rob cos
    }

    public void Reflection(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.Reflection);

        //rob cos
    }

    public void ScoreBoost1(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.ScoreBoost1);

        //rob cos
    }

    public void ScoreBoost2(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.ScoreBoost2);

        //rob cos
    }

    public void Lives(Button button)
    {
        TryToBuyUpgrade(button, (int)Upgrade.Lives);

        //rob cos
    }


    public void Reset()
    {
        bool[] upgradeBought = new bool[(int)Upgrade.UpgradesCount];
    }
}
