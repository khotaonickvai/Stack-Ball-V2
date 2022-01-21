using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider slider;
    private GameObject ball;
    private GameObject stacks;
    private int stacksCount;
    private void Awake()
    {
        ball = GameObject.FindWithTag("Ball");
        stacks = GameObject.FindWithTag("Stacks");
        stacksCount = stacks.GetComponent<Stacks>().GetListCount();
        slider.minValue = 0;
        slider.maxValue = stacksCount;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = slider.maxValue - ball.transform.position.y - 1;
    }
}
