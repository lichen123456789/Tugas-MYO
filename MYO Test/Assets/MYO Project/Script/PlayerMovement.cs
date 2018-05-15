using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class PlayerMovement : MonoBehaviour {


	private Pose _lastPose = Pose.Unknown;

	public float MoveSpeeed;

	public GameObject myo = null;
	// Use this for initialization
	Rigidbody m_Rigidbody;
    public bool isFlat = true;

    float smooth = 1.0f;
    float tiltAngle = 60;

    float tiltAroundZ;
    float tiltAroundX;

    bool Play = false;

    bool End = false;

    public float Sensitif = 70;
    public float Loudness = 0;
    AudioSource _audio;

    public bool Move = true;
    public GameObject Roket,Ledakan;
    public static PlayerMovement Instance;

    void Start () {
		m_Rigidbody = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
        _audio.clip = Microphone.Start(null, true, 10, 44100);
        _audio.loop = true;
        //_audio.
        while (!(Microphone.GetPosition(null) > 0)){ }
        _audio.Play();

        if (Instance == null)
            Instance = this;
	}

    float GetAverage()
    {
        float[] data = new float[256];
        float a = 0;
        _audio.GetOutputData(data, 0);
        foreach(float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }
	
	// Update is called once per frame
	void Update () {


        /*if (Input.GetKey (KeyCode.LeftArrow)) {
            transform.Rotate(0,-5 * Time.deltaTime,0);
        } else if (Input.GetKey (KeyCode.RightArrow)) {
            transform.Rotate(0,5 * Time.deltaTime, 0);
        } 
		else if (Input.GetKey (KeyCode.UpArrow)) {
            transform.Rotate(5 * Time.deltaTime, 0,0);
        }
		else if (Input.GetKey (KeyCode.DownArrow)) {
            transform.Rotate(-5 * Time.deltaTime, 0,0);
        }


        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		// Update references when the pose becomes fingers spread or the q key is pressed.
		bool updateReference = false;
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;

			if (thalmicMyo.pose == Pose.FingersSpread) {
				//updateReference = true;

				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			}
		}

        if (End == false)
        {
            if (thalmicMyo.pose == Pose.WaveIn)
            {
                //renderer.material = waveInMaterial;
                //transform.Rotate (new Vector3 (0, -0.3f, 0), Space.World);

                tiltAroundZ -= 1;


                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.WaveOut)
            {
                //renderer.material = waveOutMaterial;
                //transform.Rotate (new Vector3 (0, 0.3f, 0), Space.World);

                tiltAroundZ += 1;


                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.FingersSpread)
            {
                //renderer.material = doubleTapMaterial;
                //transform.Rotate (new Vector3 (0.3f, 0, 0), Space.World);


                tiltAroundX += 1;


                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.Fist)
            {
                //transform.Rotate (new Vector3 (-0.3f, 0, 0), Space.World);


                tiltAroundX -= 1;


                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.DoubleTap)
            {
                //transform.Rotate (new Vector3 (-0.3f, 0, 0), Space.World);


                //tiltAroundX -= 1;


                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else
            {
                if (tiltAroundX != 0)
                {
                    tiltAroundX = 0;
                }
                //if (tiltAroundZ != 0)
                //{
                //    tiltAroundZ = 0;
                //}

            }

            Quaternion target = Quaternion.Euler(tiltAroundX, tiltAroundZ, 0);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

            if (Input.GetKeyDown("r"))
            {
                updateReference = true;
            }
            //transform.Rotate (new Vector3 ((int)myo.transform.forward.x, (int)myo.transform.forward.y, 0), Space.World);
            // Update references. This anchors the joint on-screen such that it faces forward away
            // from the viewer when the Myo armband is oriented the way it is when these references are taken.
            if (updateReference)
            {


                if (thalmicMyo.pose != _lastPose)
                {
                    _lastPose = thalmicMyo.pose;

                    // Vibrate the Myo armband when a fist is made.
                    if (thalmicMyo.pose == Pose.Fist)
                    {

                        // Change material when wave in, wave out or double tap poses are made.
                    }
                    else if (thalmicMyo.pose == Pose.WaveIn)
                    {

                    }
                    else if (thalmicMyo.pose == Pose.WaveOut)
                    {

                    }
                }
            }
        }
        else
        {
            if (thalmicMyo.pose == Pose.WaveIn)
            {
                //renderer.material = waveInMaterial;
                //transform.Rotate (new Vector3 (0, -0.3f, 0), Space.World);
                Time.timeScale = 1;
                Application.LoadLevel(0);

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }*/


        if (Move)
        {
            Vector2 tilt = Vector2.zero;

            tilt.x = Input.acceleration.x;
            tilt.y = Input.acceleration.y;

            transform.Rotate(new Vector3(tilt.y, tilt.x, 0), Space.World);

            Loudness = GetAverage() * Sensitif;
            if (Loudness > 4)
            {
                Roket.SetActive(true);
                transform.Translate(Vector3.forward * Loudness*MoveSpeeed * Time.deltaTime);
            }else
            {
                Roket.SetActive(false);
            }
        }

    }

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Death") {
            GameObject Get = Instantiate(Ledakan, transform.position, Quaternion.identity);
            Destroy(Get, 1);
            GenerateCoin.Instance.GameEnd ();
		} else if (other.gameObject.tag == "Score") {
            GameObject Get = Instantiate(Ledakan, other.gameObject.transform.position, Quaternion.identity);
            Destroy(Get, 1);
			Destroy (other.gameObject);
			GenerateCoin.Instance.Timer += 5;
			GenerateCoin.Instance.CreateCoin ();
			GenerateCoin.Instance.GetScore ();
		}
	}

	void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
	{
		ThalmicHub hub = ThalmicHub.instance;

		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock (UnlockType.Timed);
		}

		myo.NotifyUserAction ();
	}
}
