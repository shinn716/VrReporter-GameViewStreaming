using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class TakePhoto : MonoBehaviour {
    
    public new Camera camera;
    public void SetCamera(Camera _cam)
    {
        camera = _cam;
    }

    private SteamVR_TrackedObject trackObj;
    private SteamVR_Controller.Device controller
    {
        get { return SteamVR_Controller.Input((int)trackObj.index); }
    }

    [Header("Picture Resolution")]
    public int resWidth = 1920;
    public int resHeight = 1080;
    public KeyCode debugKey = KeyCode.K;

    [Header("Photo Setting")]
    public PathType pathtype;
    public string AbsolutePath = "C:/test";
    public GameObject flash;
    public GameObject flash2;
    public AudioClip sounds_shot;

    [Header("Grap Anim")]
    public Animator grap;

    private AudioSource AS;
    private bool m_takeShot = false;
    private bool m_delay = false;

    public enum PathType
    {
        Absolute,
        Opposite
    }

    public string ScreenShotName(int width, int height)
    {
        if (pathtype == PathType.Absolute)
        {
            return string.Format(AbsolutePath + "/screen_{1}x{2}_{3}.png",
                     Application.dataPath,
                     width, height,
                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }
        else
        {
            return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                                 Application.dataPath,
                                 width, height,
                                 System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }
    }

    private void Start()
    {
        trackObj = GetComponent<SteamVR_TrackedObject>();
        AS = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if (controller.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger) || Input.GetKeyDown(debugKey))
        {
            m_takeShot = true;
            grap.SetBool("IsGrabbing", true);
            Invoke("GrapSetDefault", .25f);
        }        

        if (m_takeShot && !m_delay)
        {

            Invoke("DelayAnim", .1f);
            AS.PlayOneShot(sounds_shot);

            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);

            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));

            m_takeShot = false;
            m_delay = true;
            Invoke("CoolDown", 1.3f);
        }
    }

    private void GrapSetDefault()
    {
        grap.SetBool("IsGrabbing", false);
    }

    private void CoolDown()
    {
        m_delay = false;
    }

    void DelayAnim()
    {
        flash.SetActive(true);
        flash2.SetActive(true);
    }
}
