using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToRankings : MonoBehaviour
{

    public void GoLeaderboard()
    {
        Music.Instance.Effect();
        SceneManager.LoadScene("Leaderboard");
    }
}
