using UnityEditor;
using UnityEngine;

namespace Editor.Renderer
{
    [CustomEditor((typeof(MeshRenderer)))]
    public class MeshRendererEditor : UnityEditor.Editor
    {
        private MeshRenderer _meshRenderer;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _meshRenderer = target as MeshRenderer;
            string[] layerNames = new string[SortingLayer.layers.Length];
            for (int i = 0; i < SortingLayer.layers.Length; i++)
            { layerNames[i] = SortingLayer.layers[i].name; }

            int layerValue = SortingLayer.GetLayerValueFromID(_meshRenderer.sortingLayerID) - SortingLayer.layers[0].value;
            layerValue = EditorGUILayout.Popup("Sorting Layer", layerValue, layerNames);
            SortingLayer layer = SortingLayer.layers[layerValue];
            _meshRenderer.sortingLayerName = layer.name;
            _meshRenderer.sortingLayerID = layer.id;
            _meshRenderer.sortingOrder = EditorGUILayout.IntField("Order in Layer", _meshRenderer.sortingOrder);


        }

    }
}