using UnityEditor;

namespace Bipolar.ProceduralMeshes.Editor
{
    [CustomEditor(typeof(ProceduralMeshController))]
    public class ProceduralMeshControllerEditor : UnityEditor.Editor
    {
        private const string ProceduralmMeshPropertyName = "proceduralMesh";

        ProceduralMeshEditor meshInnerEditor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            var proceduralMeshProperty = serializedObject.FindProperty(ProceduralmMeshPropertyName);
            if (proceduralMeshProperty != null && proceduralMeshProperty.objectReferenceValue is ProceduralMesh proceduralMesh)
            {
                if (meshInnerEditor == null || meshInnerEditor.target != proceduralMesh)
                {
                    if(meshInnerEditor)
                        meshInnerEditor.Dispose();
                    meshInnerEditor = (ProceduralMeshEditor)CreateEditor(proceduralMesh);
                }
                meshInnerEditor.DrawDefaultInspector();
            }

            EditorGUI.indentLevel--;
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            if (meshInnerEditor)
                meshInnerEditor.Dispose();
        }
    }
}
