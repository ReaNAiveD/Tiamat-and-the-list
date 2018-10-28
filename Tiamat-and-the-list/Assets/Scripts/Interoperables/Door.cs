using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Door : Interoperable
{

    public string nextSceneName = "";
    public string dialogSection;
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
        {
            test = false;
        }
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
            //先不管这些，本来想试试这样能不能做加载页面，结果资源太少了闪过去了，先放着吧——NA
            SceneManager.LoadScene("Loading");
            StartCoroutine(LoadAnotherScene(nextSceneName));
        }
    }

    IEnumerator LoadAnotherScene(string name)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        yield return asyncOperation;
    }
}
