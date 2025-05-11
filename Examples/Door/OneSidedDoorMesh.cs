using UnityEngine;

namespace Bipolar.ProceduralMeshes.Examples
{
    [CreateAssetMenu(menuName = CreateAssetMenuPath.Root + "One Sided Door Mesh")]
    public class OneSidedDoorMesh : ProceduralMesh
    {
        public enum PivotPositionType
        {
            Front,
            Center,
            Back,
        }

        private const int verticesCount = 24;

        [SerializeField]
        private Vector2 size = new Vector2(4, 3);
        public Vector2 Size
        {
            get => size;
            set => size = value;
        }

        [SerializeField]
        private Vector2 holeSize = new Vector2(1, 2);
        public Vector2 HoleSize
        {
            get => holeSize;
            set => holeSize = value;
        }

        [SerializeField, Min(0)]
        private float depth = 0.5f;
        public float Depth
        {
            get => depth;
            set => depth = value;
        }

        [SerializeField]
        private PivotPositionType pivotPosition;
        public PivotPositionType PivotPosition
        {
            get => pivotPosition;
            set => pivotPosition = value;
        }

        public override void Validate()
        {
            holeSize.x = Mathf.Max(holeSize.x, 0);
            holeSize.y = Mathf.Max(holeSize.y, 0);
            size.x = Mathf.Max(size.x, holeSize.x);
            size.y = Mathf.Max(size.y, holeSize.y);
        }

        private readonly int[] triangles =
        {
        // front
        0, 4, 5, 5, 1, 0,
        2, 6, 7, 7, 3, 2,
        4, 8, 9, 9, 5, 4,
        5, 9, 10, 10, 6, 5,
        6, 10, 11, 11, 7, 6,

        // tunnel
        12, 14, 15, 15, 13, 12,
        16, 18, 19, 19, 17, 16,
        20, 22, 23, 23, 21, 20,
    };

        public override void BuildMesh(Mesh mesh)
        {
            var vertices = new Vector3[verticesCount];
            var uvs = new Vector2[verticesCount];

            float halfWidth = size.x / 2;
            float halfHoleWidth = holeSize.x / 2;
            float halfDepth = depth / 2;
            float holeHeight = holeSize.y;
            float height = size.y;

            PopulateVertices();
            void PopulateVertices()
            {
                float front = pivotPosition switch
                {
                    PivotPositionType.Front => 0,
                    PivotPositionType.Center => -halfDepth,
                    _ => -depth,
                };

                float back = pivotPosition switch
                {
                    PivotPositionType.Front => depth,
                    PivotPositionType.Center => halfDepth,
                    _ => 0,
                };

                vertices[0] = new Vector3(-halfWidth, 0, front);
                vertices[1] = vertices[12] = vertices[13]
                    = new Vector3(-halfHoleWidth, 0, front);
                vertices[2] = vertices[17] = vertices[16]
                    = new Vector3(+halfHoleWidth, 0, front);
                vertices[3] = new Vector3(+halfWidth, 0, front);

                vertices[4] = new Vector3(-halfWidth, holeHeight, front);
                vertices[5] = vertices[14] = vertices[22] = vertices[20] = vertices[15]
                    = new Vector3(-halfHoleWidth, holeHeight, front);
                vertices[6] = vertices[19] = vertices[23] = vertices[21] = vertices[18]
                    = new Vector3(+halfHoleWidth, holeHeight, front);
                vertices[7] = new Vector3(+halfWidth, holeHeight, front);

                vertices[8] = new Vector3(-halfWidth, height, front);
                vertices[9] = new Vector3(-halfHoleWidth, height, front);
                vertices[10] = new Vector3(+halfHoleWidth, height, front);
                vertices[11] = new Vector3(+halfWidth, height, front);

                vertices[13].z = back;
                vertices[16].z = back;
                vertices[20].z = back;
                vertices[15].z = back;
                vertices[21].z = back;
                vertices[18].z = back;
            }

            PopulateUVs();
            void PopulateUVs()
            {
                float oneOverWidth = 1f / size.x;
                float oneOverHeight = 1f / size.y;

                uvs[0] = new Vector2(0, 0);
                uvs[1] = uvs[12] = new Vector2(oneOverWidth * (halfWidth - halfHoleWidth), 0);
                uvs[2] = uvs[17] = new Vector2(oneOverWidth * (halfWidth + halfHoleWidth), 0);
                uvs[3] = new Vector2(1, 0);

                uvs[4] = new Vector2(0, holeHeight * oneOverHeight);
                uvs[5] = uvs[14] = uvs[22] = new Vector2(oneOverWidth * (halfWidth - halfHoleWidth), holeHeight * oneOverHeight);
                uvs[6] = uvs[19] = uvs[23] = new Vector2(oneOverWidth * (halfWidth + halfHoleWidth), holeHeight * oneOverHeight);
                uvs[7] = new Vector2(1, holeHeight * oneOverHeight);

                uvs[8] = new Vector2(0, 1);
                uvs[9] = new Vector2(oneOverWidth * (halfWidth - halfHoleWidth), 1);
                uvs[10] = new Vector2(oneOverWidth * (halfWidth + halfHoleWidth), 1);
                uvs[11] = new Vector2(1, 1);

                uvs[13] = new Vector2(oneOverWidth * (halfWidth - halfHoleWidth + depth), 0);
                uvs[15] = new Vector2(oneOverWidth * (halfWidth - halfHoleWidth + depth), holeHeight * oneOverHeight);
                uvs[18] = new Vector2(oneOverWidth * (halfWidth + halfHoleWidth - depth), holeHeight * oneOverHeight);
                uvs[16] = new Vector2(oneOverWidth * (halfWidth + halfHoleWidth - depth), 0);

                uvs[20] = new Vector2(oneOverWidth * (halfWidth - halfHoleWidth), (holeHeight - depth) * oneOverHeight);
                uvs[21] = new Vector2(oneOverWidth * (halfWidth + halfHoleWidth), (holeHeight - depth) * oneOverHeight);
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
        }
    }
}