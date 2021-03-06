using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public enum EGameMode
{
    DeathMatch,
    TeamDeathMatch,
    Tournament,

    Count
}

public enum EMapType
{
    Crossline,
    HarborCity,
    School,

    Count
}

public class RoomSettingPanel : MonoBehaviour
{
    private string[] m_mapTypeArray;
    private string[] m_gameModeArray;
    private string[] m_timeLimitArray;

    [Header("Map Info")]
    public Image mapImage;

    [Header("Room Setting")]
    public Text mapSelectText;
    public Text gameModeText;
    public Text timeLimitText;
    public Button[] selectButtons;

    private int m_mapSelectIndex = 0;
    private int m_gameModeIndex = 0;
    private int m_timeLimitIndex = 0;


    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        // Init
        m_mapTypeArray = new string[(int)EMapType.Count];
        string[] _mapNames = Enum.GetNames(typeof(EMapType));

        for (int i = 0; i < m_mapTypeArray.Length; ++i)
        {
            m_mapTypeArray[i] = _mapNames[i];
        }
        mapSelectText.text = m_mapTypeArray[0];

        m_gameModeArray = new string[(int)EGameMode.Count];
        string[] _modeNames = Enum.GetNames(typeof(EGameMode));

        for (int i = 0; i < m_gameModeArray.Length; ++i)
        {
            m_gameModeArray[i] = _modeNames[i];
        }
        gameModeText.text = m_gameModeArray[0];

        m_timeLimitArray = new string[4] { "120", "180", "240", "300" };
        timeLimitText.text = m_timeLimitArray[0];
        
        foreach (Player _player in PhotonNetwork.PlayerList)
        {
            if (!_player.IsMasterClient) continue;
            
            object _setting;
            if (_player.CustomProperties.TryGetValue(GameData.ROOM_SET_MAP, out _setting))
            {
                int _index = (int) _setting;
                Hashtable _props = new Hashtable() {{GameData.ROOM_SET_MAP, _index}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(_props);
                SetMapType(_index);
            }
                
            if (_player.CustomProperties.TryGetValue(GameData.ROOM_SET_MODE, out _setting))
            {
                int _index = (int) _setting;
                Hashtable _props = new Hashtable() {{GameData.ROOM_SET_MODE, _index}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(_props);
                SetGameMode(_index);
            }
                
            if (_player.CustomProperties.TryGetValue(GameData.ROOM_SET_TIME_LIMIT, out _setting))
            {
                int _index = (int) _setting;
                Hashtable _props = new Hashtable() {{GameData.ROOM_SET_TIME_LIMIT, _index}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(_props);
                SetTimeLimit(_index);
            }
                
            break;
        }
    }

    public void CheckMasterClient()
    {
        //Debug.Log(PhotonNetwork.IsMasterClient);
        if (!PhotonNetwork.IsMasterClient)
        {
            foreach (Button _button in selectButtons)
            {
                _button.gameObject.SetActive(false);
            }

            return;
        }

        foreach (Button _button in selectButtons)
        {
            _button.gameObject.SetActive(true);
            //Debug.Log(_button.gameObject.activeSelf);
        }
    }

    public void OnSelectMap(bool isNext)
    {
        m_mapSelectIndex = OnSelectButtonClicked(mapSelectText, m_mapTypeArray, m_mapSelectIndex, isNext);

        Hashtable _prop = new Hashtable() { { GameData.ROOM_SET_MAP, m_mapSelectIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);

        OnRoomSettingChanged(GameData.ROOM_SET_MAP, m_mapSelectIndex);
    }

    public void OnSelectGameMode(bool isNext)
    {
        m_gameModeIndex = OnSelectButtonClicked(gameModeText, m_gameModeArray, m_gameModeIndex, isNext);

        Hashtable _prop = new Hashtable() { { GameData.ROOM_SET_MODE, m_gameModeIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
        
        OnRoomSettingChanged(GameData.ROOM_SET_MODE, m_gameModeIndex);
    }

    public void OnSelectTimeLimit(bool isNext)
    {
        m_timeLimitIndex = OnSelectButtonClicked(timeLimitText, m_timeLimitArray, m_timeLimitIndex, isNext);

        Hashtable _prop = new Hashtable() { { GameData.ROOM_SET_TIME_LIMIT, m_timeLimitIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
        
        OnRoomSettingChanged(GameData.ROOM_SET_TIME_LIMIT, m_timeLimitIndex);
    }

    /// <summary>
    /// </summary>
    /// <param name="targetText">
    /// 세팅을 바꿔줄 해당 텍스트
    /// </param>
    /// <param name="cachedArray">
    /// 세팅에 해당하는 캐싱된 배열
    /// </param>
    /// <param name="currentIndex">
    /// 해당 세팅이 현재 가리키고있는 배열의 인덱스
    /// </param>
    /// <param name="isNext">
    /// 다음 인덱스로 이동하는 버튼이 클릭되었는가
    /// </param>
    /// <returns>세팅이 가리킬 인덱스(currentIndex)</returns>
    private int OnSelectButtonClicked(Text targetText, string[] cachedArray, int currentIndex, bool isNext)
    {
        if (!targetText)
        {
            Debug.LogError($"{targetText.name}가 지정되어있지 않음");
            return currentIndex;
        }

        if (currentIndex < cachedArray.Length - 1)
        {
            if (isNext)
            {
                targetText.text = cachedArray[currentIndex + 1];
                return ++currentIndex;
            }
        }

        if (!isNext && currentIndex > 0)
        {
            targetText.text = cachedArray[currentIndex - 1];
            return --currentIndex;
        }

        return currentIndex;
    }

    public void SetMapType(int index)
    {
        if (m_mapTypeArray.Length == 0) return;
        mapSelectText.text = m_mapTypeArray[index];
        m_mapSelectIndex = index;
    }
    
    public void SetGameMode(int index)
    {
        if (m_gameModeArray.Length == 0) return;
        gameModeText.text = m_gameModeArray[index];
        m_gameModeIndex = index;
    }
    
    public void SetTimeLimit(int index)
    {
        if (m_timeLimitArray.Length == 0) return;
        timeLimitText.text = m_timeLimitArray[index];
        m_timeLimitIndex = index;
    }
    
    private void OnRoomSettingChanged(string key, int value)
    {
        foreach (Player _player in PhotonNetwork.PlayerList)
        {
            if (_player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) continue;
            
            Hashtable _prop = new Hashtable() {{key, value}};
            _player.SetCustomProperties(_prop);
        }
    }
}
