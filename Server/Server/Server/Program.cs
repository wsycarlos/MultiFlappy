using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Thrift.Server;
using Thrift.Transport;

namespace Bird
{
    class BirdHandler : Flappy.Iface
    {
        int index = 0;

        Dictionary<int, Place> statusMap;

        bool starting = false;

        bool begining = false;

        List<int> crashMap;

        public int session()
        {
            if (!begining)
            {
                begining = true;
                return -1;
            }
            else
            {
                index++;
                return index;
            }
        }

        public Dictionary<int,Place> start()
        {
            starting = true;
            return statusMap;
        }

        public Dictionary<int, Place> waitstart()
        {
            if (starting)
            {
                return statusMap;
            }
            else
            {
                return new Dictionary<int, Place>();
            }
        }

        public void move(int ses, Place p)
        {
            if(statusMap == null)
            {
                statusMap = new Dictionary<int, Place>();
            }
            if (statusMap.ContainsKey(ses))
            {
                statusMap[ses] = p;
            }
            else
            {
                statusMap.Add(ses, p);
            }
        }

        public Dictionary<int,Place> msync()
        {
            return statusMap;
        }

        public List<int> nsync()
        {
            if (crashMap == null)
            {
                return new List<int>();
            }
            else
            {
                return crashMap;
            }
        }
        
        public void crash(int ses)
        {
            if(crashMap == null)
            {
                crashMap = new List<int>();
            }
            if (!crashMap.Contains(ses))
            {
                crashMap.Add(ses);
            }
        }
    }

    class Sever
    {
        static void Main(string[] args)
        {
            try
            {
                BirdHandler handler = new BirdHandler();
                Flappy.Processor processor = new Flappy.Processor(handler);
                TServerTransport serverTransport = new TServerSocket(9090);
                TServer server = new TThreadedServer(processor, serverTransport);

                // Use this for a multithreaded server
                // server = new TThreadPoolServer(processor, serverTransport);

                Console.WriteLine("Starting the server...");
                server.Serve();
            }
            catch (Exception x)
            {
                Console.WriteLine(x.StackTrace);
            }
            Console.WriteLine("done.");
        }
    }
}
