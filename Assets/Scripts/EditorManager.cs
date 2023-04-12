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
    public GameObject dragSlider;
    public GameObject[] itemImage;
    private float mouseXStart;
    private float mouseXEnd;
    private float mouseYStart;
    private float mouseYEnd;

    private void Update()
    {

        if (Input.GetMouseButtonDown(2) && currentButtonPressed > 15 && itemButtons[currentButtonPressed].clicked)
        {
            itemButtons[currentButtonPressed].clicked = false;
            mouseXStart = Input.mousePosition.x;
            mouseYStart = Input.mousePosition.y;

            Vector2 screenPos = new Vector2(mouseXStart, mouseYStart);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
            go.transform.localScale = new Vector3(dragSlider.GetComponent<Slider>().value * 10, dragSlider.GetComponent<Slider>().value * 10, 0);
            go.GetComponent<Renderer>().sortingOrder = 5;

            Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
        }
        else if (Input.GetMouseButtonDown(0) && currentButtonPressed < 16 && itemButtons[currentButtonPressed].clicked)
        {
            itemButtons[currentButtonPressed].clicked = false;
            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
            go.transform.localScale = new Vector3(200 * scaleSlider.GetComponent<Slider>().value, 200 * scaleSlider.GetComponent<Slider>().value, 0);
            go.GetComponent<Renderer>().sortingOrder = 5;

            Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
        }
        if (Input.GetMouseButtonUp(2) && currentButtonPressed > 15)
        {
            itemButtons[currentButtonPressed].clicked = false;
            mouseYEnd = Input.mousePosition.y;
            mouseXEnd = Input.mousePosition.x;
            Debug.Log(mouseXStart + " //// " + mouseXEnd);
            Debug.Log(mouseYStart + " //// " + mouseYEnd);
            if (((Mathf.Abs(mouseXEnd) - Mathf.Abs(mouseXStart)) > 10) || ((Mathf.Abs(mouseYEnd) - Mathf.Abs(mouseYStart)) > 10))
            {
                if ((Mathf.Abs(mouseXEnd) - Mathf.Abs(mouseXStart)) >= 0 || (Mathf.Abs(mouseYEnd) - Mathf.Abs(mouseYStart)) >= 0)
                {
                    if ((Mathf.Abs(mouseXEnd) - Mathf.Abs(mouseXStart)) > (Mathf.Abs(mouseYEnd) - Mathf.Abs(mouseYStart)))
                    {
                        if (mouseXEnd > mouseXStart)
                        {
                            for (int i = 0; i <= mouseXEnd - mouseXStart; i += 20)
                            {
                                Vector2 screenPos = new Vector2(mouseXStart + (i), mouseYStart);
                                Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                                GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                                go.transform.localScale = new Vector3(20 * dragSlider.GetComponent<Slider>().value, 10 * dragSlider.GetComponent<Slider>().value, 0);
                                go.GetComponent<Renderer>().sortingOrder = 5;

                                Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
                            }
                           
                        }
                        
                    }
                    else
                    {
                        for (int i = 0; i <= mouseYEnd - mouseYStart; i += 20)
                        {
                            Vector2 screenPos = new Vector2(mouseXStart, mouseYStart + i);
                            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                            GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                            go.transform.localScale = new Vector3(10 * dragSlider.GetComponent<Slider>().value, 10 * dragSlider.GetComponent<Slider>().value, 0);
                            go.GetComponent<Renderer>().sortingOrder = 5;

                            Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
                        }
                        
                    }
                    
                }
                
            }
            else
            {
                if ((Mathf.Abs(mouseXStart) - Mathf.Abs(mouseXEnd)) > (Mathf.Abs(mouseYStart) - Mathf.Abs(mouseYEnd)))
                {

                    for (int i = 0; i >= mouseXEnd - mouseXStart; i -= 20)
                    {
                        Debug.Log("Bin im Loop");
                        Vector2 screenPos = new Vector2(mouseXStart + (i), mouseYStart);
                        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                        GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                        go.transform.localScale = new Vector3(20 * dragSlider.GetComponent<Slider>().value, 10 * dragSlider.GetComponent<Slider>().value, 0);
                        go.GetComponent<Renderer>().sortingOrder = 5;

                        Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
                    }
                    
                }
                else
                {
                    for (int i = 0; i >= mouseYEnd - mouseYStart; i -= 20)
                    {
                        Vector2 screenPos = new Vector2(mouseXStart, mouseYStart + i);
                        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                        GameObject go = Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                        go.transform.localScale = new Vector3(10 * dragSlider.GetComponent<Slider>().value, 10 * dragSlider.GetComponent<Slider>().value, 0);
                        go.GetComponent<Renderer>().sortingOrder = 5;


                        Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
                    }
                    
                }
                
            }
            
        }
        
    }
}
