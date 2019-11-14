using UnityEngine;


public class ShapeGenerator  {

	private ShapeSettings shapeSettings;

	public ShapeGenerator(ShapeSettings settings){
		shapeSettings = settings;
	}

	public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
	{
		return pointOnUnitSphere * shapeSettings.planetRadius;
	}
	

}
