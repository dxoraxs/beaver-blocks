using Cysharp.Threading.Tasks;
using BeaverBlocks.Initialization.LoadingTasks;
using UnityEngine;
using VContainer.Unity;

namespace BeaverBlocks.Initialization
{
    public class InitializationController : IInitializable
    {
        public void Initialize()
        {
            Application.targetFrameRate = 60;
            StartInitialization().Forget();
        }

        private async UniTaskVoid StartInitialization()
        {
            await WaitLoadScene();
        }

        private async UniTask WaitLoadScene()
        {
            await new LoadSceneTask(Constants.MainScene).LoadAsync();
        }
    }
}