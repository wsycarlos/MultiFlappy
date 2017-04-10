using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bird;

public class FlappyPool : MonoBehaviour
{
    private static FlappyPool mInstance;

    public static FlappyPool Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<FlappyPool>();
            }
            return mInstance;
        }
    }

    void Awake()
    {
        mInstance = this;
    }

    void OnDestory()
    {
        if (mInstance == this)
        {
            mInstance = null;
        }
    }
    public GameObject currentPlayer;

    public GameObject[] prefabList;

    public GameObject[] mapList;

    public GameObject CreateBird()
    {
        int size = prefabList.Length;
        int index = Random.Range(1, size - 1);
        Random.InitState(2017);
        GameObject go = GameObject.Instantiate(prefabList[index]);
        CSharpClient.Instance.SyncPos(go.transform.localPosition);
        go.AddComponent<FlappyMove>();
        currentPlayer = go;
        return go;
    }

    private void CreateMap(int loc)
    {
        int size = mapList.Length;
        int index = Random.Range(0, size - 1);
        GameObject go = GameObject.Instantiate(mapList[index]);
        go.transform.localPosition = new Vector3(6 * loc, 0f, 0f);
    }

    private Dictionary<int, GameObject> otherPlayer;

    public void OtherMap(Dictionary<int, Place> map)
    {
        otherPlayer = new Dictionary<int, GameObject>();
        foreach (int i in map.Keys)
        {
            if (i != CSharpClient.Instance.sessionKey)
            {
                GameObject _g = GameObject.Instantiate(prefabList[0]);
                otherPlayer.Add(i, _g);
                _g.transform.localPosition = new Vector3(0f, (float)map[i].Y, (float)map[i].X);
            }
        }
    }

    private int locIndex = 0;

    void Update()
    {
        if (CSharpClient.Instance.starting)
        {
            Dictionary<int, Place> map = CSharpClient.Instance.GetPos();
            foreach(int i in map.Keys)
            {
                if (otherPlayer.ContainsKey(i))
                {
                    otherPlayer[i].transform.localPosition = new Vector3(0f, (float)map[i].Y, (float)map[i].X);
                }
            }
            List<int> crash = CSharpClient.Instance.GetCrash();
            foreach(int i in crash)
            {
                if (otherPlayer.ContainsKey(i))
                {
                    Destroy(otherPlayer[i]);
                    otherPlayer.Remove(i);
                }
            }

            if(Camera.main.transform.localPosition.x > locIndex * 6f)
            {
                locIndex++;
                CreateMap(locIndex);
            }
        }
    }
}
