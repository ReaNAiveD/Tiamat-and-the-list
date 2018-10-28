using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image equipmentImage;
    private Sprite emptyEquipment;

    private static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        emptyEquipment = equipmentImage.sprite;
    }

    public static void SetEquipmentIcon(string path) { instance._SetEquipmentIcon(path); }
    private void _SetEquipmentIcon(string path)
    {
        var icon = Resources.Load<Sprite>(path);
        Debug.Log(icon);
        if (icon != null)
        {
            equipmentImage.sprite = icon;
        }
    }

    public static void ClearEquipmentIcon() { instance._ClearEquipmentIcon(); }
    private void _ClearEquipmentIcon()
    {
        equipmentImage.sprite = emptyEquipment;
    }
}
