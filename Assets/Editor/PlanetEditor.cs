using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuadSphere))]
public class PlanetEditor : Editor {

	private QuadSphere planet;

	private Editor shapeEditor;
	private Editor colorEditor;

	public override void OnInspectorGUI()
	{
		using( var check = new EditorGUI.ChangeCheckScope())
		{
			base.OnInspectorGUI();
			if(check.changed)
				planet.GeneratePlanet();
		}

		if(GUILayout.Button("Generate Planet"))
		{
			planet.GeneratePlanet();
		}
		
		DrawSettingsEditor(planet.ShapeSettings, planet.OnShapeSettingsUpdated,ref planet.shapeSettingsFoldout,ref shapeEditor);
		DrawSettingsEditor(planet.ColorSettings,planet.OnColorSettingsUpdated,ref planet.colorSettingsFoldout,ref colorEditor);
	}

	private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated,ref bool foldout,ref Editor editor)
	{

		if(settings != null)
		{
			foldout = EditorGUILayout.InspectorTitlebar(foldout,settings);
			using( var check = new EditorGUI.ChangeCheckScope())
			{
				// Si esta desplegado creamos el editor,sino no.
				if(foldout)
				{			
					CreateCachedEditor(settings,null,ref editor);
					editor.OnInspectorGUI();

					if(check.changed)
					{
						if(onSettingsUpdated !=null)
							onSettingsUpdated();
					}
				}
			}
		}

	}

	private void OnEnable() 
	{
		planet = (QuadSphere)target;
	}



}
