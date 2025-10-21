using UnityEngine;

namespace Bipolar.ProceduralMeshes.Examples
{
    [CreateAssetMenu(menuName = CreateAssetMenuPath.Root + "Cone Procedural Mesh")]
    public class ProceduralCone : ProceduralMesh
    {
        [field: SerializeField, Min(3)]
        public int Resolution { get; set; } = 8;

        [field: SerializeField, Min(0)]
        public float Radius1 { get; set; } = 0;

        [field: SerializeField, Min(0)]
        public float Radius2 { get; set; } = 1;

        [field: SerializeField, Min(0)]
        public float Depth { get; set; } = 2;
        
        [field: SerializeField, Range(0, 1)]
        public float PivotRelativeHeight{ get; set; } = 0;

        public override void BuildMesh(Mesh mesh)
        {
            var vertices = new Vector3[2 * Resolution];
            var uvs = new Vector2[2 * Resolution];

            int triCount = 2 * 3 * Resolution;
            var triangles = new int[triCount];
            int currentTriangleIndex = 0;

            Vector3 topPosition = (1f - PivotRelativeHeight) * Depth * Vector3.up;
            Vector3 bottomPosition = PivotRelativeHeight * Depth * Vector3.down;
            float angleDelta = 360f / Resolution;
            for (int i = 0; i < Resolution; i++)
            {
                float angle = angleDelta * i;
                var radialDirection = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
                vertices[i] = topPosition + radialDirection * Radius1;
                vertices[Resolution + i] = bottomPosition + radialDirection * Radius2;

                int nextIndex = (i + 1) % Resolution;
                AddQuad(Resolution + i, Resolution + nextIndex, i, nextIndex);
            }

            void AddTriangle(int a, int b, int c)
            {
                triangles[currentTriangleIndex++] = a;
                triangles[currentTriangleIndex++] = b;
                triangles[currentTriangleIndex++] = c;
            }

            void AddQuad(int a, int b, int c, int d)
            {
                AddTriangle(a, b, c);
                AddTriangle(c, b, d);
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
        }
    }
}