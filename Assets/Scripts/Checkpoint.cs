using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Material regular, start;
    public MeshRenderer renderer;
    
    public void Activate(bool start)
    {
        if (start) renderer.material = this.start;
        else renderer.material = regular;

        gameObject.SetActive(true);

        
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
