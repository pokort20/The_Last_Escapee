using UnityEngine;

public class FPSLimit : MonoBehaviour
{
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
