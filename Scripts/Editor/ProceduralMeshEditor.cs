using System;
using UnityEditor;
using UnityEngine;

namespace Bipolar.ProceduralMeshes.Editor
{
    [CustomEditor(typeof(ProceduralMesh), editorForChildClasses: true)]
    public class ProceduralMeshEditor : UnityEditor.Editor, IDisposable
    {
        private MeshPreview meshPreview;
        private Mesh previewMesh;
        
        private void OnEnable()
        {
            if (previewMesh == null) 
                previewMesh = new Mesh();

            meshPreview ??= new MeshPreview(previewMesh);
        }

        [MenuItem("CONTEXT/" + nameof(ProceduralMesh) + "/Export Mesh")]
        private static void ExportMesh(MenuCommand command)
        {
            var meshAsset = (ProceduralMesh)command.context;

            var mesh = new Mesh();
            meshAsset.BuildMesh(mesh);
            mesh.name = meshAsset.name;

            var meshAssetPath = AssetDatabase.GetAssetPath(meshAsset);
            var directoryPath = meshAssetPath.Substring(0, meshAssetPath.LastIndexOf('/'));
            AssetDatabase.CreateAsset(mesh, $"{directoryPath}/{mesh.name} {System.Guid.NewGuid()}.mesh");
            AssetDatabase.SaveAssets();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var meshAssetProperty = serializedObject.FindProperty("mesh");
            if (meshAssetProperty == null)
                return;
        }

        public override bool HasPreviewGUI() => true;

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            ((ProceduralMesh)target).BuildMesh(previewMesh);
            meshPreview.OnPreviewGUI(r, background);
        }

        public override void OnPreviewSettings()
        {
            base.OnPreviewSettings();
            meshPreview?.OnPreviewSettings();
        }

        private void OnDisable()
        {
            Dispose();
            previewMesh = null;
        }

        public void Dispose()
        {
            meshPreview.Dispose();
            meshPreview = null;
        }
    }
}
