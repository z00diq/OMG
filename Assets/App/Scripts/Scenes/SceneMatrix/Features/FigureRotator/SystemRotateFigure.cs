using App.Scripts.Features.LevelSelection;
using App.Scripts.Modules.Grid;
using App.Scripts.Modules.Systems;
using App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services;
using App.Scripts.Scenes.SceneMatrix.Features.Grid.Signals;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureRotator
{
    public class SystemRotateFigure : ISystem
    {
        private readonly IViewSwitchNavigator _switchNavigator;
        private readonly IFigureRotator _figureRotator;
        public SystemContext Context { get; set; }

        public SystemRotateFigure(IViewSwitchNavigator switchNavigator, IFigureRotator figureRotator)
        {
            _switchNavigator = switchNavigator;
            _figureRotator = figureRotator;
        }
        
        public void Init()
        {
            _switchNavigator.ChangeLevel += RotateFigure;
        }

        private void RotateFigure(int rotateIndex)
        {
            var grid = Context.Data.GetComponent<Grid<bool>>();

            var nextGrid = _figureRotator.RotateFigure(grid, rotateIndex);
            
            Context.Signals.SetComponent(new RequestUpdateFigure
            {
                Figure = nextGrid
            });
            
        }

        public void Update(float dt)
        {
        }

        public void Cleanup()
        {
            _switchNavigator.ChangeLevel -= RotateFigure;
        }
    }
}