using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHistory_ui : ui_basement
{
    public Transform HistoryList;
    public GameObject HistoryElement;

    private List<ElementHistory> allHistory;
    public override void Activate()
    {
        base.Activate();
        allHistory = new List<ElementHistory>();
        placePrefabs();

    }

    public void placePrefabs()
    {
        var history = History.ShowHistory();
        if(history != null && history.Count != 0)
        {
            foreach (Element data in history)
            {
                GameObject tmpCell = Instantiate(HistoryElement);
                tmpCell.transform.SetParent(HistoryList, false);

                switch (data.type)
                {
                    case 0:
                        {
                            tmpCell.name = "@" + data.value;
                            tmpCell.GetComponentInChildren<Text>().text = "@" + data.value;
                            break;
                        }

                    case 1:
                        {
                            tmpCell.name = "#" + data.value;
                            tmpCell.GetComponentInChildren<Text>().text = "#" + data.value;
                            break;
                        }

                }
                //tmpCell.GetComponentInChildren<Button>().onClick.AddListener(delegate { DeleteElement(data.value,data.type); });
                foreach (Transform child in tmpCell.transform)
                {
                    if (child.name == "close")
                        child.GetComponent<Button>().onClick.AddListener(delegate { DeleteElement(data.value, data.type); });
                }
                Debug.Log("name child "+tmpCell.GetComponentInChildren<Button>().name);
                
                tmpCell.GetComponent<ElementHistory>().id = data.id;
                tmpCell.GetComponent<ElementHistory>().type = data.type;
                tmpCell.GetComponent<ElementHistory>().value = data.value;


                allHistory.Add(tmpCell.GetComponent<ElementHistory>());

            }
        }
        
    }

    public void ClearHistory()
    {
        allHistory.Clear();
        foreach (Transform child in HistoryList)
        {
            GameObject.Destroy(child.gameObject);
        }
        History.ClearHistory();
    }

    public void DeleteElement(string value, int type)
    {
        string _V = type + value;
        switch (type)
        {
            case 0:
                {
                    _V = "@" + value;
                    break;
                }

            case 1:
                {
                    _V = "#" + value;
                    break;
                }

        }
        
        Debug.Log(_V);
        foreach (Transform child in HistoryList)
        {
            if(child.name == _V)
                GameObject.Destroy(child.gameObject);
        }
        History.DeleteUserFromHistory(value);
    }
}
