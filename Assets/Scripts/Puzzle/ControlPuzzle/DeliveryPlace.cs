using UnityEngine;

public class DeliveryPlace : MonoBehaviour
{
    [Tooltip("0 = Up Left | 1 = Down Left | 2 = Up Right | 3 = Down Right")]
    [Range(0,3)]
    public int deliveryPlaceLocal;
    public ControlPuzzleManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ControlledObject"))
        {
            ControlledObject controlled = other.gameObject.GetComponent<ControlledObject>();
            if (controlled.objectType == deliveryPlaceLocal)
            {
                manager.Completed();
            }
            else
            {
                GameManager.instance.Strike();
                controlled.RestetPosition();
            }
        }
    }
}
