using System;
using System.Collections.Generic;
using UniRx;

public static class AddressableManager
{
    private static Dictionary<string, UnityEngine.Object> _AssetDirectory = new Dictionary<string, UnityEngine.Object>();

    public static IObservable<T> GetAssetAsObservable<T>(string _assetKey) where T : UnityEngine.Object
    {
        if (_AssetDirectory.ContainsKey(_assetKey))
        {
            return Observable.Create<T>(_observer =>
            {
                var disposable = Observable.EveryUpdate().Subscribe(_ =>
                {
                    _observer.OnNext((T) _AssetDirectory[_assetKey]);
                    _observer.OnCompleted();
                }, _observer.OnError);

                return Disposable.Create(() => disposable?.Dispose());
            });
        }

        return AddressablesExtensions.LoadAssetAsync<T>(_assetKey).Do(_ => _AssetDirectory[_assetKey] = _);
    }

    public static bool RemoveFromAssetDirectory(string _assetKey) => _AssetDirectory.Remove(_assetKey);
}