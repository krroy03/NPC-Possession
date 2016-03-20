using UnityEngine;
using System;

namespace UnityStandardAssets.ImageEffects
{

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
public class SelectiveGrayScale : ImageEffectBase {
	public Texture  textureRamp;
	[Range(-1.0f,1.0f)]
	public float    rampOffset;
	public float _RedPower;
	public float _RedDelta;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetTexture("_RampTex", textureRamp);
		material.SetFloat("_RampOffset", rampOffset);
		material.SetFloat("_RedPower", _RedPower);
		material.SetFloat("_RedDelta", _RedDelta);
		Graphics.Blit (source, destination, material);
	}
}
}

