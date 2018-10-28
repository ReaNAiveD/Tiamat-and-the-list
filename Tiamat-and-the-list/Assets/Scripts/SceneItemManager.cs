using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItemManager : MonoBehaviour {

    public string levelName;
    public string sceneName;
    public static SceneItemManager instance;
    public List<Interoperable> interoperables;
    public PlayerManager player;

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
        ArchiveManager.Init();
        Debug.Log("SceneItemManager.Start");
        ArchiveManager.LoadArchive(interoperables);
	}
	
	// Update is called once per frame
	void Update () {
        SetNearPlayer();
	}

    private void OnDestroy()
    {
        ArchiveManager.SaveArchive(interoperables);
    }
    

    void SetNearPlayer()
    {
        float radio = 2;
        Interoperable tempNearest = null;

        foreach (Interoperable interoperable in interoperables)
        {
            float distance = Mathf.Abs(player.transform.position.x - interoperable.transform.position.x);
            
            if (interoperable.floorLayer == player.floorLayer && interoperable.interoperable && 
                distance <= interoperable.detectDist && radio > (distance / interoperable.detectDist))
            {
                radio = distance / interoperable.detectDist;
                tempNearest = interoperable;
            }
        }
        foreach (Interoperable interoperable in interoperables)
        {
            if (interoperable == tempNearest)
            {
                if (!interoperable.NearPlayer)
                {
                    interoperable.ShowHint();
                    interoperable.NearPlayer = true;
                    Debug.Log("ShowHintAbout:" + interoperable);
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

    public static string GetLevelName()
    {
        return instance.levelName;
    }

    public static string GetSceneName()
    {
        return instance.sceneName;
    }
}
