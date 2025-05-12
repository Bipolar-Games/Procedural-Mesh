using UnityEditor;
using UnityEngine;

namespace Bipolar.ProceduralMeshes
{
    public abstract class ProceduralMesh : ScriptableObject
    {
        public event System.Action<ProceduralMesh> OnChanged;
        
        public abstract void BuildMesh(Mesh mesh);

        public virtual void Validate() { }

        protected virtual void OnValidate()
        {
            Validate(); 
            OnChanged?.Invoke(this);
        }
    }
}
