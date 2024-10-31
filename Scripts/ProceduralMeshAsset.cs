using UnityEngine;

namespace Bipolar.ProceduralMeshes
{
    public abstract class ProceduralMeshAsset : ScriptableObject
    {
        public event System.Action<ProceduralMeshAsset> OnChanged;

        private Mesh sharedMesh;
        public Mesh SharedMesh
        {
            get
            {
                if (sharedMesh == null)
                    sharedMesh = CreateMesh();
                return sharedMesh;
            }
        }

        public Mesh Mesh => CreateMesh();

        public abstract void BuildMesh(Mesh mesh);

        private Mesh CreateMesh()
        {
            var mesh = new Mesh();
            BuildMesh(mesh);
            mesh.name = name;
            return mesh;
        }

        public virtual void Validate() { }

        protected virtual void OnValidate()
        {
            Validate(); 
            OnChanged?.Invoke(this);
        }
    }
}
