using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public PerlinNoiseMenu perlinNoiseMenu;
    public void Pause(bool pause)
    {
        pausePanel.SetActive(pause);

        perlinNoiseMenu.Active(pause);
        
    }

    public void RestartButton()
    {
        FindObjectOfType<RaceGameHandler>().RestartLevel();
    }
    public void MainMenuButton()
    {
        FindObjectOfType<RaceGameHandler>().MainMenu();
    }
    public void PerlinNoiseControlsButton()
    {
        perlinNoiseMenu.Active(!perlinNoiseMenu.active);
    }
}
