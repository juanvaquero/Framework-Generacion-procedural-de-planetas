using UnityEngine;

public class PerlinNoise2DTexture : MonoBehaviour {
    
    [Range(1,256)]
    public int resolution = 16;
    public float scale = 20f;

    public int offsetX = 0;
    public int offsetZ = 0;
    [Range(0,3f)]
    public float strength = 2f;

    private void OnValidate() 
    {
        Renderer render = gameObject.GetComponent<Renderer>();
        render.sharedMaterial.mainTexture = GenerateTexture();
    }

    private Texture2D GenerateTexture()
    {
        Texture2D perlinNoiseTexture = new Texture2D(resolution,resolution);

        for(int x = 0; x < resolution; x++)
        {
            for(int z = 0;  z < resolution; z++)
            {
                Color actualColor = CalculateColor(x , z );
                perlinNoiseTexture.SetPixel(x,z,actualColor);
            }
        }
        perlinNoiseTexture.Apply();
        return perlinNoiseTexture;
    }

    private Color CalculateColor(int x, int z)
    {
        float xCoordenate = (float)x / resolution * scale + offsetX;
        float zCoordenate = (float)z / resolution * scale + offsetZ;

        float value = Mathf.PerlinNoise(xCoordenate,zCoordenate) * strength;
        return new Color(value,value,value);
    }
}