using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject errorPanel;
    [SerializeField]
    private Text errorText;

    public void ShowError(string error)
    {
        errorPanel.SetActive(true);
        errorText.text = error;
        Debug.LogError(error);
    }
}
