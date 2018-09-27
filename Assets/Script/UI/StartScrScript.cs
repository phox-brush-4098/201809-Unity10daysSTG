using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScrScript : MonoBehaviour
{
    public FadeCanvas ff;
    public static bool useEffect = true;
    public Toggle toggle;

    private void Awake()
    {
        Application.targetFrameRate = 60;
	Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }

    // Use this for initialization
    void Start()
    {
        if (useEffect) { toggle.isOn = true; }
        else { toggle.isOn = false; }
    }

    int cnt = 0;

    public void ToggleUpdate()
    {
        useEffect = toggle.isOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && cnt == 0)
        {
            ff.isFadeIn = false;
            ff.StartProc();
            cnt = 1;
        }

        if (Input.anyKeyDown && cnt == 0)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) { }
            else
            {
                ff.isFadeIn = false;
                ff.StartProc();
                cnt = 1;
            }
        }

        if (cnt > 0)
        {
            ++cnt;

            if (cnt > 22)
            {
                SceneManager.LoadScene("game");
            }
        }
    }
}
