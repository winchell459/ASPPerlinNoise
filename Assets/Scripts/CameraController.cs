using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.MouseLook m_MouseLook;
    [SerializeField] private Camera m_Camera;

    // Start is called before the first frame update
    void Start()
    {
        m_MouseLook.Init(transform, m_Camera.transform);
    }

    // Update is called once per frame
    void Update()
    {
        RotateView();
    }

    private void FixedUpdate()
    {
        m_MouseLook.UpdateCursorLock();
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }
}
