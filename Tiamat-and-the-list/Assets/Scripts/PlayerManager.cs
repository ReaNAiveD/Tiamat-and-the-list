﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;
using SimpleJSON;

public class PlayerManager : MonoBehaviour {

    public delegate void NoneParaFunc();

    public List<float> maxX;
    public List<float> minX;
    //剧本，作用是获得进入关卡后初始位置
    public Scenario scenario;

    //移动速度
    public float moveSpeed = 8.0f;
    //手电筒
    public GameObject torchPrefab;
    //技能分身
    public GameObject SkillPrefab;
    //判断是否使用道具
    [HideInInspector]
    public bool itemOn = false;
    //当前道具
    [HideInInspector]
    public EquipmentType currentEquipType = EquipmentType.None;

    //高度层，最低为0，向上递增，用于判断是否与道具在同一层从而判断是否可交互。
    public int floorLayer = 0;

    //当前移动速度
    private float currentSpeed = 0f;
    private float lastPositionX;

    //音效控制器
    public AudioClip audioTorchSwitch;
    private AudioSource audioSource;

    //角色动画控制器
    public Animator playerAnima;

    [HideInInspector]
    public bool isLeft = false;
    private bool canMove = true;
    // Use this for initialization
    void Start () {
        InputManager.AddOnLeftMove(LeftMove);
        InputManager.AddOnRightMove(RightMove);
        InputManager.AddOnSwitchItemState(UseEquip);
        InputManager.AddOnSkill(UseSkill);
        lastPositionX = this.transform.position.x;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioTorchSwitch;
    }

	
	// Update is called once per frame
	void Update () {
        float currentPositionX = this.transform.position.x;
        currentSpeed = Math.Abs(currentPositionX - lastPositionX) / Time.deltaTime;
        lastPositionX = currentPositionX;
        playerAnima.SetFloat("MoveSpeed", currentSpeed);
    }

