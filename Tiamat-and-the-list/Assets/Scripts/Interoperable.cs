﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interoperable : MonoBehaviour {
    //index，用于加载存档时，存档项和物品的匹配。
    private int index = 0;
    public int Index { get; set; }
    //是否是由于其他原因后续生成的而非初始布置在场景中
    [HideInInspector]
    public bool generated = false;

    //是否可与玩家交互（机关，魔法阵等范围强制触发认为不可交互）
    public bool interoperable = true;

    //判定在主角附近的检测范围，交错按比例计算（范围强制触发即使交错仍是全范围的）
    public float detectDist = 2;

    //高度层，最低为0，向上递增，用于判断是否与主角在同一层从而判断是否可交互。
    public List<int> floorLayer;

    //是否在主角附近
    private bool nearPlayer = false;
    public bool NearPlayer { get; set; }

	public virtual string GetArchive()
    {
        return null;
    }

    public virtual void LoadArchive(string archiveLine)
    {
        
    }

    //显示提示交互，例如浮现一个UI：标有F什么的
    public virtual void ShowHint()
    {
        
    }

    //提示交互消失
    public virtual void UnshowHint()
    {

    }

    //在检测范围内强制每帧触发
    public virtual void WithinRange()
    {

    }

}
