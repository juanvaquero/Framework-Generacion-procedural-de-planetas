using UnityEngine;
using System;
using System.Collections;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideSettings : PropertyAttribute
{
    public string conditionalSourceField;
    public int enumIndex;

    public ConditionalHideSettings(string boolVariableName)
    {
        conditionalSourceField = boolVariableName;
    }

    public ConditionalHideSettings(string enumVariableName, int enumIndex)
    {
        conditionalSourceField = enumVariableName;
        this.enumIndex = enumIndex;
    }

}