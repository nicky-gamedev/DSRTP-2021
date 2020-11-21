using UnityEngine;

public class Rune : MonoBehaviour
{
    public RuneManager rune;

    public char text;
    public GameObject runePrefab;
    public int order;
    public bool selected;
    bool listening;

    public ParticleSystem interaction;

    private void Awake()
    {
        rune = transform.parent.gameObject.GetComponent<RuneManager>();
    }

    void Update()
    {
        if (listening)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interaction.Play();
                selected = true;
                rune.CheckOrder(this);
            }
        }
    }

    public void Steup()
    {
        interaction.playOnAwake = false;
        interaction.Stop();
        GameObject go = Instantiate(runePrefab, transform);
        go.transform.position += Vector3.up * 2;
        go.transform.localScale = new Vector3(.3f, .3f, .3f);
        go.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            listening = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            listening = false;
        }
    }
}
