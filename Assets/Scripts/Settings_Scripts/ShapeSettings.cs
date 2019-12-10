using UnityEngine;


[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject {
    public float planetRadius = 1f;

    [SerializeField]
    public NoiseLayer[] noiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        [HideInInspector]
        public string Name;
        public bool enabled = true;
        public bool useFirstLayerAsMask;
        public FilterSettings filterSettings;
    }



}