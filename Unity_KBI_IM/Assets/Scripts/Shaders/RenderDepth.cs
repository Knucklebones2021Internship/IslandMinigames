using UnityEngine;

[ExecuteInEditMode]
public class RenderDepth : MonoBehaviour {
	[SerializeField] Material mat;

	private void OnEnable() {
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		//print("blit");
		//Graphics.Blit(source, destination, mat);
	}
}
