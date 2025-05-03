using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public void ChangeBGMToBoss()
    {
        AudioManager.instance.ChangeBGMToBoss();
    }

    public void ChangeBGMToFly()
    {
        AudioManager.instance.ChangeBGMToFly();
    }
}
