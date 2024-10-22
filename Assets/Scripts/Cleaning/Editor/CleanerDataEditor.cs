using Object = UnityEngine.Object;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CleanerData), true)]
[CanEditMultipleObjects]
public class CleanerDataEditor : Editor
{
    private CleanerData cleanerData { get { return (target as CleanerData); } }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Texture2D defaultIcon = null;

        if(cleanerData.sprite == null)
        {
            return defaultIcon;
        }

        Object preview = AssetPreview.GetAssetPreview(cleanerData.sprite);

        if(preview == null)
        {
            return defaultIcon;
        }

        Texture2D cache = new Texture2D(width, height);
        EditorUtility.CopySerialized(preview, cache);

        return cache;
    }
}
