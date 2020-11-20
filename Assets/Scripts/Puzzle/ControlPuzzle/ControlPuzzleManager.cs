using UnityEngine;

public class ControlPuzzleManager : MonoBehaviour
{
    public Transform[] buttons = new Transform[4];
    public Transform[] delivery = new Transform[4];
    public Transform door;

    private AudioSource ad => GetComponent<AudioSource>();

    public void Completed()
    {
        foreach (Transform button in buttons)
        {
            button.gameObject.GetComponent<ControlButton>().enabled = false;
            button.GetComponent<BoxCollider>().enabled = false;
            button.GetComponentInChildren<ParticleSystem>().Play();
        }
        foreach (Transform delivery in delivery)
        {
            delivery.gameObject.GetComponent<DeliveryPlace>().enabled = false;
            delivery.GetComponent<BoxCollider>().enabled = false;
        }
        door.gameObject.SetActive(false);
        ad.Play();
    }
}
