using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.ProceduralMeshes.Examples
{
    public class OneSidedDoorCollider : MonoBehaviour
    {
        [SerializeField]
        private ProceduralMeshController proceduralMesh;

        private readonly List<BoxCollider> colliders = new List<BoxCollider>();

        private void Awake()
        {
            Subscribe();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            if (proceduralMesh != null)
            {
                proceduralMesh.OnMeshChanged -= RefreshColliders;
                proceduralMesh.OnMeshChanged += RefreshColliders;
                RefreshColliders(proceduralMesh);
            }
        }

        private void RefreshColliders(ProceduralMeshController proceduralMesh)
        {
            if (this == null || Application.isPlaying && gameObject.scene.buildIndex < 0)
            {
                this.proceduralMesh.OnMeshChanged -= RefreshColliders;
                return;
            }

            if (proceduralMesh.ProceduralMesh is OneSidedDoorMesh doorParameters)
            {
                colliders.Clear();
                GetComponents(colliders);
                int missingCollidersCount = 3 - colliders.Count;
                if (missingCollidersCount > 0)
                    for (int i = 0; i < missingCollidersCount; i++)
                        colliders.Add(gameObject.AddComponent<BoxCollider>());

                float sideWidth = (doorParameters.Size.x - doorParameters.HoleSize.x) / 2;
                float topHeight = (doorParameters.Size.y - doorParameters.HoleSize.y);
                float zCenter = doorParameters.PivotPosition switch
                {
                    OneSidedDoorMesh.PivotPositionType.Front => doorParameters.Depth / 2,
                    OneSidedDoorMesh.PivotPositionType.Center => 0,
                    _ => -doorParameters.Depth / 2
                };

                colliders[0].center = new Vector3((-doorParameters.Size.x + sideWidth) / 2, doorParameters.HoleSize.y / 2, zCenter);
                colliders[1].center = new Vector3((doorParameters.Size.x - sideWidth) / 2, doorParameters.HoleSize.y / 2, zCenter);
                colliders[0].size = colliders[1].size = new Vector3(sideWidth, doorParameters.HoleSize.y, doorParameters.Depth);

                colliders[2].center = new Vector3(0, doorParameters.Size.y - topHeight / 2, zCenter);
                colliders[2].size = new Vector3(doorParameters.Size.x, topHeight, doorParameters.Depth);
            }
        }

        private void OnDisable()
        {
            if (proceduralMesh != null)
            {
                proceduralMesh.OnMeshChanged -= RefreshColliders;
            }
        }

        private void OnDestroy()
        {
            if (proceduralMesh != null)
            {
                proceduralMesh.OnMeshChanged -= RefreshColliders;
            }
        }

        private void OnValidate()
        {
            Subscribe();
        }
    }
}