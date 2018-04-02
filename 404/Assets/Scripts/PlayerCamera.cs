using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    Transform cameraPointLeft;

    [SerializeField]
    Transform cameraPointRight;

    public Transform CameraPointLeft { get { return cameraPointLeft; } }

    public Transform CameraPointRight { get { return cameraPointRight; } }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
