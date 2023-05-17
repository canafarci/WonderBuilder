using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeUpgrade : Upgrade
{

    List<BuilderNPC> _npcsToMerge = new List<BuilderNPC>();
    List<BuilderNPC> _currentList = new List<BuilderNPC>();
    bool _hasLoaded = false;
    bool _isMerging = false;
    bool _canMerge = false;
    protected override void OnEnable()
    {
        base.OnEnable();
        _manager.NPCCountChangeHandler += CheckCount;
    }
    override protected void OnDisable()
    {
        base.OnDisable();
        _manager.NPCCountChangeHandler -= CheckCount;
    }
    override public void OnUpgradeClicked()
    {
        if (!_canMerge || _isMerging) return;
        _isMerging = true;
        base.OnUpgradeClicked();
        DisableButton();
        _text.SetMerging();
    }

    public void OnMergeFinished()
    {
        _isMerging = false;
        _text.SetCost(_moneyToUpgrade);
    }

    protected override void SetLevelValues(int level)
    {
        GameConfig config = References.Instance.GameConfig;
        _moneyToUpgrade = config.MergeUpgrades[_level - 1].Cost;
        if (_hasLoaded)
            _manager.OnMergeBuilders(_npcsToMerge);
        else
            _hasLoaded = true;

        _npcsToMerge = new List<BuilderNPC>();
        base.SetLevelValues(level);
    }
    void CheckCount(List<BuilderNPC> builderList)
    {
        _currentList = builderList;
        if (builderList.Count < 1)
        {
            DisableWithMax();
            _canMerge = false;
            return;
        }
        int maxLevel = builderList.Max(x => x.Tier);

        List<List<BuilderNPC>> masterList = new List<List<BuilderNPC>>();

        for (int i = 0; i < maxLevel + 1; i++)
        {
            masterList.Add(builderList.Where(x => x.Tier == i).ToList());
        }

        foreach (List<BuilderNPC> lst in masterList)
        {
            if (lst.Count > 2)
            {
                _npcsToMerge = lst;
                _canMerge = true;
                if (CheckCanClick(Resource.Instance.Money))
                    EnableButton();
                else
                {
                    DisableButton();
                    _text.SetCost(_moneyToUpgrade);
                }
                return;
            }
        }
        _canMerge = false;
        DisableWithMax();
    }

    void DisableWithMax()
    {
        DisableButton();
        _text.SetMax();
    }
    protected override void EnableButton()
    {
        base.EnableButton();
        _text.SetCost(_moneyToUpgrade);
    }
    protected override bool CheckCanClick(float currentMoney)
    {
        if (!_canMerge) return false;
        else
        {
            return base.CheckCanClick(currentMoney);
        }
    }
}
