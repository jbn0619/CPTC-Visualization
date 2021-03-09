using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Text.RegularExpressions;

public class VideoManager: MonoBehaviour
{
    #region Fields
    [SerializeField]
    private List<VideoClip> attackVideos;
    [SerializeField]
    private List<VideoClip> injectVideos;

    public GameObject screen;
    private VideoPlayer videoPlayer;
    private Canvas canvas;

    //private bool shiftActive;
    private bool isVideoPlaying;

    Regex regex;

    #endregion Fields
    
    #region Properties
    
    public bool IsVideoPlaying
    {
        get { return isVideoPlaying; }
    }

    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = UIManager.Instance.ActiveCanvas;
        screen.SetActive(false);
        videoPlayer = screen.GetComponent<VideoPlayer>();
        //shiftActive = false;
        regex = new Regex("[0-9]+");
        isVideoPlaying = false;
    }

    ///// <summary>
    ///// On GUI handles user input for videos.
    /////     The number key pressed will be parsed and passed in as the index.
    /////     This code may be irrelevant now.
    ///// </summary>
    //private void OnGUI()
    //{
    //    Event e = Event.current;
    //    if (e.isKey)
    //    {
    //        int index = -1;

    //        if (Input.GetKeyDown(KeyCode.LeftShift))
    //        {
    //            shiftActive = true;
    //        }

    //        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.LeftShift))
    //        {
    //            try
    //            {
    //                string keyCode = e.keyCode.ToString();
    //                Debug.Log(keyCode);

    //                MatchCollection matches = regex.Matches(keyCode);
                    
    //                if(matches[0] != null)
    //                {
    //                    index = int.Parse(matches[0].Value);
    //                }

    //                //index = int.Parse(keyCode);


    //                if (shiftActive)
    //                {
    //                    index += 10;
    //                }
    //            }
    //            catch
    //            {
    //                Debug.Log("Cannot parse int from that string.");
    //            }
    //        }

    //        if (index >= 0)
    //        {
    //            Debug.Log("Trying to access index " + index);
    //            PlayAttackVideo(index);
    //        }
    //        else
    //        {
    //            Debug.Log(index + " is not a valid index.");
    //        }

    //        if (Input.GetKeyUp(KeyCode.LeftShift))
    //        {
    //            shiftActive = false;
    //        }
    //    }
    //}

    /// <summary>
    /// Play Attack Video
    ///     Plays the attack video with the passed in index.
    /// </summary>
    /// <param name="_index"></param>
    public void PlayAttackVideo(int _index)
    {
        // Check if the index is valid
        if(_index < attackVideos.Count
            && _index >= 0)
        {
            canvas = UIManager.Instance.ActiveCanvas;
            Debug.Log("Playing attack video...");

            canvas.gameObject.SetActive(false);

            videoPlayer.clip = attackVideos[_index];
            screen.SetActive(true);
            //Invoke("CloseVideo", (float) videoPlayer.clip.length);
            Invoke("CloseVideo", 90);
            isVideoPlaying = true;
        }
        else
        {
            Debug.Log(_index + " is not a valid index.");
        }
    }

    /// <summary>
    /// Play Inject Video
    ///     Plays the video at the top of the InjectVideos list and
    ///     pops it from the list.
    /// </summary>
    public void PlayInjectVideo()
    {
        canvas = UIManager.Instance.ActiveCanvas;
        Debug.Log("Playing inject video...");

        canvas.gameObject.SetActive(false);

        videoPlayer.clip = injectVideos[0];
        screen.SetActive(true);
        
        Invoke("CloseVideo", (float) videoPlayer.clip.length);
        isVideoPlaying = true;
        injectVideos.RemoveAt(0);
    }
    /// <summary>
    /// Invoke this method using the videoPlayer.clip.length as its time.
    ///     This will hide the screen as soon as the video ends.
    /// </summary>
    public void CloseVideo()
    {
        screen.SetActive(false);
        isVideoPlaying = false;
        canvas.gameObject.SetActive(true);
    }
}