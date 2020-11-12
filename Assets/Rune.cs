using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rune : MonoBehaviour
{
    public RuneManager rune;

    public char text;
    public int order;
    public bool selected;
    [SerializeField] Text UItext;
    bool listening;

    private void Awake()
    {
        rune = transform.parent.gameObject.GetComponent<RuneManager>();
    }

    void Update()
    {
        UItext.text = text.ToString();

        if (listening)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                selected = true;
                rune.CheckOrder(this);
            }
        }
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
