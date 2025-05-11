using UnityEditor;
using UnityEngine;

namespace Bipolar.ProceduralMeshes
{
    public abstract class ProceduralMesh : ScriptableObject
    {
        public event System.Action<ProceduralMesh> OnChanged;
        
        public abstract void BuildMesh(Mesh mesh);

#if UNITY_EDITOR
        [ContextMenu("Export")]
        private void ExportMesh()
        {
            var mesh = new Mesh();
            BuildMesh(mesh);
            mesh.name = name;

            var meshAssetPath = AssetDatabase.GetAssetPath(this);
            var directoryPath = meshAssetPath[..meshAssetPath.LastIndexOf('/')];
            AssetDatabase.CreateAsset(mesh, $"{directoryPath}/{mesh.name} {System.Guid.NewGuid()}.mesh");
            AssetDatabase.SaveAssets();
        }
#endif

        public virtual void Validate() { }

        protected virtual void OnValidate()
        {
            Validate(); 
            OnChanged?.Invoke(this);
        }
    }
}
