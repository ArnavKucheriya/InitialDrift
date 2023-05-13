using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpeedCalculator : MonoBehaviour
{

    public float Speed;
    public Rigidbody rb;

    public Text SpeedText;
    public Text GearText;
    public Text RPMText;


    void FixedUpdate()
    {
        Vector3 vel = rb.velocity;
        Speed = rb.velocity.magnitude * 1.13693629f;

        SpeedText.text = Speed.ToString("0");
    }


}
