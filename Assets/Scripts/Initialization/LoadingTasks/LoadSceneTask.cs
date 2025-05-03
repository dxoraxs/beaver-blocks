using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;

namespace BeaverBlocks.Initialization.LoadingTasks
{
    public class LoadSceneTask : ILoadingTask
    {
        private readonly string _sceneName;

        [Preserve]
        public LoadSceneTask(string sceneName)
        {
            _sceneName = sceneName;
        }

        public UniTask LoadAsync()
        {
            return SceneManager.LoadSceneAsync(_sceneName).ToUniTask();
        }
    }
}