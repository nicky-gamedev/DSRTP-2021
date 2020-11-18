using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorseManager : MonoBehaviour
{
    public LightBlink[] lights = new LightBlink[3];
    public List<float> possibleIntevals = new List<float>();

    public ButtonClick button;
    public GameObject door;

    private void OnEnable()
    {
        SetupLights();
    }

    private void SetupLights()
    {
        foreach (LightBlink light in lights)
        {
            light.interval = possibleIntevals[Random.Range(0, possibleIntevals.Count)];
            light.Setup();
        }
    }

    public void CheckResolution(int clicks)
    {
        if (clicks == 0) return;

        string code = "";

        foreach (LightBlink light in lights)
        {
            for (int i = 0; i < possibleIntevals.Count; i++)
            {
                if (light.interval == possibleIntevals[i])
                {
                    code += $"{i}";
                }
            }
        }

        int clicksNeeded = 4;

        for (int i = 0; i < code.Length; i++)
        {
            switch (code[i])
            {
                case '0':
                    clicksNeeded--;
                    break;
                case '1':
                    clicksNeeded++;
                    break;
                case '2':
                    clicksNeeded = 3;
                    break;
                default:
                    Debug.Log("Switch cant compare char");
                    break;
            }
        }

        Debug.Log(clicksNeeded.ToString() + " clicks needed");

        Debug.Log($"The code is: {code} and made {clicks} clicks");

        if (clicks == clicksNeeded)
        {
            Debug.Log("Right answer");
            button.TurnOff();
            door.SetActive(false);
        }
        else
        {
            Debug.Log("Strike");
            GameManager.instance.Strike();
            SetupLights();
        }
    }
}
