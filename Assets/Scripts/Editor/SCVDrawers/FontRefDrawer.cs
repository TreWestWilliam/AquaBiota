using UnityEditor;
using UnityEngine;
using TMPro;

[CustomPropertyDrawer(typeof(FontRef))]
public class FontRefDrawer : PropertyDrawer
{
    /// Options to display in the popup to select constant or variable.
    private readonly string[] popupOptions =
        { "Use Constant", "Use Variable" };

    /// Cached style to use to draw the popup button.
    private GUIStyle popupStyle;

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

        if(!useConstant.boolValue && variable.objectReferenceValue)
        {
            float xMax = position.xMax;
            float width = Mathf.Floor((xMax - position.xMin) / 2f);
            position.xMax = position.xMin + width - 1;
            TMP_FontAsset varVal = (variable.objectReferenceValue as FontVariable).Value;
            GUI.enabled = false;
            TMP_FontAsset newValue = EditorGUI.ObjectField(position, "", varVal, typeof(TMP_FontAsset), false) as TMP_FontAsset;
            GUI.enabled = true;
            position.xMin = position.xMax + 2;
            position.xMax = xMax;
        }

        EditorGUI.PropertyField(position,
            useConstant.boolValue ? constantValue : variable,
            GUIContent.none);

        if(EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}