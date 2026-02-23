using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapDataStore : MonoBehaviour
{
    private bool[] WindDir = new bool[4]; //0 north, 1 east, 2 south, 3 west
    private int speed;
    private bool[] Hemisphere = new bool[2]; //0 Northern, 1 Southern

    public int DirectionGetter()
    {
        for (int i = 0; i < WindDir.Length; i++)
        {
            if(WindDir[i] == true) { return i; }
        }
        return 0;
    }

    public void DirectionSetter(int TrueBool)
    {
        for (int i = 0; i < WindDir.Length; i++)
        {
            if (i == TrueBool) { WindDir[i] = true; }
            else { WindDir[i] = false; }
        }
    }

    public int WindSpeedGetter()
    {
        return speed;
    }

    public void WindSpeedSetter(TMP_InputField field)
    {
        if(int.TryParse(field.text, out speed))
        {
            
        }
    }

    public int GetHemisphereIndex()
    {
        for(int i = 0;i < Hemisphere.Length;i++)
        {
            if(Hemisphere[i] == true) { return i; }
        }
        return 0;
    }

    public void SetHemisphere(int TrueBool)
    {
        for (int i = 0; i < Hemisphere.Length; i++)
        {
            if (i == TrueBool) { Hemisphere[i] = true; }
            else { Hemisphere[i] = false; }
        }
    }
}
