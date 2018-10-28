using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class TutorialS2Scenario : Scenario {

    public PlayerManager player;

    private bool hint4Shown = false;
    private bool hint5Shown = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!hint4Shown)
        {
            hint4Shown = true;
            scenarioHintOn = true;
            DialogManager.ShowDialog("Hint4", OnHint4ShownEnd);
        }
        if (!hint5Shown && player.transform.position.x > 7.0f)
        {
            hint5Shown = true;
            scenarioHintOn = true;
            DialogManager.ShowDialog("Hint5", OnHint5ShownEnd);
        }
	}

    public override string GetArchive()
    {
        JSONNode root = new JSONClass()
        {
            { "Hint4Shown", new JSONData(hint4Shown) },
            { "Hint5Shown", new JSONData(hint5Shown) }
        };
        return root.ToString();
    }

    public override void LoadArchive(string archiveLine)
    {
        Debug.Log("ScenarioLoadArchive");
        JSONNode root = JSON.Parse(archiveLine);
        hint4Shown = root["Hint4Shown"].AsBool;
        hint5Shown = root["Hint5Shown"].AsBool;
    }

    void OnHint4ShownEnd()
    {
        scenarioHintOn = false;
    }
    void OnHint5ShownEnd()
    {
        scenarioHintOn = false;
    }
}
