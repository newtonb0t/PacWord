using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardClear : MonoBehaviour
{
    public Text clearText;
    bool clickedOnce;

    public void Click() {
        Music.Instance.Effect();
        if (!clickedOnce) {
            clickedOnce = true;
            StartCoroutine(ResetCooldown());
            clearText.text = "Confirm?";
        }
        else {
            clickedOnce = false;
            clearText.text = "Clear";
            StopCoroutine(ResetCooldown());
            LeaderboardDataManager.ResetData();
            GameObject.Find("LeaderboardObj").GetComponent<Leaderboard>().Load();
            GameObject.Find("LeaderboardObj").GetComponent<Leaderboard>().Load();
            GameObject.Find("LeaderboardObj").GetComponent<Leaderboard>().Display();
        }    
    }

    IEnumerator ResetCooldown() {
        yield return new WaitForSeconds(2);
        clickedOnce = false;
        clearText.text = "Clear";
    }
}
