using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;

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
    TestMap,

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

        m_timeLimitArray = new string[4] { "30", "60", "90", "120" };
        timeLimitText.text = m_timeLimitArray[0] + "초";

        Hashtable _prop = new Hashtable() { { GameData.ROOM_SET_MAP, m_mapSelectIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);

        _prop = new Hashtable() { { GameData.ROOM_SET_MODE, m_gameModeIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);

        _prop = new Hashtable() { { GameData.ROOM_SET_TIMELIMIT, m_timeLimitIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
    }

    public void CheckMasterClient()
    {
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
        }
    }

    public void OnSelectMap(bool isNext)
    {
        m_mapSelectIndex = OnSelectButtonClicked(mapSelectText, m_mapTypeArray, m_mapSelectIndex, isNext);

        Hashtable _prop = new Hashtable() { { GameData.ROOM_SET_MAP, m_mapSelectIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
    }

    public void OnSelectGameMode(bool isNext)
    {
        m_gameModeIndex = OnSelectButtonClicked(gameModeText, m_gameModeArray, m_gameModeIndex, isNext);

        Hashtable _prop = new Hashtable() { { GameData.ROOM_SET_MODE, m_gameModeIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
    }

    public void OnSelectTimeLimit(bool isNext)
    {
        m_timeLimitIndex = OnSelectButtonClicked(timeLimitText, m_timeLimitArray, m_timeLimitIndex, isNext);

        Hashtable _prop = new Hashtable() { { GameData.ROOM_SET_TIMELIMIT, m_timeLimitIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
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

    public void SetRoomSettings()
    {

    }
}
