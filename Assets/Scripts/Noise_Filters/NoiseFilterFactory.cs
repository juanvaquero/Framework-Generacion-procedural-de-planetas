using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilterFactory  {
    
     public static NoiseFilter CreateNoiseFilter(FilterSettings settings)
    {
        switch (settings.filterType)
        {
            case FilterSettings.FilterType.Perlin:
                return new PerlinNoiseFilter(settings.perlinNoiseSettings);
            case FilterSettings.FilterType.RidgidMulti:
                return new RidgedMultiNoiseFilter(settings.ridgedMultiNoiseSettings);
        }
        return null;
    }

}