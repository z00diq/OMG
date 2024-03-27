using App.Scripts.Features.GridField.GridContainer;
using App.Scripts.Modules.Factory;
using App.Scripts.Modules.Grid;
using App.Scripts.Modules.Systems;
using App.Scripts.Scenes.SceneMatrix.Features.Grid.Signals;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.Grid.Systems
{
    public class SystemRebuildField : ISystem
    {
        private readonly IViewGridContainer _viewGridContainer;
        private readonly IFactory<ViewCell> _factory;
        public SystemContext Context { get; set; }

        public SystemRebuildField(IViewGridContainer viewGridContainer, IFactory<ViewCell> factory)
        {
            _viewGridContainer = viewGridContainer;
            _factory = factory;
        }
        
        public void Init()
        {
            
        }

        public void Update(float dt)
        {
            if (Context.Signals.TryGetComponent<RequestUpdateFigure>(out var request) is false)
            {
                return;
            }

            UpdateField(request.Figure);
        }

        private void UpdateField(Grid<bool> componentFigure)
        {
            _viewGridContainer.ClearCells();

            _viewGridContainer.UpdateGrid(componentFigure.Size);
            
            for (int i = 0; i < componentFigure.Height; i++)
            {
                for (int j = 0; j < componentFigure.Width; j++)
                {
                    if (componentFigure[j, i])
                    {
                        var view = _factory.Create();
                        _viewGridContainer.AddViewCell(view, new Vector2Int(j, i));
                    }
                }
            }
            
            Context.Data.SetComponent(componentFigure);
        }

        public void Cleanup()
        {
        }
    }
}