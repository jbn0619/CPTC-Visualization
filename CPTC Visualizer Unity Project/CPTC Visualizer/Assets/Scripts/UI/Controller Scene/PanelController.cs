using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController: MonoBehaviour
{
    #region Fields
    [SerializeField]
    private List<GameObject> panels;
    [SerializeField]
    private GameObject activePanel;

    #endregion Fields
    
    #region Properties

    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        GatherControlPanels();
        activePanel = panels[0];
        activePanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GatherControlPanels()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            panels.Add(gameObject.transform.GetChild(i).transform.GetChild(1).gameObject);
            if(i != 0)
            {
                panels[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Changes the active panel.
    /// </summary>
    /// <param name="_panel"></param>
    public void ChangeActivePanel(GameObject _panel)
    {
        activePanel.SetActive(false);
        activePanel = _panel;
        activePanel.SetActive(true);
    }
}
