using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float ScrollSpeed;

    private Camera camera;
    private float StartSize;
    [SerializeField]
    private float maxZoom;

    private Vector3 dragOrigin;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        StartSize = camera.orthographicSize;

       
    }

    // Update is called once per frame
    void Update()
    {
        HandleScrolling();
        HandleMouseDrag();
    }

    private void HandleScrolling()
    {  
        if ((camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed < StartSize) && (camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed > maxZoom))
        {
            camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed;
        }
        else if (camera.orthographicSize < StartSize / 3)
        {
            camera.orthographicSize = maxZoom + 1;
        }
    }

    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);

            //print("origin " + dragOrigin + " new Position " + cam.ScreenToWorldPoint(Input.mousePosition) + " = difference " + difference);

            camera.transform.position += difference;
        }
    }


}
