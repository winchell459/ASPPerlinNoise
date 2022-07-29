using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
//using InputManager;

public class CameraRig : MonoBehaviour
{

    //[SerializeField] public InputActionProperty leftHand;
    //[SerializeField] public InputActionProperty rightHand;

    public GameObject cameraHead, cameraRig;
    public Transform playPos, pausedPos;
    [SerializeField] private bool active;
    public float zoomSpeed = 10;
    public float rotateSpeed = 10;
    public bool invertRotate;
    public float leftThreshold = 0.1f;

    public InputManager inputManager;
    public RaceGameHandler rch;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        playPos.position = cameraHead.transform.position;
        playPos.parent = cameraHead.transform.parent;
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
        }

        if(rch.paused && !paused)
        {
            //move head to pausePos
            cameraHead.transform.position = pausedPos.position;
            paused = true;
        }else if(!rch.paused && paused)
        {
            //move head to playPos
            cameraHead.transform.position = playPos.position;
            paused = false;
        }

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
            cameraRig.transform.Rotate(Vector3.up * rotateSpeed * rotation * Time.deltaTime);
        }
        else
        {
            cameraRig.transform.Rotate(-Vector3.up * rotateSpeed * rotation * Time.deltaTime);
        }
        
    }

    public void SetActivate(bool active)
    {
        this.active = active;
    }

}
