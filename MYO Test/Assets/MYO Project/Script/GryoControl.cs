using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GryoControl : MonoBehaviour {

    private bool gryoEnabled;
    private Gyroscope gyro;

    private GameObject cameraContainer;
    private Quaternion rot;
    public Transform Player;

	// Use this for initialization
	void Start () {

        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);
        cameraContainer.transform.SetParent(Player.transform);
        gryoEnabled = EnableGyro();
	}

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            cameraContainer.transform.rotation = Quaternion.Euler(90,90, 0f);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;
    }
	
	// Update is called once per frame
	private void Update () {
		if (gryoEnabled)
        {
            if (Input.GetMouseButton(0))
            {
                PlayerMovement.Instance.Move = false;
                transform.localRotation = gyro.attitude * rot;
            }

            if (Input.GetMouseButtonUp(0))
            {
                PlayerMovement.Instance.Move = true;
            }
        }
	}
}
