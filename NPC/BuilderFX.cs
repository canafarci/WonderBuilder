using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BuilderFX : MonoBehaviour
{
    [SerializeField] ParticleSystem _mergeFX, _unloadFX;
    public GameObject[] _buildMaterials;
    GameObject _slider;
    Animator _animator;
    TextMeshProUGUI _text;
    Vector3 _textStartPos, _canvasStartPos;
    Canvas _canvas;
    private void Awake()
    {
        _animator = GetComponentsInChildren<Animator>(true).Where(x => x.gameObject.name == "Slider").First();
        _slider = _animator.gameObject;
        _text = GetComponentInChildren<TextMeshProUGUI>(true);
        _canvas = GetComponentInChildren<Canvas>(true);
        _textStartPos = _text.transform.localPosition;
        _canvasStartPos = _canvas.transform.localPosition;
    }
    private void Start()
    {
        BuilderFX.ChangedAnimationBlend(_animator, .5f, 0.1f);
    }
    public void PlayMergeFX()
    {
        _mergeFX.transform.parent = null;
        _mergeFX.Play();
        Destroy(_mergeFX.gameObject, 1.5f);
    }
    public void PlayUnloadFX()
    {
        _unloadFX.transform.parent = null;
        _unloadFX.Play();
        Invoke("DelayedReparentUnloadFX", 1f);
    }

    void DelayedReparentUnloadFX()
    {
        _unloadFX.transform.parent = this.transform;
        _unloadFX.transform.localPosition = Vector3.zero;
    }
    public void EnableBuildObject(PileType pileType)
    {
        switch (pileType)
        {
            case (PileType.Brick):
                _buildMaterials[0].SetActive(true);
                break;
            case (PileType.Steel):
                _buildMaterials[1].SetActive(true);
                break;
            default:
                break;
        }
    }
    public void DisableBuildObject()
    {
        _buildMaterials[0].SetActive(false);
        _buildMaterials[1].SetActive(false);
    }
    public void FillSlider(float time)
    {
        StartCoroutine(SliderCoroutine(time));
    }
    IEnumerator SliderCoroutine(float time)
    {
        _slider.SetActive(true);
        _animator.speed = 1f / time;
        _animator.Play("SliderAnim2", 0, 0f);
        yield return new WaitForSeconds(time);
        _slider.SetActive(false);
    }
    public void MoneyGainFX(float money)
    {
        // _canvas.transform.parent = null; 
        _text.gameObject.SetActive(true);
        _text.transform.localScale = Vector3.one;
        _text.DOFade(1f, 0.01f);
        if (money == (int)money)
            _text.text = "$" + money.ToString("F0");
        else
            _text.text = "$" + money.ToString("F1");

        _text.transform.DOLocalMoveY(_textStartPos.y + 1f, 0.5f).onComplete = () =>
        {
            _text.transform.DOScale(0.1f, 0.5f);
            _text.DOFade(0f, 0.5f).onComplete = () =>
            {
                _canvas.transform.localPosition = _canvasStartPos;
                _text.transform.localPosition = _textStartPos;
            };

        };
    }
    public void FaceObject(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 50f);
    }
    static public Tween ChangedAnimationBlend(Animator animator, float end, float time)
    {
        float blend = animator.GetFloat("Blend");
        Tween tween = DOTween.To(() => blend, x => blend = x, end, .3f);
        tween.onUpdate = () => animator.SetFloat("Blend", blend);
        return tween;
    }
}
