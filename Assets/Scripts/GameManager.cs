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
        spriteArray = Resources.LoadAll<spriteArray>("")                                    
    }      
        }         
    } 
} 