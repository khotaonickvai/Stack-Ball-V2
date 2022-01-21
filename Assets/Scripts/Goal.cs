using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private GameObject ball;

    private void Awake()
    {

        ball = GameObject.FindWithTag("Ball");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (ball.transform.position.y <= 0)
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
        }*/
    }
}