    void LeftMove()
    {
        if (canMove)
        {
            float playerX = transform.position.x;
            float bg_1_x = minX[floorLayer];
            //transform.LookAt(new Vector3(transform.position.x-5,transform.position.y,transform.position.z));
            if (!isLeft)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                isLeft = true;
            }
            if (playerX >= bg_1_x)
            {
                transform.Translate(Time.deltaTime * Vector3.left * moveSpeed, Space.World);
            }
        }
        
    }

    void RightMove()
    {
        if (canMove)
        {
            float playerX = transform.position.x;
            float bg_1_x = maxX[floorLayer];
            //transform.LookAt(new Vector3(transform.position.x + 5, transform.position.y, transform.position.z));
            if (isLeft)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                isLeft = false;
            }
            if (playerX <= bg_1_x)
            {
                transform.Translate(Time.deltaTime * Vector3.right * moveSpeed, Space.World);
            }
        }
    }

    public void SetLeft(bool isLeft)
    {
        if (isLeft)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            this.isLeft = true;
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            this.isLeft = false;
        }
    }

    public void setEquip(EquipmentType equipmentType)
    {
        if (canMove)
        {
            var existedTorch = transform.Find("Torch(Clone)");
            if (existedTorch != null)
            {
                Destroy(existedTorch.gameObject);
            }
            itemOn = true;
            switch (equipmentType)
            {
                case EquipmentType.FlashLight:
                    currentEquipType = EquipmentType.FlashLight;
                    GameObject torch = Instantiate(torchPrefab) as GameObject;
                    if (!isLeft)
                        torch.transform.position = new Vector3(transform.position.x + 3.1f, transform.position.y, transform.position.z);
                    else
                        torch.transform.position = new Vector3(transform.position.x - 3.1f, transform.position.y, transform.position.z);
                    torch.transform.parent = transform;
                    if (itemOn)
                    {
                        turnOffTorch();
                        itemOn = false;
                    }
                    else
                    {
                        turnOnTorch();
                        itemOn = true;
                    }
                    break;
            }
        }
    }

    void UseEquip()
    {
        if (canMove)
        {
            switch (currentEquipType)
            {
                case EquipmentType.FlashLight:
                    audioSource.Play();
                    if (itemOn)
                    {
                        turnOffTorch();
                        itemOn = false;
                    }
                    else
                    {
                        turnOnTorch();
                        itemOn = true;
                    }
                    break;
            }
        }
    }

    void turnOnTorch()
    {
        transform.Find("Torch(Clone)").GetComponent<FlashLightEquipment>().TurnOnTorch();
    }
    void turnOffTorch()
    {
        transform.Find("Torch(Clone)").GetComponent<FlashLightEquipment>().TurnOffTorch();
    }

    void UseSkill()
    {
        if (canMove == true)
        {
            canMove = false;
            GameObject SkillCharacter = Instantiate(SkillPrefab) as GameObject;
            SkillCharacter.transform.localScale = transform.localScale;
            if (!isLeft)
                SkillCharacter.transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
            else
                SkillCharacter.transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
            SkillCharacter.GetComponent<SkillManager>().maxX = maxX[floorLayer];
            SkillCharacter.GetComponent<SkillManager>().minX = minX[floorLayer];
            SkillCharacter.GetComponent<SkillManager>().isLeft =isLeft;
        }
        else
        {
            canMove = true;
            var existedSkill = transform.Find("SkillCharacter(Clone)");
            transform.position=existedSkill.transform.position;
            transform.localScale = existedSkill.transform.localScale;
            isLeft = existedSkill.GetComponent<SkillManager>().isLeft;
            if (existedSkill != null)
            {
                Destroy(existedSkill.gameObject);
            }
        }
    }


    public void LoadArchive(string archiveLine)
    {
        var root = JSON.Parse(archiveLine);
        transform.position = new Vector3(root["position"][0].AsFloat, root["position"][1].AsFloat, root["position"][2].AsFloat);
        transform.localScale = new Vector3(root["scale"][0].AsFloat, root["scale"][1].AsFloat, root["scale"][2].AsFloat);
        floorLayer = root["floorLayer"].AsInt;
        setEquip((EquipmentType)Enum.Parse(typeof(EquipmentType), root["currentEquipType"]));
        itemOn = root["itemOn"].AsBool;
        isLeft = root["isLeft"].AsBool;

        switch (currentEquipType)
        {
            case EquipmentType.FlashLight:
                UIManager.SetEquipmentIcon("EquipmentSprite\\Stage00_shoudiantong");
                break;
        }

        string lastSceneName = PlayerPrefs.GetString("LastSceneName");
        if (lastSceneName != SceneItemManager.GetLevelName() + "-" + SceneItemManager.GetSceneName())
        {
            transform.position = scenario.GetPlayerInitPos(lastSceneName);
        }

        canMove = root["canMove"].AsBool;

        if (!canMove)
        {
            GameObject skill = Instantiate(SkillPrefab) as GameObject;
            skill.transform.localScale = new Vector3(root["skillScale"][0].AsFloat, root["skillScale"][1].AsFloat, root["skillScale"][2].AsFloat);
            skill.transform.position = new Vector3(root["skillPosition"][0].AsFloat, root["skillPosition"][1].AsFloat, root["skillPosition"][2].AsFloat);
            skill.GetComponent<SkillManager>().isLeft = root["skillIsLeft"].AsBool;
        }
    }

    public string SaveArchive()
    {
        var pos = new JSONArray()
        {
            { new JSONData(transform.position.x) },
            { new JSONData(transform.position.y) },
            { new JSONData(transform.position.z) }
        };
        var scale = new JSONArray()
        {
            { new JSONData(transform.localScale.x) },
            { new JSONData(transform.localScale.y) },
            { new JSONData(transform.localScale.z) }
        };
        if (canMove)
        {
            JSONClass root = new JSONClass()
            {
                { "canMove",new JSONData(canMove) },
                { "position", pos },
                { "scale",scale },
                { "floorLayer", new JSONData(floorLayer) },
                { "currentEquipType", new JSONData(currentEquipType.ToString()) },
                { "itemOn", new JSONData(itemOn) },
                { "isLeft",new JSONData(isLeft) }
            };
            return root.ToString();
        }
        else
        {
            var skillPos = new JSONArray()
            {
                { new JSONData(getSkillTransform().position.x) },
                { new JSONData(getSkillTransform().position.y) },
                { new JSONData(getSkillTransform().position.z) }
            };
            var skillScale = new JSONArray()
            {
                { new JSONData(getSkillTransform().localScale.x) },
                { new JSONData(getSkillTransform().localScale.y) },
                { new JSONData(getSkillTransform().localScale.z) }
            };
            var existedSkill = transform.Find("SkillCharacter(Clone)");
            JSONClass root = new JSONClass()
            {
                { "canMove",new JSONData(canMove) },
                {"skillPosition",skillPos },
                { "skillScale",skillScale },
                { "position", pos },
                { "scale",scale },
                { "floorLayer", new JSONData(floorLayer) },
                { "currentEquipType", new JSONData(currentEquipType.ToString()) },
                { "itemOn", new JSONData(itemOn) },
                { "isLeft",new JSONData(isLeft) },
                { "skillIsLeft",new JSONData(existedSkill.GetComponent<SkillManager>().isLeft) }
            };
            return root.ToString();
        }

    }

    public bool getCanMoved()
    {
        return canMove;
    }

    public Transform getSkillTransform()
    {
        if (transform.Find("SkillCharacter(Clone)") != null)
        {
            return transform.Find("SkillCharacter(Clone)").transform;
        }
        return null;
    }

    public IEnumerator MoveTo(Vector2 target)
    {
        if (target.x > transform.position.x)
        {
            SetLeft(false);
            float offsetX = target.x - transform.position.x;
            float offsetY = target.y - transform.position.y;
            while (target.x > transform.position.x)
            {
                RightMove();
                float deltaX = moveSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x,
                    deltaX / offsetX * offsetY + transform.position.y);
                yield return null;
            }
            transform.position = target;
        }
        else
        {
            SetLeft(true);
            float offsetX = transform.position.x - target.x;
            float offsetY = target.y - transform.position.y;
            while (target.x < transform.position.x)
            {
                LeftMove();
                float deltaX = moveSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x,
                    deltaX / offsetX * offsetY + transform.position.y);
                yield return null;
            }
            transform.position = target;
        }
    }

    public IEnumerator MoveTo(Vector2 target, NoneParaFunc noneParaFunc)
    {
        if (target.x > transform.position.x)
        {
            float offsetX = target.x - transform.position.x;
            float offsetY = target.y - transform.position.y;
            while (target.x > transform.position.x)
            {
                RightMove();
                float deltaX = moveSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, 
                    deltaX / offsetX * offsetY + transform.position.y);
                yield return null;
            }
            transform.position = target;
        }
        else
        {
            float offsetX = transform.position.x - target.x;
            float offsetY = target.y - transform.position.y;
            while (target.x < transform.position.x)
            {
                LeftMove();
                float deltaX = moveSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x,
                    deltaX / offsetX * offsetY + transform.position.y);
                yield return null;
            }
            transform.position = target;
        }
        noneParaFunc();
    }
}
