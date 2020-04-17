using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class PostProcess : MonoBehaviour
{
	Camera AttachedCamera;
	public Shader DrawSimple;
	public Shader Post_Outline;
	Camera TempCam;
	Material Post_Mat;

	Material OutlineMaskMat;
	private RenderTexture TempRT = null;

	// PostProcess
	[SerializeField]
	private Material postprocessMaterial;
	[SerializeField]
	private Material postprocessMaterialBlurPlusAO;

	private CommandBuffer m_outlineCommandBuffer;

	void Start()
	{
		AttachedCamera = GetComponent<Camera>();
		TempCam = new GameObject().AddComponent<Camera>();
		TempCam.enabled = false;
		Post_Mat = new Material(Post_Outline);
		OutlineMaskMat = new Material(DrawSimple);
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//make the temporary rendertexture
		if (TempRT == null)
			TempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

		//put it to video memory
		TempRT.Create();
		Graphics.Blit(source, TempRT, postprocessMaterial);
		postprocessMaterialBlurPlusAO.SetTexture("_SceneTex", source);
		Graphics.Blit(TempRT, destination, postprocessMaterialBlurPlusAO);
	}

}