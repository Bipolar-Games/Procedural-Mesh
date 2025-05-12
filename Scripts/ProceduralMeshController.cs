using UnityEngine;

namespace Bipolar.ProceduralMeshes
{
    [RequireComponent(typeof(MeshFilter))]
    [AddComponentMenu("Bipolar/Procedural Mesh Controller")]
    public class ProceduralMeshController : MonoBehaviour
    {
        public event System.Action<ProceduralMeshController> OnMeshChanged;

        [SerializeField]
        private ProceduralMesh proceduralMesh;
        public ProceduralMesh ProceduralMesh
        {
            get => proceduralMesh;
            set
            {
                bool changed = proceduralMesh != value;
                proceduralMesh = value;
                if (changed)
                    OnMeshChanged?.Invoke(this);
            }
        }

        private MeshFilter _meshFilter;
        public MeshFilter MeshFilter
        {
            get
            {
                if (_meshFilter == null)
                    _meshFilter = GetComponent<MeshFilter>();
                return _meshFilter;
            }
        }

        private Mesh _mesh;
        protected Mesh Mesh
        {
            get
            {
                if (_mesh == null)
                    _mesh = new Mesh();
                return _mesh;
            }
        }

        private void OnEnable()
        {
            Subscribe();
            proceduralMesh.BuildMesh(Mesh);
        }

        [ContextMenu("Refresh Mesh")]
        private void Refresh() => Refresh(proceduralMesh);

#if UNITY_EDITOR
        private ProceduralMesh previousMesh;
#endif

        private void Refresh(ProceduralMesh mesh)
        {
#if UNITY_EDITOR
            if (this == null && proceduralMesh)
            {
                proceduralMesh.OnChanged -= Refresh;
                return;
            }
#endif
            if (enabled && proceduralMesh)
            {
                proceduralMesh.BuildMesh(Mesh);
                Mesh.name = proceduralMesh.name + " Generated";
                MeshFilter.sharedMesh = Mesh;
                OnMeshChanged?.Invoke(this);
            }
        }

        private void OnDisable()
        {
            if (proceduralMesh)
                proceduralMesh.OnChanged -= Refresh;
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (previousMesh)
                previousMesh.OnChanged -= Refresh;
            previousMesh = proceduralMesh;
#endif
            Subscribe();
            Refresh();
        }

        private void Subscribe()
        {
            if (proceduralMesh)
            {
                proceduralMesh.OnChanged -= Refresh;
                proceduralMesh.OnChanged += Refresh;
            }
        }
    }
}
