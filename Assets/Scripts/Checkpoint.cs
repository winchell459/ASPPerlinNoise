using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Material regular, start, player;
    public MeshRenderer renderer;
    private bool isStart;
    private int trackCompleteCounter = 3;
    private AITrack track;
    private Collider aiCollider = null;
    
    public void Activate(bool start)
    {
        isStart = start;
        hasPlayer = start;
        if (start)
        {
            renderer.material = this.start;
            
        }
        else renderer.material = regular;

        track = FindObjectOfType<AIFollow>().track;
        gameObject.SetActive(true);

        
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private int playerLap = 0, aiLap = 0;
    [SerializeField]private bool hasPlayer;
    public void CheckpointTriggered(Collider other)
    {

        if (other.CompareTag("Player") && isStart && hasPlayer && track.trackComplete)
        {
            playerLap += 1;
            FindObjectOfType<HUDHandler>().Debug($"Player Laps: {playerLap}");
        }
        else if (other.CompareTag("AI") && isStart && track.trackComplete)
        {
            if (aiCollider == null) aiCollider = other;
            if(aiCollider == other)
            {
                aiLap += 1;
                FindObjectOfType<HUDHandler>().Debug($"AI Laps: {aiLap}");
            }
            
        }

        if (hasPlayer && other.CompareTag("Player") && track.trackComplete)
        {
            FindObjectOfType<AIFollow>().track.GetNextWaypoint(this).SetHasPlayer(true);
            SetHasPlayer(false);
        }

        if (other.CompareTag("Player") && isStart && trackCompleteCounter > 0)
        {
            trackCompleteCounter -= 1;
        }

        //if(other.CompareTag("Player")) FindObjectOfType<HUDHandler>().Debug($"Player at Checkpoint: {transform.name}");
    }

    public void SetHasPlayer(bool hasPlayer)
    {
        if(hasPlayer && !isStart)
        {
            renderer.material = player;
        }else if (!hasPlayer && !isStart)
        {
            renderer.material = regular;
        }
        this.hasPlayer = hasPlayer;
    }
}
