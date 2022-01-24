using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stacks : MonoBehaviour
{
    private const float omega = 100f;
    [SerializeField] List<GameObject> stackList;
    [SerializeField] private Color _color;
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
        
            _color = GetRandomColor();
        
        for(int i = 0; i < stackList.Count; i ++ )
        {
           
            var item = stackList[i];
            foreach (var factor in item.GetComponentsInChildren<Factor>())
            {
                if (factor.IsDestroyable())
                {
                    factor.gameObject.GetComponentInChildren<Renderer>().material.color = _color;
                }
            }
            
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

    private Color GetRandomColor()
    {
        int r = Random.Range(0, 5);
        switch (r)
        {
            case 0 : return Color.yellow;
            case 1 : return Color.cyan;
            case 2 : return Color.green;
            case 3 : return Color.blue;
            case 4 : return Color.magenta;
        }
        return Color.black;
    }
}
