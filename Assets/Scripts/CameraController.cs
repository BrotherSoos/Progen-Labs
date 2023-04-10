using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float ScrollSpeed;

    private Camera camera;
    private float StartSize;
    [SerializeField]
    private float maxZoom;

    private Vector3 dragOrigin;
    private int UIlayer;
    private bool uiHover;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        StartSize = camera.orthographicSize;

        UIlayer = LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update()
    {
        uiHover = IsPointerOverUIElement();
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
        if (Input.GetMouseButtonDown(2))
            dragOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
       

        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);

            camera.transform.position += difference;
        }
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UIlayer)
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }


}
