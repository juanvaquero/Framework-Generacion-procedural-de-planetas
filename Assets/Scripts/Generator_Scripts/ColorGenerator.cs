using UnityEngine;

public class ColorGenerator {

    private ColorSettings colorSettings;
    //Textura en la que almacenar el gradiente del planeta.
    private Texture2D texture;
    private const int resolutionTexture = 50;

    public void UpdateSettings(ColorSettings settings)
    {
        colorSettings = settings;
        //Creamos la textura para almacenar nuestro gradiente de colores.(50x1)
        if(texture == null) // Asi solo generamos la primera vez la textura.
            texture = new Texture2D(resolutionTexture,1);
    }

    public void UpdateElevation(MinMaxValue elevationMinMax)
    {
        colorSettings.material.SetVector("_elevationMinMax", new Vector4(elevationMinMax.min,elevationMinMax.max));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[resolutionTexture];

        for(int i = 0; i < resolutionTexture; i++)
        {
            //De esta manera conseguimos el color correspondiente para cada 50º trozo del gradiente.
            colors[i] = colorSettings.gradient.Evaluate(i / (resolutionTexture -1f));
        }
        texture.SetPixels(colors);
        texture.Apply();
        colorSettings.material.SetTexture("_planetTexture",texture);
    }
}