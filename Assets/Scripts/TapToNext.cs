using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TapToNext : MonoBehaviour
{
    // Start is called before the first frame update
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(delegate
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            if (index == SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(index + 1);
            }
        });
    }
}
