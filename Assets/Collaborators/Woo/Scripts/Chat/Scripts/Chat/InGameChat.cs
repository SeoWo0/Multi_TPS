using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;

public class InGameChat : MonoBehaviourPunCallbacks, IChatClientListener
{
    public static InGameChat instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    Queue<Text> chatQueue = new Queue<Text>();

    private ChatClient chatClient;
    private string userName;
    private string currentChannelName;

    public string UserName
    {
        get
        {
            return userName;
        }

        private set
        {
            
        }
    }


    public Animator animator;
    public InputField inputField;
    public Text outputText;
    public GameObject showText;

    private bool m_buttonDown;

    private void Start()
    {
        Application.runInBackground = true;

        currentChannelName = "Channel 001";

        chatClient = new ChatClient(this);

        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(userName));

        userName = PhotonNetwork.LocalPlayer.NickName;

        
    }

    private void Update()
    {
        chatClient.Service();

        IsActiveChat();
    }

    public void IsActiveChat()
    {
        m_buttonDown = Input.GetButtonDown("Submit");

        if (!animator.GetBool("isActive"))
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                inputField.enabled = true;
                inputField.ActivateInputField();
                animator.SetTrigger("isTrigger");
                animator.SetBool("isActive", true);
            }
            return;
        }

        if (m_buttonDown)
        {
            AddLine($"{userName} : {inputField.text}");
            animator.SetBool("isActive", false);
            inputField.DeactivateInputField();
            inputField.enabled = false;
        }
    }

    public void OnApplicationQuit()
    {
        if (chatClient != null)
            chatClient.Disconnect();
    }

    public void AddLine(string lineString)
    {
        if (chatClient.CanChatInChannel(currentChannelName) && m_buttonDown)
        {
            Input_OnEndEdit(lineString);
        }

        if (chatQueue.Count > 10)
        {
            Text _disableText = chatQueue.Dequeue();
            Destroy(_disableText.gameObject);
        }

        inputField.text = "";
        inputField.ActivateInputField();

        Text _text = Instantiate(outputText);
        _text.text = lineString;
        _text.transform.SetParent(showText.transform);
        _text.transform.SetPositionAndRotation(showText.transform.position, Quaternion.identity);
        _text.transform.localScale = Vector3.one;
        chatQueue.Enqueue(_text);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public override void OnConnected()
    {
        AddLine("서버에 연결되었습니다.");

        chatClient.Subscribe(new string[] { currentChannelName });
    }

    public void OnDisconnected()
    {
        AddLine("서버와 연결이 끊어졌습니다.");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            if (messages[i].ToString() == "") continue;
            if (chatClient.UserId == senders[i]) continue;

            AddLine(string.Format("{0}", messages[i].ToString()));
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("채널 입장 ({0})", string.Join(",", channels)));

        AddLine("T키를 눌러 채팅을 시작하십시오.");
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("채널 퇴장 ({0})", string.Join(",", channels)));
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void Input_OnEndEdit(string text)
    {
        chatClient.PublishMessage(currentChannelName, $"{userName} : {inputField.text}");

        inputField.text = "";
    }
}
