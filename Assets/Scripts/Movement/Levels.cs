using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{
    private string[] levels = {"Level 1 (3-6)","Level 2 (6-9)","Level 3 (9-12)"};
    private int i = 0;

    public Text levelsText;
    public Text instructionText;

    public void Click() {
        Music.Instance.Effect();
        if (i < 2) i++;
        else i = 0;
        instructionText.gameObject.SetActive(false);
        levelsText.text = levels[i];
        MainManager.MaxRounds = (3*(i+1))+3;
        MainManager.InitialRound = 3*(i+1);
    }
}
