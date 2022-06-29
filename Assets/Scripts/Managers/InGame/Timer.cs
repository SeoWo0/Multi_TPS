using System;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Timer : MonoBehaviour, IPunObservable
{
    public enum ETimerType
    {
        Stopwatch,
        Countdown,
    }
    public ETimerType type;
    
    public TextMeshProUGUI minutesText;
    public TextMeshProUGUI separatorText;
    public TextMeshProUGUI secondsText;
    
    public byte minutes;
    public float seconds;

    public bool isDone;
    private bool m_netTimerIsDone;
    private bool m_isRunning;
    
    // Delegate && Events
    private delegate void EvaluatedTimer();
    private event EvaluatedTimer SelectedTimer;

    private RaiseEventOptions m_raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
    private SendOptions m_sendOptions = new SendOptions {Reliability = true};
    public const byte ON_TIMER_DONE_EVENT = 0;

    private void Awake()
    {
        switch (type)
        {
            case ETimerType.Stopwatch:
                SelectedTimer = Stopwatch;
                break;
            case ETimerType.Countdown:
                SelectedTimer = CountDown;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (PhotonNetwork.MasterClient.CustomProperties.TryGetValue(GameData.ROOM_SET_TIME_LIMIT, out object _timeLimitIndex))
        {
            seconds = ((int)_timeLimitIndex + 2) * 60;
        }

        minutes += (byte)(seconds / 60);
        seconds -= minutes * 60;
    }

    private void LateUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        if (m_isRunning)
            SelectedTimer?.Invoke();
    }

    private void Stopwatch()
    {
        seconds += Time.deltaTime;

        secondsText.text = $"{(int)seconds}";
        minutesText.text = minutes.ToString();

        if (seconds < 60) return;
        
        seconds = 0;
        minutes++;
    }

    private void CountDown()
    {
        if (minutes == 0 && seconds <= 0)
        {
            Stop();
            PhotonNetwork.RaiseEvent(ON_TIMER_DONE_EVENT, null, m_raiseEventOptions, m_sendOptions);
            return;
        }
        
        seconds -= Time.deltaTime;
        SetTimeUI(minutes, seconds);
        
        if (seconds > 0 || minutes == 0) return;

        seconds = 60;
        minutes--;
    }

    private void SetTimeUI(byte minute, float second)
    {
        secondsText.text = minute == 0 ? $"{second:N1}" : $"{(int)second}";
        minutesText.text = minute.ToString();

        if (minute != 0) return;
        
        minutesText.gameObject.SetActive(false);
        separatorText.gameObject.SetActive(false);
        secondsText.alignment = TextAlignmentOptions.Center;
    }

    public void SetTimerState(bool isRunning)
    {
        m_isRunning = isRunning;
    }
    
    public void Stop()
    {
        m_isRunning = false;
        SelectedTimer = null;
        isDone = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(minutes);
            stream.SendNext(seconds);
        }
        else
        {
            byte _netMinutes = (byte) stream.ReceiveNext();
            float _netSeconds = (float) stream.ReceiveNext();

            SetTimeUI(_netMinutes, _netSeconds);
        }
    }
}
