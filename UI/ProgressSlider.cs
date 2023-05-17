using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Slider _slider;
    int _totalSteps, _currentStep;
    private void Awake()
    {
        _currentStep = 0;

        List<WonderStage> stages = FindObjectOfType<Wonder>().Stages;

        foreach (WonderStage stage in stages)
        {
            _totalSteps += stage.TotalStageParts;
        }
        SetUI();
    }

    public void OnPartBuilt()
    {
        _currentStep += 1;
        SetUI();
    }

    private void SetUI()
    {
        float pct = (float)_currentStep / (float)_totalSteps;

        _slider.value = pct;
        _text.text = (pct * 5000f).ToString("F0") + "/5000";
    }
}
