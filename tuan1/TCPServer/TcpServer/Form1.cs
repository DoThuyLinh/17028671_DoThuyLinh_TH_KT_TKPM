using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpServer
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        SimpleTcpServer tcpServer;
        private TcpClient _connectedClient = null;
        const int MAX_CONNECTION = 10;

        private void Server_Load(object sender, EventArgs e)
        {
            tcpServer = new SimpleTcpServer();
            tcpServer.Delimiter = 0x13;
            tcpServer.StringEncoder = Encoding.UTF8;

        }

        private void TcpServer_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Clear();
                txtStatus.Text += "Client"+((System.Net.IPEndPoint)e.TcpClient.Client.RemoteEndPoint).Address.ToString()+" send: " + e.MessageString;
                e.ReplyLine(string.Format("Server send: {0}", e.MessageString + "\n"));
            });
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            txtStatus.Text += "Server starting...";
            IPAddress ip = IPAddress.Any;
            tcpServer.Start(ip, Convert.ToInt32(txtPort.Text));
            LoopClients();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (tcpServer.IsStarted)
                tcpServer.Stop();

        }

        public void LoopClients()
        {
            for (int i = 0; i < MAX_CONNECTION; i++)
            {
                new Thread(HandleClient).Start();
            }
            //while (_connectionsCount < MAX_CONNECTION)
            //{
            //    tcpServer.ClientConnected += TcpServer_ClientConnected;
            //    _connectionsCount++;
            //    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
            //    t.Start(_connectedClient);
            //}
        }
        
        private void TcpServer_ClientConnected(object sender, TcpClient e)
        {
            if (_connectedClient == null)
            {
                _connectedClient = e;
            }
            else
            {
                e.Dispose();
            }
        }

        public void HandleClient(object obj)
        {
            tcpServer.ClientConnected += TcpServer_ClientConnected;
            tcpServer.DataReceived += TcpServer_DataReceived;
        }
    }
}
