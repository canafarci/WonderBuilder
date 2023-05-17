using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootSceneSwitch : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.HasKey(ConstantValues.CURRENT_SCENE))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt(ConstantValues.CURRENT_SCENE));
        }
        else
        {
            PlayerPrefs.GetInt(ConstantValues.CURRENT_SCENE, 1);
            SceneManager.LoadScene(1);
        }
    }
}
