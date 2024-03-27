using App.Scripts.Features.LevelSelection;
using App.Scripts.Modules.Systems;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider;
using App.Scripts.Scenes.SceneMatrix.Features.Grid.Signals;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureSwitcher
{
    public class SystemSwitchFigure : ISystem
    {
        private readonly IFigureProvider _figureProvider;
        private readonly IViewSwitchNavigator _viewSwitchNavigator;
        public SystemContext Context { get; set; }

        public SystemSwitchFigure(IFigureProvider figureProvider, IViewSwitchNavigator viewSwitchNavigator)
        {
            _figureProvider = figureProvider;
            _viewSwitchNavigator = viewSwitchNavigator;
        }
        
        public void Init()
        {
            _viewSwitchNavigator.ChangeLevel += SwitchSwitch;
            Context.Data.SetComponent(new ComponentCurrentFigure());
            SetupFigure(0);
        }

        private void SwitchSwitch(int index)
        {
            var currentFigure = Context.Data.GetComponent<ComponentCurrentFigure>();
            var nextIndex = currentFigure.figureIndex + index;

            if (nextIndex >= _figureProvider.TotalFiguresCount)
            {
                nextIndex = 0;
            }

            if (nextIndex < 0)
            {
                nextIndex = _figureProvider.TotalFiguresCount - 1;
            }

            currentFigure.figureIndex = nextIndex;

            SetupFigure(currentFigure.figureIndex);
        }

        private void SetupFigure(int currentFigureFigureIndex)
        {
            Context.Signals.SetComponent(new RequestUpdateFigure
            {
                Figure = _figureProvider.GetFigure(currentFigureFigureIndex)
            });
        }

        public void Update(float dt)
        {
            
        }

        public void Cleanup()
        {
            _viewSwitchNavigator.ChangeLevel -= SwitchSwitch;
        }
    }
}