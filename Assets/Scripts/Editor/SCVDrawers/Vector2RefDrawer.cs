using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Vector2Ref))]
public class Vector2RefDrawer : PropertyDrawer
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

        float extraHeight = useConstant.boolValue ? 0f : (GetPropertyHeight(property, label) / 2) + 1;

        position.height -= extraHeight;


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
            Vector2 varVal = (variable.objectReferenceValue as Vector2Variable).Value;
            GUI.enabled = false;
            Vector2 newValue = EditorGUI.Vector2Field(position, "", varVal);
            GUI.enabled = true;
        }

        if(!useConstant.boolValue)
            position.y += extraHeight;

        EditorGUI.PropertyField(position,
            useConstant.boolValue ? constantValue : variable,
            GUIContent.none);

        if(EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        bool useConstant = property.FindPropertyRelative("UseConstant").boolValue;
        if(useConstant)
            return base.GetPropertyHeight(property, label);
        return (base.GetPropertyHeight(property, label) * 2) + 2;
    }
}