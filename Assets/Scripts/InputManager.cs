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

#if UNITY_ANDROID
        bool vr = true;

#else
    bool vr = false;
#endif
        public bool pause()
        {
            bool pressed = false;

            return pressed;
        }

        public float gas()
        {
            if (vr)
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
            if (vr)
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
            if (vr)
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
            if (vr)
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
            if (vr)
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
            if (vr)
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

