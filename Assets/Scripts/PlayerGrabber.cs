using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabber : MonoBehaviour
{
    public Transform grabber;
    public GameObject player, ai;
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

    public VegetationSelection vegetationSelection;
    public List<GameObject> vegetation = new List<GameObject>();

    public RaceGameHandler gameHandler;

    public bool placingPlayer, placingAI;


    public void ClearVegetation()
    {
        foreach (GameObject vege in vegetation) Destroy(vege);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
  
        if (!gameHandler.paused && inputManager.PlayerSelectionDown())
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
                } else if (hit.transform.CompareTag("AI"))
                {
                    hasPlayer = true;
                    //player = hit.transform.parent.parent.gameObject;

                    hook.transform.position = ai.transform.position + ai.transform.up * hookLength;
                    hook.connectedBody = ai.GetComponent<Rigidbody>();
                    radius = Vector3.Distance(ai.transform.position, grabber.position);
                }
                else if (hit.transform.CompareTag("Track") && FindObjectOfType<CameraRig>().followType != CameraRig.FollowTypes.car)
                {
                    GameObject vegetation = Instantiate(vegetationSelection.Random());
                    vegetation.transform.position = hit.point;

                    float randomScale = Random.Range(vegetationSelection.scaleMultiplierRange.min, vegetationSelection.scaleMultiplierRange.max);
                    vegetation.transform.localScale *= randomScale;

                    float randomRotation = Random.Range(0, 360);
                    vegetation.transform.Rotate(new Vector3(0, randomRotation, 0));

                    this.vegetation.Add(vegetation);
                }
                else if (hit.transform.CompareTag("Vegetation"))
                {
                    Destroy(hit.transform.gameObject);
                }else if (hit.transform.CompareTag("Checkpoint") && ai.GetComponent<AIFollow>().track.trackComplete)
                {
                    ai.GetComponent<AIFollow>().RemoveWaypoint(hit.transform);
                }
            }
        }else if(gameHandler.paused && inputManager.PlayerSelectionDown())
        {
            if(placingAI || placingPlayer)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Ray(grabber.position, grabber.forward), out hit, 10000, layerMask))
                {

                    if (hit.transform.CompareTag("Track"))
                    {
                        Transform car = null;
                        Transform otherCar = null;
                        if (placingPlayer)
                        {
                            car = player.transform;
                            otherCar = ai.transform;
                        }
                        else if (placingAI)
                        {
                            car = ai.transform;
                            otherCar = player.transform;
                        }
                        car.position = hit.point;
                        car.forward = Utility.CopyForward(otherCar);
                        if (placingAI) car.GetComponent<AIFollow>().RestartTrack();
                        //gameHandler.ResetPlayerVertical(car);
                    }
                }
                placingAI = false;
                placingPlayer = false;
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

[System.Serializable]
public class VegetationSelection
{
    public GameObject[] prefab;
    [SerializeField]
    public RangeAttribute scaleMultiplierRange = new RangeAttribute(2.5f, 10);
    public GameObject Random()
    {
        int index = UnityEngine.Random.Range(0, prefab.Length);
        return prefab[index];
    }
}
