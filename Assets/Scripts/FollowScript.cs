using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowScript : MonoBehaviour
{

    public GameObject scaleSlider;

    private void Start()
    {
        if (this.gameObject.name == "drag_sand(Clone)")
        {
            this.gameObject.transform.localScale = new Vector3(scaleSlider.GetComponent<Slider>().value * 15, scaleSlider.GetComponent<Slider>().value * 15, 0);
        }
        else if (this.gameObject.name == "StoneWayDrag(Clone)")
        {
            this.gameObject.transform.localScale = new Vector3(scaleSlider.GetComponent<Slider>().value * 15, scaleSlider.GetComponent<Slider>().value * 15, 0);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(scaleSlider.GetComponent<Slider>().value * 200, scaleSlider.GetComponent<Slider>().value * 200, 0);
        }
    }

    private void Update()
    {
        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = worldPos;
    }
}
