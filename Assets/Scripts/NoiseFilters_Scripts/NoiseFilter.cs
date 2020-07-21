using UnityEngine;
using LibNoise;

public abstract class NoiseFilter  {

    public ModuleBase moduleFilter;


	public float strength = 0.5f;
	public Vector3 offsetCenter = Vector3.zero;	
	public float inferiorBound = 0;
    public float superiorBound = 1f;

	public bool enableFilter = true;

	protected ModuleBase AplicateStrength()
	{
		ModuleBase strengthFilter = new LibNoise.Generator.Const(strength);
		return new LibNoise.Operator.Multiply(moduleFilter,strengthFilter);
	}

	public abstract ModuleBase AplicateClamp(ModuleBase filter);

    // public abstract ModuleBase AplicateClamp(ModuleBase filter)
    // {
    //     return new LibNoise.Operator.Clamp(inferiorBound, superiorBound, filter);
    // }

	protected ModuleBase AplicateMininumValue(ModuleBase filter)
	{	ModuleBase minimunFilter = new LibNoise.Generator.Const(inferiorBound);
		ModuleBase zeroFilter = new LibNoise.Generator.Const(0);
		ModuleBase substractFilter = new LibNoise.Operator.Subtract(filter,minimunFilter);
		return new LibNoise.Operator.Max(zeroFilter,substractFilter);
	}

	protected ModuleBase AplicateTranslation(ModuleBase filter)
	{
		return new LibNoise.Operator.Translate(offsetCenter.x,offsetCenter.y,offsetCenter.z,filter);
	}

	public abstract ModuleBase AplicateSettings();

}
