using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberMenu : MonoBehaviour
{
    public bool placingPlayer;
    public bool placingAI;
    public PlayerGrabber playerGrabber;

    public void PlacePlayerButton()
    {
        placingPlayer = true;
        playerGrabber.placingPlayer = placingPlayer;
        FindObjectOfType<PauseMenu>().HideMenus(true);
    }
    public void PlaceAIButton()
    {
        placingAI = true;
        FindObjectOfType<PauseMenu>().HideMenus(true);
        playerGrabber.placingAI = placingAI;
        playerGrabber.ai.GetComponent<AIFollow>().RestartTrack();
    }
}
