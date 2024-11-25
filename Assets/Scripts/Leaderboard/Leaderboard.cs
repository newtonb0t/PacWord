using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboard : MonoBehaviour
{

    private LeaderboardData data = new LeaderboardData();

    public Text text;
    public TMP_InputField input;
    private TMP_Text inputText;
    public GameObject inputArea;
    public GameObject buttons;

    public void Start() {
        inputText = input.transform.Find("Text Area").gameObject.transform.Find("Text").GetComponent<TMP_Text>();
        inputArea.SetActive(false);
        Load();
        Display();
    }

    public void Update() {
        if (inputArea.activeSelf) input.MoveToEndOfLine(false, false);
    }


    public void Load() {
        LeaderboardDataManager.LoadJsonData(data);
    }

    private void Save() {
        LeaderboardDataManager.SaveJsonData(data);
    }

    // Method Call to Handle New Score
    public void NewScore(int score) {
        // Checks if score is higher than lowest leaderboard score
        if (!(data.m_scores.Count == 0)) if ((data.m_scores.Last().m_score > score) && data.m_scores.Count > 5) {Display(); return;}

        // Prompts Initial input
        inputArea.SetActive(true);
        buttons.SetActive(false);
        StartCoroutine(Score(score));
    }

    IEnumerator Score(int score) {
        // Waits unitl initials are entered
        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Return) && inputText.text.Length == 4));
        Music.Instance.Effect();
        LeaderboardData.Scores newScore = new LeaderboardData.Scores();
        newScore.m_score = score;
        newScore.m_user = inputText.text;
        List<LeaderboardData.Scores> scores = data.m_scores;
        scores.Sort(new ScoreSorter());
        // Checks if there are 5 scores
        bool scored = false;
        for (int i = 0; i < scores.Count; i++) {
            if (score > scores[i].m_score) {
                scores.Insert(i, newScore);
                scored = true;
                if (scores.Count > 5) scores = scores.GetRange(0, 5);
                break;
            }
        }
        if (!scored && scores.Count < 5) scores.Add(newScore);
        data.m_scores = scores;
        Display();
        Save();
        inputArea.SetActive(false);
        buttons.SetActive(true);
    }

    public void Display() {
        // Displays Score Data
        string k = "";
        int loops = 0;
        foreach (LeaderboardData.Scores s in data.m_scores) {
            k += s.m_user +": "+s.m_score +"\n";
            loops++;
        }
        for (int i = loops; i < 5; i++) {
            k += "AAA:\n";
        }
        text.text = k;
    }
}

class ScoreSorter : IComparer<LeaderboardData.Scores> {
    public int Compare(LeaderboardData.Scores a, LeaderboardData.Scores b) {
        return b.m_score-a.m_score;
    }
}