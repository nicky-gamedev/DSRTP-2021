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
        bool has2 = false;
        foreach (LightBlink light in lights)
        {
            if (has2) 
            { 
                light.interval = possibleIntevals[Random.Range(0, possibleIntevals.Count - 1)]; 
            }
            else 
            { 
                light.interval = possibleIntevals[Random.Range(0, possibleIntevals.Count)];
                if (light.interval == possibleIntevals[2]) has2 = true;
            }

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
                    clicksNeeded *= 2;
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
