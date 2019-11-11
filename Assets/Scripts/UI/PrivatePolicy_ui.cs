using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivatePolicy_ui : ui_basement
{
    public override void Activate()
    {
        base.Activate();

    }

    public void OpenPrivatePolicy()
    {
        Application.OpenURL("https://appsbygem.com/PrivacyPolicyru/");
    }

    public void Close()
    {
        MainMenuControl.instance.StartTutourial();
        base.DeActivate();
    }
}
