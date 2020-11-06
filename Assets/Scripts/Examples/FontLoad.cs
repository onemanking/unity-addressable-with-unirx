using TMPro;
using UnityEngine;
using UniRx;

public class FontLoad : AddressableObject<TMP_FontAsset>
{
    [SerializeField] private TextMeshProUGUI m_Text;

    protected override void Start()
    {
        OnLoadedCompleteAsObservable().Subscribe(_fontAsset => LoadCompleted(_fontAsset)).AddTo(this);
        base.Start();
    }

    protected override void LoadCompleted(TMP_FontAsset _loadedAsset)
    {
        m_Text.font = _loadedAsset;
    }

}