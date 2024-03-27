using App.Scripts.Features.GridField.GridContainer;
using App.Scripts.Features.GridField.GridContainer.Step;
using App.Scripts.Modules.Factory;
using App.Scripts.Modules.SceneContainer.Installer;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Bootstrap
{
    public class InstallerMatrixViews : MonoInstaller
    {
        [SerializeField] private ViewGridContainer viewGridContainer;
        [SerializeField] private StepInitializeGridView.Config configField;
        [SerializeField] private ViewCellSpriteRender prefabCell;

        protected override void OnInstallBindings()
        {
            Container.SetService<IViewGridContainer, ViewGridContainer>(viewGridContainer);
            Container.SetServiceSelf(configField);

            
            Container.SetService<IFactory<ViewCell>, FactoryViewCell>(new FactoryViewCell(prefabCell));
        }
    }
}