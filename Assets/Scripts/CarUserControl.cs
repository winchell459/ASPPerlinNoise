using System;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityStandardAssets.Vehicles.Car;

//namespace UnityEngine.XR.Interaction.Toolkit
//namespace UnityStandardAssets.Vehicles.Car
//{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        //[SerializeField] public InputActionProperty leftHand;
        //[SerializeField] public InputActionProperty rightHand;
        public InputManager inputManager;
    public bool interruptCarUserControl;
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


    private void FixedUpdate()
    {
        if (interruptCarUserControl) return;
        float steering = inputManager.horizontal();
        float accel = inputManager.gas();
        float footbreak = -inputManager.brake();

        if (footbreak > 0.00001f && accel > 0.001f) accel = footbreak;

#if !MOBILE_INPUT
            
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(steering, accel, footbreak, 0f);
#endif
        }
    }
//}
