using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;

public enum EChatCommand
{
    KillLog,
    Notice,
}

public class Chat : MonoBehaviour, IChatClientListener
{
    public static Chat instance { get; private set; }

    private void Awake()
    {
        instance = this;

        inputField.enabled = false;
    }

    Queue<Text> chatQueue = new Queue<Text>();
    
    private ChatClient chatClient;
    private string userName;
    private string currentChannelName;

    public string UserName => userName;
    public Animator animator;
    public InputField inputField;
    public Text outputText;
    public GameObject showText;

    private bool m_buttonDown;

    private void OnEnable()
    {
        ClearChat();

        Application.runInBackground = true;

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(GameData.ROOM_CHAT_CHANNEL, out var _chatChannel))
        {
            currentChannelName = (string)_chatChannel;
            //Debug.Log(currentChannelName);
        }

        chatClient = new ChatClient(this);

        userName = PhotonNetwork.LocalPlayer.NickName;
    
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(userName));

        AddLine(string.Format("연결시도 중" , userName), Color.cyan);
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
            if (Input.GetButtonDown("Chat"))
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
            bool _isCommand = EvaluateChatCommand(inputField.text);
            
            if (!_isCommand)
            {
                AddLine($"{userName} : {inputField.text}", Color.cyan);
            }
            
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

    public void AddLine(string lineString, Color color)
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

        //inputField.text = "";
        inputField.ActivateInputField();

        Text _text = Instantiate(outputText);
        _text.text = lineString;
        _text.color = color;
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

    public void OnConnected()
    {
        AddLine("서버에 연결되었습니다.", Color.cyan);
        chatClient.Subscribe(new string[] { currentChannelName});
    }

    public void OnDisconnected()
    {
        AddLine("서버와 연결이 끊어졌습니다.", Color.cyan);
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

            // 채팅 명령어 체크
            bool _isCommand;
            if (messages[i].ToString().StartsWith("/c "))
            {
                _isCommand = EvaluateChatCommand(messages[i].ToString());
            }
            else
            {
                int _startIdx = messages[i].ToString().IndexOf(":", StringComparison.Ordinal);
                string _targetText = messages[i].ToString().Substring(_startIdx + 2);
                
                _isCommand = EvaluateChatCommand(_targetText);
            }

            if (!_isCommand)
            {
                AddLine($"{messages[i]}", Color.cyan);
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("채널 입장 ({0})", string.Join(",", channels)), Color.cyan);

        AddLine("T키를 눌러 채팅을 시작하십시오.", Color.cyan);
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("채널 퇴장 ({0})", string.Join(",", channels)), Color.cyan);

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        //Debug.Log(currentChannelName);
        chatClient.Unsubscribe(new string[] { currentChannelName });
    }

    private void ClearChat()
    {
        int _deleteCount = chatQueue.Count;

        for (int i = 0; i < _deleteCount; i++)
        {
            Text _text = chatQueue.Dequeue();
            Destroy(_text.gameObject);
        }
    }

    public void Input_OnEndEdit(string text)
    {
        chatClient.PublishMessage(currentChannelName, $"{userName} : {inputField.text}");

        inputField.text = "";
    }

    public void KillLog(string text)
    {
        EvaluateChatCommand(text);
        
        chatClient.PublishMessage(currentChannelName, text);
    }

    private bool EvaluateChatCommand(string text)
    {
        if (!text.StartsWith("/c ")) return false;

        if (text.Contains("log "))
        {
            string _text = text.Substring(6);
            PublishStyledChat(EChatCommand.KillLog, _text);
            return true;
        }

        if (text.Contains("nt "))
        {
            string _text = text.Substring(5);
            PublishStyledChat(EChatCommand.Notice, _text);
            return true;
        }

        return false;
    }

    private void PublishStyledChat(EChatCommand chatCommand, string text)
    {
        switch (chatCommand)
        {
            case EChatCommand.KillLog:
                AddLine($"KILL : {text}", Color.red);
                break;
            case EChatCommand.Notice:
                AddLine($"NOTICE : {text}", Color.yellow);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(chatCommand), chatCommand, null);
        }
    }
}
