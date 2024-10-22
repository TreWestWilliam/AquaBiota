using Object = UnityEngine.Object;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PollutionData), true)]
[CanEditMultipleObjects]
public class PollutionDataEditor : Editor
{
    private PollutionData pollutionData { get { return (target as PollutionData); } }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Texture2D defaultIcon = null;

        if(pollutionData.sprite == null)
        {
            return defaultIcon;
        }

        Object preview = AssetPreview.GetAssetPreview(pollutionData.sprite);

        if(preview == null)
        {
            return defaultIcon;
        }

        Texture2D cache = new Texture2D(width, height);
        EditorUtility.CopySerialized(preview, cache);

        return cache;
    }
}
