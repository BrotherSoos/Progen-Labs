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

    public int sandRepetitionsX = 1;
    public int sandRepetitionsY = 1;

    public int plainsRepetitionsX = 1;
    public int plainsRepetitionsY = 1;

    public int hillsRepetitionsX = 1;
    public int hillsRepetitionsY = 1;

    public int waterRepetitionsX = 1;
    public int waterRepetitionsY = 1;

    Dictionary<string, TextureDimension> prefabs = new Dictionary<string, TextureDimension>();
    public GameObject InterpolateTexture(GameObject prefab, int mapWidth, int mapHeight, int x, int y)
    {
        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        Texture2D texture = spriteRenderer.sprite.texture;
        TextureDimension dim = prefabs[texture.name];
        int height = texture.height;
        int width = texture.width;
        int startPixelX = (int)(dim.mapToTileWidthRatio * x) % width;
        int startPixelY = (int)(dim.mapToTileHeightRatio * y) % height;
        int endPixelX = (int)(startPixelX + width * dim.widthRatio);
        int endPixelY = (int)(startPixelY + height * dim.heightRatio);
        Color32[] textureSection = RetrieveTextureSection(texture, startPixelX, startPixelY, endPixelX, endPixelY);
        Texture2D interpolated = new Texture2D(endPixelX - startPixelX + 1, endPixelY - startPixelY + 1);
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        //interpolated.name = texture.name;
        interpolated.SetPixels32(textureSection, 0);
        interpolated.Apply();
        Sprite sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        Rect rect = new Rect(sprite.rect.x, sprite.rect.y, (endPixelX - startPixelX + 1), (endPixelY - startPixelY + 1));
        Sprite textureSprite = Sprite.Create(interpolated, rect, new Vector2(.5f, .5f), sprite.pixelsPerUnit);
        //textureSprite.name = sprite.name;
        prefab.GetComponent<SpriteRenderer>().sprite = textureSprite;
        return prefab;
    }

    public void SetupPrefabs(int mapWidth, int mapHeight)
    {
        TextureDimension sand = new TextureDimension();
        sand.repetitionsX = sandRepetitionsX;
        sand.repetitionsY = sandRepetitionsY;
        sand.widthRatio = sandRepetitionsX / (double)(mapWidth);
        sand.heightRatio = sandRepetitionsY / (double)(mapHeight);
        sand.width = 512;
        sand.height = 512;
        sand.mapToTileWidthRatio = sand.width / mapWidth;
        sand.mapToTileHeightRatio = sand.height / mapHeight;
        TextureDimension hills = new TextureDimension();
        hills.repetitionsX = hillsRepetitionsX;
        hills.repetitionsY = hillsRepetitionsY;
        hills.widthRatio = hillsRepetitionsX / (double)(mapWidth);
        hills.heightRatio = hillsRepetitionsY / (double)(mapHeight);
        hills.width = 512;
        hills.height = 512;
        hills.mapToTileWidthRatio = hills.width / mapWidth;
        hills.mapToTileHeightRatio = hills.height / mapHeight;
        TextureDimension plains = new TextureDimension();
        plains.repetitionsX = plainsRepetitionsX;
        plains.repetitionsY = plainsRepetitionsY;
        plains.widthRatio = plainsRepetitionsX / (double)(mapWidth);
        plains.heightRatio = plainsRepetitionsY / (double)(mapHeight);
        plains.width = 512;
        plains.height = 512;
        plains.mapToTileWidthRatio = plains.width / mapWidth;
        plains.mapToTileHeightRatio = plains.height / mapHeight;
        TextureDimension water = new TextureDimension();
        water.repetitionsX = waterRepetitionsX;
        water.repetitionsY = waterRepetitionsY;
        water.width = 512;
        water.height = 512;
        water.widthRatio = waterRepetitionsX / (double)(mapWidth);
        water.heightRatio = waterRepetitionsY / (double)(mapHeight);
        water.mapToTileWidthRatio = water.width / mapWidth;
        water.mapToTileHeightRatio = water.height / mapHeight;

        prefabs.Add("sand", sand);
        prefabs.Add("plains", plains);
        prefabs.Add("hill", hills);
        prefabs.Add("water", water);
    }

    private Color32[] RetrieveTextureSection(Texture2D texture, int startX, int startY, int endX, int endY)
    {
        Color32[] colors = texture.GetRawTextureData<Color32>().ToArray();
        Color32[] section = new Color32[(endY - startY + 1) * (endX - startX + 1)];
        int len = (endX - startX + 1) * (endY - startY + 1);
        for (int i = startY; i <= endY; i++)
        {
            for(int j = startX; j <= endX; j++)
            {
                section[(i-startY) * (endX - startX + 1) + j-startX] = colors[i * texture.width + j];
            }
        }
        return section;
    }
}
