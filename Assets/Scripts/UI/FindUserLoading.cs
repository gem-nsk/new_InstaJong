using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindUserLoading : ui_basement
{
    public Text _ErrorText;
    public GameObject _RollingIcon;

    public override void CanvasControllerClose()
    {
        StartCoroutine(NotFoundAnimate());
    }

    public void CloseCanvas_Button()
    {
        base.CanvasControllerClose();
    }

    IEnumerator NotFoundAnimate()
    {
        _ErrorText.text = LocalizationManager.instance.GetLocalizedValue("_error_notfound");
        _RollingIcon.SetActive(false);
        yield return new WaitForSeconds(3);
        base.CanvasControllerClose();

    }
}
