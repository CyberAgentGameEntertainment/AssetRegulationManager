using System;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.TinyRx;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    /// <summary>
    ///     Draw the <see cref="AssetRegulationEditorConstraintsPanel" />.
    /// </summary>
    internal sealed class AssetRegulationEditorConstraintsPanelPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AssetRegulationEditorConstraintsPanel _view;

        public AssetRegulationEditorConstraintsPanelPresenter(AssetRegulation regulation,
            AssetRegulationEditorConstraintsPanel view)
        {
            _view = view;
            view.Enabled = true;

            regulation.Constraints.ObservableAdd
                .Subscribe(x =>
                {
                    view.AddConstraint(x.Value);
                    view.SetConstraintOrder(x.Value.Id, regulation.GetConstraintOrder(x.Value.Id));
                })
                .DisposeWith(_disposables);

            regulation.Constraints.ObservableRemove
                .Subscribe(x => view.RemoveConstraint(x.Value.Id))
                .DisposeWith(_disposables);

            regulation.Constraints.ObservableClear
                .Subscribe(_ => view.ClearConstraints())
                .DisposeWith(_disposables);

            regulation.ConstraintOrderChangedAsObservable
                .Subscribe(x => _view.SetConstraintOrder(x.id, x.index))
                .DisposeWith(_disposables);

            foreach (var constraint in regulation.Constraints.Values.OrderBy(x => regulation.GetConstraintOrder(x.Id)))
            {
                view.AddConstraint(constraint);
                view.SetConstraintOrder(constraint.Id, regulation.GetConstraintOrder(constraint.Id));
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _view.Enabled = false;
        }
    }
}
