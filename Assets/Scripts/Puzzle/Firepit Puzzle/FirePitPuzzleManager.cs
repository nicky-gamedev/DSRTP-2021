using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePitPuzzleManager : MonoBehaviour
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
    [SerializeField]
    private Firepit[] firepits = new Firepit[6];
    [SerializeField]
    private GameObject door;


    private void OnEnable()
    {
        SetupFires(firepits);
    }


    public void Interact(int position)
    {
        if (CheckInteraction(position, firepits))
        {
            door.SetActive(false);
            DeactivateFire(firepits);
        }
        else
        {
            GameManager.instance.Strike();
        }
    }

    private bool CheckInteraction(int pos, Firepit[] fps)
    {
        //Se tiver Vermelho e a interação foi feita no Top. Centro a interação foi correta
        switch (CheckColor(pos, fps, 0, 1))
        {
            case 1:
                return true;
            case 0:
                return false;
            case -1:
                //Se chegou aqui, não existia Vermelho
                switch (CheckColor(pos, fps, 1, 3))
                {
                    case 1:
                        return true;
                    case 0:
                        return false;
                    case -1:
                        //Se chegou aqui não existia nem Vermelho, nem Branco
                        switch (CheckColor(pos, fps, 2, 2))
                        {
                            case 1:
                                return true;
                            case 0:
                                return false;
                            case -1:
                                return true;
                            default:
                                Debug.LogError("Número retornado foi difetente do que era esperado", this);
                                break;
                        }
                        break;
                    default:
                        Debug.LogError("Número retornado foi difetente do que era esperado", this);
                        break;
                }
                break;
            default:
                Debug.LogError("Número retornado foi difetente do que era esperado", this);
                break;
        }
        return true;
    }

    private int CheckColor(int pos, Firepit[] fps, int colorCode, int rightPos)
    {
        foreach (Firepit fp in fps)
        {
            if (fp.colorCode == colorCode)
            {
                if (pos == rightPos)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        return -1;
    }

    private void DeactivateFire(Firepit[] fps)
    {
        foreach (Firepit fp in fps)
        {
            fp.listening = false;
            fp.enabled = false;
            fp.interact.Stop();
        }
    }

    private void SetupFires(Firepit[] fps)
    {
        foreach (Firepit fp in fps)
        {
            fp.colorCode = Random.Range(0, 6);
            fp.Setup();
        }
    }
}
