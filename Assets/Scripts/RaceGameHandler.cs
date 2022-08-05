using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RaceGameHandler : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public bool paused = false;
    public Transform player, ai;
    private bool raceStarted = false;
    private bool raceOver = false;
    public float startCountdown = 3;
    public HUDHandler hudHandler;
    public Camera mainCamera;
    public Transform cameraRig;
    public Sebastian.MapGenerator mapGenerator;
    public bool newBuild;
    public bool hasAI;
    public InputManager inputManager;

    public bool GetRaceStarted() { return raceStarted; }


    private bool pauseTrigger { get { return inputManager.pause(); } }



    // Start is called before the first frame update
    void Start()
    {
        if (!paused) Time.timeScale = 1;
        else Time.timeScale = 0;
        pauseMenu.Pause(false);
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
        if (raceStarted)
        {
            player.gameObject.SetActive(true);
#if UNITY_ANDROID
            //cameraRig.transform.position = new Vector3(player.position.x, cameraRig.transform.position.y, player.position.z);



#else
            cameraRig.transform.parent = player;
            cameraRig.transform.localPosition = Vector3.zero;
#endif


            if (hasAI) ai.gameObject.SetActive(true);
        }

        if (pauseTrigger)
        {
            if (paused)
            {
                Time.timeScale = 1;
                pauseMenu.Pause(false);
                paused = false;
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.Pause(true);
                paused = true;
            }
        }

        //if(paused && inputManager.UISelection())
        //{
        //    RestartLevel();
        //}
        
    }

    IEnumerator CountdownTimer(float countdown, float startBuffer, float endBuffer)
    {
        hudHandler.SetTimer(countdown.ToString());
        hudHandler.DisplayTimer(true);
        yield return new WaitForSeconds(startBuffer);
        mapGenerator.AddMesh();
        while (countdown > 0)
        {
            if (mapGenerator.trackReady)
            {
                mapGenerator.trackReady = false;
                List<node> track = mapGenerator.track.track[0];
                foreach(List<node> loop in mapGenerator.track.track)
                {
                    if (loop.Count > track.Count) track = loop;
                }

                Vector2Int spawn = track[Random.Range(0, track.Count)].pos * 5 ;
                player.position = new Vector3(spawn.x, player.position.y, -spawn.y) + new Vector3Int(-495 / 2,0, 495 / 2);
            }
            countdown = Mathf.Clamp(countdown - Time.deltaTime, 0, float.MaxValue);
            
            hudHandler.SetTimer( Utility.FormatTime(countdown));
            yield return null;
        }
        raceStarted = true;
        yield return new WaitForSeconds(endBuffer);
        hudHandler.DisplayTimer(false);
    }

    public void ResetPlayerVertical()
    {

    }

    public void ResetAIVertical()
    {

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
