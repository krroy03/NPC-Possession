using UnityEngine;
using System.Collections;

using UnityStandardAssets.ImageEffects;

public class TransitionEffect : MonoBehaviour {

	public Camera cam;
	public MotionBlur motionBlur;
	public SelectiveGrayScale grayScale; 

	private static float BLUR_MIN = 0f;
	private static float BLUR_MAX = 1f;
	private static float CAM_MIN = 60f;
	private static float CAM_MAX = 80f;

	private static float GRAYSCALE_MIN = 0f;
	private static float GRAYSCALE_MAX = 1f;

	private static float FX_ALT_TIME = 1f;
	private static float FX_HOLD_TIME = 1f;

	private bool _isSuperCoolFX = false;

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.O))
			StartFX ();
		if (Input.GetKeyDown (KeyCode.P))
			EndFX ();
	}

	void Start() {
		motionBlur.blurAmount = 0f;
	}
	public void StartFX() {
		if (!_isSuperCoolFX) {

			_isSuperCoolFX = true;

			StopCoroutine ("EndBlur");
			//StopCoroutine ("DecreaseCamView");
			StopCoroutine ("EndGrayScale");

			StartCoroutine ("StartBlur");
			//StartCoroutine ("IncreasCamView");
			StartCoroutine("StartGrayScale");
		}
	}

	public void EndFX() {
		if (_isSuperCoolFX) {

			_isSuperCoolFX = false;

			StopCoroutine ("StartBlur");
			//StopCoroutine ("IncreaseCamView");
			StopCoroutine("StartGrayScale");

			StartCoroutine ("EndBlur");
			//StartCoroutine ("DecreaseCamView");
			StartCoroutine ("EndGrayScale");
		}
	}

	IEnumerator StartBlur() {
		motionBlur.enabled = true;
		float gap = (BLUR_MAX - BLUR_MIN) / FX_ALT_TIME * Time.fixedDeltaTime;
		while (motionBlur.blurAmount < BLUR_MAX) {
			motionBlur.blurAmount += gap;
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds (FX_HOLD_TIME);
		EndFX ();
	}

	IEnumerator EndBlur() {
		float gap = (BLUR_MAX - BLUR_MIN) / FX_ALT_TIME * Time.fixedDeltaTime;
		while (motionBlur.blurAmount > BLUR_MIN) {
			motionBlur.blurAmount -= gap;
			yield return new WaitForFixedUpdate();
		}
		motionBlur.blurAmount = BLUR_MIN;
		motionBlur.enabled = false;
	}


	IEnumerator IncreasCamView() {
		float gap = (CAM_MAX - CAM_MIN) / FX_ALT_TIME * Time.fixedDeltaTime;
		while (cam.fieldOfView < CAM_MAX) {
			cam.fieldOfView += gap;
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator DecreaseCamView() {
		float gap = (CAM_MAX - CAM_MIN) / FX_ALT_TIME * Time.fixedDeltaTime;
		while (cam.fieldOfView > CAM_MIN) {
			cam.fieldOfView -= gap;
			yield return new WaitForFixedUpdate();
		}
		cam.fieldOfView = CAM_MIN;
	}

	IEnumerator StartGrayScale() {
		grayScale.enabled = true;
		float gap = (GRAYSCALE_MAX - GRAYSCALE_MIN) / FX_ALT_TIME * Time.fixedDeltaTime;
		while (grayScale.rampOffset < GRAYSCALE_MAX) {
			grayScale.rampOffset += gap;
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds (FX_HOLD_TIME);
		EndFX ();
	}

	IEnumerator EndGrayScale() {
		float gap = (GRAYSCALE_MAX - GRAYSCALE_MIN) / FX_ALT_TIME * Time.fixedDeltaTime;
		while (grayScale.rampOffset > GRAYSCALE_MIN) {
			grayScale.rampOffset-= gap;
			yield return new WaitForFixedUpdate();
		}
		grayScale.rampOffset = GRAYSCALE_MIN;
		grayScale.enabled = false;
	}


}
