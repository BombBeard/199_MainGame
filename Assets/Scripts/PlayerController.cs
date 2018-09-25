using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public enum CONTROL_TYPE
    {
        TANK,
        UNICYCLE,
        CAR,
        BIPED
    };

    public CONTROL_TYPE controlType;

    float x = 0.0f;
    float z = 0.0f;

    [SerializeField]
    float turnSpeedTank = 150.0f;
    [SerializeField]
    float forwardSpeedTank = 3.0f;

    private void Start()
    {
        controlType = CONTROL_TYPE.TANK;
    }
    // Update is called once per frame
    void Update () {
        switch (controlType)
        {
            case CONTROL_TYPE.TANK:
                Debug.Log("TANK");
                TankControls();
                break;
            case CONTROL_TYPE.UNICYCLE:
                Debug.Log("UNICYCLE");
                break;
            case CONTROL_TYPE.CAR:
                Debug.Log("CAR");
                break;
            case CONTROL_TYPE.BIPED:
                Debug.Log("BIPED");
                break;
            default:
                Debug.Log("ERROR: NO CONTROL TYPE SELECTED");
                break;
        }
        

    }

    void TankControls()
    {
        x = Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeedTank;
        z = Input.GetAxis("Vertical") * Time.deltaTime * forwardSpeedTank;

        transform.Rotate(0, x, 0);
        transform.Translate(0f, 0f, z);
    }
}
