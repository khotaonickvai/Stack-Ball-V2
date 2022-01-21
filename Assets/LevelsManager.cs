using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    private const int LevelsCount = 4;
    
    private int currentLevelIndex;
    private const string lv1 = "Map_1";
    private const string lv2 = "Map_1 3";
    private const string lv3 = "Map_1 2";
    private const string lv4 = "Map_1 1";

    private void Awake()
    {
        currentLevelIndex = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(GetLevelGameObjectPrefab(0),transform.position + Vector3.forward * 3,Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
    }
    public GameObject GetLevelGameObjectPrefab(int levelIndex)
    {
        switch (levelIndex)
        {
            case 0 :
                return Load(lv1);
            case 1 :
                return Load(lv2);
            case 2 :
                return Load(lv3);
            case 3 :
                return Load(lv4);
        }
        return null;
    }

    private GameObject Load(string path)
    {
        GameObject g;
        g = Resources.Load(path,typeof(GameObject)) as GameObject;
        if(g == null) Debug.Log("khong tim thay prefabs");
        return g;
    }
    
    public GameObject LoadNextLevel()
    {
        if (currentLevelIndex == LevelsCount - 1)
        {
            currentLevelIndex = 0;
            return GetLevelGameObjectPrefab(0);
        }

        currentLevelIndex += 1;
        Debug.Log(currentLevelIndex);
        return GetLevelGameObjectPrefab(currentLevelIndex);
    }

    public GameObject ReloadLevel()
    {
        return GetLevelGameObjectPrefab(currentLevelIndex);
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }
}
