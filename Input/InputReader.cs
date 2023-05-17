using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputReader : MonoBehaviour
{
    public event Action ClickHandler;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            ClickHandler?.Invoke();
    }
}
