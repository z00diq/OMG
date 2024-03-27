using App.Scripts.Features.FieldSizeProvider;
using App.Scripts.Modules.SceneContainer.Installer;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.ProviderResource;
using App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Bootstrap
{
    public class InstallerMatrixServices : MonoInstaller
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private FigureProviderFiles.Config configFigures;
        
        protected override void OnInstallBindings()
        {
            Container.SetService<IFieldSizeProvider, FieldSizeProviderCamera>(new FieldSizeProviderCamera(gameCamera));

            var figureProviderFiles = new FigureProviderFiles(configFigures, new ParserFigureDummy(), new ProviderResourceUnity());
            Container.SetService<IFigureProvider, FigureProviderFiles>(figureProviderFiles);
            
            Container.SetService<IFigureRotator, FigureRotatorDummy>(new FigureRotatorDummy());
        }
    }
}