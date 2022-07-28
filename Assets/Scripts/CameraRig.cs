using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraRig : MonoBehaviour
{

    [SerializeField] public InputActionProperty leftHand;
    [SerializeField] public InputActionProperty rightHand;

    public GameObject cameraHead, cameraRig;
    [SerializeField] private bool active;
    public float zoomSpeed = 10;
    public float rotateSpeed = 10;
    public bool invertRotate;
    public float leftThreshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
#if UNITY_ANDROID

            Vector2 leftHandValue = leftHand.action?.ReadValue<Vector2>() ?? Vector2.zero;
            Vector2 rightHandValue = rightHand.action?.ReadValue<Vector2>() ?? Vector2.zero;
            float right = rightHandValue.y;
            float left = leftHandValue.x;
            Zoom(right);
            if(leftThreshold < Mathf.Abs(left)) Pan(left);
#else
            
#endif
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
