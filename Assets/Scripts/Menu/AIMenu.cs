using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AIMenu : MonoBehaviour
{
    public UnityStandardAssets.Vehicles.Car.CarController vehicle;
    public AIFollow aiFollow;

    public AIFollow.FollowTypes followType;

    public float speed = 40;
    public float followRange = 40;
    public float waypointDistance = 10;

    public float avoidanceDistance = 5;
    public float avoidanceAngle = 0;
    public bool avoidanceAngleGlobal = false;

    public Text speedText, followRangeText, waypointDistanceText, avoidanceDistanceText, avoidanceAngleText, followTypeText;
    public Toggle avoidanceAngleGlobalToggle;

    private void Start()
    {
        
        

    }

    public void UpdateButton()
    {
        UpdateMenu();
    }

    public void UpdateMenu()
    {
        speed = int.Parse(speedText.text);
        followRange = int.Parse(followRangeText.text);
        waypointDistance = int.Parse(waypointDistanceText.text);
        avoidanceDistance = int.Parse(avoidanceDistanceText.text);
        avoidanceAngle = int.Parse(avoidanceAngleText.text);
        avoidanceAngleGlobal = avoidanceAngleGlobalToggle.isOn;
        followType = GetFollowType();
        UpdateAI();
    }

    public void UpdateAI()
    {
        vehicle.TopSpeed = speed;
        aiFollow.followRange = followRange;
        aiFollow.waypointDistance = waypointDistance;
        aiFollow.avoidanceDistance = avoidanceDistance;
        aiFollow.avoidanceAngle = avoidanceAngle;
        aiFollow.avoidanceAngleGlobal = avoidanceAngleGlobal;
        aiFollow.followType = followType;
    }

    private AIFollow.FollowTypes GetFollowType()
    {
        switch (followTypeText.text)
        {
            case "TrackBuilding":
                return AIFollow.FollowTypes.TrackBuilding;
            case "Racing":
                return AIFollow.FollowTypes.Racing;
            case "Avoiding":
                return AIFollow.FollowTypes.Avoiding;
            default:
                return AIFollow.FollowTypes.Following;
        }
    }
}
