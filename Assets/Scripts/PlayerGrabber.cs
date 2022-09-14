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
    public AnimalSelection animalSelection;

    public RaceGameHandler gameHandler;

    public bool placingPlayer, placingAI;


    public void ClearVegetation()
    {
        //foreach (GameObject vege in vegetation) Destroy(vege);

        for (int i = vegetationSelection.vegetation.Count - 1; i >= 0; i -= 1)
        {
            GameObject vege = vegetationSelection.vegetation[i];
            vegetationSelection.vegetation.Remove(vege);
            GameObject.Destroy(vege.gameObject);
        }
    }

    public void ClearAnimals()
    {
        animalSelection.RemoveAllAnimals();
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

                    this.vegetationSelection.vegetation.Add(vegetation);
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
                        Transform startPos = FindObjectOfType<RaceGameHandler>().playerStartPos;
                        if (placingPlayer)
                        {
                            car = player.transform;
                            if(ai) otherCar = ai.transform;
                        }
                        else if (placingAI)
                        {
                            car = ai.transform;
                            otherCar = player.transform;
                            startPos = FindObjectOfType<RaceGameHandler>().aiStartPos;
                        }
                        car.position = hit.point;
                        car.forward = Utility.CopyForward(otherCar);
                        startPos.position = car.position;
                        startPos.forward = car.forward;
                        if (placingAI && FindObjectOfType<AIFollow>()) car.GetComponent<AIFollow>().RestartTrack();
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
            if (interruptCarUserControl) interruptCarUserControl.interruptCarUserControl = false;
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
            if(interruptCarUserControl)interruptCarUserControl.interruptCarUserControl = true;
            hook.GetComponent<Rigidbody>().MovePosition(grabber.position + player.transform.up * hookLength + grabber.forward.normalized * radius);
            //player.GetComponent<Rigidbody>().useGravity = !player.GetComponent<Rigidbody>().useGravity;
        }

    }
}

[System.Serializable]
public class VegetationSelection
{
    public List<GameObject> vegetation = new List<GameObject>();
    public GameObject[] prefab;
    [SerializeField]
    public RangeAttribute scaleMultiplierRange = new RangeAttribute(2.5f, 10);
    public GameObject Random()
    {
        int index = UnityEngine.Random.Range(0, prefab.Length);
        return prefab[index];
    }

    public GameObject Random(System.Random random)
    {
        int index = random.Next(0, prefab.Length);
        return prefab[index];
    }

    public int vegetationCount = 50;
    public float minHeight = 5, maxHeight = 10;
    public float maxRadius = 200;
    public void RandomPlacement(int seed, Vector2 origin, Vector3[,] map)
    {
        System.Random random = new System.Random(seed);
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        int vegetationAdded = 0;
        while (vegetationAdded < vegetationCount)
        {
            int i = random.Next(0, width);
            int j = random.Next(0, height);
            if (Vector2.Distance(origin, new Vector2(map[i, j].x, map[i, j].z)) < maxRadius)
            {
                GameObject veg = GameObject.Instantiate(Random(random));
                veg.transform.position = map[i, j];

                float randomScale = ((float)random.NextDouble()) *(scaleMultiplierRange.max - scaleMultiplierRange.min) + scaleMultiplierRange.min;
                veg.transform.localScale *= randomScale;

                float randomRotation = random.Next(0, 360);
                veg.transform.Rotate(new Vector3(0, randomRotation, 0));

                vegetation.Add(veg);
                vegetationAdded += 1;
            }
            
        }
    }
}

[System.Serializable]
public class AnimalSelection
{
    public Animal chickenPrefab, spiderPrefab;
    
    public List<Spider> spiders;
    public List<Chicken> chickens;

    public void AddAnimal(Animal animal)
    {
        if (animal.animalType == Animal.AnimalType.spider)
        {
            spiders.Add((Spider)animal);
            GameObject.FindObjectOfType<HUDHandler>().SetCounts(0, 1);
        }
        else if (animal.animalType == Animal.AnimalType.chicken)
        {
            chickens.Add((Chicken)animal);
            GameObject.FindObjectOfType<HUDHandler>().SetCounts(1, 0);
        }
    }

    public void RemoveAnimal(Animal animal)
    {
        if (animal.animalType == Animal.AnimalType.spider)
        {
            spiders.Remove((Spider)animal);
            GameObject.FindObjectOfType<HUDHandler>().SetCounts(0, -1);
        }

        else if (animal.animalType == Animal.AnimalType.chicken)
        {
            chickens.Remove((Chicken)animal);
            GameObject.FindObjectOfType<HUDHandler>().SetCounts(-1, 0);
        }
    }

    public void RemoveAllAnimals()
    {
        for (int i = spiders.Count - 1; i >= 0; i-=1 )
        {
            Spider spider = spiders[i];
            spiders.Remove(spider);
            GameObject.Destroy(spider.gameObject);
        }
        for (int i = chickens.Count - 1; i >= 0; i -= 1)
        {
            Chicken chicken = chickens[i];
            chickens.Remove(chicken);
            GameObject.Destroy(chicken.gameObject);
        }
    }

    public int chickenCount = 10, spiderCount = 3;
    public float maxRadius = 200;
    public void RandomPlacement(int seed, Vector2 origin, Vector3[,] map)
    {
        System.Random random = new System.Random(seed);
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        float chickensAdded = 0;
        float spidersAdded = 0;

        while(chickensAdded < chickenCount)
        {
            int i = random.Next(0, width);
            int j = random.Next(0, height);
            
            if(Vector2.Distance(origin, new Vector2(map[i,j].x, map[i,j].z)) < maxRadius)
            {
                Chicken chicken = GameObject.Instantiate(chickenPrefab, map[i, j], Quaternion.identity).GetComponent<Chicken>();
                AddAnimal(chicken);
                chickensAdded += 1;
            }
            
        }

        while(spidersAdded < spiderCount)
        {
            int i = random.Next(0, width);
            int j = random.Next(0, height);
            if (Vector2.Distance(origin, new Vector2(map[i, j].x, map[i, j].z)) < maxRadius)
            {
                Spider spider = GameObject.Instantiate(spiderPrefab, map[i, j], Quaternion.identity).GetComponent<Spider>();
                AddAnimal(spider);
                spidersAdded += 1;
            }
        }
    }
}
