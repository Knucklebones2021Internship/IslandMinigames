using UnityEngine;

[CreateAssetMenu]
public class Mesh2D : ScriptableObject {
	[System.Serializable]
	public class Vertex {
		public Vector2 point;
		public Vector2 normal;
		public float u; // UVs with the V haha
	}

	public Vertex[] vertices;
	public int[] lineIndices;

	public int VertexCount => vertices.Length;
	public int LineCount => lineIndices.Length;

	public float CalcUSpan() {
		float dist = 0;
		for (int i=0; i<LineCount; i+=2) {
			Vector2 uA = vertices[lineIndices[i]].point;
			Vector2 uB = vertices[lineIndices[i+1]].point;

			dist += (uA - uB).magnitude;
		} return dist;
	}
}
