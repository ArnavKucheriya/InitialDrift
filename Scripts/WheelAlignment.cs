using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAlignment : MonoBehaviour {

    public GameObject wheelBase;
    public GameObject wheelGraphic;

    public WheelCollider wheelCol;

    public bool steerable;

    public float steeringAngle;

    RaycastHit hit;
	
	// Update is called once per frame
	void Update ()
    {
        alignMeshToCollider();
	}

    //Rotates graphical wheel and updates its position
    void alignMeshToCollider()
    {
        //Updates graphical wheel position by raycasting beneath the wheel with a max distance equaling the suspension distance and seeing if any ground is hit
        if (Physics.Raycast(wheelCol.transform.position, -wheelCol.transform.up, out hit, wheelCol.suspensionDistance + wheelCol.radius))
        {
            wheelGraphic.transform.position = hit.point + wheelCol.transform.up *  wheelCol.radius;
        }
        else
        {
            wheelGraphic.transform.position = wheelCol.transform.position - (wheelCol.transform.up * wheelCol.suspensionDistance);
        }

        //Sees if wheel is steerable, if yes then applies steering angle
        if (steerable)
        {
            wheelCol.steerAngle = steeringAngle;
        }

        wheelGraphic.transform.eulerAngles = new Vector3(wheelBase.transform.eulerAngles.x, wheelBase.transform.eulerAngles.y + wheelCol.steerAngle, wheelBase.transform.eulerAngles.z);
        wheelGraphic.transform.Rotate(wheelCol.rpm / 60 * 360 * Time.deltaTime, 0, 0);
    }
}
