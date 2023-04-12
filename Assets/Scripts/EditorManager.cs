using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    public ItemController[] itemButtons;
    public GameObject[] itemPrefabs;
    public int currentButtonPressed;
    public GameObject scaleSlider;
    public GameObject[] itemImage;
    private float mouseXStart;
    private float mouseXEnd;
    private float mouseYStart;
    private float mouseYEnd;

    private void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            mouseXStart = Input.mousePosition.x;
            mouseYStart = Input.mousePosition.y;

            Vector2 screenPos = new Vector2(mouseXStart, mouseYStart);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            if (currentButtonPressed == 16)
            {
                GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                go.transform.localScale = new Vector3(scaleSlider.GetComponent<Slider>().value * 10, scaleSlider.GetComponent<Slider>().value * 10, 0);
                go.GetComponent<Renderer>().sortingOrder = 5;
            }
            else
            {
                GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                go.transform.localScale = new Vector3(200 * scaleSlider.GetComponent<Slider>().value, 200 * scaleSlider.GetComponent<Slider>().value, 0);
                go.GetComponent<Renderer>().sortingOrder = 5;
            }


            Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseYEnd = Input.mousePosition.y;
            mouseXEnd = Input.mousePosition.x;
            if (((Mathf.Abs(mouseXEnd) - Mathf.Abs(mouseXStart)) > 10) || ((Mathf.Abs(mouseYEnd) - Mathf.Abs(mouseYStart)) > 10)) {
                if ((Mathf.Abs(mouseXEnd) - Mathf.Abs(mouseXStart)) > (Mathf.Abs(mouseYEnd) - Mathf.Abs(mouseYStart)))
                {
                    for (int i = 0; i <= mouseXEnd - mouseXStart; i += 10)
                    {
                        Vector2 screenPos = new Vector2(mouseXStart + (i), mouseYStart);
                        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                        if (currentButtonPressed == 16)
                        {
                            GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                            go.transform.localScale = new Vector3(20 * scaleSlider.GetComponent<Slider>().value, 10 * scaleSlider.GetComponent<Slider>().value, 0);
                            go.GetComponent<Renderer>().sortingOrder = 5;
                        }
                        else
                        {
                            GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                            go.transform.localScale = new Vector3(200 * scaleSlider.GetComponent<Slider>().value, 200 * scaleSlider.GetComponent<Slider>().value, 0);
                            go.GetComponent<Renderer>().sortingOrder = 5;
                        }



                        Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
                    }
                }
                else
                {
                    for (int i = 0; i <= mouseYEnd - mouseYStart; i += 5)
                    {
                        Vector2 screenPos = new Vector2(mouseXStart, mouseYStart + i);
                        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                        if (currentButtonPressed == 16)
                        {
                            GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                            go.transform.localScale = new Vector3(20 * scaleSlider.GetComponent<Slider>().value, 10 * scaleSlider.GetComponent<Slider>().value, 0);
                            go.GetComponent<Renderer>().sortingOrder = 5;
                        }
                        else
                        {
                            GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                            go.transform.localScale = new Vector3(200 * scaleSlider.GetComponent<Slider>().value, 200 * scaleSlider.GetComponent<Slider>().value, 0);
                            go.GetComponent<Renderer>().sortingOrder = 5;
                        }

                        Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
                    }
                }
            }
        }

    }
}
