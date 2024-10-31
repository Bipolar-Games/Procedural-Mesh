using UnityEngine;

namespace Bipolar.ProceduralMeshes
{
    [RequireComponent(typeof(MeshFilter))]
    public class ProceduralMesh : MonoBehaviour
    {
        public event System.Action<ProceduralMesh> OnMeshChanged;

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

        [SerializeField]
        private ProceduralMeshAsset meshAsset;
        public ProceduralMeshAsset MeshAsset
        {
            get => meshAsset;
            set
            {
                bool changed = meshAsset != value;
                meshAsset = value;
                if (changed)
                    OnMeshChanged?.Invoke(this);
            }
        }

        private void OnEnable()
        {
            Subscribe();
            MeshFilter.sharedMesh = meshAsset.SharedMesh;
        }

        private void Subscribe()
        {
            if (meshAsset)
            {
                meshAsset.OnChanged -= MeshChangedCallback;
                meshAsset.OnChanged += MeshChangedCallback;
            }
        }

#if UNITY_EDITOR
        private ProceduralMeshAsset previousMesh;
#endif
        [ContextMenu("Regenerate")]
        private void OnValidate()
        {
            if (meshAsset)
            {
                meshAsset.Validate();
                MeshFilter.sharedMesh = meshAsset.SharedMesh;
            }

#if UNITY_EDITOR
            if (previousMesh)
                previousMesh.OnChanged -= MeshChangedCallback;
            previousMesh = meshAsset;
#endif
            Subscribe();
        }

        private void MeshChangedCallback(ProceduralMeshAsset mesh)
        {
            MeshFilter.sharedMesh = meshAsset.SharedMesh;
            OnMeshChanged?.Invoke(this);
        }

        private void OnDisable()
        {
            if (meshAsset)
                meshAsset.OnChanged -= MeshChangedCallback;
        }
    }
}
