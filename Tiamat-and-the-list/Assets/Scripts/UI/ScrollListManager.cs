using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollListManager : MonoBehaviour {
   
    private VerticalLayoutGroup layoutGroup;
    //内容
    private List<Button> contents;
    //这个layout的最高度，通过计算得出。
    private float height = 0.0f;
    //宽度，根据layout得到
    private float width = 750.0f;
    //两个组件之间的空隙大小
    private float space = 0.0f;

	// Use this for initialization
	void Awake () {
        width = GetComponent<RectTransform>().rect.width;
        layoutGroup = GetComponent<VerticalLayoutGroup>();
        space = layoutGroup.spacing;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Add(Button btnToShow)
    {
        RectTransform rectTrans = btnToShow.GetComponent<RectTransform>();
        rectTrans.sizeDelta = new Vector2(750.0f, rectTrans.sizeDelta.y);

        height += rectTrans.sizeDelta.y;
        btnToShow.transform.parent = gameObject.transform;
    }
}
