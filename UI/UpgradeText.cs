using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _costText;

    public void SetCost(int cost)
    {
        _costText.text = "$" + cost.ToString();
    }

    public void SetMax()
    {
        _costText.text = "MAX";
    }
    public void SetMerging()
    {
        _costText.text = "MERGING";
    }
}
