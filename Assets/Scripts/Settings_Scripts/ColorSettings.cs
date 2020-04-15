using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject {

    public Material material;
    public BiomeColorSettings biomeColorSettings;
    public Gradient oceanColor;

    [System.Serializable]
    public class BiomeColorSettings 
    {
        public Biome[] biomes = {new Biome()};
        public FilterSettings biomeFilterSettings;
        public float filterOffest;
        public float filterStrength;
        [Range(0,1)]
        public float blendStrength;

        [System.Serializable]
        public class Biome 
        {
            public Gradient gradient;
            public Color tint;

            [Range(0,1)]
            public float startLatitude;
            [Range(0,1)]
            public float strengthTint;
        }
    }

}