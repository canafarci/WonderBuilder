using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class StarTween : MonoBehaviour
{
    [SerializeField] Transform _target, _child;
    [SerializeField] GameObject[] _stars;
    Vector3 _startPos;
    int _index = 0;
    private void Start()
    {
        _startPos = transform.position;
        Load();
    }
    public void OnLevelProgress()
    {
        print("called");
        _child.gameObject.SetActive(true);

        transform.DOMove(_target.transform.position, 1.5f).SetEase(Ease.InSine);

        Sequence seq = DOTween.Sequence();
        int numOfMoves = UnityEngine.Random.Range(2, 5);
        float interval = 1.5f / (float)(numOfMoves + 1);

        for (int i = 0; i < numOfMoves; i++)
            seq.Append(_child.DOLocalMoveX(UnityEngine.Random.Range(100f, 200f) * Mathf.Pow(-1, i), interval).SetEase(Ease.InSine));

        seq.Append(_child.DOLocalMoveX(0, interval));

        seq.onComplete = () =>
        {
            _child.gameObject.SetActive(false);
            transform.position = _startPos;
            _stars[_index].SetActive(true);
            Save();
        };
    }
    void Save()
    {
        _index++;
        string scene = SceneManager.GetActiveScene().buildIndex.ToString();
        PlayerPrefs.SetInt(scene + "STAR", _index);
    }

    void Load()
    {
        string scene = SceneManager.GetActiveScene().buildIndex.ToString();
        if (!PlayerPrefs.HasKey(scene + "STAR"))
        {
            _stars[0].SetActive(true);
        }
        else
        {
            int index = PlayerPrefs.GetInt(scene + "STAR");
            for (int i = 0; i < index; i++)
            {
                _stars[i].SetActive(true);
            }
            _index = index;
        }
    }

}
