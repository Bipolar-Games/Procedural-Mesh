using UnityEngine;

namespace Bipolar.ProceduralMeshes.Examples
{
    [CreateAssetMenu(menuName = CreateAssetMenuPath.Root + "Disk Procedural Mesh")]
    public class ProceduralDisk : ProceduralMesh
    {
        [field: SerializeField, Min(0)]
        public float Radius { get; set; } = 1;

        [field: SerializeField, Min(3)]
        public int Resolution { get; set; } = 8;

        public override void BuildMesh(Mesh mesh)
        {
            var vertices = new Vector3[Resolution + 1];
            vertices[0] = Vector3.zero;
            var triangles = new int[Resolution * 3];
            int currentTriangleIndex = 0;   

            float angleDelta = 360f / Resolution;
            for (int i = 0; i < Resolution; i++)
            {
                int index = i + 1;
                float angle = angleDelta * i;
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
                vertices[index] = direction * Radius;
                int previousIndex = i == 0 ? Resolution : i;
                AddTriangle(0, previousIndex, index);
            }

            void AddTriangle(int a, int b, int c)
            {
                triangles[currentTriangleIndex++] = a;
                triangles[currentTriangleIndex++] = b;
                triangles[currentTriangleIndex++] = c;
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }
    }
}