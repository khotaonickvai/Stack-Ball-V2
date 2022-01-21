using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stacks : MonoBehaviour
{
    private const float omega = 100f;
    [SerializeField] List<GameObject> stackList;
    // Start is called before the first frame update
    private void Awake()
    {
        InEditmode();
    }
    void Update()
    {
        transform.Rotate(0,omega*Time.deltaTime,0);
    }
    void InEditmode()
    {
        for(int i = 0; i < stackList.Count; i ++ )
        {
            var item = stackList[i];
            item.transform.position = new Vector3(0, i, 0);
            var phi = item.transform.localEulerAngles;
            item.transform.localEulerAngles = 
                new Vector3(phi.x,
                    phi.y + item.transform.position.y * omega / Ball.speed
                    ,phi.z);
        }
    }

    public int GetListCount()
    {
        return stackList.Count;
    }
}
