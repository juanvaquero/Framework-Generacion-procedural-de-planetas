using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;


public class PerlinNoiseFilter : NoiseFilter
{
    public PerlinNoiseFilter(FilterSettings.PerlinNoiseSettings settings)
    {
        moduleFilter = new Perlin((float)settings.frecuency,
                                 (float)settings.lucanarity,
                                 (float)settings.persistence,
                                 settings.numberOctaves,
                                 settings.seed,
                                 settings.noiseQuality);

        offsetCenter = settings.offsetCenter;
        strength = settings.strength;
        inferiorBound = settings.inferiorBound;
        superiorBound = settings.superiorBound;
    }


    public override ModuleBase AplicateSettings()
	{

        ModuleBase filterModificated = AplicateStrength();// Aplicamos la fuerza con la que se va a manifestar ese filtro.
        // filterModificated = AplicateClamp(filterModificated);// Aplicamos un clamp para restringir ambos lados y asi crear efecto de continentes y montañas.

        // filterModificated = AplicateMininumValue(filterModificated);
        filterModificated = AplicateTranslation(filterModificated);// Aplicamos un offset del punto de generacion del filtro.

        return filterModificated;

	}

    public override ModuleBase AplicateClamp(ModuleBase filter)
    {
        return new LibNoise.Operator.Clamp(inferiorBound, superiorBound, filter);
    }
    
}
