using UnityEngine;

namespace Laz
{
    public static class LineFactory
    {
        private const float LINE_WIDTH = 0.1f;
        
        public static LineRenderer CreateLineBetween(Vector3 source, Vector3 destination)
        {
            var tempGameObject = new GameObject {name = "Line"};
            var lineRenderer = AddLineRenderer(tempGameObject);
            var arrayOfPositions = CreateArrayOfPositionsFrom(source, destination);
            SetLineRendererPositions(lineRenderer, arrayOfPositions);
            SetLineStyle(lineRenderer);
            return lineRenderer;
        }

        private static void SetLineStyle(LineRenderer line)
        {
            line.material = DebugGenerateMaterial();
            line.startWidth = LINE_WIDTH;
            line.endWidth = LINE_WIDTH;
        }

        private static void SetLineRendererPositions(LineRenderer line, Vector3[] positions)
        {
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }

        private static LineRenderer AddLineRenderer(GameObject gameObject)
        {
            return gameObject.AddComponent<LineRenderer>();
        }

        private static Vector3[] CreateArrayOfPositionsFrom(Vector3 source, Vector3 destination)
        {
            return new[] {source, destination};
        }

        private static Material DebugGenerateMaterial()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            var material = go.GetComponent<MeshRenderer>().sharedMaterial;
            GameObject.Destroy(go);
            return material;
        }
    }
}