using Cinemachine;
using UnityEngine;

public class EndCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera = null;
    [SerializeField]
    private Collider northWallCollider;
    [SerializeField]
    private GameObject bridgeColliders;

    private void OnEnable()
    {
        virtualCamera.enabled = false;
        northWallCollider.enabled = true;
        bridgeColliders.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = true;
            northWallCollider.enabled = false;
            bridgeColliders.SetActive(true);
        }
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
