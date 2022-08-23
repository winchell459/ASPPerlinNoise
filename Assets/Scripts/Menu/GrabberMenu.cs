using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabberMenu : MonoBehaviour
{
    public bool placingPlayer;
    public bool placingAI;
    public PlayerGrabber playerGrabber;

    //CameraRigMenu
    public Toggle CameraRigPlayerToggle, CameraRigAIToggle, CameraRigFreeToggle;
    public CameraRig cameraRig;

    //AIMenu
    public AIMenu ai;
    

    
    private void Start()
    {
        
    }
    public void PlacePlayerButton()
    {
        placingPlayer = true;
        playerGrabber.placingPlayer = placingPlayer;
        FindObjectOfType<PauseMenu>().HideMenus(true);
        FindObjectOfType<PerlinNoiseMenu>().AddMeshButton();
    }
    public void PlaceAIButton()
    {
        placingAI = true;
        FindObjectOfType<PauseMenu>().HideMenus(true);
        playerGrabber.placingAI = placingAI;
        FindObjectOfType<PerlinNoiseMenu>().AddMeshButton();
    }

    public void CameraRigPlayerToggleOnChanged()
    {
        if (CameraRigPlayerToggle.isOn)
        {
            CameraRigAIToggle.isOn = false;
            CameraRigFreeToggle.isOn = false;
            cameraRig.followType = CameraRig.FollowTypes.car;
        }
        //else
        //{
        //    CameraRigPlayerToggle.isOn = true;
        //}
    }
    public void CameraRigAIToggleOnChanged()
    {
        if (CameraRigAIToggle.isOn)
        {
            CameraRigPlayerToggle.isOn = false;
            CameraRigFreeToggle.isOn = false;
            cameraRig.followType = CameraRig.FollowTypes.ai;
        }
        //else
        //{
        //    CameraRigAIToggle.isOn = true;
        //}
    }
    public void CameraRigFreeToggleOnChanged()
    {
        if (CameraRigFreeToggle.isOn)
        {
            CameraRigAIToggle.isOn = false;
            CameraRigPlayerToggle.isOn = false;
            cameraRig.followType = CameraRig.FollowTypes.free;
        }
        //else
        //{
        //    CameraRigFreeToggle.isOn = true;
        //}
    }
}


