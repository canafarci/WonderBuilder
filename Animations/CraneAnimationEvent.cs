using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneAnimationEvent : MonoBehaviour
{
    ResourcePile _pile;
    [SerializeField] Animator _animator;
    private void Awake()
    {
        _pile = GetComponentInParent<ResourcePile>();
    }
    public void Restock()
    {
        _pile.RestockPile();
    }
}
