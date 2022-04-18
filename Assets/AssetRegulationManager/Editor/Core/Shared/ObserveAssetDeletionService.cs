using System;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Shared
{
    /// <summary>
    ///     Observe the <see cref="UnityEngine.Object" /> and notify when it is deleted.
    /// </summary>
    internal sealed class ObserveAssetDeletionService : IDisposable
    {
        private readonly Subject<Empty> _assetDeletedSubject = new Subject<Empty>();
        private readonly Object _target;

        private bool _didInvoke;

        public ObserveAssetDeletionService(Object target)
        {
            _target = target;
            EditorApplication.update += Update;
            Update();
        }

        public IObservable<Empty> AssetDeletedAsObservable => _assetDeletedSubject;

        public void Dispose()
        {
            _assetDeletedSubject.Dispose();
            EditorApplication.update -= Update;
        }

        private void Update()
        {
            if (_didInvoke) return;

            if (_target == null)
            {
                _assetDeletedSubject.OnNext(Empty.Default);
                _didInvoke = true;
            }
        }
    }
}
