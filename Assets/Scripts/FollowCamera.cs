using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 20f;
    [SerializeField] CinemachineVirtualCamera vCam = null;
    [SerializeField] float zoomSpeed = 7f;
    Rigidbody myRigidbody;
    Transform character;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (character == null) return;

        transform.position = character.position;

        if(Input.GetMouseButton(2))
        {
            Vector3 rotation = new Vector3(0f, Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed, 0f);
            myRigidbody.AddTorque(rotation,ForceMode.Impulse);
            //transform.Rotate(0f, Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed, 0f);

        }
        CinemachineFramingTransposer camCtrl = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        float camDistance = camCtrl.m_CameraDistance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        camCtrl.m_CameraDistance =  Mathf.Clamp(camDistance, 7f, 30f);
    }

    public void SetFollow(Transform target)
    {
        character = target;
    }

}
