using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rules : MonoBehaviour
{
    public void ToRules()
    {
        Music.Instance.Effect();
        SceneManager.LoadScene("Rules");
    }
}
