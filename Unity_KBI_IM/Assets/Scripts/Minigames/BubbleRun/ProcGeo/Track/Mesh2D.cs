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
}
