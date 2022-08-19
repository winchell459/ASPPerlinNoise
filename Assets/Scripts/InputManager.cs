using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.InputSystem;
#endif

//namespace InputManager
//{
public class InputManager : MonoBehaviour
{
#if UNITY_ANDROID

#endif
#if UNITY_ANDROID
    [SerializeField] private InputActionProperty leftHandTrigger, leftHandGrip, leftHandJoystick;
    [SerializeField] private InputActionProperty rightHandTrigger, rightHandGrip, rightHandJoystick, rightHand;
#endif


    public bool debugVR = false;
#if UNITY_ANDROID
    bool vr = true;

#else
    bool vr = false;
#endif
    bool leftHandGripDown = false, leftHandGripUp = false, leftHandGripPressed = false;
    bool leftHandTriggerDown = false, leftHandTriggerUp = false, leftHandTriggerPressed = false;
    bool rightHandTriggerDown = false, rightHandTriggerUp = false, rightHandTriggerPressed = false;

    private void Update()
    {
        if (vr && !debugVR)
        {
#if UNITY_ANDROID
            SetButton(leftHandGrip.action?.ReadValue<float>() ?? 0, ref leftHandGripDown, ref leftHandGripUp, ref leftHandGripPressed, 0);

            SetButton(rightHandTrigger.action?.ReadValue<float>() ?? 0, ref rightHandTriggerDown, ref rightHandTriggerUp, ref rightHandTriggerPressed, 0);

            SetButton(leftHandTrigger.action?.ReadValue<float>() ?? 0, ref leftHandTriggerDown, ref leftHandTriggerUp, ref leftHandTriggerPressed, 0);
#endif

        }
        else
        {
            SetButton(Input.GetMouseButton(0) ? 1 : 0, ref rightHandTriggerDown, ref rightHandTriggerUp, ref rightHandTriggerPressed, 0);
        }
    }

    private void SetButton(float value, ref bool down, ref bool up, ref bool pressed, float threshold)
    {
        if (Mathf.Abs(value) > threshold)
        {
            if (!pressed)
            {
                down = true;
            }
            else
            {
                down = false;
            }
            up = false;
            pressed = true;
        }
        else
        {
            if (pressed)
            {
                up = true;
            }
            else
            {
                up = false;
            }
            down = false;
            pressed = false;
        }
    }

    public bool PlayerSelectionDown()
    {
        return rightHandTriggerDown;
    }
    public bool PlayerSelection()
    {
        return rightHandTriggerPressed;
    }
    public bool PlayerSelectionUp()
    {
        return rightHandTriggerUp;
    }
    public bool UISelection()
    {
        if (debugVR) return Input.GetKeyDown(KeyCode.LeftShift);
        return leftHandTriggerDown;
    }
    public bool UISelectionUp()
    {
        if (debugVR) return Input.GetKeyUp(KeyCode.LeftShift);
        return leftHandTriggerUp;
    }
    public bool pause()
    {
        if (vr && !debugVR)
        {
            return leftHandGripDown;
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }



    }

    public float gas()
    {
        if (vr && !debugVR)
        {
#if UNITY_ANDROID
            return rightHandTrigger.action?.ReadValue<float>() ?? 0;
#endif
            return 0;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }

    public float brake()
    {
        if (vr && !debugVR)
        {
#if UNITY_ANDROID
            return rightHandGrip.action?.ReadValue<float>() ?? 0;
#endif
            return 0;
        }
        else
        {
            return Input.GetKey(KeyCode.Space) ? 1 : 0;
        }
    }

    public float horizontal()
    {
        if (vr && !debugVR)
        {
#if UNITY_ANDROID
            return leftHandJoystick.action?.ReadValue<Vector2>().x ?? 0;
#endif
            return 0;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float vertical()
    {
        if (vr && !debugVR)
        {
#if UNITY_ANDROID
            return leftHandJoystick.action?.ReadValue<Vector2>().y ?? 0;
#endif
            return 0;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }

    public float rotate()
    {
        if (vr && !debugVR)
        {
#if UNITY_ANDROID
            return rightHandJoystick.action?.ReadValue<Vector2>().x ?? 0;
#endif
            return 0;
        }
        else
        {
            return Input.mouseScrollDelta.x * 10;// Input.GetAxis("Vertical");
        }
    }

    public float zoom()
    {
        if (vr && !debugVR)
        {
#if UNITY_ANDROID
            return rightHandJoystick.action?.ReadValue<Vector2>().y ?? 0;
#endif
            return 0;
        }
        else
        {
            //Debug.Log(Input.mouseScrollDelta.y);
            return Input.mouseScrollDelta.y * 10;// Input.GetAxis("Vertical");
        }
    }
}
//}

