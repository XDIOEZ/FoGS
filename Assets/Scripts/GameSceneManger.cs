using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManger : MonoBehaviour
{
    public string IntroSceneName = "IntroScene";
    public void StartGame()
    {
        SceneManager.LoadScene(IntroSceneName);
    }
}
