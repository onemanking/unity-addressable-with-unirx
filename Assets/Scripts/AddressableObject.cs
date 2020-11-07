using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public abstract class AddressableObject : MonoBehaviour
{
    [Header("Addressable Config")]
    [SerializeField] protected string m_AssetAddressKey;
    [SerializeField] protected bool m_LoadOnStart;

    protected abstract void LoadCompleted(Object _loadedAsset);

    protected readonly BoolReactiveProperty IsLoading = new BoolReactiveProperty();

    private Action<Object> OnLoadedComplete;
    private Object LoadedAsset;

    private IDisposable _LoadDisposable;
    protected virtual void Start()
    {
        if (m_LoadOnStart) LoadAsset(m_AssetAddressKey);
    }

    public void LoadAsset(string _addressKey)
    {
        IsLoading.Value = true;

        _LoadDisposable?.Dispose();
        _LoadDisposable = AddressableManager
            .GetAssetAsObservable<Object>(_addressKey)
            .Subscribe(_ =>
            {
                if (LoadedAsset && !m_AssetAddressKey.Equals(_addressKey))
                    Release(LoadedAsset);

                LoadedAsset = _;
                m_AssetAddressKey = _addressKey;
                OnLoadedComplete?.Invoke(_);
                IsLoading.Value = false;
            }, Debug.LogError)
            .AddTo(this);
    }

    private void Release(Object _releaseObject)
    {
        Debug.Log($"Release Addressable Asset {_releaseObject ?? null}");
        Addressables.Release(_releaseObject ?? null);
        AddressableManager.RemoveFromAssetDirectory(m_AssetAddressKey);
    }

    protected IObservable<Object> OnLoadedCompleteAsObservable() => Observable.FromEvent<Object>(
        _action => OnLoadedComplete += _action,
        _action => OnLoadedComplete -= _action
    );

    protected virtual void OnDestroy() => Release(LoadedAsset);
}