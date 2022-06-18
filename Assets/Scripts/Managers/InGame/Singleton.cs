using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

namespace Managers
{
    // protected MyClassname() {} 을 선언해서 비 싱글톤 생성자 사용을 방지할 것
    public class Singleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
    {
        // Destroy 여부 확인용
        private static bool s_shuttingDown = false;
        private static readonly object LOCK = new object();
        private static T s_instance;

        public static T Instance
        {
            get
            {
                // 게임 종료 시 Object 보다 싱글톤의 OnDestroy 가 먼저 실행 될 수도 있다.
                // 해당 싱글톤을 gameObject.Ondestory() 에서는 사용하지 않거나 사용한다면 null 체크를 해주자
                if (s_shuttingDown)
                {
                    Debug.Log("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                    return null;
                }

                lock (LOCK)    //Thread Safe
                {
                    s_instance = (T)FindObjectOfType(typeof(T));
                    
                    // 인스턴스 존재 여부 확인
                    if (s_instance != null) return s_instance;
                    
                    // 아직 생성되지 않았다면 인스턴스 생성
                    // 새로운 게임오브젝트를 만들어서 싱글톤 Attach
                    var _singletonObject = new GameObject();
                    s_instance = _singletonObject.AddComponent<T>();
                    _singletonObject.name = typeof(T).ToString() + " (Singleton)";

                    // Make instance persistent.
                    DontDestroyOnLoad(_singletonObject);
                    
                    return s_instance;
                }
            }
        }

        private void OnApplicationQuit()
        {
            s_shuttingDown = true;
        }

        private void OnDestroy()
        {
            s_shuttingDown = true;
        }
    }
}
