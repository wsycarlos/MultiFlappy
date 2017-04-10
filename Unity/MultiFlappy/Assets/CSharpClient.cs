using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace Bird
{
    public class CSharpClient : MonoBehaviour
    {
        public bool starting = false;

        private bool host = false;

        public int sessionKey;

        private static CSharpClient mInstance;

        public static CSharpClient Instance
        {
            get
            {
                if(mInstance == null)
                {
                    mInstance = FindObjectOfType<CSharpClient>();
                }
                return mInstance;
            }
        }

        TTransport transport;
        TProtocol protocol;
        Flappy.Client client;

        void Start()
        {
            try
            {
                transport = new TSocket("25.89.114.214", 9090);
                protocol = new TBinaryProtocol(transport);
                client = new Flappy.Client(protocol);

                transport.Open();

                sessionKey = client.session();
                if(sessionKey == -1)
                {
                    host = true;
                }

                FlappyPool.Instance.CreateBird();
            }
            catch (TApplicationException x)
            {
                Console.WriteLine(x.StackTrace);
            }
        }

        void Awake()
        {
            mInstance = this;
        }

        void OnGUI()
        {
            if (!starting)
            {
                if (host)
                {
                    if (GUI.Button(new Rect(50, 50, 300, 150), "Start"))
                    {
                        FlappyPool.Instance.OtherMap(client.start());
                        starting = true;
                    }
                }
                else
                {
                    var map = client.waitstart();
                    if(map!=null && map.Count>0)
                    {
                        FlappyPool.Instance.OtherMap(map);
                        starting = true;
                    }
                }
            }
        }

        public void SyncPos(Vector3 position)
        {
            Place p = new Place();
            p.X = position.z;
            p.Y = position.y;
            client.move(sessionKey, p);
        }

        public Dictionary<int, Place> GetPos()
        {
            return client.msync();
        }

        public List<int> GetCrash()
        {
            return client.nsync();
        }

        public void ReportCrash()
        {
            client.crash(sessionKey);
        }
        
        void OnDestroy()
        {
            if (mInstance == this)
            {
                mInstance = null;
            }
            transport.Close();
        }
    }
}