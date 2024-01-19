using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField]
    private Button every;
    [SerializeField]
    private GameObject everyPanel;
    [SerializeField]
    private Button system;
    [SerializeField]
    private GameObject systemPanel;

    private Image everyImage;
    private Image systemImage;
    private Color everyColor;
    private Color systemColor;

    public void ClickEveryTab()
    {
        everyColor = everyImage.color;
        systemColor = systemImage.color;

        everyColor.a = 1f;
        everyImage.color = everyColor;
        systemColor.a = 0.4f;
        systemImage.color = systemColor;

        everyPanel.SetActive(true);
        systemPanel.SetActive(false);
    }

    public void ClickSystemTab()
    {
        everyColor = everyImage.color;
        systemColor = systemImage.color;

        everyColor.a = 0.4f;
        everyImage.color = everyColor;
        systemColor.a = 1f;
        systemImage.color = systemColor;

        everyPanel.SetActive(false);
        systemPanel.SetActive(true);
    }

    private void Start()
    {
        everyImage = every.GetComponent<Image>();
        systemImage = system.GetComponent<Image>();
    }
}
