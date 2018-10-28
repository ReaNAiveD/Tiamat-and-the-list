using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DialogLoader {
    public List<Dialog> context = new List<Dialog>();
    private string dialogDataPath;
    
    public void loadData()
    {
        dialogDataPath = Application.dataPath + "/Resources/" + SceneItemManager.GetLevelName() + "-Dialog.txt";
        Debug.Log(File.Exists(dialogDataPath));
        if (File.Exists(dialogDataPath))
        {
            string[] strs = File.ReadAllLines(dialogDataPath);
            foreach (string rawData in strs)
            {
                if (rawData == "")
                    continue;
                string[] data = rawData.Replace("\\", "\n").Split('|');
                Dialog dialog = new Dialog
                {
                    id = int.Parse(data[0]),
                    section = data[1],
                    characterName = data[2],
                    text = data[3],
                    imagePath = data[4],
                    branchNum = int.Parse(data[5]),
                    branches = new List<Dialog.branch>()
                };
                if (dialog.branchNum > 0)
                {
                    for (int i=0; i<dialog.branchNum; i++)
                    {
                        Dialog.branch branch = new Dialog.branch();
                        branch.switch_id = int.Parse(data[6 + i * 2]);
                        branch.text = data[6 + i * 2 + 1];
                        dialog.branches.Add(branch);
                    }
                }
                context.Add(dialog);
            }
        }
    }
}
