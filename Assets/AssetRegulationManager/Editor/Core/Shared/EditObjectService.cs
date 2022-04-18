using System;
using AssetRegulationManager.Editor.Foundation.CommandBasedUndo;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Shared
{
    /// <summary>
    ///     Service to manage storing and history of <see cref="UnityEngine.Object" />.
    /// </summary>
    internal sealed class EditObjectService : IDisposable
    {
        private readonly AutoIncrementHistory _history = new AutoIncrementHistory();
        private readonly ObservableProperty<bool> _isDirty;
        private readonly Object _store;
        private readonly AssetSaveService _saveService = new AssetSaveService();

        private bool _saveReserved;

        public EditObjectService(Object store)
        {
            _store = store;
            _isDirty = new ObservableProperty<bool>(EditorUtility.IsDirty(store));
            EditorApplication.update += OnUpdate;
        }

        public int SaveIntervalFrame { get; set; } = 10;

        public IReadOnlyObservableProperty<bool> IsDirty => _isDirty;

        public void Dispose()
        {
            if (_saveReserved)
                Save();

            _isDirty.Dispose();
            EditorApplication.update -= OnUpdate;
        }

        public void EditWithoutUndo(Action editAction, bool markAsDirty = true, bool enableAutoSave = true)
        {
            editAction();

            if (markAsDirty)
                MarkAsDirty();

            if (enableAutoSave)
                ReserveSave();
        }

        public void Edit(string commandName, Action redoAction, Action undoAction, bool markAsDirty = true,
            bool enableAutoSave = true)
        {
            _history.Register(commandName, () =>
            {
                redoAction.Invoke();

                if (markAsDirty)
                    MarkAsDirty();

                if (enableAutoSave)
                    ReserveSave();
            }, () =>
            {
                undoAction.Invoke();

                if (markAsDirty)
                    MarkAsDirty();

                if (enableAutoSave)
                    ReserveSave();
            });
        }

        public void ReserveSave()
        {
            _saveReserved = true;
        }

        public void Save()
        {
            _saveService.Run(_store);
        }

        public void MarkAsDirty()
        {
            EditorUtility.SetDirty(_store);
        }

        public void Undo()
        {
            _history.Undo();
        }

        public void Redo()
        {
            _history.Redo();
        }

        public void ClearDirty()
        {
            EditorUtility.ClearDirty(_store);
        }

        private void OnUpdate()
        {
            CheckIsDirty();

            if (_saveReserved && Time.frameCount % SaveIntervalFrame == 0)
            {
                Save();
                _saveReserved = false;
            }
        }

        private void CheckIsDirty()
        {
            var isDirty = EditorUtility.IsDirty(_store);
            if (isDirty != _isDirty.Value)
                _isDirty.Value = isDirty;
        }
    }
}
