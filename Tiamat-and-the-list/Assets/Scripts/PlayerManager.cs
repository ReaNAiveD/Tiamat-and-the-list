using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public List<float> maxX;
    public List<float> minX;


    //移动速度
    private int moveSpeed;
    //手电筒
    public GameObject torchPrefab;
    //判断是否使用道具
    bool itemOn = false;
    //当前道具
    private Equipment currentEquip=new Equipment();


    // Use this for initialization
    void Start () {
        InputManager.AddOnLeftMove(LeftMove);
        InputManager.AddOnRightMove(RightMove);
        InputManager.AddOnUpStair(UpMove);
        InputManager.AddOnDownStair(DownMove);
        InputManager.AddOnSwitchItemState(UseEquip);
        moveSpeed = 8;
        
    }
    //高度层，最低为0，向上递增，用于判断是否与道具在同一层从而判断是否可交互。
    public int floorLayer = 0;

	
	// Update is called once per frame
	void Update () {
		
	}
    void LeftMove()
    {
        float playerX = transform.localPosition.x;
        float bg_1_x = minX[floorLayer];
        if (playerX >= bg_1_x)
        {
            transform.Translate(Time.deltaTime * Vector3.left * moveSpeed, Space.World);

          
        }
        
    }

    void RightMove()
    {
        float playerX = transform.localPosition.x;
        float bg_1_x = maxX[floorLayer];
        if (playerX <= bg_1_x)
        {
            transform.Translate(Time.deltaTime * Vector3.right * moveSpeed, Space.World);
            
        }
        
    }

    void UpMove()
    {

    }

    void DownMove()
    {

    }

    public void setEquip(EquipmentType equipmentType)
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        itemOn = false;
        switch (equipmentType) {
            case EquipmentType.Torch:
                currentEquip.type = EquipmentType.Torch;
                torchPrefab = Instantiate(torchPrefab) as GameObject;
                torchPrefab.transform.position = new Vector3(transform.position.x + 3.1f, transform.position.y, transform.position.z);
                torchPrefab.transform.parent = transform;
            break;
        }
    }

    void UseEquip()
    {
        switch (currentEquip.type) {
            case EquipmentType.Torch:
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

    void turnOnTorch()
    {
        torchPrefab.GetComponent<Torch>().TurnOnTorch();
    }
    void turnOffTorch()
    {
        torchPrefab.GetComponent<Torch>().TurnOffTorch();
    }

    private PlayerSave CreateSavePlayer()
    {
        PlayerSave save = new PlayerSave();
        save.x = transform.position.x;
        save.y = transform.position.y;
        save.z = transform.position.z;
        save.floorLayer = floorLayer;

        save.currentEquipType = currentEquip.type;
        save.itemOn = itemOn;

        return save;
    }

    public void SavePlayer()
    {
        PlayerSave save = new PlayerSave();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath+"/player.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Player Saved");
    }
}
