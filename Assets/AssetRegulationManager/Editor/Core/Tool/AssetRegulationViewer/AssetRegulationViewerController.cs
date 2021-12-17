// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Foundation.Observable;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer
{
    internal sealed class AssetRegulationViewerController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AssetRegulationTestExecuteService _executeService;
        private readonly AssetRegulationTestGenerateService _generateService;
        private readonly AssetRegulationManagerStore _store;
        private CancellationTokenSource _testExecuteTaskCancellationTokenSource;
        private AssetRegulationTreeView _treeView;
        private AssetRegulationViewerWindow _window;

        internal AssetRegulationViewerController(AssetRegulationManagerStore store)
        {
            _store = store;
            var assetDatabaseAdapter = new AssetDatabaseAdapter();
            _generateService = new AssetRegulationTestGenerateService(store, assetDatabaseAdapter);
            _executeService = new AssetRegulationTestExecuteService(store);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        internal void Setup(AssetRegulationViewerWindow window)
        {
            _window = window;
            _treeView = _window.TreeView;

            window.AssetPathOrFilterObservable.Subscribe(_generateService.Run).DisposeWith(_disposables);
            window.CheckAllButtonClickedObservable
                .Subscribe(_ =>
                {
                    var __ = CheckAllAsync();
                })
                .DisposeWith(_disposables);
            window.CheckSelectedAddButtonClickedObservable.Subscribe(_ =>
                {
                    var __ = CheckSelectedAsync();
                })
                .DisposeWith(_disposables);
        }

        private async Task CheckAllAsync()
        {
            if (_testExecuteTaskCancellationTokenSource != null)
            {
                _testExecuteTaskCancellationTokenSource.Cancel();
            }

            _testExecuteTaskCancellationTokenSource = new CancellationTokenSource();
            await CheckAllAsync(_testExecuteTaskCancellationTokenSource.Token);
            _testExecuteTaskCancellationTokenSource = null;
        }

        private async Task CheckSelectedAsync()
        {
            if (_testExecuteTaskCancellationTokenSource != null)
            {
                _testExecuteTaskCancellationTokenSource.Cancel();
            }

            _testExecuteTaskCancellationTokenSource = new CancellationTokenSource();
            await CheckSelectedAsync(_testExecuteTaskCancellationTokenSource.Token);
            _testExecuteTaskCancellationTokenSource = null;
        }

        private async Task CheckAllAsync(CancellationToken cancellationToken)
        {
            var targets = _store.Tests.Values.ToArray();
            foreach (var test in targets)
            {
                var sequence = _executeService.CreateRunAllSequence(test.Id);
                foreach (var _ in sequence)
                {
                    await Task.Delay(1, cancellationToken);
                }
            }
        }

        private async Task CheckSelectedAsync(CancellationToken cancellationToken)
        {
            var targetEntryIds = new Dictionary<string, HashSet<string>>();
            foreach (var selection in _treeView.GetSelection())
            {
                var item = _treeView.GetItem(selection);
                if (item is AssetRegulationTestTreeViewItem testItem)
                {
                    // If the root item is selected, all child test entries will be targeted.
                    var testId = testItem.TestId;
                    if (!targetEntryIds.TryGetValue(testId, out var entryIds))
                    {
                        entryIds = new HashSet<string>();
                        targetEntryIds.Add(testId, entryIds);
                    }

                    foreach (var child in testItem.children)
                    {
                        var testEntryItem = (AssetRegulationTestEntryTreeViewItem)child;
                        entryIds.Add(testEntryItem.EntryId);
                    }
                }
                else
                {
                    var testEntryItem = (AssetRegulationTestEntryTreeViewItem)item;
                    var testId = ((AssetRegulationTestTreeViewItem)testEntryItem.parent).TestId;

                    if (!targetEntryIds.TryGetValue(testId, out var entryIds))
                    {
                        entryIds = new HashSet<string>();
                        targetEntryIds.Add(testId, entryIds);
                    }

                    entryIds.Add(testEntryItem.EntryId);
                }
            }

            // Run all the tests.
            foreach (var value in targetEntryIds)
            {
                var tests = _executeService.CreateRunSequence(value.Key, value.Value.ToArray());
                foreach (var _ in tests)
                {
                    await Task.Delay(1, cancellationToken);
                }
            }
        }
    }
}