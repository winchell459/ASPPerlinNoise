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

    // Start is called before the first frame update
    void Start()
    {
        RestartTrack();
    }

    // Update is called once per frame
    void Update()
    {
        if(!trackComplete &&  Vector3.Distance(transform.position, target.position) < followRange)
        {
            following = true;
        }

        if (following)
        {
            waypointCircuit.position = target.position;
            if(Time.time > lastWaypointSetTime + waypointVelocity)
            {
                track.Add(GetWaypoint(waypointCircuit));
            }

            if(track.Count > 20 && Vector3.Distance(track[0].position, waypointCircuit.position) <= waypointDistance)
            {
                following = false;
                trackComplete = true;
                waypointIndex = 1;
            }
        }
        else
        {
            Transform current = track[waypointIndex];
            int nextIndex = (waypointIndex + 1) % track.Count;
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
        track.Add(GetWaypoint(target));
        following = true;
        trackComplete = false;
    }
    private Transform GetWaypoint(Transform target)
    {
        lastWaypointSetTime = Time.time;
        Transform waypoint = new GameObject().transform;
        waypoint.position = target.position;

        waypoint.name = $"waypoint {(int)target.position.x}, {(int)target.position.y}";
        return waypoint;
    }
}
