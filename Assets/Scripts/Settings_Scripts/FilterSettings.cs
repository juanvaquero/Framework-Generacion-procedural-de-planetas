using UnityEngine;
using LibNoise;



[System.Serializable]
public class FilterSettings  {
    
    public enum FilterType { Perlin, RidgidMulti };
        
        public FilterType filterType;

        [ConditionalHideSettings("filterType", 0)]
        public PerlinNoiseSettings perlinNoiseSettings;
        [ConditionalHideSettings("filterType", 1)]
        public RidgedMultiNoiseSettings ridgedMultiNoiseSettings;

        [System.Serializable]
        public class PerlinNoiseSettings
        {
            [Range(1,8)]
            public int numberOctaves = 4;
            public float frecuency = 1f;
            [Range(1.5f,3.5f)]
            public float lucanarity = 1.5f;
            [Range(0f,1f)]
            public float persistence = 0.5f; // Para controlar la robusted(roughness) con al que se genera el sonido.

            [Range(0,999)]
            public int seed = 0;
            [HideInInspector]
            public QualityMode noiseQuality = QualityMode.Medium;

            public Vector3 offsetCenter;
            
            [Range(0,1f)]
            public float strength = 0.5f;

            public float inferiorBound = 0f;
            public float superiorBound = 1f;
        }

        [System.Serializable]
        public class RidgedMultiNoiseSettings
        {
            [Range(1,8)]
            public int numberOctaves = 4;
            public float frecuency = 1f;
            [Range(1.5f,3.5f)]
            public float lucanarity = 1.5f;

            [Range(0,999)]
            public int seed = 0;
            [HideInInspector]
            public QualityMode noiseQuality = QualityMode.Medium;

            public Vector3 offsetCenter;
            
            [Range(0,1f)]
            public float strength = 0.5f;

            public float inferiorBound = 0f;
            public float superiorBound = 1f;
        }

}