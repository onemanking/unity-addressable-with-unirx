using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class ImageLoad : AddressableObject
{
    private RawImage _Image;

    protected override void Start()
    {
        OnLoadedCompleteAsObservable().Subscribe(_imageAsset => LoadCompleted(_imageAsset)).AddTo(this);
        base.Start();
    }

    protected override void LoadCompleted(Object _loadedAsset)
    {
        _Image = _Image ?? GetComponent<RawImage>();
        _Image.texture = (Texture2D) _loadedAsset;
    }

}