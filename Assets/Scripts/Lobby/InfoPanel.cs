using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InfoPanel : MonoBehaviour
{
    public Text stateText;

    public GameObject errorPanel;
    public Text errorText;

    private Photon.Realtime.ClientState state;
    
    void Update()
    {
        if (state == PhotonNetwork.NetworkClientState)
            return;

        state = PhotonNetwork.NetworkClientState;
        stateText.text = state.ToString();
        Debug.Log("PhotonNetwork State : " + state.ToString());
    }

    public void ShowError(string error)
    {
        errorPanel.SetActive(true);
        errorText.text = error;
        Debug.LogError(error);
    }
}
