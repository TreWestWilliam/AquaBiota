using Object = UnityEngine.Object;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialVariable), true)]
[CanEditMultipleObjects]
public class MaterialVarEditor : Editor
{
    private MaterialVariable materialVar { get { return (target as MaterialVariable); } }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Texture2D defaultIcon = null;

        if(materialVar.Value == null)
        {
            return defaultIcon;
        }

        Object preview = AssetPreview.GetAssetPreview(materialVar.Value);

        if(preview == null)
        {
            return defaultIcon;
        }

        Texture2D cache = new Texture2D(width, height);
        EditorUtility.CopySerialized(preview, cache);

        return cache;
    }
}
