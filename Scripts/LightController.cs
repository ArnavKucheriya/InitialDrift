using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    public Material LightsMatrial;
    public float brake;

    void Start()
    {
       

    }


    void Update()
    {
        brake = Input.GetAxis("Vertical");
        LightManager();
    }

    public void LightManager()
    {

        if(brake <= -0.1f)
        {
            LightsMatrial.EnableKeyword("_EMISSION");
        }
        else
        {
            LightsMatrial.DisableKeyword("_EMISSION");
        }

    }
}
