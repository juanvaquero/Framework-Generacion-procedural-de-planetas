using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;

public class RidgedMultiNoiseFilter : NoiseFilter  {

	public RidgedMultiNoiseFilter(FilterSettings.RidgedMultiNoiseSettings settings)
	{
		moduleFilter = new RidgedMultifractal(settings.frecuency,
											  settings.lucanarity,
											  settings.numberOctaves,
											  settings.seed,
											  settings.noiseQuality);
		
		
		strength = settings.strength;
		offsetCenter = settings.offsetCenter;
        inferiorBound = settings.inferiorBound;
        superiorBound = settings.superiorBound;
	}


	public override ModuleBase AplicateSettings()
	{
	    ModuleBase filterModificated = AplicateStrength();
		filterModificated = AplicateClamp(filterModificated);
		filterModificated = AplicateTranslation(filterModificated);// Aplicamos un offset del punto de generacion del filtro.


		return filterModificated;
	}
}
