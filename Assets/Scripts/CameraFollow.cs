using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
	#region Variables
	[SerializeField] Transform target;

    [Range(0,1)]
    [SerializeField] float minPercentage;
    [Range(0, 1)]
    [SerializeField] float maxPercentage;

    [SerializeField] RectTransform image;

    [SerializeField] Vector3 lastPosToLerp;

    Camera cam = null;
    CharacterController controller;
    CharacterControllerSimpleConfig config;

    [SerializeField] float smoothTime = .5f;
    [SerializeField] float maxSpeed = 50f;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        cam = Camera.main;
        lastPosToLerp = transform.position;
        controller = target.gameObject.GetComponent<CharacterController>();
        config = target.gameObject.GetComponent<CharacterControllerSimpleConfig>();
	}

    void Update()
	{
        Vector3 min = new Vector3(
            Screen.width * minPercentage,
            Screen.height * minPercentage, 
            cam.nearClipPlane
            );

        Vector3 max = new Vector3(
            Screen.width * maxPercentage, 
            Screen.height * maxPercentage, 
            cam.nearClipPlane
            );

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        float width = max.x - min.x;
        float height = max.y - min.y;

        image.sizeDelta = new Vector3(Mathf.Abs(width), Mathf.Abs(height));
        image.position = min;

        bool inside = screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y;
        if (!inside)
        {
            lastPosToLerp = new Vector3(target.position.x, Mathf.Round(target.position.y) + 3, transform.position.z);
        }

        if (controller.isGrounded)
        {
            lastPosToLerp.y = config.actualFloor.position.y + 6;
        }

        Vector3 vel = new Vector3();
        transform.position = Vector3.SmoothDamp(transform.position, lastPosToLerp, ref vel, smoothTime, maxSpeed);
    }
    #endregion
}
