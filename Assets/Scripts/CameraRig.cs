using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
//using InputManager;

public class CameraRig : MonoBehaviour
{

    //[SerializeField] public InputActionProperty leftHand;
    //[SerializeField] public InputActionProperty rightHand;

    public GameObject cameraHead, cameraRig, cameraSwivel;
    public Transform playPos, pausedPos;
    [SerializeField] private bool active;
    public float zoomSpeed = 10;
    public float rotateSpeed = 10;
    public bool invertRotate;
    public float leftThreshold = 0.1f;

    public InputManager inputManager;
    public RaceGameHandler rch;
    private bool paused = false;
    [SerializeField] private bool followPlayerForward;

    // Start is called before the first frame update
    void Start()
    {
        //playPos.position = cameraHead.transform.position;
        //playPos.parent = cameraHead.transform.parent;
    }
    private void FixedUpdate()
    {
        cameraRig.transform.position = new Vector3(playPos.position.x, cameraRig.transform.position.y, playPos.position.z);
        if (!paused && followPlayerForward) cameraRig.transform.forward = forward(playPos.forward);
    }
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            float right = inputManager.zoom();
            float left = inputManager.rotate();
            Zoom(right);
            if (leftThreshold < Mathf.Abs(left)) Pan(left);

            //cameraRig.transform.position = new Vector3(playPos.position.x, cameraRig.transform.position.y, playPos.position.z);
            //if (!paused && followPlayerForward) cameraRig.transform.forward = forward(playPos.forward);
        }

        if (rch.paused && !paused)
        {
            //move head to pausePos
            cameraRig.transform.position = pausedPos.position;
            cameraRig.transform.forward = Vector3.forward;
            paused = true;
            //followPlayerForward = !followPlayerForward;
            
        }
        else if (!rch.paused && paused)
        {
            //move head to playPos
            cameraRig.transform.position = playPos.position;
            cameraRig.transform.forward = forward(playPos.forward);
            paused = false;
        }

        if(!paused && inputManager.UISelection())
        {
            followPlayerForward = !followPlayerForward;
            if (followPlayerForward)
            {
                Vector3 headForward = cameraSwivel.transform.forward;
                cameraRig.transform.forward = forward(playPos.forward);
                cameraSwivel.transform.forward = headForward;
            }
        }
    }

    
    
    private Vector3 forward(Vector3 target)
    {
        return new Vector3(target.x, 0, target.z).normalized;
    }

    private void Zoom(float delta)
    {
        Vector3 direction = cameraHead.transform.position - cameraRig.transform.position;
        float length = direction.magnitude;
        float newLength = length - delta * zoomSpeed * Time.deltaTime;
        cameraHead.transform.position = cameraRig.transform.position + newLength * direction.normalized;
    }

    private void Pan(float rotation)
    {
        if (invertRotate)
        {
            cameraSwivel.transform.Rotate(Vector3.up * rotateSpeed * rotation * Time.deltaTime);
        }
        else
        {
            cameraSwivel.transform.Rotate(-Vector3.up * rotateSpeed * rotation * Time.deltaTime);
        }
        
    }

    public void SetActivate(bool active)
    {
        this.active = active;
    }

}
