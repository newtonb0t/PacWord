using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardData
{
    [System.Serializable]
    public struct Scores
    {
        public string m_user;
        public int m_score;
    }

    public List<Scores> m_scores = new List<Scores>();
    
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}
