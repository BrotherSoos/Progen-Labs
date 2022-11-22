using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float ScrollSpeed = 2000;

    private Camera camera;
    private float StartSize;

    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;

    private bool drag = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        StartSize = camera.orthographicSize;

        ResetCamera = camera.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (camera.orthographicSize <= StartSize)
        {
            camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed;  
        } else
        {
            camera.orthographicSize = StartSize;
        }

        if(Input.GetMouseButton(0))
        {
            Difference = camera.ScreenToWorldPoint(Input.mousePosition) - camera.transform.position;
            if (drag == false)
            {
                drag = true;
                Origin = camera.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                drag = false;
            }

            if (drag)
            {
                Debug.Log(Origin - Difference);
                camera.transform.position = Origin - Difference;
            }

            if (Input.GetMouseButton(1))
            {
                camera.transform.position = ResetCamera;
            }
        }
        
    }
}
