using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void randomStats()
    {
        int RandDirection = Random.Range(0, 8);

        Toggle toggle = GameObject.Find("North_Toggle").GetComponent<Toggle>();

        switch (RandDirection)
        {
            case 0:
                toggle = GameObject.Find("North_Toggle").GetComponent<Toggle>();
                break;
            case 1:
                toggle = GameObject.Find("East_Toggle").GetComponent<Toggle>();
                break;
            case 2:
                toggle = GameObject.Find("South_Toggle").GetComponent<Toggle>();
                break;
            case 3:
                toggle = GameObject.Find("West_Toggle").GetComponent<Toggle>();
                break;
            case 4:
                toggle = GameObject.Find("NorthEast_Toggle").GetComponent<Toggle>();
                break;
            case 5:
                toggle = GameObject.Find("NorthWest_Toggle").GetComponent<Toggle>();
                break;
            case 6:
                toggle = GameObject.Find("SouthEast_Toggle").GetComponent<Toggle>();
                break;
            case 7:
                toggle = GameObject.Find("SouthWest_Toggle").GetComponent<Toggle>();
                break;
        }

        toggle.isOn = true; //needs to be called twice to cancel previous selection
        toggle.isOn = true;
        toggle.onValueChanged.Invoke(true);

        int randspeed = Random.Range(0, 100);

        GameObject.Find("SpeedBoxComponent").GetComponent<TMP_InputField>().text = randspeed.ToString();

        int randHemisphere = Random.Range(0, 2);

        if(randHemisphere == 0)
        {
            toggle = GameObject.Find("Northern_Toggle").GetComponent<Toggle>();
        }
        else
        {
            toggle = GameObject.Find("Southern_Toggle").GetComponent<Toggle>();
        }

        toggle.isOn = true; 
        toggle.isOn = true;
        toggle.onValueChanged.Invoke(true);
    }
}
