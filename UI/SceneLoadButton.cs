using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadButton : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt(ConstantValues.CURRENT_SCENE, SceneManager.GetActiveScene().buildIndex);
    }
    public void OnButtonPressed(int index)
    {
        SceneLoader.Instance.LoadScene(index);
    }

}
