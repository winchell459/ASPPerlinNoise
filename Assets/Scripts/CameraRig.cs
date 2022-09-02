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
    public Transform playPos { get { return getPlayerPos(); } }
    public Transform carPos, pausedPos, aiPos;
    public Vector3 carOffset, pausedOffset, aiOffset;
    [SerializeField] private bool active;
    public float zoomSpeed = 10;
    public float rotateSpeed = 10;
    public bool invertRotate;
    public float leftThreshold = 0.1f;

    public InputManager inputManager;
    public RaceGameHandler rch;
    private bool paused = false;
    [SerializeField] private bool followPlayerForward;

    private Vector3 pauseHeadForward;
    public bool interruptCameraRig;

    public FollowTypes followType;
    public enum FollowTypes
    {
        car,
        ai,
        free
    }
    Transform getPlayerPos()
    {
        switch (followType)
        {
            case FollowTypes.car:
                return carPos;

            case FollowTypes.ai:
                return aiPos;
            default:
                return pausedPos;
        }
    }

    void SetPlayerOffset(Vector3 offset)
    {
        switch (followType)
        {
            case FollowTypes.car:
                carOffset = offset;
                break;

            case FollowTypes.ai:
                aiOffset = offset;
                break;
            default:
                pausedOffset = offset;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //playPos.position = cameraHead.transform.position;
        //playPos.parent = cameraHead.transform.parent;
    }
    private void FixedUpdate()
    {
        if(!interruptCameraRig) cameraRig.transform.position = new Vector3(playPos.position.x, cameraRig.transform.position.y, playPos.position.z);
        if ((rch.GetRaceStarted() && !paused) && followPlayerForward) cameraRig.transform.forward = forward(playPos.forward);
        //else if (paused || !rch.GetRaceStarted())
        //{
        //    SetPlayerPosPauseMenu();
        //}
    }
    // Update is called once per frame
    void Update()
    {

        if (inputManager.BDown()) FindObjectOfType<HUDHandler>().Debug("buttonB pressed");
        if (inputManager.XDown()) FindObjectOfType<HUDHandler>().Debug("buttonX pressed");

        if (rch.paused && !paused)
        {
            //move head to pausePos
            SetPlayerPosPauseMenu();
            paused = true;
            //followPlayerForward = !followPlayerForward;

            //pauseHeadForward = cameraSwivel.transform.forward;
        }
        else if (!rch.paused && paused)
        {
            //move head to playPos
            SetPlayerPosCar();
            paused = false;

            //cameraSwivel.transform.forward = pauseHeadForward;
        }

        if(!paused && inputManager.UISelection() || inputManager.UISelectionUp())
        {
            followPlayerForward = !followPlayerForward;
            if (followPlayerForward)
            {
                Vector3 headForward = cameraSwivel.transform.forward;
                cameraRig.transform.forward = forward(playPos.forward);
                cameraSwivel.transform.forward = headForward;
            }
        }

        if (active && !paused)
        {
            float right = inputManager.zoom();
            float left = inputManager.rotate();
            Zoom(right);
            if (leftThreshold < Mathf.Abs(left)) Pan(left);

            //cameraRig.transform.position = new Vector3(playPos.position.x, cameraRig.transform.position.y, playPos.position.z);
            //if (!paused && followPlayerForward) cameraRig.transform.forward = forward(playPos.forward);
        }
    }

    
    
    private Vector3 forward(Vector3 target)
    {
        return new Vector3(target.x, 0, target.z).normalized;
    }

    private void Zoom(float delta)
    {
        Vector3 direction = cameraHead.transform.position - cameraSwivel.transform.position;
        float length = direction.magnitude;
        float newLength = Mathf.Clamp(length - delta * zoomSpeed /** Time.deltaTime*/, 0.1f, float.MaxValue);
        cameraHead.transform.position = cameraSwivel.transform.position + newLength * direction.normalized;

        SetPlayerOffset(cameraHead.transform.localPosition);
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

    private void SetPlayerPosPauseMenu()
    {
        cameraSwivel.transform.forward = Vector3.forward;
        cameraHead.transform.position = pausedPos.position;
        Debug.Log(cameraHead.transform.position);
        
    }

    private void SetPlayerPosCar()
    {
        cameraRig.transform.position = playPos.position;
        cameraHead.transform.localPosition = carOffset;
        cameraSwivel.transform.forward = forward(playPos.forward);
        

    }

    private void SetPlayerPosAICar()
    {

    }
}
