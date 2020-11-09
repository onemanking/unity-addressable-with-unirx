using UnityEngine;

public class PrefabLoad : AddressableObject
{
    protected override void LoadCompleted(Object _loadedAsset)
    {
        var go = (GameObject) _loadedAsset;
        go.transform.position = transform.position;
    }
}