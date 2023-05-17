using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] GameObject _baseWonder, _nightWonder, _fireworksObj, _NPCParent;

    public void OnGameOver()
    {
        RotateCamera camera = FindObjectOfType<RotateCamera>();
        camera.RemoveControl = true;
        camera.mouseX = 1f;
        _button.interactable = true;
        _baseWonder.SetActive(false);
        _nightWonder.SetActive(true);
        _fireworksObj.SetActive(true);
        FindObjectOfType<LightChanger>().DimLights();
        StartCoroutine(NextButtonLoop());
        Invoke("EndGameWorkerDisable", 3.0f);
    }

    IEnumerator NextButtonLoop()
    {
        Vector3 baseScale = _button.transform.localScale;
        while (true)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_button.transform.DOScale(baseScale * 1.08f, 0.25f));
            seq.Append(_button.transform.DOScale(baseScale, 0.75f));
            yield return seq.WaitForCompletion();
        }
    }
    void EndGameWorkerDisable ()
    {
        _NPCParent.SetActive(false);

    }

}
