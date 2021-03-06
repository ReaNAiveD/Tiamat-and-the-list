﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneItemManager : MonoBehaviour {

    public string levelName;
    public string sceneName;
    public static SceneItemManager instance;
    public List<Interoperable> interoperables;
    public PlayerManager player;
    public RectTransform equipmentUI;
    [HideInInspector]
    public bool paused = false;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        //为每个场景中的原生物品生成index
        for (int i = 0; i < interoperables.Count; i++)
        {
            interoperables[i].Index = i;
        }
        //加载存档
        ArchiveManager.LoadArchive(interoperables);
        InputManager.AddOnEscape(OnEscape);
	}
	
	// Update is called once per frame
	void Update () {
        SetNearPlayer();
        TriggerRangeCompel();
	}
    
    private void OnApplicationQuit()
    {
        ArchiveManager.SaveArchive(interoperables);
    }
    public static void SaveArchive()
    {
        ArchiveManager.SaveArchive(instance.interoperables);
    }


    void SetNearPlayer()
    {
        float radio = 2;
        Interoperable tempNearest = null;

        //选出可触发的距离主角最近的可交互物体
        foreach (Interoperable interoperable in interoperables)
        {
            float distance = Mathf.Abs(player.transform.position.x - interoperable.transform.position.x);
            
            if (interoperable.floorLayer.Contains(player.floorLayer) && interoperable.interoperable && 
                distance <= interoperable.detectDist && radio > (distance / interoperable.detectDist))
            {
                radio = distance / interoperable.detectDist;
                tempNearest = interoperable;
            }
        }
        //显示提示或是关闭提示
        foreach (Interoperable interoperable in interoperables)
        {
            if (interoperable == tempNearest)
            {
                if (!interoperable.NearPlayer)
                {
                    interoperable.ShowHint();
                    interoperable.NearPlayer = true;
                }
            }
            else
            {
                if (interoperable.NearPlayer)
                {
                    interoperable.UnshowHint();
                    interoperable.NearPlayer = false;
                }
            }
        }
    }

    //触发范围内强制触发机关
    void TriggerRangeCompel()
    {
        foreach (Interoperable interoperable in interoperables)
        {
            float distance = Mathf.Abs(player.transform.position.x - interoperable.transform.position.x);

            if (interoperable.floorLayer.Contains(player.floorLayer) && distance <= interoperable.detectDist)
            {
                interoperable.WithinRange();
            }

        }
    }

    void OnEscape()
    {
        Debug.Log("OnEscape-SceneItemManager");
        if (!paused)
        {
            SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
            GameObject dialogBox = GameObject.FindGameObjectWithTag("DialogBox");
            if (dialogBox != null){
                foreach (var text in dialogBox.transform.GetComponentsInChildren<Text>()){
                    Color c = text.color;
                    text.color = new Color(c.r, c.g, c.b, 0);
                }
                foreach (var image in dialogBox.transform.GetComponentsInChildren<Image>()){
                    Color c = image.color;
                    image.color = new Color(c.r, c.g, c.b, 0);
                }
            }
            Pause();
            Debug.Log("Pause");
            equipmentUI.anchoredPosition = new Vector3(-1000.0f, 0.0f, 0.0f);
        }
        else
        {
            equipmentUI.anchoredPosition = Vector3.zero;
        }
    }

    void Pause()
    {
        Time.timeScale = 0.0f;
        paused = true;
        InputManager.gamePaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        paused = false;
        InputManager.gamePaused = false;
    }

    public static string GetLevelName()
    {
        return instance.levelName;
    }

    public static string GetSceneName()
    {
        return instance.sceneName;
    }
}
