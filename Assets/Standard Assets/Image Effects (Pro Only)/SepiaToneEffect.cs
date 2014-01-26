using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Sepia Tone")]
public class SepiaToneEffect : ImageEffectBase 
{
	public float transitionVelocity = 1f;
	public float deactivateTransitionVelocity = 2f;

	float intensity = 0f;
	bool activated = false;

	void Update()
	{
		if (activated) {
			intensity = Mathf.Lerp(intensity, 1f, Time.deltaTime * transitionVelocity);
		} else {
			intensity = Mathf.Lerp(intensity, 0f, Time.deltaTime * deactivateTransitionVelocity);
			
			if(0 == intensity) {
				enabled = false;
			}
		}
	}

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {

		material.SetFloat("_Intensity", intensity);

		Graphics.Blit (source, destination, material);
	}

	public void activate()
	{
		enabled = true;
		intensity = 0f;

		activated = true;
	}

	public void deactivate()
	{
		activated = false;
	}
}
