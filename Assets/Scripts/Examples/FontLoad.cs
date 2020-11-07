using TMPro;
using UnityEngine;
using UniRx;

public class FontLoad : AddressableObject
{
    private TextMeshProUGUI _Text;
    protected override void Start()
    {
        OnLoadedCompleteAsObservable().Subscribe(_fontAsset => LoadCompleted(_fontAsset)).AddTo(this);
        base.Start();
    }

    protected override void LoadCompleted(Object _loadedAsset)
    {
        _Text = _Text ?? GetComponent<TextMeshProUGUI>();
        _Text.font = (TMP_FontAsset) _loadedAsset;
    }
}