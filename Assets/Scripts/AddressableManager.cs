using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public static class AddressableManager
{
    private static Dictionary<string, Object> _AssetDirectory = new Dictionary<string, Object>();

    public static IObservable<Object> GetAssetAsObservable(string _assetKey)
    {
        if (_AssetDirectory.ContainsKey(_assetKey))
        {
            return Observable.Create<Object>(_observer =>
            {
                var disposable = Observable.EveryUpdate().Subscribe(_ =>
                {
                    _observer.OnNext(_AssetDirectory[_assetKey]);
                    _observer.OnCompleted();
                }, _observer.OnError);

                return Disposable.Create(() => disposable?.Dispose());
            });
        }

        return AddressablesExtensions.LoadAssetAsync<Object>(_assetKey).Do(_ => _AssetDirectory[_assetKey] = _);
    }

    public static IObservable<Object> InstantiateAsObservable(string _assetKey)
    {
        if (_AssetDirectory.ContainsKey(_assetKey))
        {
            return Observable.Create<Object>(_observer =>
            {
                var disposable = Observable.EveryUpdate().Subscribe(_ =>
                {
                    _observer.OnNext(_AssetDirectory[_assetKey]);
                    _observer.OnCompleted();
                }, _observer.OnError);

                return Disposable.Create(() => disposable?.Dispose());
            });
        }

        return AddressablesExtensions.InstantiateAsync(_assetKey).Do(_ => _AssetDirectory[_assetKey] = _);
    }

    public static bool RemoveFromAssetDirectory(string _assetKey) => _AssetDirectory.Remove(_assetKey);
}