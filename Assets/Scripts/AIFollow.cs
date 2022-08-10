using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFollow : MonoBehaviour
{
    public Transform target;
    public Transform waypointCircuit;
    public bool following;
    public bool trackComplete;
    [SerializeField] private List<Transform> track = new List<Transform>();
    public float followRange = 5;
    public float waypointDistance = 10;
    public float waypointVelocity = 1;

    public int waypointIndex = 0;
    private float lastWaypointSetTime;

    public bool reversingTrack;
    // Start is called before the first frame update
    void Start()
    {
        RestartTrack();
    }

    // Update is called once per frame
    void Update()
    {
        if(!trackComplete && !following &&  Vector3.Distance(transform.position, target.position) < followRange && Vector3.Distance(track[track.Count - 1].position, target.position) <= waypointDistance)
        {
            following = true;
        }else if(following && !trackComplete && Vector3.Distance(transform.position, target.position) > followRange)
        {
            following = false;
            reversingTrack = true;
        }

        if (following)
        {
            waypointCircuit.position = target.position;
            if(Vector3.Distance(track[track.Count - 1].position, target.position) > waypointDistance)
            {
                track.Add(GetWaypoint(waypointCircuit));
                waypointIndex += 1;
            }

            if(track.Count > 20 && Vector3.Distance(track[0].position, waypointCircuit.position) <= waypointDistance)
            {
                following = false;
                trackComplete = true;
                waypointIndex = 0;
            }
        }
        else
        {
            Transform current = track[waypointIndex];
            int nextIndex = waypointIndex;
            if (trackComplete) nextIndex = (nextIndex + 1) % track.Count;
            else if (!trackComplete)
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
                    if (nextIndex + 1 >= track.Count)
                    {
                        nextIndex = track.Count - 2;
                        reversingTrack = true;
                    }
                    else nextIndex += 1;
                }
            }

            if(Vector3.Distance(transform.position, current.position) < waypointDistance)
            {
                current = track[nextIndex];
                waypointIndex = nextIndex;

            }
            waypointCircuit.position = current.position;
        }
    }

    public void RestartTrack()
    {
        track.Clear();
        track.Add(GetWaypoint(transform));
        following = false;
        trackComplete = false;
    }
    private Transform GetWaypoint(Transform target)
    {
        lastWaypointSetTime = Time.time;
        Transform waypoint = new GameObject().transform;
        waypoint.position = target.position;

        waypoint.name = $"waypoint {(int)target.position.x}, {(int)target.position.z}";
        FindObjectOfType<HUDHandler>().Debug(waypoint.name);
        return waypoint;
    }
}
