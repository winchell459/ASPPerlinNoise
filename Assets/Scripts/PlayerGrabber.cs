using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabber : MonoBehaviour
{
    public Transform grabber;
    public GameObject player;
    private bool hasPlayer;
    private Vector3 grabOffset;
    
    private float radius;
    public HingeJoint hook;
    public float hookLength = 1;
    public InputManager inputManager;
    public CameraRig interruptCameraRig;
    public CarUserControl interruptCarUserControl;
    public LayerMask layerMask;

    public bool debugging;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
  
        if (inputManager.PlayerSelectionDown())
        {
            
            RaycastHit hit;
            if (Physics.Raycast(new Ray(grabber.position, grabber.forward), out hit, 100, layerMask))
            {
                
                if (hit.transform.CompareTag("Player"))
                {
                    
                    hasPlayer = true;
                    //player = hit.transform.parent.parent.gameObject;
                    
                    hook.transform.position = player.transform.position + player.transform.up * hookLength;
                    hook.connectedBody = player.GetComponent<Rigidbody>();
                    radius = Vector3.Distance(player.transform.position, grabber.position);
                    //player.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
        else if (hasPlayer && inputManager.PlayerSelectionUp())
        {
            player.GetComponent<Rigidbody>().useGravity = true;
            hasPlayer = false;
            hook.connectedBody = null;
            interruptCameraRig.interruptCameraRig = false;
            interruptCarUserControl.interruptCarUserControl = false;
        }

    }

    private void debug()
    {
        if (debugging)
        {
            GameObject debugCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            debugCube.transform.position = grabber.position;
        }
        
    }

    private void FixedUpdate()
    {
        if (hasPlayer && inputManager.PlayerSelection())
        {
            interruptCameraRig.interruptCameraRig = true;
            interruptCarUserControl.interruptCarUserControl = true;
            hook.GetComponent<Rigidbody>().MovePosition(grabber.position + player.transform.up * hookLength + grabber.forward.normalized * radius);
            //player.GetComponent<Rigidbody>().useGravity = !player.GetComponent<Rigidbody>().useGravity;
        }

    }
}
