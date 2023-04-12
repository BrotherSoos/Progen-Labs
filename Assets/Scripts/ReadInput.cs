using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadInput : MonoBehaviour
{

    private string input;
    public GameObject textPrefabs;
    private Vector2 screenPos;
    private Vector2 worldPos;
    private bool textFinished;
    public GameObject scaleSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (Input.GetMouseButtonDown(0) && textFinished)
        {
            GameObject go = Instantiate(textPrefabs, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
            go.transform.localScale = new Vector3(5*scaleSlider.GetComponent<Slider>().value, 5*scaleSlider.GetComponent<Slider>().value, 0);
            go.GetComponent<Renderer>().sortingOrder = 5;
            go.GetComponent<TextMesh>().text = input;
            textFinished = false;
        }
    }

    public void ReadStringInput(string text)
    {
        textFinished = true;
        input = text;
    }


}
