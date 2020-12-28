using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Text scroe;
    public Text highScroe;

    private GameController gameController;

    public GameObject pauseMenu;
   
    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    
    void Update()
    {
        scroe.text = gameController.score.ToString();
        if (gameController.score > PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore",gameController.score);
        }
        highScroe.text = "Best " + PlayerPrefs.GetInt("HighScore", 0);
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    public void UnPause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

}
