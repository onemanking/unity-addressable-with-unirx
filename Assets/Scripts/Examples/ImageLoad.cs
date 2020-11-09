using UnityEngine;
using UnityEngine.UI;

public class ImageLoad : AddressableObject
{
    private RawImage _Image;

    protected override void LoadCompleted(Object _loadedAsset)
    {
        _Image = _Image ?? GetComponent<RawImage>();
        _Image.texture = (Texture2D) _loadedAsset;
    }

}