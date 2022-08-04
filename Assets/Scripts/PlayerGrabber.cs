using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabber : MonoBehaviour
{
    public Transform grabber;
    public GameObject player;
    private float mouseY;
    public float moveSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("left clicked");
            RaycastHit hit;
            if(Physics.Raycast(new Ray(grabber.position, grabber.forward), out hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    player = hit.transform.gameObject;
                    mouseY = Input.mousePosition.y;
                }
            }
        }else if (player && Input.GetMouseButton(0))
        {
            float dy = mouseY - Input.mousePosition.y;
            player.transform.position += Vector3.up * dy * moveSpeed;
        }else if (player)
        {
            player = null;
        }
    }
}
