#region Assembly Unity.Addressables, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// location unknown
#endregion

using System;
using TMPro;
using UnityEditor;
using UnityEngine.AddressableAssets;

[Serializable]
public class AssetReferenceFontAsset : AssetReferenceT<TMP_FontAsset>
{
    public AssetReferenceFontAsset(string guid) : base(guid) { }

    /// <summary>
    /// Typeless override of parent editorAsset. Used by the editor to represent the asset referenced.
    /// </summary>
    public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
        if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(TMP_FontAsset))
            return true;
#endif
        return false;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Typeless override of parent editorAsset. Used by the editor to represent the asset referenced.
    /// </summary>
    public new Object editorAsset
    {
        get
        {
            if (CachedAsset != null || string.IsNullOrEmpty(AssetGUID))
                return CachedAsset;

            var prop = typeof(AssetReference).GetProperty("editorAsset");
            return prop.GetValue(this, null) as Object;
        }
    }
#endif

}