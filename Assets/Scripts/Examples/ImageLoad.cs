using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class ImageLoad : AddressableObject<Texture2D>
{
    [SerializeField] private RawImage m_Image;

    protected override void Start()
    {
        OnLoadedCompleteAsObservable().Subscribe(_fontAsset => LoadCompleted(_fontAsset)).AddTo(this);
        base.Start();
    }

    protected override void LoadCompleted(Texture2D _loadedAsset)
    {
        m_Image.texture = _loadedAsset;
    }

}