using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelText : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Level: " + (SceneManager.GetActiveScene().buildIndex + 1);
    }
}
