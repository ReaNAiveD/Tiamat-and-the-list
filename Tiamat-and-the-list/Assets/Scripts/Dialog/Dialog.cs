﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dialog
{
    // example
    // 23|Scence1|Apkal|再见了，#叔叔。$|Apkal_serious|0|clap
    // #代表停顿，$代表有音效
    public struct branch                    // 对分支进行定义
    {
        public int switch_id;               // 跳转对话id号
        public string text;                 // 分支选项
    }
    public int id;                   // Dialog的唯一指定id
    public string section;           // 当前对话所处section
    public string characterName;     // 角色名
    public string text;              // 对话内容
    public string imagePath;         // 加载立绘文件路径
    public int branchNum;            // 分支数
    public List<branch> branches;
    public List<string> sounds;      // 音效组
}