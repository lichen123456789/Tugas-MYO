using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class GenerateCoin : MonoBehaviour {
    private Pose _lastPose = Pose.Unknown;
    public GameObject myo = null;

    public static GenerateCoin Instance = null;

	public GameObject Player;

	public GameObject[] Pos;

	public GameObject Coin;
	int Last;

	public Text[] Label;

	public int Score=0;
	public float Timer = 100;

	public GameObject[] LayerGame;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
		if (Instance == null) {
			Instance = this;
		}
		CreateCoin ();
		Label [0].text = "Score : " + Score;

	}

	public void Click(int Pil)
	{
		if (Pil == 0) {
			Time.timeScale = 1;
			LayerGame [0].SetActive (false);
			LayerGame [1].SetActive (true);
		} else if (Pil == 1) {
            Time.timeScale = 1;
            Application.LoadLevel (0);
		}
	}

	void Update()
	{
        if (PlayerMovement.Instance.Move)
        {
            if (Timer >= 0)
            {
                Timer -= Time.deltaTime;
            }
            else
            {
                GameEnd();
            }
            Label[1].text = "Timer : " + Timer.ToString("0");
        }
        
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        // Update references when the pose becomes fingers spread or the q key is pressed.
        bool updateReference = false;
        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.FingersSpread)
            {
                

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }


        if (thalmicMyo.pose == Pose.WaveIn)
        {
            Time.timeScale = 1;
            Application.LoadLevel(0);
            ExtendUnlockAndNotifyUserAction(thalmicMyo);
        }
    }

    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo.Unlock(UnlockType.Timed);
        }

        myo.NotifyUserAction();
    }

    public void GameEnd()
	{
		Time.timeScale = 0;
		Label [2].text = "Your Score : " + Score;
		LayerGame [1].SetActive (false);
		LayerGame [2].SetActive (true);
	}

	public void GetScore()
	{
		Score++;
		Label [0].text = "Score : " + Score;
	}

	public void CreateCoin()
	{
		int Ran = Random.Range (0, 9);
		if (Ran != Last) {
			Last = Ran;
		}
		Instantiate (Coin, Pos[Ran].transform.position, Quaternion.identity);
	}
}
