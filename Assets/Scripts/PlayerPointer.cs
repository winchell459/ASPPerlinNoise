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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(RightHandController.position, RightHandController.forward), out hit, 500, layerMask))
        {
            if (hit.transform.CompareTag("Track"))
            {
                StartCoroutine(SmoothRigWieght(1));
                RightHand_target.position = hit.transform.position;
            }
            else
            {
                StartCoroutine(SmoothRigWieght(0));
            }
        }
        else
        {
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
