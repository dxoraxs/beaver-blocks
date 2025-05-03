using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BeaverBlocks.Configs
{
    public interface IConfigsService
    {
        UniTask Initialize();
        T Get<T>() where T : ScriptableObject;
    }
}