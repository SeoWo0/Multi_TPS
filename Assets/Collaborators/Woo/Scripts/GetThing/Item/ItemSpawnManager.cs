using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Photon.Pun;

public class ItemSpawnManager : Singleton<ItemSpawnManager>
{
    public GameObject[] item;
    public Transform[] itemZone;

    private List<int> itemList;
    private List<int> checkList;

    // 아이템 리스폰 지역
    private int itemRespawn = 8;

    private int ranZone = -1;


    private void Awake()
    {
        itemList = new List<int>();
        checkList = new List<int>();

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            // 맵 상에서 아이템 뽑아내기
            int randomItem = Random.Range(0, 2);
            itemList.Add(randomItem);
            bool isTrue = true;

            while (isTrue)
            {
                ranZone = Random.Range(0, itemRespawn);
                isTrue = false;

                foreach (var item in checkList)
                {
                    if (ranZone == item)
                        isTrue = true;
                }
            }

            // GameObject instantItem = PhotonNetwork.Instantiate(item[itemList[0]].name, itemZone[ranZone].position, item[itemList[0]].transform.rotation);
            GameObject instantItem = Instantiate(item[itemList[0]], itemZone[ranZone].position, item[itemList[0]].transform.rotation);
            instantItem.GetComponent<GetThing>().index = ranZone;
            checkList.Add(ranZone);

            itemList.RemoveAt(0);
            if (checkList.Count == itemRespawn)
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
            if (checkList.Count < itemRespawn)
            {
                StartCoroutine(Spawn());
                yield break;
            }
            yield return null;
        }
    }
}
