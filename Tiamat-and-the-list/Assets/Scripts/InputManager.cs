using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    //键盘按下时委托
    public delegate void KeyInputDown();
    //拾取物品事件
    private event KeyInputDown OnPick;
    //与场景物品交互事件
    private event KeyInputDown OnInteract;
    //切换持有道具状态事件
    private event KeyInputDown OnSwitchItemState;
    //上楼梯事件
    private event KeyInputDown OnUpStair;
    //下楼梯事件
    private event KeyInputDown OnDownStair;
    //向左移动事件
    private event KeyInputDown OnLeftMove;
    //向右移动事件
    private event KeyInputDown OnRightMove;
    //敲击Esc退出事件
    private event KeyInputDown OnEscape;
    //对话下一句事件
    private event KeyInputDown OnNextDialog;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        bool dialogOn = DialogManager.IsDialogOn();
		if (Input.GetKeyDown(KeyCode.F) && OnPick != null && !dialogOn)
        {
            OnPick();
        }
        if (Input.GetKeyDown(KeyCode.E) && OnInteract != null && !dialogOn)
        {
            OnInteract();
        }
        if (Input.GetKeyDown(KeyCode.Q) && OnSwitchItemState != null && !dialogOn)
        {
            OnSwitchItemState();
        }
        if (Input.GetKeyDown(KeyCode.W) && OnUpStair != null && !dialogOn)
        {
            OnUpStair();
        }
        if (Input.GetKeyDown(KeyCode.S) && OnDownStair != null && !dialogOn)
        {
            OnDownStair();
        }
        if (Input.GetKey(KeyCode.A) && OnLeftMove != null && !dialogOn)
        {
            OnLeftMove();
        }
        if (Input.GetKey(KeyCode.D) && OnRightMove !=null && !dialogOn)
        {
            OnRightMove();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && OnEscape != null)
        {
            OnEscape();
        }
        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E)) && OnNextDialog != null && dialogOn){
            OnNextDialog();
        }
	}

    //注册特定事件
    public static void AddOnPick(KeyInputDown onPick) { instance._AddOnPick(onPick);  }
    private void _AddOnPick(KeyInputDown onPick)
    {
        this.OnPick += onPick;
    }

    public static void AddOnInteract(KeyInputDown onInteract) { instance._AddOnInteract(onInteract); }
    private void _AddOnInteract(KeyInputDown onInteract)
    {
        this.OnInteract += onInteract;
    }

    public static void AddOnSwitchItemState(KeyInputDown onSwitch) { instance._AddOnSwitchItemState(onSwitch); }
    private void _AddOnSwitchItemState(KeyInputDown onSwitch)
    {
        this.OnSwitchItemState += onSwitch;
    }

    public static void AddOnUpStair(KeyInputDown onUp) { instance._AddOnUpStair(onUp); }
    private void _AddOnUpStair(KeyInputDown onUp)
    {
        this.OnUpStair += onUp;
    }

    public static void AddOnDownStair(KeyInputDown onDown) { instance._AddOnDownStair(onDown); }
    private void _AddOnDownStair(KeyInputDown onDown)
    {
        this.OnDownStair += onDown;
    }

    public static void AddOnLeftMove(KeyInputDown onLeft) { instance._AddOnLeftMove(onLeft); }
    private void _AddOnLeftMove(KeyInputDown onLeft)
    {
        this.OnLeftMove += onLeft;
    }

    public static void AddOnRightMove(KeyInputDown onRight) { instance._AddOnRightMove(onRight); }
    private void _AddOnRightMove(KeyInputDown onRight)
    {
        this.OnRightMove += onRight;
    }

    public static void AddOnEscape(KeyInputDown onEsc) { instance._AddOnEscape(onEsc); }
    private void _AddOnEscape(KeyInputDown onEsc)
    {
        this.OnEscape += onEsc;
    }

    public static void AddOnNextDialog(KeyInputDown onNextDialog) { instance._AddOnNextDialog(onNextDialog); }
    private void _AddOnNextDialog(KeyInputDown onNextDialog)
    {
        this.OnNextDialog += onNextDialog;
    }
}
