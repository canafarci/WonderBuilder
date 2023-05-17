using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class WonderStage : MonoBehaviour
{
    public int TotalStageParts { get { return _constructionParts.Length; } }
    Wonder _wonder;
    int _stageLevel, _buildedIndex;
    [SerializeField] GameObject[] _constructionParts;
    [SerializeField] float _stepDifficulty = 1f;
    [SerializeField] float _reward = 20f;
    [SerializeField] bool _sortLastObject = true;
    [SerializeField] bool _tweenStar = false;
    [SerializeField] string _uniqueIdentifier;
    ProgressSlider _slider;

    private void Awake()
    {
        _wonder = GetComponentInParent<Wonder>();
        _slider = FindObjectOfType<ProgressSlider>();
        Init();
    }

    void Init()
    {

        if (_sortLastObject)
        {
            GameObject[] sortedObjects = _constructionParts.OrderBy(x => x.transform.position.y).ToArray();
            _constructionParts = sortedObjects;
        }
        else
        {
            GameObject lastObject = _constructionParts[_constructionParts.Length - 1];
            List<GameObject> objList = _constructionParts.ToList();
            objList.RemoveAt(_constructionParts.Length - 1);
            objList.OrderBy(x => Vector3.Distance(x.transform.position, Vector3.zero)).ToArray();
            objList.Add(lastObject);
            _constructionParts = objList.ToArray();
        }

        Load();
    }
    public (GameObject, WonderStage) GetNextBuildObject()
    {
        if (_stageLevel < _constructionParts.Length)
        {
            GameObject returnObj = _constructionParts[_stageLevel];
            _stageLevel++;
            return (returnObj, this);
        }
        else
        {
            if (_tweenStar)
                FindObjectOfType<StarTween>().OnLevelProgress();
            PlayerPrefs.SetInt(_uniqueIdentifier, _constructionParts.Length);

            return _wonder.StageComplete();
        }
    }
    public float GetBuildTime(float workerSpeed)
    {
        return _stepDifficulty / workerSpeed;
    }
    public float GetReward(int tier)
    {
        return _reward * (Mathf.Pow(5, tier));
    }
    public void Build(GameObject buildPart)
    {
        buildPart.SetActive(!buildPart.activeSelf);
        _buildedIndex++;
        Save();
        // Vector3 scale = buildPart.transform.localScale;
        _slider.OnPartBuilt();

        //TODO
        Renderer[] renderers = buildPart.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Sequence seq = DOTween.Sequence();
            Material mat = renderer.material;
            Color color = mat.color;
            seq.Append(mat.DOColor(Color.white, 0.01f));
            seq.AppendInterval(0.2f);
            seq.Append(mat.DOColor(color, 0.3f));
        }
    }
    protected void Load()
    {
        if (PlayerPrefs.HasKey(_uniqueIdentifier))
        {
            _buildedIndex = PlayerPrefs.GetInt(_uniqueIdentifier);
            _stageLevel = _buildedIndex;
            for (int i = 0; i < _buildedIndex; i++)
            {
                _constructionParts[i].SetActive(!_constructionParts[i].activeSelf);
                _slider.OnPartBuilt();
            }
        }
        else
        {
            PlayerPrefs.SetInt(_uniqueIdentifier, 0);
            _stageLevel = 0;
            _buildedIndex = 0;
        }
    }

    protected void Save()
    {
        PlayerPrefs.SetInt(_uniqueIdentifier, _buildedIndex);
    }
}
