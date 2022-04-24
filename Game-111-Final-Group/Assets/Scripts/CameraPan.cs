using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPan : MonoBehaviour
{
    public float speed = 2;
    private CinemachineVirtualCamera vCam;

    private PlayerInput pInput;
    private Vector2 direction;
    private float offsetRate,resetRate;
    private CinemachineTransposer transposer;
    private Vector3 origOffset,cacheOffset;
    
    private void Awake()
    {
        pInput = new PlayerInput();
        pInput.Enable();
        vCam = GetComponent<CinemachineVirtualCamera>();
        transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        origOffset = transposer.m_FollowOffset;     
    }
    private void Update()
    {
        direction = pInput.Player.Camera.ReadValue<Vector2>();

        if(direction.magnitude > 0)
        {
            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset,
                                        new Vector3(direction.x * 2, 0, direction.y * 2) + origOffset,
                                        Time.deltaTime * speed);
        } 
        else
            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, origOffset, Time.deltaTime * speed);         
    }
}
