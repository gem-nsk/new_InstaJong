using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Accounts.Convert.preferAccount;

public class PreferAccountElement : MonoBehaviour
{
    protected PreferAccountLoading _controller { get { return (PreferAccountLoading)FindObjectOfType(typeof(PreferAccountLoading)); } }
    public Text _Text_Category;
    public Text _Text_Id;

    public string _id;

    public void Setup(string _category, string _id)
    {
        _Text_Category.text = _category;
        _Text_Id.text = "@" + _id;

        this._id = _id;
    }

    public void Interact()
    {
        _controller.Play(_id);
    }
}
