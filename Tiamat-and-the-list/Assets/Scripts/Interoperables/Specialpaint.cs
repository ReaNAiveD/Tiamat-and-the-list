using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Specialpaint : Interoperable
{
    public string dialogSection1;
    public string dialogSection2;
    public string dialogSection3;
    private int section = 0;

    public Animator dropAnimator;

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
            section++;
            if (section == 1)
                DialogManager.ShowDialog(dialogSection1);
            else if (section == 2)
                DialogManager.ShowDialog(dialogSection2);
            else if (section == 3)
            {
                DialogManager.ShowDialog(dialogSection3);
                dropAnimator.SetBool("drop", true);
            }
        }
    }
}
