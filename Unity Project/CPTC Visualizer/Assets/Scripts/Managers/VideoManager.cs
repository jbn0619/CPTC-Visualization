using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Text.RegularExpressions;

public class VideoManager: MonoBehaviour
{
    #region Fields
    [SerializeField]
    private List<VideoClip> videos;

    public GameObject screen;
    private VideoPlayer videoPlayer;

    private bool shiftActive;
    private bool videoPlaying;

    Regex regex;

    #endregion Fields
    
    #region Properties
    
    public bool VideoPlaying
    {
        get { return videoPlaying; }
    }

    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        screen.SetActive(false);
        videoPlayer = screen.GetComponent<VideoPlayer>();
        shiftActive = false;
        videoPlaying = false;
        regex = new Regex("[0-9]+");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            int index = -1;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                shiftActive = true;
            }

            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.LeftShift))
            {
                try
                {
                    string keyCode = e.keyCode.ToString();
                    Debug.Log(keyCode);

                    MatchCollection matches = regex.Matches(keyCode);
                    
                    if(matches[0] != null)
                    {
                        index = int.Parse(matches[0].Value);
                    }

                    //index = int.Parse(keyCode);


                    if (shiftActive)
                    {
                        index += 10;
                    }
                }
                catch
                {
                    Debug.Log("Cannot parse int from that string.");
                }
            }

            if (index >= 0)
            {
                Debug.Log("Trying to access index " + index);
                PlayVideo(index);
            }
            else
            {
                Debug.Log(index + " is not a valid index.");
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                shiftActive = false;
            }
        }
    }

    public void GetInput()
    {
        int index = -1;

        Event e = Event.current;
        if(e.isKey)
        {
            Debug.Log(e.keyCode);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (e.keyCode != KeyCode.LeftShift)
                {
                    index = int.Parse(e.keyCode.ToString());
                    index += 10;
                }
            }
            else
            {
                if (e.keyCode != KeyCode.LeftShift)
                {
                    try
                    {
                        //Debug.Log("we in boys");
                        Debug.Log(e.keyCode);
                        //index = int.Parse(e.keyCode);
                    }
                    catch
                    {
                        return;
                    }
                }
            }
        }

        if(index >= 0)
        {
            Debug.Log(index);
            //PlayVideo(index);
        }
        else
        {
            Debug.Log(index + " is not a valid index.");
        }
    }

    public void PlayVideo(int _index)
    {
        // Check if the index is valid
        if(_index < videos.Count
            && _index >= 0)
        {
            videoPlayer.clip = videos[_index];
            screen.SetActive(true);
            Invoke("CloseVideo", (float) videoPlayer.clip.length);
            videoPlaying = true;
        }
        else
        {
            Debug.Log(_index + " is not a valid index.");
        }
    }

    // Create inject queue and pop off on method

    public void CloseVideo()
    {
        screen.SetActive(false);
        videoPlaying = false;
    }
}