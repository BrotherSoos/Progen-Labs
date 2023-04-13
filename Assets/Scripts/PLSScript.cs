using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PLSScript : MonoBehaviour
{
    public Slider _slider;
    public Text _text;
    public PerlinNoiseTileSet perlinNoiseTileSet;
    public TilemapTextureSpread textureScript;
    public Button generateMapButton;

    // Start is called before the first frame update
    void Start()
    {
        _slider.value = perlinNoiseTileSet.plainsLayers;
        _text.text = perlinNoiseTileSet.plainsLayers.ToString();
        _slider.onValueChanged.AddListener((v) =>
        {
            _text.text = v.ToString("0");
            perlinNoiseTileSet.plainsLayers = Mathf.RoundToInt(v);
        });
        generateMapButton.onClick.AddListener(() =>
        {
            perlinNoiseTileSet.GenerateMap();
            textureScript.ApplyLayers();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
