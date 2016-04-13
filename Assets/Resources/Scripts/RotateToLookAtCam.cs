using UnityEngine;
using System.Collections;

public class RotateToLookAtCam : MonoBehaviour {

	public bool m_LookatCamera = true;
	// Whether the UI element should rotate to face the camera.
	public Transform m_UIElement;
	// The transform of the UI to be affected.
	public Transform m_Camera;
	// The transform of the camera.

	private void Start ()
	{
		Vector3 camUp = (m_UIElement.position -  m_Camera.position).normalized;
		Vector3 localUp = m_UIElement.transform.up;

		// Allign the body's up axis with the centre of planet
		m_UIElement.rotation = Quaternion.FromToRotation(localUp,camUp) * m_UIElement.rotation;
	}


	private void Update ()
	{
		// If the UI should look at the camera set it's rotation to point from the UI to the camera.
		if (m_LookatCamera) {
			Quaternion rotation = Quaternion.LookRotation (m_UIElement.position - m_Camera.position);
			Vector3 angles = rotation.eulerAngles;
			angles.x = 0f;
			angles.z = 0f;
			rotation.eulerAngles = angles;
			m_UIElement.rotation = rotation;
		}
	}
}
