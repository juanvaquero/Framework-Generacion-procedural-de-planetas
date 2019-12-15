
public class MinMaxValue  {

	public float min;
	public float max;

	public MinMaxValue()
	{
		//Inicializamos los puntos minimo y maximo a valores extremos.
		min = float.MaxValue;
		max = float.MinValue;
	}

	public void AddValue(float value)
	{
		if(value > max)
			max = value;
		
		if(value <min)
			min = value;
	}

}
