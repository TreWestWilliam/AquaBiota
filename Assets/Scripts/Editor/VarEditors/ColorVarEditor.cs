using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

[CustomEditor(typeof(ColorVariable), true)]
[CanEditMultipleObjects]
public class ColorVarEditor : Editor
{
    private ColorVariable colorVar { get { return (target as ColorVariable); } }

    private Sprite generatedSprite;

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        if(generatedSprite == null)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA64, false);
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.Apply();
            generatedSprite = Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0f, 1f));
        }

        if(colorVar.Value != null)
        {
            Type t = GetType("UnityEditor.SpriteUtility");
            if(t != null)
            {
                MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) });
                if(method != null)
                {
                    object ret = method.Invoke("RenderStaticPreview", new object[] { generatedSprite, colorVar.Value, width, height });
                    if(ret is Texture2D)
                        return ret as Texture2D;
                }
            }

        }
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
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