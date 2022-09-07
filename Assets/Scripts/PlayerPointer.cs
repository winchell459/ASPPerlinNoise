using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerPointer : MonoBehaviour
{
    public Rig rig;
    public Transform RightHand_target;
    public Transform RightHandController;
    public InputManager inputManager;
    public LayerMask layerMask;
    public float weightSmoothing = 1;
    private float weightTarget = 1;

    protected RaycastHit hit;
    protected bool targetFound = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetPointer();

    }

    protected virtual void SetPointer()
    {
        
        if (Physics.Raycast(new Ray(RightHandController.position, RightHandController.forward), out hit, 500, layerMask))
        {
            if (hit.transform.CompareTag("Track"))
            {
                targetFound = true;
                StartCoroutine(SmoothRigWieght(1));
                RightHand_target.position = hit.point;
                RightHand_target.forward = Vector3.up;
                RightHand_target.right = RightHand_target.position - transform.position;

            }
            else
            {
                targetFound = false;
                StartCoroutine(SmoothRigWieght(0));
            }
        }
        else
        {
            targetFound = false;
            StartCoroutine(SmoothRigWieght(0));
        }
    }

    IEnumerator SmoothRigWieght(float rigWeight)
    {
        weightTarget = rigWeight;
        while(Mathf.Abs(rig.weight - weightTarget) > 0.01f)
        {
            rig.weight = Mathf.Lerp(weightTarget, rig.weight, weightSmoothing * Time.deltaTime);
            yield return null;
        }
        rig.weight = weightTarget;
    }
}
