using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ResourcePile : MonoBehaviour
{
    public PileType PileType;
    [SerializeField] Transform[] _pickupPoints;
    [SerializeField] GameObject[] _resourceObjects;
    int _destinationIndex, _pileIndex;
    Animator _craneAnimator;
    bool _isRestocking;
    private void Awake()
    {
        _craneAnimator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        GameObject[] sortedObjects = _resourceObjects.OrderBy(x => Vector3.Distance(x.transform.position, Vector3.up * (10 ^ 6))).ToArray();
        _resourceObjects = sortedObjects;
    }
    public Vector3 GetPickupPoint()
    {
        if (_destinationIndex >= _pickupPoints.Length - 2)
            _destinationIndex = 0;
        else
            _destinationIndex++;

        Vector3 point = _pickupPoints[_destinationIndex].position;
        return point;
    }

    public void TakeFromPile()
    {
        if (_pileIndex < _resourceObjects.Length)
        {
            _resourceObjects[_pileIndex].SetActive(false);
            _pileIndex++;
        }
        if (!_isRestocking && _pileIndex > (_resourceObjects.Length / 2))
        {
            _craneAnimator.Play("NewVincAnim", 0, 0f);
            _isRestocking = true;
        }
    }
    public void RestockPile()
    {
        float timestep = 1f / _resourceObjects.Length;
        for (int i = _resourceObjects.Length - 1; i >= 0; i--)
        {
            Transform tr = _resourceObjects[i].transform;
            if (tr.gameObject.activeSelf)
                continue;
            else
                tr.gameObject.SetActive(true);

            Vector3 pos = tr.position;
            tr.position = pos + Vector3.up / 4f;
            Sequence seq = DOTween.Sequence();
            seq.Append(tr.DOMoveY(pos.y, 0.2f));
            seq.PrependInterval(i * timestep);
        }
        _pileIndex = 0;
        _isRestocking = false;
    }
}
