/// FPS limit class
/**
    This class limits the target framerate of the application.
*/
using UnityEngine;

public class FPSLimit : MonoBehaviour
{
    //Public variables defined in Unity inspector
    public int fpslimit = 0;
    void Start()
    {
        if (fpslimit < 30)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = fpslimit;
        }
    }
}
