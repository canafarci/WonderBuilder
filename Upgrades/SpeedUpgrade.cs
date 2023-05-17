using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : Upgrade
{

    override public void OnUpgradeClicked()
    {
        int maxLevel = References.Instance.GameConfig.SpeedUpgrades.Length;
        if (maxLevel == _level) { return; }

        base.OnUpgradeClicked();
    }
    protected override void SetLevelValues(int level)
    {
        GameConfig config = References.Instance.GameConfig;

        _moneyToUpgrade = config.SpeedUpgrades[_level - 1].Cost;
        _manager.SetAgentSpeed(config.SpeedUpgrades[_level - 1].AgentBaseSpeed);
        _text.SetCost(_moneyToUpgrade);
        _manager.UpdateText();

        base.SetLevelValues(level);

        int maxLevel = References.Instance.GameConfig.SpeedUpgrades.Length;
        if (maxLevel == _level) { OnMaxLevel(); }
    }
}
