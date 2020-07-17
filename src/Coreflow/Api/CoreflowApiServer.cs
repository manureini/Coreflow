using Coreflow.Api.Responses;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Coreflow.Api
{
    public class CoreflowApiServer
    {
        private TcpListener mTcpListener;
        private Coreflow mCoreFlow;

        private int Port { get; set; }
        private IPAddress LocalAddress { get; set; }

        internal CoreflowApiServer(Coreflow pInstance, IPAddress pLocalAddress, int pPort)
        {
            mCoreFlow = pInstance;
            Port = pPort;
            LocalAddress = pLocalAddress;
        }

        public void Start()
        {
            mTcpListener = new TcpListener(LocalAddress, Port);
            mTcpListener.Start();

            Thread th = new Thread(listenClients);
            th.Start();
        }

        public void listenClients()
        {
            while (true)
            {
                try
                {
                    TcpClient newClient = mTcpListener.AcceptTcpClient();
                    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                    t.Start(newClient);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void HandleClient(object obj)
        {
            using (TcpClient client = (TcpClient)obj)
            using (StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.UTF8))
            using (StreamReader sReader = new StreamReader(client.GetStream(), Encoding.UTF8))
            {
                while (true)
                {
                    string sData = sReader.ReadLine();

                    if (string.IsNullOrWhiteSpace(sData))
                        break;

                    if (sData == nameof(LastCompiledFlowInfo))
                    {
                        var respObj = LastCompiledFlowInfo.FromCompileResult(mCoreFlow.LastCompileResult);
                        var resp = JsonConvert.SerializeObject(respObj);

                        sWriter.WriteLine(resp);
                        sWriter.Flush();
                    }
                }

                sWriter.Close();
                sReader.Close();
                client.Close();
            }
        }
    }
}
