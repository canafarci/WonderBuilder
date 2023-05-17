using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] float _duration;
    [SerializeField] Material[] _brickMaterials, _steelMaterials;
    [SerializeField, ColorUsage(true, true)] Color[] _brickBaseColors;
    [SerializeField, ColorUsage(true, true)] Color[] _steelBaseColors;
    [ColorUsage(true, true)] Color _noOutlineColor = new Color(0f, 0f, 0f, 1);
    float _baseIntensity;
    private void Start()
    {
        _baseIntensity = _light.intensity;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(DOTween.To(x => _light.intensity = x, _light.intensity, 0, _duration / 2f).SetEase(Ease.Linear));
            seq.Append(DOTween.To(x => _light.intensity = x, 0, _baseIntensity, _duration / 2f).SetEase(Ease.Linear));

            for (int i = 0; i < _brickMaterials.Length; i++)
            {
                Sequence seq2 = DOTween.Sequence();
                seq2.Append(_brickMaterials[i].DOColor(_noOutlineColor, "_OutlineColor", _duration / 2f));
                seq2.Append(_brickMaterials[i].DOColor(_brickBaseColors[i], "_OutlineColor", _duration / 2f));
            }
            for (int i = 0; i < _steelMaterials.Length; i++)
            {
                Sequence seq2 = DOTween.Sequence();
                seq2.Append(_steelMaterials[i].DOColor(_noOutlineColor, "_OutlineColor", _duration / 2f));
                seq2.Append(_steelMaterials[i].DOColor(_steelBaseColors[i], "_OutlineColor", _duration / 2f));
            }
        }
    }
#endif

    public void DimLights()
    {
        DOTween.To(x => _light.intensity = x, _light.intensity, 0, _duration / 2f).SetEase(Ease.Linear);
        for (int i = 0; i < _brickMaterials.Length; i++)
        {
            Sequence seq2 = DOTween.Sequence();
            seq2.Append(_brickMaterials[i].DOColor(_noOutlineColor, "_OutlineColor", _duration / 2f));
        }
        for (int i = 0; i < _steelMaterials.Length; i++)
        {
            Sequence seq2 = DOTween.Sequence();
            seq2.Append(_steelMaterials[i].DOColor(_noOutlineColor, "_OutlineColor", _duration / 2f));
        }
    }
}


