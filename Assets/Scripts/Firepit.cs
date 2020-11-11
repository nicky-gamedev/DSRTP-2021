using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firepit : MonoBehaviour
{
    /*
     *  Usar o codigo de cores:
     *  0 = Vermelho
     *  1 = Branco
     *  2 = Azul
     *  3 = Amarelo
     *  4 = Preto
     *  5 = Rosa
     *  6 = Verde
     */

    //[Tooltip("Usar o codigo de cores: 0 = Vermelho| 1 = Branco| 2 = Azul| 3 = Amarelo| 4 = Preto| 5 = Rosa| 6 = Verde")]
    //public int colorCode;
    public enum enumColorCode { Vermelho, Branco, Azul, Amarelo, Preto, Rosa, Verde }
    public enumColorCode chosenColorCode;
    [Range(0,7)]
    public int colorCode;
    [Tooltip("0 = topo esquerda| 1 = topo centro| 2 = topo direita| 3 = baixo esquerda| 4 = baixo centro| 5 = baixo direita")]
    [Range(0,5)]
    public int position;
    [SerializeField]
    private FirePitPuzzleManager manager;
    private bool listening;

    private void Start()
    {
        switch (chosenColorCode)
        {
            case enumColorCode.Vermelho:
                colorCode = 0;
                break;
            case enumColorCode.Branco:
                colorCode = 1;
                break;
            case enumColorCode.Azul:
                colorCode = 2;
                break;
            case enumColorCode.Amarelo:
                colorCode = 3;
                break;
            case enumColorCode.Preto:
                colorCode = 4;
                break;
            case enumColorCode.Rosa:
                colorCode = 5;
                break;
            case enumColorCode.Verde:
                colorCode = 6;
                break;
            default:
                colorCode = 7;
                break;
        }
        Material mt = GetComponentInChildren<MeshRenderer>().material;
        switch (colorCode)
        {
            case 0:
                mt.color = Color.red;
                break;
            case 1:
                mt.color = Color.white;
                break;
            case 2:
                mt.color = Color.blue;
                break;
            case 3:
                mt.color = Color.yellow;
                break;
            case 4:
                mt.color = Color.black;
                break;
            case 5:
                mt.color = Color.magenta;
                break;
            case 6:
                mt.color = Color.green;
                break;
            default:
                mt.color = Color.grey;
                break;
        }
        listening = false;
    }
    private void Update()
    {
        if (listening)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                manager.Interact(position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //se o jogador entra no trigger, começa a ouvir se o input foi pressionado
            listening = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //se o jogador sai do trigger para de ouvir
            listening = false;
        }
    }
}
