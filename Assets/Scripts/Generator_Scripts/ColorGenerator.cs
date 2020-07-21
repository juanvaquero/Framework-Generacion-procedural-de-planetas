
using UnityEngine;

public class ColorGenerator
{

    private ColorSettings colorSettings;       
    //Textura en la que almacenar el gradiente del planeta.
    private Texture2D texture;
    private const int textureResolution = 50;
    NoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        colorSettings = settings;
        int numbiomes = colorSettings.biomeColorSettings.biomes.Length;
        //Creamos la textura para almacenar nuestro gradiente de colores.(50 x numero de biomas)
        if (texture == null || texture.height != numbiomes)
        {
            // Doblamos el ancho de la textura de nuestro planeta, para guardar en este espacio los colores del oceano del planeta.
            texture = new Texture2D(textureResolution*2, numbiomes,TextureFormat.RGBA32, false);
            // texture = new Texture2D(textureResolution*2, numbiomes,TextureFormat.RGBA32, false);
        }

        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(colorSettings.biomeColorSettings.biomeFilterSettings);
        
    }

    public void UpdateElevation(MinMaxValue elevationMinMax)
    {
        colorSettings.material.SetVector("_elevationMinMax", new Vector4(elevationMinMax.min, elevationMinMax.max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        // Se suma 1 para pasar el rango de valoes de y de (-1,1) a (0,2);
        float heightPercent = (pointOnUnitSphere.y + 1f) / 2f;

        heightPercent += ((float)biomeNoiseFilter.moduleFilter.GetValue(pointOnUnitSphere) 
                            - colorSettings.biomeColorSettings.filterOffest) 
                            * colorSettings.biomeColorSettings.filterStrength;

        float indexBiome = 0;
        var biomes = colorSettings.biomeColorSettings.biomes;
        // Se le suma un valor pequeño porque con un valor de 0 absoluto no funciona bien el blend de lso biomas. 
        float blendRange = colorSettings.biomeColorSettings.blendStrength / 2f + 0.001f;
        for (int i = 0; i < biomes.Length; i++)
        {
            float distance = heightPercent - biomes[i].startLatitude;
            //TODO explicar esto bien.
            float weight = Mathf.InverseLerp(-blendRange,blendRange,distance);
            indexBiome *= (1 - weight);
            indexBiome += i * weight;

        }
        return indexBiome / Mathf.Max(1,(biomes.Length -1));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;
        foreach (var biome in colorSettings.biomeColorSettings.biomes)
        {
            // for (int i = 0; i < textureResolution*2; i++)
            for (int i = 0; i < textureResolution*2; i++)
            {
                Color colorGradient;

                if(i < textureResolution){
                    //De esta manera conseguimos el color correspondiente para cada 50º trozo del gradiente.
                    colorGradient = colorSettings.oceanColor.Evaluate(i / (textureResolution - 1f));
                }else
                {
                    //De esta manera conseguimos el color correspondiente para cada 50º trozo del gradiente.
                    // Restamos textureResolution para poder empezar en 0.
                    colorGradient = biome.gradient.Evaluate((i - textureResolution)/ (textureResolution - 1f));
                }

                Color tintColor = biome.tint;
                colors[colorIndex] = colorGradient * (1 - biome.strengthTint) + tintColor * biome.strengthTint;
                colorIndex++;
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        colorSettings.material.SetTexture("_planetTexture", texture);
    }
}