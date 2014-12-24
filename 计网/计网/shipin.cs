using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using AForge.Imaging;

//using System.Windows.Media.Imaging;
using System.Media;
using AForge;
using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using Size = System.Drawing.Size;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AForge.Video.DirectShow;


using AForge.Controls;
using AForge.Video;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using AForge.Imaging;
using System.Drawing.Imaging;
using System.IO;
namespace 计网
{
    
    public partial class shipin : Form
    {
        private TcpListener MyListener;
        private Thread m_MyThread;
        private Thread m_MyThread1;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        public bool m_IsListen;
        public bool shi;
        public string FIP;
        private Socket socket = null;
        private IPEndPoint remoteEndPoint = null;
        //接受图片的缓冲
        private byte[] buffTemp = new byte[2 * 1024 * 1024];
      //  private G729 G7291;
     //   private SoundPlayer SoundPlayer2;
       // Bitmap bitmap;
      //  public  Image;
        public shipin(string FIP1,string MIP)
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = host.AddressList[1];
           
            shi = false;
            FIP = FIP1;
            InitializeComponent();
            m_IsListen = true;
           // MyListener = new TcpListener(3333);    //建立监听
            //MyListener.Start();
           // Control.CheckForIllegalCrossThreadCalls = false;
            m_MyThread = new Thread(new ThreadStart(recv));        //打开监听的线程
            m_MyThread.Start();
          
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
            this.remoteEndPoint = new IPEndPoint(IPAddress.Parse(FIP),3333);
            this.socket.Bind(new IPEndPoint(IPAddress.Parse(MIP),3333));
        //    this.btnKinescopeStop.Enabled = false;
        }
        private void recv()
        {
            
          
            while (m_IsListen)
            {
              
                    //每隔5000微妙查询一次socket状态
                    if (this.socket.Poll(5000, SelectMode.SelectRead))//为可读时，读取
                    {
                        this.socket.BeginReceive(buffTemp, 0, buffTemp.Length, SocketFlags.None, ReceiveFrame, this.socket);
                    }
                             
            }
        }
        private void ReceiveFrame(IAsyncResult ar)
        {
            Socket socketTemp = (Socket)ar.AsyncState;
            int count = socketTemp.EndReceive(ar);
            if (count > 0)
            {
                byte[] buff = new byte[count];
                Buffer.BlockCopy(buffTemp, 0, buff, 0, count);
                MemoryStream memoryStream = new MemoryStream(buff);
                Bitmap bitmap = (Bitmap)System.Drawing.Image.FromStream(memoryStream);
                this.pictureBox1.Image = bitmap;
            }
        }
        private object obcetBitmapTemp = new object();
        private void fuction(Bitmap bitmap)
        {
            lock (obcetBitmapTemp)
            {
                //this.pictureBox1.Image = bitmap;
                System.Drawing.Image imgTemp = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
                MemoryStream stream = new MemoryStream();
                imgTemp.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;
                byte[] buffImage = new byte[stream.Length];
                stream.Read(buffImage, 0, buffImage.Length);
                this.socket.BeginSendTo(buffImage, 0, buffImage.Length, SocketFlags.None, this.remoteEndPoint, SendData, this.socket);
                stream.Dispose();
                stream = null;
                
            }
        }

        private void SendData(IAsyncResult ar)
        {

            Socket socket = (Socket)ar.AsyncState;
            socket.EndSendTo(ar);
        }

        private void send1()
        {

           
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void close(object sender, FormClosingEventArgs e)
        {
            if (this.videoSourcePlayer1.IsRunning)
            {
                this.videoSourcePlayer1.Stop();
                //  this.videoSource.SignalToStop();


            }
            
        }
        void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            
            fuction(image);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 枚举所有视频输入设备
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                foreach (FilterInfo device in videoDevices)
                {
                    comboBox1.Items.Add(device.Name);
                }

                comboBox1.SelectedIndex = 0;

            }
            catch (ApplicationException)
            {
                comboBox1.Items.Add("No local capture devices");
                videoDevices = null;
            }
        }
        private void CameraConn()
        {
            
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);
            videoSource.DesiredFrameSize = new System.Drawing.Size(320, 240);
            videoSource.DesiredFrameRate = 1;

            videoSourcePlayer1.VideoSource = videoSource;
            videoSourcePlayer1.Start();
            shi = true;
           // panel1.
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CameraConn();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.videoSourcePlayer1.IsRunning)
            {
                this.videoSourcePlayer1.Stop();
              //  this.videoSource.SignalToStop();
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void videoSourcePlayer1_Click(object sender, EventArgs e)
        {

        }

        private void videoSourcePlayer2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, System.EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
     
        
    }
}
