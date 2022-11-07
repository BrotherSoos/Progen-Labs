using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapTextureSpread : MonoBehaviour
{
    private class TextureDimension
    {
        public double repetitionsX;
        public double repetitionsY;
        public double widthRatio;
        public double heightRatio;
        public double mapToTileWidthRatio;
        public double mapToTileHeightRatio;
        public int width;
        public int height;
    }

    public int sandRepetitionsX = 10;
    public int sandRepetitionsY = 10;

    public int plainsRepetitionsX = 10;
    public int plainsRepetitionsY = 10;

    public int hillsRepetitionsX = 10;
    public int hillsRepetitionsY = 10;

    public int waterRepetitionsX = 10;
    public int waterRepetitionsY = 10;

    Dictionary<string, TextureDimension> prefabs = new Dictionary<string, TextureDimension>();
    Dictionary<int, GameObject> las = new Dictionary<int, GameObject>();
    Dictionary<int, Color32[]> textureData = new Dictionary<int, Color32[]>();
    Dictionary<int, Color32[]> rawTextures = new Dictionary<int, Color32[]>();
    public void InterpolateTexture(int layerId, GameObject prefab, GameObject layer, int mapWidth, int mapHeight, int x, int y)
    {
        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        Texture2D texture = spriteRenderer.sprite.texture;

        SpriteRenderer layerSpriteRenderer = layer.GetComponent<SpriteRenderer>();
        Texture2D layerTexture = layerSpriteRenderer.sprite.texture;
        TextureDimension dim = prefabs[texture.name];
        int height = texture.height;
        int width = texture.width;
        int startPixelX = (int)(dim.mapToTileWidthRatio * x) % texture.width;
        int startPixelY = (int)(dim.mapToTileHeightRatio * y) % texture.height;
        int endPixelX = (int)(startPixelX + width * dim.widthRatio);
        int endPixelY = (int)(startPixelY + height * dim.heightRatio);
        Color32[] textureSection = RetrieveTextureSection(texture, startPixelX, startPixelY, endPixelX, endPixelY, layerId);
        for (int i = startPixelY; i < endPixelY; i++)
        {
            int skippableSection = (i - startPixelY) * (endPixelX - startPixelX + 1) - startPixelX;
            int skippableTexture = i * width;
            for (int j = startPixelX; j < endPixelX; j++)
            {
                textureData[layerId][skippableTexture + j] = textureSection[skippableSection + j];
            }
        }
    }

    private Color32[] RetrieveTextureSection(Texture2D texture, int startX, int startY, int endX, int endY, int layerId)
    {
        Color32[] section = new Color32[(endY - startY + 1) * (endX - startX + 1)];
        for (int i = startY; i <= endY; i++)
        {
            int skippableSection = (i - startY) * (endX - startX + 1) - startX;
            int skippableColors = i * texture.width;
            for (int j = startX; j <= endX; j++)
            {
                section[skippableSection + j] = rawTextures[layerId][skippableColors + j];
            }
        }
        return section;
    }

    public void ApplyLayers()
    {
        foreach(KeyValuePair<int, GameObject> la in las)
        {
            Texture2D text = la.Value.GetComponent<SpriteRenderer>().sprite.texture;
            text.SetPixels32(textureData[la.Key]);
            text.Apply();
        }
    }

    public Dictionary<int, GameObject> cleanLayers(Dictionary<int, GameObject> fabs, int w, int h)
    {   
        foreach (KeyValuePair<int, GameObject> prefab_pair in fabs)
        {
            GameObject go = Instantiate(prefab_pair.Value);
            go.name = prefab_pair.Key.ToString() + "_texture_layer";
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            Sprite spr = sr.sprite;
            TextureDimension dim = prefabs[spr.texture.name];
            int totalPixelsX = (int)(dim.width * dim.repetitionsX);
            int totalPixelsY = (int)(dim.height* dim.repetitionsY);
            textureData.Add(prefab_pair.Key, new Color32[dim.width*dim.height]);
            go.transform.parent = gameObject.transform;
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(w, h, 1);
            Color32[] colors = new Color32[totalPixelsX*totalPixelsY];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color32(0, 0, 0, 0);
            }
            Texture2D texture = new Texture2D(totalPixelsX, totalPixelsY);
            texture.name = spr.texture.name;
            texture.SetPixels32(colors);
            texture.Apply();
            Rect rect = new Rect(spr.rect.x, spr.rect.y, totalPixelsX, totalPixelsY);
            Sprite cleanSprite = Sprite.Create(texture, rect, new Vector2(.5f, .5f), spr.pixelsPerUnit);
            sr.sprite = cleanSprite;
            las.Add(prefab_pair.Key, go);
        }
        return las;
    }

    public void InitRawTextures(Dictionary<int, GameObject> textures)
    {
        foreach(KeyValuePair<int, GameObject> prefab_pair in textures)
        {
            rawTextures.Add(prefab_pair.Key, prefab_pair.Value.GetComponent<SpriteRenderer>().sprite.texture.GetRawTextureData<Color32>().ToArray());
        }
    }

    public void SetupPrefabs(int mapWidth, int mapHeight)
    {
        TextureDimension sand = new TextureDimension();
        sand.repetitionsX = sandRepetitionsX;
        sand.repetitionsY = sandRepetitionsY;
        sand.widthRatio = sandRepetitionsX / (double)(mapWidth);
        sand.heightRatio = sandRepetitionsY / (double)(mapHeight);
        sand.width = 4096;
        sand.height = 4096;
        sand.mapToTileWidthRatio = sand.width / mapWidth;
        sand.mapToTileHeightRatio = sand.height / mapHeight;
        TextureDimension hills = new TextureDimension();
        hills.repetitionsX = hillsRepetitionsX;
        hills.repetitionsY = hillsRepetitionsY;
        hills.widthRatio = hillsRepetitionsX / (double)(mapWidth);
        hills.heightRatio = hillsRepetitionsY / (double)(mapHeight);
        hills.width = 4096;
        hills.height = 4096;
        hills.mapToTileWidthRatio = hills.width / mapWidth;
        hills.mapToTileHeightRatio = hills.height / mapHeight;
        TextureDimension plains = new TextureDimension();
        plains.repetitionsX = plainsRepetitionsX;
        plains.repetitionsY = plainsRepetitionsY;
        plains.widthRatio = plainsRepetitionsX / (double)(mapWidth);
        plains.heightRatio = plainsRepetitionsY / (double)(mapHeight);
        plains.width = 4096;
        plains.height = 4096;
        plains.mapToTileWidthRatio = plains.width / mapWidth;
        plains.mapToTileHeightRatio = plains.height / mapHeight;
        TextureDimension water = new TextureDimension();
        water.repetitionsX = waterRepetitionsX;
        water.repetitionsY = waterRepetitionsY;
        water.width = 4096;
        water.height = 4096;
        water.widthRatio = waterRepetitionsX / (double)(mapWidth);
        water.heightRatio = waterRepetitionsY / (double)(mapHeight);
        water.mapToTileWidthRatio = water.width / mapWidth;
        water.mapToTileHeightRatio = water.height / mapHeight;

        prefabs.Add("sand", sand);
        prefabs.Add("plains", plains);
        prefabs.Add("hill", hills);
        prefabs.Add("water", water);
    }
}
