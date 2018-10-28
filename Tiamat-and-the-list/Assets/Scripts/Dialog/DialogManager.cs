using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private static DialogManager instance;
    public GameObject DialogPrefab;
    public GameObject DialogBox;               // 对话框+立绘整体
    public double textSpeed = 0.04;            // 文字显示速度
    
    private Dialog currentDialog;              // 当前对话
    private int id;                            // temp 测试用
    private DialogLoader loader;
    private List<Dialog> currentDialogSection; // 当前加载的section

    private string tempDialog;                 // 逐字显示用
    private bool dialogFlag;                   // 判断是否在逐字显示
    private double timer;
    private bool animationLock;                // 在播放特定动画的时候锁死交互

    //委托，当对话结束时调用
    public delegate void NoneParaVoid();
    private NoneParaVoid OnEnd;

    public static bool IsDialogOn()
    {
        return instance._IsDialogOn();
    }
    private bool _IsDialogOn()
    {
        return DialogBox != null;
    }

    private void Awake()
    {
        instance = this;
    }

    //显示对话栏
    public static void ShowDialog(string section)
    {
        instance.InitDialog(section);
    }
    public static void ShowDialog(string section, NoneParaVoid OnEnd)
    {
        instance.OnEnd += OnEnd;
        instance.InitDialog(section);
    }
    public void InitDialog(string section)
    {
        if (DialogBox == null)
        {
            currentDialogSection = new List<Dialog>();
            foreach (Dialog d in loader.context)
            {
                if (d.section == section)
                    currentDialogSection.Add(d);
            }
            if (currentDialogSection.Count == 0)
                return;
            DialogBox = Instantiate(DialogPrefab) as GameObject;
            currentDialog = currentDialogSection[0];
            id = 0;
            dialogFlag = false;
            DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().text = "";
            DialogBox.transform.Find("DialogPanel").Find("DialogText").GetComponent<Text>().text = "";
            animationLock = true;
            StartCoroutine(initializeAnimation());
        }
    }

    private void displayDialog(Dialog dialog)
    {
        Image characterImage = DialogBox.transform.Find("Character").GetComponent<Image>();
        Sprite sp = Resources.Load("CharacterTachie\\" + dialog.imagePath, typeof(Sprite)) as Sprite;
        Debug.Log(dialog.imagePath);
        characterImage.sprite = sp;
        tempDialog = "";
        timer = 0;
        dialogFlag = true;
    }

    public void DestoryDialog()
    {
        if (DialogBox != null) {
            StartCoroutine(destroyAnimation());
        }
    }

    public bool IsEmptyDialog()
    {
        if (GameObject.Find("Dialogbox(Clone)"))
        {

            return false;
        }
        else
        {
            return true;
        }

    }

    void Start()
    {
        InputManager.AddOnNextDialog(OnNextDialog);
        loader = new DialogLoader();
        loader.loadData();
        //initDialog("Scene1");
    }

    void Update()
    {
        if (DialogBox != null) { 
            if (tempDialog == currentDialog.text)
            {
                dialogFlag = false;
            }
            if (dialogFlag)
            {
                Text dialogText = DialogBox.transform.Find("DialogPanel").Find("DialogText").GetComponent<Text>();
                if (timer > textSpeed)
                {
                    timer = 0;
                    tempDialog = currentDialog.text.Substring(0, tempDialog.Length + 1);
                }
                dialogText.text = tempDialog;
                timer += Time.deltaTime;
            }
        }
    }

    private IEnumerator initializeAnimation()
    {
        Sprite initialSprite = Resources.Load("CharacterTachie\\" + currentDialog.imagePath, typeof(Sprite)) as Sprite;
        DialogBox.transform.Find("Character").GetComponent<Image>().sprite = initialSprite;
        Vector3 targetNamePanelPosition = DialogBox.transform.Find("NamePanel").transform.position;
        Vector3 targetDialogPanelPosition = DialogBox.transform.Find("DialogPanel").transform.position;
        Color targetNamePanelColor = DialogBox.transform.Find("NamePanel").GetComponent<Image>().color;
        Color targetDialogPanelColor = DialogBox.transform.Find("DialogPanel").GetComponent<Image>().color;
        Vector3 targetCharacterPosition = DialogBox.transform.Find("Character").transform.position;
        Color targetCharacterColor = DialogBox.transform.Find("Character").GetComponent<Image>().color;
        for (float t = 0; t <= 1f; t += 0.05f)
        {
            DialogBox.transform.Find("NamePanel").transform.position = Vector3.Lerp(targetNamePanelPosition - new Vector3(0, 20f, 0), targetNamePanelPosition, EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("DialogPanel").transform.position = Vector3.Lerp(targetDialogPanelPosition - new Vector3(0, 20f, 0), targetDialogPanelPosition, EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("Character").transform.position = Vector3.Lerp(targetCharacterPosition - new Vector3(40f, 0, 0), targetCharacterPosition, EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("NamePanel").GetComponent<Image>().color = Color.Lerp(new Color(targetNamePanelColor.r, targetNamePanelColor.g, targetNamePanelColor.b, 0), targetNamePanelColor, EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("DialogPanel").GetComponent<Image>().color = Color.Lerp(new Color(targetDialogPanelColor.r, targetDialogPanelColor.g, targetDialogPanelColor.b, 0), targetDialogPanelColor, EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("Character").GetComponent<Image>().color = Color.Lerp(new Color(targetCharacterColor.r, targetCharacterColor.g, targetCharacterColor.b, 0), targetCharacterColor, EasingFuncs.QuartInOut(t));
            yield return null;
            yield return new WaitForSeconds(0.03f);
        }
        StartCoroutine(nameAnimation("", currentDialog.characterName));
        displayDialog(currentDialog);
    }

    private IEnumerator nameAnimation(string name1, string name2)
    {
        GameObject replacedName = Instantiate(DialogBox.transform.Find("NamePanel").Find("NameText").gameObject) as GameObject;
        replacedName.transform.position = DialogBox.transform.Find("NamePanel").Find("NameText").position - new Vector3(20f, 0, 0);
        replacedName.GetComponent<Text>().text = name2;
        Color c1 = DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().color;
        Color c2 = replacedName.GetComponent<Text>().color;
        c2 = new Color(c2.r, c2.g, c2.b, 0f);
        replacedName.GetComponent<Text>().color = c2;
        replacedName.transform.SetParent(DialogBox.transform.Find("NamePanel"));

        Vector3 targetNameTextPosition = DialogBox.transform.Find("NamePanel").Find("NameText").position + new Vector3(20f, 0, 0);
        Vector3 targetReplacedNamePosition = DialogBox.transform.Find("NamePanel").Find("NameText").position;

        for (float t = 0; t <= 1f; t += 0.05f)
        {
            DialogBox.transform.Find("NamePanel").Find("NameText").transform.position = Vector3.Lerp(targetNameTextPosition - new Vector3(20f, 0, 0), targetNameTextPosition, EasingFuncs.QuartInOut(t));
            replacedName.transform.position = Vector3.Lerp(targetReplacedNamePosition - new Vector3(20f, 0, 0), targetReplacedNamePosition, EasingFuncs.QuartInOut(t));
            
            DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().color = Color.Lerp(c1, new Color(c1.r, c1.g, c1.b, 0f), EasingFuncs.QuartInOut(t));
            replacedName.GetComponent<Text>().color = Color.Lerp(c2, new Color(c2.r, c2.g, c2.b, 1f), EasingFuncs.QuartInOut(t));
            yield return null;//下一帧继续执行for循环
            yield return new WaitForSeconds(0.006f);//0.006秒后继续执行for循环
        }
        DialogBox.transform.Find("NamePanel").Find("NameText").transform.position -= new Vector3(20f, 0, 0);
        DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().color = new Color(c1.r, c1.g, c1.b, 1.0f);
        DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().text = name2;
        Destroy(replacedName);
        animationLock = false;
    }

    private IEnumerator destroyAnimation()
    {
        Vector3 targetNamePanelPosition = DialogBox.transform.Find("NamePanel").transform.position;
        Vector3 targetDialogPanelPosition = DialogBox.transform.Find("DialogPanel").transform.position;
        Color targetNamePanelColor = DialogBox.transform.Find("NamePanel").GetComponent<Image>().color;
        Color targetDialogPanelColor = DialogBox.transform.Find("DialogPanel").GetComponent<Image>().color;
        Vector3 targetCharacterPosition = DialogBox.transform.Find("Character").transform.position;
        Color targetCharacterColor = DialogBox.transform.Find("Character").GetComponent<Image>().color;
        Vector3 targetNameTextPosition = DialogBox.transform.Find("NamePanel").Find("NameText").transform.position;
        Color targetNameTextColor = DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().color;
        Vector3 targetDialogTextPosition = DialogBox.transform.Find("DialogPanel").Find("DialogText").transform.position;
        Color targetDialogTextColor = DialogBox.transform.Find("DialogPanel").Find("DialogText").GetComponent<Text>().color;

        for (float t = 0; t <= 1f; t += 0.05f)
        {
            DialogBox.transform.Find("NamePanel").transform.position = Vector3.Lerp(targetNamePanelPosition, targetNamePanelPosition - new Vector3(0, 20f, 0), EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("DialogPanel").transform.position = Vector3.Lerp(targetDialogPanelPosition, targetDialogPanelPosition - new Vector3(0, 20f, 0), EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("Character").transform.position = Vector3.Lerp(targetCharacterPosition, targetCharacterPosition + new Vector3(40f, 0, 0), EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("NamePanel").Find("NameText").transform.position = Vector3.Lerp(targetNameTextPosition, targetNameTextPosition - new Vector3(0, 20f, 0), EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("DialogPanel").Find("DialogText").transform.position = Vector3.Lerp(targetDialogTextPosition, targetDialogTextPosition - new Vector3(0, 20f, 0), EasingFuncs.QuartInOut(t));

            DialogBox.transform.Find("NamePanel").GetComponent<Image>().color = Color.Lerp(targetNamePanelColor, new Color(targetNamePanelColor.r, targetNamePanelColor.g, targetNamePanelColor.b, 0), EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("DialogPanel").GetComponent<Image>().color = Color.Lerp(targetDialogPanelColor, new Color(targetDialogPanelColor.r, targetDialogPanelColor.g, targetDialogPanelColor.b, 0), EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("Character").GetComponent<Image>().color = Color.Lerp(targetCharacterColor, new Color(targetCharacterColor.r, targetCharacterColor.g, targetCharacterColor.b, 0), EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().color = Color.Lerp(targetNameTextColor, new Color(targetNameTextColor.r, targetNameTextColor.g, targetNameTextColor.b, 0), EasingFuncs.QuartInOut(t));
            DialogBox.transform.Find("DialogPanel").Find("DialogText").GetComponent<Text>().color = Color.Lerp(targetDialogTextColor, new Color(targetDialogTextColor.r, targetDialogTextColor.g, targetDialogTextColor.b, 0), EasingFuncs.QuartInOut(t));

            yield return null;
            yield return new WaitForSeconds(0.03f);
        }

        Destroy(DialogBox);
        DialogBox = null;

        //调用对话结束时委托
        if (OnEnd != null)
        {
            OnEnd();
            OnEnd = null;
        }
    }

    private void setNextDialog()
    {
        string name1 = currentDialog.characterName;
        id++;
        if (id >= currentDialogSection.Count)
        {
            animationLock = true;
            DestoryDialog();
            return;
        }
        currentDialog = currentDialogSection[id];
        if (name1 != currentDialog.characterName)
        {
            animationLock = true;
            StartCoroutine(nameAnimation(name1, currentDialog.characterName));
        }
        displayDialog(currentDialog);
    }

    private void OnNextDialog()
    {
        if (!animationLock && DialogBox != null) {
            // 如果当前文字已经全部出现，则进入下一句
            // 否则将当前这句话直接显示出来
            if (tempDialog == currentDialog.text) {
                setNextDialog();
            }
            else { 
                tempDialog = currentDialog.text;
                Text dialogText = DialogBox.transform.Find("DialogPanel").Find("DialogText").GetComponent<Text>();
                dialogText.text = tempDialog;
            }
        }
    }
}