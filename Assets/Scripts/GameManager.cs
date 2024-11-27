using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform letters;
    public AudioSource source;
    // 0 = 
    public AudioClip[] clips;

    public Dictionary<int,List<string>> words;
    public Sprite[] spriteArray;
    private Random rnd = new Random();

    public bool hard;
    public int maxRounds;

    private string word;
    private int wordStreak = 1;
    private bool letterStreak = true;
    private int onLetter = 0;
    private int crIndexOffset = 0;

    public Text gameOverText;
    public Text victoryText;
    public Text pressButtonText;
    public Text scoreText;
    public Text livesText;
    public Text wordText;
    private string wordShow = "";
    private string wordRemaining = "";
    public Text indicatorText;
    private string indicator = "";

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }
    public int difficulty { get; private set; } = 3;

    private void SetControl(bool wat) {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].movement.on = wat;
        }
        pacman.on = wat;
        pacman.movement.on = wat;
    }

    private void Start()
    {
        Music.Instance.PlayGameSong();
        hard = MainManager.Mode;
        maxRounds = MainManager.MaxRounds;
        spriteArray = Resources.LoadAll<Sprite>("GameLetters");
        words = MainManager.words;
        string test = words[4][5];
        if (test[test.Length-1] == (char)13) crIndexOffset = 1;
        Debug.Log(crIndexOffset+" offset");
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown) {
            MainManager.Instance.StartCoroutine(MainManager.Instance.ScoreUpdate(score));
        }
    }

    private void NewGame() {
        indicatorText.gameObject.SetActive(false);
        difficulty = MainManager.InitialRound;
        wordStreak = difficulty - 2;
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(1.0f);
        SetControl(true);
    }

    private void NewRound()
    {
        SetControl(false);
        setLetters();
        SetScore(score);
        gameOverText.enabled = false;
        victoryText.enabled = false;
        pressButtonText.enabled = false;
        wordText.color = Color.green;
        letterStreak = true;
        onLetter = 0;
        ResetState();
        StartCoroutine(Wait());
    }

    private void setLetters() {
        List<string> wordList = words[difficulty+crIndexOffset];
        int index = rnd.Next(0, wordList.Count-1);
        word = wordList[index];
        if (hard) {
            wordShow = "";
            wordRemaining = word;
            foreach (char c in word) {
                wordShow += "_ ";
            }
            wordText.text = wordShow;
        } else {
            indicatorText.gameObject.SetActive(true);
            wordText.text = word;
            indicator = "";
            for (int i = 0; i < word.Length-crIndexOffset; i++) {
                if (i != 0) {
                    if (!new char[] {'t','j','i','l'}.Contains(word[i])) indicator += " ";
                    indicator += " ";
                } else indicator += "^"; 
            }
            indicatorText.text = indicator;
        }
        List<Transform> tileList = new List<Transform>();
        foreach (Transform t in letters) {
            tileList.Add(t);
            t.gameObject.SetActive(false);
        }
        List<Transform> spots = tileList.OrderBy(x => rnd.Next()).Take(word.Length).ToList();
        for (int i = 0; i < word.Length-crIndexOffset; i++) {
            SpriteRenderer sr = spots[i].gameObject.GetComponent<SpriteRenderer>();
            Debug.Log((int) word[i]);
            sr.sprite = spriteArray[(int) word[i]-87];
            spots[i].gameObject.SetActive(true);
        }
    }

    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
    {
        SoundEffect(3);
        gameOverText.enabled = true;
        pressButtonText.enabled = true;

        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = "x"+wordStreak+" "+score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        SoundEffect(3);
        pacman.DeathSequence();

        SetLives(lives - 1);

        if (lives > 0) {
            Invoke(nameof(ResetState), 3f);
        } else {
            GameOver();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        ghostMultiplier++;
    }

    public void LetterEaten(Letter letter)
    {
        SoundEffect(1);
        letter.gameObject.SetActive(false);
        Debug.Log(letter.gameObject.GetComponent<SpriteRenderer>().sprite.name);
        Debug.Log(word[onLetter]);
        char cletter = char.ToLower(letter.gameObject.GetComponent<SpriteRenderer>().sprite.name[12]);
        if (cletter != word[onLetter]) {
            if (letterStreak) SoundEffect(3);
            Debug.Log("bad");
            letterStreak = false;
            wordText.color = Color.red;
            indicatorText.gameObject.SetActive(false);
        } else if (!hard) {   
            indicator = "";
            for (int i = 0; i < word.Length; i++) {
                if (i != onLetter+1) {
                    if (!new char[] {'t','j','i','l'}.Contains(word[i])) indicator += " ";
                    indicator += " ";
                } else indicator += "^"; 
            }
            indicatorText.text = indicator;
        }
        if (hard) {
            int index = wordRemaining.IndexOf(cletter);
            char[] wr = wordRemaining.ToCharArray();
            wr[index] = '_';
            wordRemaining = new string(wr);
            char[] wd = wordShow.ToCharArray();
            wd[index*2] = cletter;
            wordShow = new string(wd);
            wordText.text = wordShow;
        }
        onLetter++;
        SetScore(score + letter.points);

        if (!HasRemainingPellets())
        {
            if (letterStreak) {
                SetScore(score + (100*wordStreak));
                wordStreak++;
                SoundEffect(0);
            } else {
                wordStreak = 1;
            }
            pacman.gameObject.SetActive(false);
            difficulty++;
            if (difficulty < maxRounds+1) {
                Invoke(nameof(NewRound), 3f);
            } else {
                Invoke(nameof(Win), 2f);
            }
        }
    }

    private void Win() {
        SoundEffect(2);
        lives = 0;
        victoryText.enabled = true;
        pressButtonText.enabled = true;

        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform letter in letters)
        {
            if (letter.gameObject.activeSelf) {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }


    private void SoundEffect(int i) {
        source.Stop();
        source.clip = clips[i];
        source.Play();
    }
}
