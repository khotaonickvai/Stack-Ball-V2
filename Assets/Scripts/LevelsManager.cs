using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    private const int LevelsCount = 18;

    private int currentLevelIndex;
    private readonly string[] levelStrings =
    {
        "Map_1","Map_1 3","Map_1 2","Map_1 1","Map_3","Map_3 1","Map_3 2",
        "Map_4", "Map_4 1","Map_4 2","Map_5","Map_5 1","Map_5 2",
        "Map_6","Map_6 1","Map_7","Map_7 1","Map_8", "Map_9","Map_10"
    };
    private void Awake()
    {
        currentLevelIndex = 0;
    }
    public GameObject GetLevelGameObjectPrefab(int levelIndex)
    {
        /*switch (levelIndex)
        {
            case 0 :
                return Load(lv1);
            case 1 :
                return Load(lv2);
            case 2 :
                return Load(lv3);
            case 3 :
                return Load(lv4);
        }*/
        return Load(levelStrings[levelIndex]);
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
        return GetLevelGameObjectPrefab(currentLevelIndex);
    }

    public GameObject ReloadLevel()
    {
        return GetLevelGameObjectPrefab(currentLevelIndex);
    }

    public int GetCurrentLevelIndex()
    {

        try
        {
            currentLevelIndex = PlayerPrefs.GetInt("Current Level", currentLevelIndex);
            return currentLevelIndex;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return 0;
        }
    }

    public void SaveCurrentLevelIndex()
    {
        PlayerPrefs.SetInt("Current Level",currentLevelIndex);
    }
    
}
