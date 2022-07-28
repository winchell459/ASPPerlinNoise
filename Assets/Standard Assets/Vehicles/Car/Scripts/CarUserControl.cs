using System;
using UnityEngine;
using UnityEngine.InputSystem;


//namespace UnityEngine.XR.Interaction.Toolkit
namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        [SerializeField] public InputActionProperty leftHand;
        [SerializeField] public InputActionProperty rightHand;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
#if UNITY_ANDROID
            float h = 0;
            float v = 0;
            Vector2 leftHandValue = leftHand.action?.ReadValue<Vector2>() ?? Vector2.zero;
            Vector2 rightHandValue = rightHand.action?.ReadValue<Vector2>() ?? Vector2.zero;
            v = leftHandValue.y;
            h = rightHandValue.x;
#else
            // pass the input to the car!
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
#endif
#if !MOBILE_INPUT
            float handbrake = Input.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
