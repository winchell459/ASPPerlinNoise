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
    [SerializeField] private InputActionProperty buttonA, buttonB, buttonX, buttonY;
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
    bool buttonADown = false, buttonAUp = false, buttonAPressed = false;
    bool buttonBDown = false, buttonBUp = false, buttonBPressed = false;
    bool buttonXDown = false, buttonXUp = false, buttonXPressed = false;
    bool buttonYDown = false, buttonYUp = false, buttonYPressed = false;

    bool rightHandGripDown = false, rightHandGripUp = false, rightHandGripPressed = false;

    private void Update()
    {
        if (vr && !debugVR)
        {
#if UNITY_ANDROID
            SetButton(leftHandGrip.action?.ReadValue<float>() ?? 0, ref leftHandGripDown, ref leftHandGripUp, ref leftHandGripPressed, 0);

            SetButton(rightHandTrigger.action?.ReadValue<float>() ?? 0, ref rightHandTriggerDown, ref rightHandTriggerUp, ref rightHandTriggerPressed, 0);

            SetButton(leftHandTrigger.action?.ReadValue<float>() ?? 0, ref leftHandTriggerDown, ref leftHandTriggerUp, ref leftHandTriggerPressed, 0);

            SetButton(buttonA.action.IsPressed() ? 1 : 0, ref buttonADown, ref buttonAUp, ref buttonAPressed, 0);
            SetButton(buttonB.action.IsPressed() ? 1 : 0, ref buttonBDown, ref buttonBUp, ref buttonBPressed, 0);
            SetButton(buttonX.action.IsPressed() ? 1 : 0, ref buttonXDown, ref buttonXUp, ref buttonXPressed, 0);
            SetButton(buttonY.action.IsPressed() ? 1 : 0, ref buttonYDown, ref buttonYUp, ref buttonYPressed, 0);

            SetButton(rightHandGrip.action?.ReadValue<float>() ?? 0, ref rightHandGripDown, ref rightHandGripUp, ref rightHandGripPressed, 0);
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
        return leftHandGripDown;
        //return rightHandTriggerDown;
    }
    public bool PlayerSelection()
    {
        return leftHandGripPressed;
    }
    public bool PlayerSelectionUp()
    {
        return leftHandGripUp;
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
            return XDown();
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }
    public bool BUp()
    {
        return buttonBUp;
    }
    public bool BDown()
    {
        return buttonBDown;
    }

    public bool BPressed()
    {
        return buttonBPressed;
    }
    public bool AUp()
    {
        return buttonAUp;
    }
    public bool ADown()
    {
        return buttonADown;
    }

    public bool APressed()
    {
        return buttonAPressed;
    }

    public bool XPressed()
    {
        return buttonXPressed;
    }
    public bool XDown()
    {
        return buttonXDown;
    }

    public bool rightGripUp()
    {
        return rightHandGripUp;
    }
    public bool rightGripDown()
    {
        return rightHandGripDown;
    }

    public bool rightGripPressed()
    {
        return rightHandGripPressed;
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

            if (BDown() && !ADown()) return 1;
            else if (!BDown() && ADown()) return -1;
            else return 0;

            //return rightHandJoystick.action?.ReadValue<Vector2>().y ?? 0;
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

