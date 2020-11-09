using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class AddressablesExtensions
{
    public static IObservable<TObject> LoadAssetAsync<TObject>(string _addressKey) where TObject : UnityEngine.Object
    {
        return Observable.Create<TObject>(_observer =>
        {
            var disposable = new CompositeDisposable();
            disposable.Add(
                Addressables.LoadResourceLocationsAsync(_addressKey)
                .ToObservable<IList<IResourceLocation>>()
                .Subscribe(_locationResultList =>
                {
                    try
                    {
                        var assetLocation = _locationResultList.Where(_x => _x.PrimaryKey == _addressKey).FirstOrDefault();

                        if (assetLocation == null) throw new Exception($"Can not find asset with given addressKey : {_addressKey}");

                        disposable.Add(
                            Addressables.LoadAssetAsync<TObject>(assetLocation)
                            .ToObservable<TObject>()
                            .Subscribe(_ =>
                            {
                                _observer.OnNext(_);
                                _observer.OnCompleted();
                            }, _observer.OnError)
                        );
                    }
                    catch (Exception ex)
                    {
                        _observer.OnError(ex);
                    }
                }, _observer.OnError));

            return Disposable.Create(() => disposable.Dispose());
        });
    }

    public static IObservable<GameObject> InstantiateAsync(string _addressKey)
    {
        return Observable.Create<GameObject>(_observer =>
        {
            var disposable = new CompositeDisposable();
            disposable.Add(
                Addressables.LoadResourceLocationsAsync(_addressKey)
                .ToObservable<IList<IResourceLocation>>()
                .Subscribe(_locationResultList =>
                {
                    try
                    {
                        var assetLocation = _locationResultList.Where(_x => _x.PrimaryKey == _addressKey).FirstOrDefault();

                        if (assetLocation == null) throw new Exception($"Can not find asset with given addressKey : {_addressKey}");

                        disposable.Add(
                            Addressables.InstantiateAsync(_addressKey)
                            .ToObservable<GameObject>()
                            .Subscribe(_ =>
                            {
                                _observer.OnNext(_);
                                _observer.OnCompleted();
                            }, _observer.OnError)
                        );
                    }
                    catch (Exception ex)
                    {
                        _observer.OnError(ex);
                    }
                }, _observer.OnError));

            return Disposable.Create(() => disposable.Dispose());
        });
    }

    public static IObservable<TObject> ToObservable<TObject>(this AsyncOperationHandle<TObject> addressableAsync)
    {
        return Observable.Create<TObject>(_observer =>
        {
            var disposable = Observable.FromEvent<AsyncOperationHandle<TObject>>(
                    _action => addressableAsync.Completed += _action,
                    _action => addressableAsync.Completed -= _action)
                .Subscribe(_ =>
                {
                    switch (_.Status)
                    {
                        case AsyncOperationStatus.Succeeded:
                            _observer.OnNext(_.Result);
                            _observer.OnCompleted();
                            break;
                        case AsyncOperationStatus.Failed:
                            _observer.OnError(_.OperationException);
                            break;
                        default:
                            break;
                    }
                }, _observer.OnError);

            return Disposable.Create(() => disposable.Dispose());
        });
    }
}