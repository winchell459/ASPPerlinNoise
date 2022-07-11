using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceGameHandler : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public bool paused = false;
    public Transform player, ai;

    // Start is called before the first frame update
    void Start()
    {
        if (!paused) Time.timeScale = 1;
        else Time.timeScale = 0;
        pauseMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Time.timeScale = 1;
                pauseMenuPanel.SetActive(false);
                paused = false;
            }
            else
            {
                Time.timeScale = 0;
                pauseMenuPanel.SetActive(true);
                paused = true;
            }
        }
    }

    public void ResetPlayerVertical()
    {

    }

    public void ResetAIVertical()
    {

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
