using NUnit.Framework.Internal.Execution;
using Unity.Plastic.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringRef))]
public class StringRefDrawer : PropertyDrawer
{
    /// Options to display in the popup to select constant or variable.
    private readonly string[] popupOptions =
        { "Use Constant", "Use Variable" };

    /// Cached style to use to draw the popup button.
    private GUIStyle popupStyle;
    private int lines = 1;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(popupStyle == null)
        {
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get properties
        SerializedProperty useConstant = property.FindPropertyRelative("UseConstant");
        SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");
        SerializedProperty variable = property.FindPropertyRelative("Variable");

        string str;
        if(useConstant.boolValue)
        {
            str = constantValue.stringValue;
        }
        else 
        {
            if(variable.objectReferenceValue)
            {
                str = (variable.objectReferenceValue as StringVariable).Value;
            }
            else
            {
                str = string.Empty;
            }
        }

        float extraHeight = GetPropertyHeight(property, label);
        lineCount(str);
        extraHeight = GetPropertyHeight(property, label) - extraHeight;
        position.height = GetPropertyHeight(property, label);
        GUILayoutUtility.GetRect(0f, extraHeight);
        lines = 1;

        // Calculate rect for configuration button
        Rect buttonRect = new Rect(position);
        buttonRect.yMin += popupStyle.margin.top;
        buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
        position.xMin = buttonRect.xMax + 2;

        // Store old indent level and set it to 0, the PrefixLabel takes care of it
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, popupOptions, popupStyle);

        useConstant.boolValue = result == 0;

        if(!useConstant.boolValue)
        {
            if(variable.objectReferenceValue)
            {
                float xMax = position.xMax;
                float width = Mathf.Floor((xMax - position.xMin) / 2f);
                position.xMax = position.xMin + width - 1;
                string varVal = (variable.objectReferenceValue as StringVariable).Value;
                GUI.enabled = false;
                string newValue = EditorGUI.TextArea(position, varVal);
                GUI.enabled = true;
                position.xMin = position.xMax + 2;
                position.xMax = xMax;
            }

            EditorGUI.PropertyField(position, variable, GUIContent.none);
        }
        else
        {
            constantValue.stringValue = EditorGUI.TextArea(position, constantValue.stringValue);
        }

        if(EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (base.GetPropertyHeight(property, label) * lines);
    }

    private void lineCount(string str)
    {
        int count = 1;
        for(int i = 0; i < str.Length; i++)
        {
            if(str[i] == '\n')
                ++count;
        }
        if(count < 3)
        {
            lines = count;
        }
        else
        {
            lines = 3;
        }
    }
}