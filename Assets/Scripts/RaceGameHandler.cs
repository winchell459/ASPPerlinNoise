using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceGameHandler : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public bool paused = false;
    public Transform player, ai;
    private bool raceStarted = false;
    private bool raceOver = false;
    public float startCountdown = 3;
    public Text countdownText;
    public Camera mainCamera;
    public Sebastian.MapGenerator mapGenerator;
    public bool newBuild;
    public bool hasAI;

    // Start is called before the first frame update
    void Start()
    {
        if (!paused) Time.timeScale = 1;
        else Time.timeScale = 0;
        pauseMenuPanel.SetActive(false);
        //
        StartCoroutine(CountdownTimer(startCountdown, 1, 3));
        if (newBuild)
        {
            mapGenerator.seed = Random.Range(0, 10000);
            mapGenerator.GenerateMap();
            //mapGenerator.AddMesh();
        }

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

        if (raceStarted)
        {
            player.gameObject.SetActive(true);
            mainCamera.transform.parent = player;
            if(hasAI) ai.gameObject.SetActive(true);
        }
    }

    IEnumerator CountdownTimer(float countdown, float startBuffer, float endBuffer)
    {
        countdownText.text = countdown.ToString();
        countdownText.gameObject.SetActive(true);
        yield return new WaitForSeconds(startBuffer);
        mapGenerator.AddMesh();
        while (countdown > 0)
        {
            countdown = Mathf.Clamp(countdown - Time.deltaTime, 0, float.MaxValue);
            
            countdownText.text = Utility.FormatTime(countdown);
            yield return null;
        }
        raceStarted = true;
        yield return new WaitForSeconds(endBuffer);
        countdownText.gameObject.SetActive(false);
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

    public void RebuildMesh()
    {
        mapGenerator.AddMesh();
    }
}
