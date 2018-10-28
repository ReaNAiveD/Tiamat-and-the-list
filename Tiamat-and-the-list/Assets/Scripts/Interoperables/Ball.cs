using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Interoperable
{

    public string dialogSection;
    // Use this for initialization
    void Start()
    {
        InputManager.AddOnInteract(OnInteract);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnInteract()
    {
        if (NearPlayer)
        {
            DialogManager.ShowDialog(dialogSection);
        }
    }
}
