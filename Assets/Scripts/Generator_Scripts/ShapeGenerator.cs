
using UnityEngine;
using LibNoise;
using LibNoise.Operator;
using LibNoise.Generator;


public class ShapeGenerator  {

	private ShapeSettings shapeSettings;
	
	private NoiseFilter[] noiseFilters;

	private ModuleBase finalFilter;
    private ModuleBase finalFilterUnClamp;

	public MinMaxValue elevationMinMax;

	public void UpdateSettings(ShapeSettings settings)
	{
		shapeSettings = settings;
		elevationMinMax = new MinMaxValue();
		
		noiseFilters = new NoiseFilter[settings.noiseLayers.Length];
		for(int i = 0; i < noiseFilters.Length; i++)
		{
			//Ponermos el nombre de la capa por defecto.
			settings.noiseLayers[i].Name = "Layer "+(i+1)+": "+settings.noiseLayers[i].filterSettings.filterType.ToString();
			//Creamos el filtro de sonido pasandole las settings del filtro em cuestion de esa capa.
			noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].filterSettings);
		}

		// Solo se genera una vez por actualizacion de las settings del planeta.
		GenerateFinalFilter();
	}

	public Vector3 CalculatePointOnQuadSphere(Vector3 pointOnUnitSphere)
	{
		// Valor de elevacion de ese punto.	
		float elevation = (float)(finalFilter.GetValue(pointOnUnitSphere) + 1f) * 0.5f;

		elevation = shapeSettings.planetRadius * (2f + elevation);
		elevationMinMax.AddValue(elevation);

		Vector3 roundedPoint = pointOnUnitSphere * elevation;
		return roundedPoint;
	}

	public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere)
	{
		// Valor de elevacion de ese punto.	
		// float elevation = (float)(finalFilterUnClamp.GetValue(pointOnUnitSphere) + 1f) * 0.5f;
        float elevation = (float)finalFilterUnClamp.GetValue(pointOnUnitSphere);

		// elevation = shapeSettings.planetRadius * (2f + elevation);
        elevation = shapeSettings.planetRadius * (elevation);
		elevationMinMax.AddValue(elevation);

		return elevation;
	}

	public float GetScaledElevation(float unScaledElevation)
	{
		float scaledElevation = Mathf.Max(0,unScaledElevation);
		scaledElevation = shapeSettings.planetRadius * ( 1 + scaledElevation);

        // float scaledElevation = (float)(finalFilter.GetValue(pointOnUnitSphere) + 1f) * 0.5f;

		return scaledElevation;
	}

	private void GenerateFinalFilter()
	{
		ModuleBase combinedModules = new LibNoise.Generator.Const(1);
        ModuleBase combinedModulesWithClamp = new LibNoise.Generator.Const(1);
		
		if(noiseFilters.Length > 0 && shapeSettings.noiseLayers[0].enabled)
		{
			combinedModules = noiseFilters[0].AplicateSettings();
            combinedModulesWithClamp = noiseFilters[0].AplicateClamp(combinedModules);
		}

		//Inicializamos el filtro a 1, si los combinasemos mediante suma se inicializaria a 0.
		ModuleBase constnull = new LibNoise.Generator.Const(1);
		for ( int i = 1; i < noiseFilters.Length; i++)
		{
			ModuleBase constnull2;

			if(shapeSettings.noiseLayers[i].useFirstLayerAsMask)
				constnull2 = combinedModules;
			else
				constnull2 = constnull;

			if(shapeSettings.noiseLayers[i].enabled)
			{
                combinedModulesWithClamp = noiseFilters[i].AplicateClamp(combinedModules);

				combinedModules = new LibNoise.Operator.Add(combinedModules,noiseFilters[i].AplicateSettings());
				combinedModules = new LibNoise.Operator.Multiply(combinedModules,constnull2);


                combinedModulesWithClamp = new LibNoise.Operator.Add(combinedModulesWithClamp, noiseFilters[i].AplicateSettings());
                combinedModulesWithClamp = new LibNoise.Operator.Multiply(combinedModulesWithClamp, constnull2);
			}
		}

        // Terrace terrace  = new LibNoise.Operator.Terrace(combinedModules);
        // terrace.IsInverted = false;
        // terrace.Add(2);
        // terrace.Add(0.3);
        // terrace.Add(0.2);
        // terrace.Add(0.1);
        // combinedModules = terrace;

        finalFilterUnClamp = combinedModules;
		finalFilter = combinedModulesWithClamp;
		// return combinedModules;
	}

}
