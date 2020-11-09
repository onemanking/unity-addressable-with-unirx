using TMPro;
using UnityEngine;

public class FontLoad : AddressableObject
{
    private TextMeshProUGUI _Text;

    protected override void LoadCompleted(Object _loadedAsset)
    {
        _Text = _Text ?? GetComponent<TextMeshProUGUI>();
        _Text.font = (TMP_FontAsset) _loadedAsset;
    }
}