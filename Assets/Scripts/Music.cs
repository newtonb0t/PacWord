using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance;
    private DoubleSource cf;
    public AudioSource clickSource;
    public AudioClip menuSong;
    public AudioClip gameSong;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(clickSource.gameObject);
        cf = GetComponent<DoubleSource>();
        cf.CrossFade(menuSong, 0.7f, 1, 0.25f);
    }

    public void PlayMenuSong() {
        cf.CrossFade(menuSong, 0.7f, 2, 0);
    }

    public void PlayGameSong() {
        cf.CrossFade(gameSong, 0.7f, 2, 0);
    }

    public void Effect() {
        clickSource.Stop();
        clickSource.Play();
    }
}
