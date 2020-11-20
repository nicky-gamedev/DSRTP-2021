using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButton : MonoBehaviour
{
    private bool playerInside;
    [Tooltip("1 = Up | -1 = Down | 2 = Left | -2 = Right")]
    [Range(-2,2)]
    public int buttonType;
    public float buttonControlForce;

    public Rigidbody controlledObjectRigidbody;

    private void OnEnable()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.playOnAwake = false;
        ps.Stop();
    }

    void Update()
    {
        if (playerInside)
        {
            switch (buttonType)
            {
                case -2:
                    MoveTransform(Vector3.right);
                    break;
                case 2:
                    MoveTransform(Vector3.left);
                    break;
                case 1:
                    MoveTransform(Vector3.forward);
                    break;
                case -1:
                    MoveTransform(Vector3.back);
                    break;
                case 0:
                    Debug.LogError("Button needs identification", this);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controlledObjectRigidbody.isKinematic = false;
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            StopControl();
        }
    }

    private void MoveTransform(Vector3 direction)
    {
        controlledObjectRigidbody.velocity = (direction * buttonControlForce);
    }

    private void StopControl()
    {
        controlledObjectRigidbody.isKinematic = true;
        controlledObjectRigidbody.velocity = Vector3.zero;
    }
}
