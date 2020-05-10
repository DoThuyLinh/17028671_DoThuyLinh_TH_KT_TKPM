using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpClient
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }
        SimpleTcpClient tcpClient;
        private void Form1_Load(object sender, EventArgs e)
        {
            tcpClient = new SimpleTcpClient();
            tcpClient.StringEncoder = Encoding.UTF8;
            tcpClient.DataReceived += TcpClient_DataReceived;
        }

        private void TcpClient_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;

            });
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            IPAddress ip = IPAddress.Parse(txtHost.Text);
            tcpClient.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            tcpClient.WriteLineAndGetReply(txtMessage.Text, TimeSpan.FromSeconds(3));
        }
    }
}
