using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerSave
{
    //坐标
    public float x;
    public float y;
    public float z;
    public int floorLayer;

    //状态
    public EquipmentType currentEquipType;
    public bool itemOn;

}
