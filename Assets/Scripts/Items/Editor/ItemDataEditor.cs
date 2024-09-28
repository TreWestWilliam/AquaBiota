using Object = UnityEngine.Object;
using UnityEditor;
using UnityEngine;
//using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(ItemData), true)]
[CanEditMultipleObjects]
public class ItemDataEditor : Editor
{
    private ItemData itemData { get { return (target as ItemData); } }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Texture2D defaultIcon = null;

        if(itemData.sprite == null)
        {
            return defaultIcon;
        }

        Object preview = AssetPreview.GetAssetPreview(itemData.sprite);

        if(preview == null)
        {
            return defaultIcon;
        }

        Texture2D cache = new Texture2D(width, height);
        EditorUtility.CopySerialized(preview, cache);

        return cache;
    }
}
