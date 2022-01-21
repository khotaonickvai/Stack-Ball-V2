using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToPlay : MonoBehaviour
{
    private Button button;

    private Ball ball;
    // Start is called before the first frame update
    private void Awake()
    {
        /*ball = GameObject.FindWithTag("Ball").GetComponent<Ball>();
        button = GetComponent<Button>();
        button.onClick.AddListener(
            delegate
            {
                ball.PlayGame();
            });*/
    }
}
