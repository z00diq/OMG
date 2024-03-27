using App.Scripts.Features.LevelSelection;
using App.Scripts.Modules.SceneContainer.Installer;
using App.Scripts.Modules.Systems;
using App.Scripts.Scenes.SceneMatrix.Features.FigureRotator;
using App.Scripts.Scenes.SceneMatrix.Features.FigureSwitcher;
using App.Scripts.Scenes.SceneMatrix.Features.Grid;
using App.Scripts.Scenes.SceneMatrix.Features.Grid.Systems;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Bootstrap
{
    public class InstallerMatrixSystems : MonoInstaller
    {
        [SerializeField] private ViewSwitchNavigator levelSwitcher;
        [SerializeField] private ViewSwitchNavigator figureRotator;
        
        protected override void OnInstallBindings()
        {
            var systemGroup = new SystemsGroup();
            
            systemGroup.AddSystem(Container.CreateInstanceWithArguments<SystemRotateFigure>(figureRotator));
            systemGroup.AddSystem(Container.CreateInstance<SystemRebuildField>());
            systemGroup.AddSystem(Container.CreateInstanceWithArguments<SystemSwitchFigure>(levelSwitcher));
            
            Container.SetServiceSelf(systemGroup);
        }
    }
}