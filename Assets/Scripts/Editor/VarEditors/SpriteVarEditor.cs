using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Reflection;
using Object = UnityEngine.Object;
using UnityEngine.UIElements;
using Unity.VectorGraphics;
using System.Security.Policy;
using UnityEngine.Rendering;
using NUnit.Framework.Constraints;

[CustomEditor(typeof(SpriteVariable), true)]
[CanEditMultipleObjects]
public class SpriteVarEditor : Editor
{
    private SpriteVariable spriteVar { get { return (target as SpriteVariable); } }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        if(spriteVar.Value != null)
        {
            if(AssetDatabase.GetAssetPath(spriteVar.Value).EndsWith(".svg"))
            {
                Material mat = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
                Vector2 size = GetDrawingDimensions(spriteVar.Value, width, height);
                return VectorUtils.RenderSpriteToTexture2D(spriteVar.Value, (int)size.x, (int)size.y, mat);
            }
            else
            {
                Type t = GetType("UnityEditor.SpriteUtility");
                if(t != null)
                {
                    MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) });
                    if(method != null)
                    {
                        object ret = method.Invoke("RenderStaticPreview", new object[] { spriteVar.Value, Color.white, width, height });
                        if(ret is Texture2D)
                            return ret as Texture2D;
                    }
                }
            }

            //return ((SpriteVariable)serializedObject.targetObject).Value.texture;
        }
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    public override void OnInspectorGUI()
    {
        GUI.Label(new Rect(20, 5, 35, EditorGUIUtility.singleLineHeight), "Value");

        if(spriteVar.Value != null)
        {
            Texture2D texture = spriteVar.Value.texture;
            Rect textureRect = spriteVar.Value.textureRect;
            if(AssetDatabase.GetAssetPath(spriteVar.Value).EndsWith(".svg"))
            {
                Material mat = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
                Vector2 size = GetDrawingDimensions(spriteVar.Value, 60, 60);
                texture = VectorUtils.RenderSpriteToTexture2D(spriteVar.Value, (int)size.x, (int)size.y, mat);
                textureRect = new Rect(0, 0, (int)size.x, (int)size.y);

            }
            DrawTexturePreview(new Rect(67, 5, 60, 60), textureRect, texture);
        }

        //Rect rect = new Rect(Screen.width / 2f, 5, (Screen.width / 2f), EditorGUIUtility.singleLineHeight);

        EditorGUI.indentLevel = 8;
        spriteVar.Value = (Sprite)EditorGUILayout.ObjectField(spriteVar.Value, typeof(Sprite), false, GUILayout.Width((Screen.width / 1f) - 23f), GUILayout.Height(EditorGUIUtility.singleLineHeight));
        EditorUtility.SetDirty(spriteVar);
    }

    private void DrawTexturePreview(Rect position, Rect textureRect, Texture2D texture)
    {
        Vector2 fullSize = new Vector2(texture.width, texture.height);
        Vector2 size = new Vector2(textureRect.width, textureRect.height);

        Rect coords = textureRect;
        coords.x /= fullSize.x;
        coords.width /= fullSize.x;
        coords.y /= fullSize.y;
        coords.height /= fullSize.y;

        Vector2 ratio;
        ratio.x = position.width / size.x;
        ratio.y = position.height / size.y;
        float minRatio = Mathf.Min(ratio.x, ratio.y);

        Vector2 center = position.center;
        position.width = size.x * minRatio;
        position.height = size.y * minRatio;
        position.center = center;

        GUI.DrawTextureWithTexCoords(position, texture, coords);
    }

    private Vector2 GetDrawingDimensions(Sprite sprite, int width, int height)
    {
        var size = new Vector2(sprite.rect.width, sprite.rect.height);

        int spriteW = Mathf.RoundToInt(size.x);
        int spriteH = Mathf.RoundToInt(size.y);

        Vector2 r = new Vector2(width, height);

        if(size.sqrMagnitude > 0.0f)
        {
            var spriteRatio = size.x / size.y;
            var rectRatio = width / height;

            if(spriteRatio > rectRatio)
                r.y = width * (1.0f / spriteRatio);
            else
                r.x = height * spriteRatio;
        }

        return r;
    }

    private static Type GetType(string TypeName)
    {
        var type = Type.GetType(TypeName);
        if(type != null)
            return type;

        if(TypeName.Contains("."))
        {
            var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));
            var assembly = Assembly.Load(assemblyName);
            if(assembly == null)
                return null;
            type = assembly.GetType(TypeName);
            if(type != null)
                return type;
        }

        var currentAssembly = Assembly.GetExecutingAssembly();
        var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach(var assemblyName in referencedAssemblies)
        {
            var assembly = Assembly.Load(assemblyName);
            if(assembly != null)
            {
                type = assembly.GetType(TypeName);
                if(type != null)
                    return type;
            }
        }
        return null;
    }
}