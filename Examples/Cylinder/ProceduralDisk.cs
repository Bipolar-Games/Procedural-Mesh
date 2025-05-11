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

        }
    }
}