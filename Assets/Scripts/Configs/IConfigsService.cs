using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BeaverBlocks.Configs
{
    public interface IConfigsService
    {
        void Initialize();
        T Get<T>() where T : ScriptableObject;
    }
}