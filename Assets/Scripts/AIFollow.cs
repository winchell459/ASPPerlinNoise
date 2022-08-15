using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFollow : MonoBehaviour
{
    public Transform target;
    public Transform waypointCircuit;
    public bool following;
   
     
    [SerializeField] public AITrack track = new AITrack();
    public float followRange = 5;
    public float waypointDistance = 10;
    public float waypointVelocity = 1;

    public int waypointIndex = 0;
    private float lastWaypointSetTime;

    public bool reversingTrack;
    private List<Checkpoint> waypointPool = new List<Checkpoint>();
    public Checkpoint checkpointPrefab;
    private Checkpoint current = null;

    // Start is called before the first frame update
    void Start()
    {
        RestartTrack();
    }

    // Update is called once per frame
    void Update()
    {
        if(!track.trackComplete && !following &&  Vector3.Distance(transform.position, target.position) < followRange 
            && 
            Vector3.Distance(track.checkpoints[track.checkpoints.Count - 1].transform.position, target.position) <= waypointDistance)
        {
            following = true;
        }else if(following && !track.trackComplete && Vector3.Distance(transform.position, target.position) > followRange)
        {
            following = false;
            reversingTrack = true;
        }

        if (following)
        {
            waypointCircuit.position = target.position;
            if(Vector3.Distance(track.checkpoints[track.checkpoints.Count - 1].transform.position, target.position) > waypointDistance)
            {
                track.checkpoints.Add(GetWaypoint(waypointCircuit, false));
                waypointIndex += 1;
            }

            if(track.checkpoints.Count > 20 && Vector3.Distance(track.checkpoints[0].transform.position, waypointCircuit.position) <= waypointDistance)
            {
                following = false;
                track.trackComplete = true;
                waypointIndex = 0;
            }
        }
        else
        {
            current = track.checkpoints[waypointIndex];
            int nextIndex = waypointIndex;
            if (track.trackComplete) nextIndex = (nextIndex + 1) % track.checkpoints.Count;
            else if (!track.trackComplete)
            {
                if (reversingTrack)
                {
                    if (nextIndex - 1 < 0)
                    {
                        nextIndex = 1;
                        reversingTrack = false;
                    }
                    else nextIndex -= 1;
                }
                else
                {
                    if (nextIndex + 1 >= track.checkpoints.Count)
                    {
                        nextIndex = track.checkpoints.Count - 2;
                        reversingTrack = true;
                    }
                    else nextIndex += 1;
                }
            }

            if(Vector3.Distance(transform.position, current.transform.position) < waypointDistance)
            {
                current = track.checkpoints[nextIndex];
                waypointIndex = nextIndex;

            }
            waypointCircuit.position = current.transform.position;
        }
    }

    public void RestartTrack()
    {
        for(int i = track.checkpoints.Count - 1; i >= 0; i -= 1)
        {
            Checkpoint checkpoint = track.checkpoints[i];
            ReturnWaypoint(checkpoint);
        }
        track.checkpoints.Add(GetWaypoint(transform, true));
        following = false;
        track.trackComplete = false;
    }
    private Checkpoint GetWaypoint(Transform target, bool start)
    {
        lastWaypointSetTime = Time.time;

        Checkpoint waypoint;
        if(waypointPool.Count > 0)
        {
            waypoint = waypointPool[0];
            waypointPool.RemoveAt(0);
        }
        else
        {
            waypoint = Instantiate(checkpointPrefab);
        }
        waypoint.transform.position = target.position;

        waypoint.Activate(start);

        waypoint.name = $"waypoint {(int)target.position.x}, {(int)target.position.z}";

        return waypoint;
    }


    private void ReturnWaypoint(Checkpoint waypoint)
    {
        waypoint.Deactivate();
        track.checkpoints.Remove(waypoint);
        waypointPool.Add(waypoint);
    }

    public void RemoveWaypoint(Transform waypoint)
    {
        for(int i = 0; i < track.checkpoints.Count; i += 1)
        {
            if(track.checkpoints[i].transform == waypoint)
            {
                RemoveWaypoint(track.checkpoints[i]);
                break;
            }
        }
    }
    public void RemoveWaypoint(Checkpoint waypoint)
    {
        if(waypoint != track.checkpoints[0]) StartCoroutine(RemovingWaypoing(waypoint));
    }

    private IEnumerator RemovingWaypoing(Checkpoint waypoint)
    {
        while(waypoint == current)
        {
            yield return null;
        }

        ReturnWaypoint(waypoint);
    }
}

[System.Serializable]
public class AITrack
{
    public List<Checkpoint> checkpoints;
    public bool trackComplete;
    public AITrack()
    {
        checkpoints = new List<Checkpoint>();
    }

    public Checkpoint GetNextWaypoint(Checkpoint waypoint)
    {
        int nextIndex = (checkpoints.IndexOf(waypoint) + 1) % checkpoints.Count;
        return checkpoints[nextIndex];
    }
}
