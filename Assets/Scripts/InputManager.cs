using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//namespace InputManager
//{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionProperty leftHandTrigger, leftHandGrip, leftHandJoystick;
        [SerializeField] private InputActionProperty rightHandTrigger, rightHandGrip, rightHandJoystick, rightHand;

    public bool debugVR = false;
#if UNITY_ANDROID
        bool vr = true ;

#else
    bool vr = false;
#endif
    bool leftHandGripDown = false, leftHandGripUp = false, leftHandGripPressed = false;
    bool leftHandTriggerDown = false, leftHandTriggerUp = false, leftHandTriggerPressed = false;

    private void Update()
    {
        if (vr && !debugVR)
        {
            float value =  leftHandGrip.action?.ReadValue<float>() ?? 0;
            SetButton(value, ref leftHandGripDown, ref leftHandGripUp, ref leftHandGripPressed, 0);

            SetButton(leftHandTrigger.action?.ReadValue<float>() ?? 0, ref leftHandTriggerDown, ref leftHandTriggerUp, ref leftHandTriggerPressed, 0);
        }
        else
        {
            
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
    public bool UISelection()
    {
        return leftHandTriggerDown;
    }
    public bool UISelectionUp()
    {
        return leftHandTriggerUp;
    }
    public bool pause()
        {
            bool pressed = leftHandGripDown;

            return pressed;
        }

        public float gas()
        {
            if (vr && !debugVR)
            {
                return rightHandTrigger.action?.ReadValue<float>() ?? 0;
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
                return rightHandGrip.action?.ReadValue<float>() ?? 0;
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
                return leftHandJoystick.action?.ReadValue<Vector2>().x ?? 0;
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
                return leftHandJoystick.action?.ReadValue<Vector2>().y ?? 0;
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
                return rightHandJoystick.action?.ReadValue<Vector2>().x ?? 0;
            }
            else
            {
                return 0;// Input.GetAxis("Vertical");
            }
        }

        public float zoom()
        {
            if (vr && !debugVR)
            {
                return rightHandJoystick.action?.ReadValue<Vector2>().y ?? 0;
            }
            else
            {
                return 0;// Input.GetAxis("Vertical");
            }
        }
    }
//}

