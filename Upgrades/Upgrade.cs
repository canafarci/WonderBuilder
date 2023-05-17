using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UpgradeText))]
public class Upgrade : MonoBehaviour
{
    [SerializeField] protected string _identifier;
    [SerializeField] Sprite _disabledSprite, _enabledSprite;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] protected Image _buttonImage;
    protected int _level, _moneyToUpgrade;
    protected UpgradeText _text;
    protected UpgradeManager _manager;
    protected bool _canClick, _maxedOut = false;
    Color _blue = new Color(0 / 255f, 158f / 255f, 249f / 255f, 1f);
    Vector3 _startScale;

    virtual protected void Awake()
    {
        _manager = FindObjectOfType<UpgradeManager>();
        _text = GetComponent<UpgradeText>();
        _startScale = transform.localScale;
    }
    virtual protected void OnEnable()
    {
        Resource.Instance.MoneyChangeHandler += CheckCanClick;
    }
    virtual protected void OnDisable()
    {
        Resource.Instance.MoneyChangeHandler -= CheckCanClick;
    }
    protected void Start()
    {
        LoadProgress();
        CheckCanClick(Resource.Instance.Money);
    }
    protected void SaveProgress(int level) => PlayerPrefs.SetInt(_identifier, level);
    protected virtual void LoadProgress()
    {
        if (!PlayerPrefs.HasKey(_identifier))
        {
            PlayerPrefs.SetInt(_identifier, 1);
        }

        _level = PlayerPrefs.GetInt(_identifier);
        SetLevelValues(_level);
    }
    virtual public void OnUpgradeClicked()
    {
        Resource resources = Resource.Instance;
        float playerMoney = resources.Money;
        if (playerMoney < _moneyToUpgrade) { return; }

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(_startScale * 0.9f, 0.1f));
        seq.Append(transform.DOScale(_startScale, 0.4f));

        Sequence seq2 = DOTween.Sequence();

        resources.OnMoneyChange(-1f * _moneyToUpgrade);
        _level++;
        SetLevelValues(_level);
        SaveProgress(_level);
    }
    virtual protected void SetLevelValues(int level)
    {
        _levelText.text = "Level " + (_level + 1).ToString();
        _manager.UpdateText();
    }

    protected void OnMaxLevel()
    {
        _text.SetMax();
        DisableButton();
        _canClick = false;
        _maxedOut = true;
    }

    protected virtual void DisableButton()
    {
        _buttonImage.sprite = _disabledSprite;
        _buttonImage.DOColor(Color.white, 0.001f);
        _canClick = false;
    }
    protected virtual void EnableButton()
    {
        if (_maxedOut) return;
        _buttonImage.sprite = _enabledSprite;
        _buttonImage.DOColor(_blue, 0.001f);
        _canClick = true;
    }

    virtual protected bool CheckCanClick(float currentMoney)
    {
        if (_maxedOut) return false;

        if (currentMoney < _moneyToUpgrade)
        {
            DisableButton();
            return false;
        }
        else
        {
            EnableButton();
            return true;
        }
    }


}
