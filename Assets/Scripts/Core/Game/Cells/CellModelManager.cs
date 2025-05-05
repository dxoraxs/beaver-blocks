using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Cells
{
    public class CellModelManager
    {
        private readonly Dictionary<(int, int), CellModel> _cellModels = new(); 
        
        public IReadOnlyDictionary<(int, int), CellModel> CellModels => _cellModels;

        [Preserve]
        public CellModelManager()
        {}
        
        public void SetModels(IReadOnlyDictionary<(int, int), CellModel> models)
        {
            _cellModels.AddRange(models);
        }
    }
}