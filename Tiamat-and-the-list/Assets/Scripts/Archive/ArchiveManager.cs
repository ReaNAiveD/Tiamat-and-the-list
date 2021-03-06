﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

/*
 * 存档管理器，保存每个可交互道具的状态，具体道具存档格式由道具自己维护
 * 目前只存了道具，之后人物和持有道具形式出来后将修改。
 * 心态崩了，之后一定重构
 */

public class ArchiveManager : MonoBehaviour {

    private static ArchiveManager instance;

    public PlayerManager player;
    public Scenario scenario;
    //存档文件名（不含.json）
    public string archiveName;
    //存档关卡标志（关卡名）
    private string levelTag;
    //存档场景标志（场景名）
    private string sceneTag;
    private string filePath;

    private void Awake()
    {
        instance = this;
        levelTag = SceneItemManager.GetLevelName();
        sceneTag = SceneItemManager.GetSceneName();
        filePath = Application.persistentDataPath + "\\" + archiveName + ".json";
    }

    public static void LoadArchive(List<Interoperable> interoperables) { instance._LoadArchive(interoperables); }
    //存档加载，参数为场景内所有初始布局的列表，根据存档修正场景布局，生成额外物品。
    private void _LoadArchive(List<Interoperable> interoperables)
    {
        Debug.Log("_LoadArchive");
        try
        {
            if (File.Exists(filePath))
            {
                StreamReader streamReader = new StreamReader(filePath, System.Text.Encoding.UTF8);
                JSONNode root = JSON.Parse(streamReader.ReadToEnd());
                streamReader.Close();
                //加载场景物件
                if (root != null)
                {
                    //加载角色信息
                    var playerNode = root["Player"];
                    player.LoadArchive(playerNode["archive"]);

                    JSONNode sceneNode = root[levelTag][sceneTag];


                    foreach (JSONNode itemNode in sceneNode["Items"].Childs)
                    {
                        int index = itemNode["index"].AsInt;
                        string archiveLine = itemNode["archive"];
                        //正常物品则令其加载存档
                        if (index != -1 && index < interoperables.Count)
                        {
                            interoperables[index].LoadArchive(archiveLine);
                        }
                        //游戏中生成物品额外生成，然后加载存档
                        else if (index == -1)
                        {
                            string resourcesPath = itemNode["resources-path"];
                            GameObject pickableItem = Object.Instantiate(Resources.Load(resourcesPath)) as GameObject;
                            pickableItem.GetComponent<Interoperable>().LoadArchive(archiveLine);
                            pickableItem.GetComponent<Interoperable>().generated = true;
                        }
                    }

                    JSONNode scenarioNode = sceneNode["Scenario"];
                    if (scenarioNode != null)
                    {
                        Debug.Log("scenarioNode !=null");
                        scenario.LoadArchive(scenarioNode["archive"]);
                    }

                }
            }
        }
        catch (IOException e)
        {
            Debug.Log("Archive Doesn't Exist: " + e.HelpLink);
        }
    }


    public static void SaveArchive(List<Interoperable> objectsToSave) { instance._SaveArchive(objectsToSave); }
    //保存存档
    private void _SaveArchive(List<Interoperable> objectsToSave)
    {
        var playerNode = new JSONClass()
        {
            { "archive", new JSONData(player.SaveArchive()) }
        };

        var sceneArchive = new JSONClass();
        var sceneItemArchive = new JSONArray();
        sceneArchive.Add("Items", sceneItemArchive);
        //存储每一个物件的存档
        for (int i = 0; i < objectsToSave.Count; i++)
        {
            Interoperable interoperable = objectsToSave[i];
            if (!interoperable.generated)
            {
                if (interoperable.GetArchive() != null)
                {
                    var archivePiece = new JSONClass
                    {
                        { "index", new JSONData(interoperable.Index) },
                        { "archive", new JSONData(interoperable.GetArchive()) }
                    };
                    sceneItemArchive.Add(archivePiece);
                }
                else
                {
                    var archivePiece = new JSONClass
                    {
                        { "index", new JSONData(interoperable.Index) },
                        { "archive", new JSONData("") }
                    };
                    sceneItemArchive.Add(archivePiece);
                }
            }
            //后续生成的物件额外存储
            else
            {
                Pickable pickable = interoperable as Pickable;
                if (pickable != null)
                {
                    var archivePiece = new JSONClass
                    {
                        { "index", new JSONData(-1) },
                        { "resources-path", new JSONData(pickable.resourcesPath) },
                        { "archive", new JSONData(interoperable.GetArchive()) }
                    };
                    sceneItemArchive.Add(archivePiece);
                }
            }
        }

        //存储剧本进度
        var scenarioNode = new JSONClass()
        {
            { "archive", new JSONData(scenario.GetArchive()) }
        };
        sceneArchive.Add("Scenario", scenarioNode);

        //更新节点信息
        if (PlayerPrefs.GetInt("HasArchive", 0) == 1)
        {
            StreamReader streamReader = new StreamReader(filePath, System.Text.Encoding.UTF8);
            JSONNode root = JSON.Parse(streamReader.ReadToEnd());
            if (root != null)
            {
                root.Remove("Player");
                root.Add("Player", playerNode);
                JSONNode levelNode = root[levelTag];
                if (levelNode != null)
                {
                    levelNode.Remove(sceneTag);
                }
                else
                {
                    levelNode = new JSONClass();
                    root.Add(levelTag, levelNode);
                }
                levelNode.Add(sceneTag, sceneArchive);
            }
            else
            {
                root = new JSONClass();
                root.Add("Player", playerNode);
                JSONClass levelNode = new JSONClass();
                levelNode.Add(sceneTag, sceneArchive);
                root.Add(levelTag, levelNode);
            }
            streamReader.Close();

            StreamWriter streamWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            streamWriter.WriteLine(root.ToString());
            streamWriter.Flush();
            streamWriter.Close();
        }
        else
        {
            JSONClass root = new JSONClass();
            root.Add("Player", playerNode);
            JSONClass levelNode = new JSONClass();
            levelNode.Add(sceneTag, sceneArchive);
            root.Add(levelTag, levelNode);
            
            StreamWriter streamWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            streamWriter.WriteLine(root.ToString());
            streamWriter.Flush();
            streamWriter.Close();
        }

        PlayerPrefs.SetInt("HasArchive", 1);
        PlayerPrefs.SetString("LastSceneName", levelTag + "-" + sceneTag);
        PlayerPrefs.Save();

        CollectionArchive.SaveArchive();
    }

    public static void ClearNormalArchive() { instance._ClearNormalArchive(); }
    private void _ClearNormalArchive()
    {
        File.Delete(filePath);
        PlayerPrefs.SetInt("HasArchive", 0);
    }

}
