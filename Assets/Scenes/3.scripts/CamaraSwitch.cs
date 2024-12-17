using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TouchControlsKit;

public class CamaraSwitch : MonoBehaviour
{
  public GameObject TPSCam;
    public GameObject FPSCam;
    private bool IsTPS;
    void Start()
    {
        TPSCam.SetActive(true);
        FPSCam.SetActive(false);
        IsTPS=true;

    }
    void Update()
    {
        if(TCKInput.GetAction( "camara", EActionEvent.Click) && IsTPS)
        {
        TPSCam.SetActive(false);
        FPSCam.SetActive(true);
        IsTPS=false;
        }else if(TCKInput.GetAction( "camara", EActionEvent.Click) && !IsTPS)
        {
        TPSCam.SetActive(true);
        FPSCam.SetActive(false);  
        IsTPS=true;
        }
  
    }


}
