using TMPro;
using UnityEngine;

public class MapDataStore : MonoBehaviour
{
    [SerializeField] private int WindDir; //0 north, 1 east, 2 south, 3 west, 4 northeast, 5 northwest, 6 southeast, 7 southwest
    [SerializeField] private int speed;
    [SerializeField] private bool[] Hemisphere = new bool[2]; //0 Northern, 1 Southern

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int DirectionGetter()
    {
        return WindDir;
    }

    public void DirectionSetter(int Direction)
    {
        WindDir = Direction;
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
