using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Photon.Pun;
using Photon.Realtime;

public class WeaponSpawnManager : Singleton<WeaponSpawnManager>
{
    public GameObject[] weapon;
    public Transform[] weaponZone;

    private List<int> weaponList;
    private List<int> checkList;

    private int weaponRespawn = 8;

    private int ranZone = -1;

    private void Awake()
    {
        weaponList = new List<int>();
        checkList = new List<int>();

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            int randomWeapon = Random.Range(0, 2);
            weaponList.Add(randomWeapon);
            bool isTrue = true;

            while (isTrue)
            {
                ranZone = Random.Range(0, weaponRespawn);
                isTrue = false;

                foreach (var weapon in checkList)
                {
                    if (ranZone == weapon)
                        isTrue = true;
                }
            }

            GameObject instantWeapon = PhotonNetwork.Instantiate(weapon[weaponList[0]].name, weaponZone[ranZone].position, weapon[weaponList[0]].transform.rotation);
            //GameObject instantWeapon = Instantiate(weapon[weaponList[0]], weaponZone[ranZone].position, weapon[weaponList[0]].transform.rotation);
            instantWeapon.GetComponent<Item>().index = ranZone;
            checkList.Add(ranZone);

            weaponList.RemoveAt(0);
            if (checkList.Count == weaponRespawn)
            {
                StartCoroutine(Check());
                yield break;
            }

            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator Check()
    {
        while (true)
        {
            if (checkList.Count < weaponRespawn)
            {
                StartCoroutine(Spawn());
                yield break;
            }
            yield return null;
        }
    }

    public void DestroyItemOnGain(GameObject go)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.Destroy(go);
    }

    public void CheckListRemove(int index)
    {
        checkList.Remove(index);
    }
}
