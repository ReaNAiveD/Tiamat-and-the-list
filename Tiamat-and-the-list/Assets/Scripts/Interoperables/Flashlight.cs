using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Pickable
{
    public string dialogSection;
    public PlayerManager player;
    public bool test;

    // Use this for initialization
    void Start()
    {
        InputManager.AddOnInteract(OnInteract);
        InputManager.AddOnPick(OnPick);
    }

    // Update is called once per frame
    void Update()
    {

        if (test)
            OnPick();
    }
    void OnInteract()
    {
        if (NearPlayer)
        {
            DialogManager.ShowDialog(dialogSection);

        }
    }
    void OnPick()
    {
        if (NearPlayer)
        {
            gameObject.transform.position = new Vector3(48.0f, -20.0f, 0.0f);
            gameObject.GetComponent<Interoperable>().interoperable = false;
            UIManager.SetEquipmentIcon("EquipmentSprite\\Stage00_shoudiantong");
            player.setEquip(EquipmentType.FlashLight);
        }

    }
}
