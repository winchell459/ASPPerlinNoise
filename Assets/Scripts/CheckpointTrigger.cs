using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public Checkpoint checkpoint;
    private void OnTriggerEnter(Collider other)
    {
        checkpoint.CheckpointTriggered(other);
    }
}
