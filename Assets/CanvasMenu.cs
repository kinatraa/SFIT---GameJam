using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasMenu : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void OutGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }

    public void Hide()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Back()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
