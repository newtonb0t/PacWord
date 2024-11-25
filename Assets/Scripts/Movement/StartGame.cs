using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string LevelName;

    public void LoadLevel()
    {
        MainManager.Mode = false;
        Music.Instance.Effect();
        SceneManager.LoadScene(LevelName);
    }
}
