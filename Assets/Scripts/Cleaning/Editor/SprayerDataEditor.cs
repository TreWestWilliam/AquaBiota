using Object = UnityEngine.Object;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SprayerData), true)]
[CanEditMultipleObjects]
public class SprayerDataEditor : Editor
{
    private SprayerData sprayerData { get { return (target as SprayerData); } }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Texture2D defaultIcon = null;

        if(sprayerData.sprite == null)
        {
            return defaultIcon;
        }

        Object preview = AssetPreview.GetAssetPreview(sprayerData.sprite);

        if(preview == null)
        {
            return defaultIcon;
        }

        Texture2D cache = new Texture2D(width, height);
        EditorUtility.CopySerialized(preview, cache);

        return cache;
    }
}
