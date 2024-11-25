using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static MainManager Instance;
    public static bool Mode;
    public static int MaxRounds = 6;
    public static int InitialRound = 3;
    public static GameObject Leaderboard;
    public static Dictionary<int,List<string>> words = new Dictionary<int, List<string>>();

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        if (words.Count == 0) {
            TextAsset txt = (TextAsset) Resources.Load("words"); 
            TextAsset ctxt = (TextAsset) Resources.Load("curse"); 
            string[] dict = txt.text.Split("\n"[0]);
            string[] cursed = ctxt.text.Split("\n"[0]);
            for (int len = 3; len < 13; len++) {
                words[len] = new List<string>();
            }
            foreach (string word in dict) {
                int n = word.Length;
                if (n < 13 && n > 2) {
                    if (cursed.Contains(word)) continue;
                    List<string> l = words[n];
                    l.Add(word);
                }
            }
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public IEnumerator ScoreUpdate(int score) {
        Music.Instance.PlayMenuSong();
        SceneManager.LoadScene("Leaderboard");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        GameObject.Find("LeaderboardObj").GetComponent<Leaderboard>().NewScore(score);
    }
}
