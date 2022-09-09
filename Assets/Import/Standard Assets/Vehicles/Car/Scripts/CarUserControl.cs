using System;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityStandardAssets.Vehicles.Car;

//namespace UnityEngine.XR.Interaction.Toolkit
namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        //[SerializeField] public InputActionProperty leftHand;
        //[SerializeField] public InputActionProperty rightHand;
        //public InputManager.InputManager inputManager;
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            float h = 0;
            float v = 0;

#if !MOBILE_INPUT
            float handbrake = Input.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
