using Cysharp.Threading.Tasks;

namespace BeaverBlocks.Initialization.LoadingTasks
{
    public interface ILoadingTask
    {
        UniTask LoadAsync();
    }
}