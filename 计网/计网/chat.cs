using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Timers;
namespace 计网
{
    public partial class chat : Form
    {
        public string Myname;   //用户名
        public string Fname;       //对方姓名
        public bool m_IsListen;    //是否进行监听
        public string FIP;           //对方IP
        private chatlist chatlist1;   //主窗口
        private Socket Toserver;         //与服务器的TCP连接
        int num;
        public chat(string myname,string firendname,chatlist M)
        {
            InitializeComponent();
            Toserver = M.Toserver;   //获取与服务器的TCP
            string sendmessager = "q";                      //以下获取对方的IP地址
            sendmessager += Fname;
            byte[] ToSend = Encoding.ASCII.GetBytes(sendmessager);

            Toserver.Send(ToSend);
            byte[] ToRec = new byte[1024];
            int bytelength = Toserver.Receive(ToRec);
            string Recmessage = Encoding.ASCII.GetString(ToRec, 0, bytelength);
          
            if (Recmessage != "n")
            {
                FIP = Recmessage;
                
            }
            else FIP = "NULL";
            num = 0;         //初始化一些值
            chatlist1 = M;
           // MyListener = new TcpListener(9999);    //新建一个线程 
          //  m_IsListen = true;
            //  Thread t = new Thread(recv);
            //t.Start();  
            Myname = myname;
            Fname = firendname;
            label1.Text = firendname;
          

        }
       private void recv()  //接收文件的线程
        {
//             while (m_IsListen)
//             {
//                 Socket sock = MyListener.AcceptSocket();
//                // m_NetStream = new NetworkStream(sock);                 //读取对方传递过来的信息                 
//                 //StreamReadersr = new StreamReader(m_NetStream);
//                 byte[] ToRec = new byte[1024];
//                 int bytelength = sock.Receive(ToRec);
//                 string Recmessage = Encoding.ASCII.GetString(ToRec, 0, bytelength);
//                // string tempChat = sr.ReadLine();
// 
//             }
            
        }
        private void set_Tcp()
       {

       }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void showme(string a,string b,string c)   //显示消息
        {
            if (c == null) return;

                                                    //在此处添加用户消息记录的数据库
             b += "   " + a;
             int x =richTextBox1.TextLength;
             //richTextBox1.Lines[richTextBox1.Lines.Length - 1];  
            
             richTextBox1.AppendText(b);                               //可在此处添加 改变记录的字体,字号,颜色的代码
             int y = richTextBox1.TextLength - x;
             richTextBox1.SelectionStart =x;
             richTextBox1.SelectionLength = y;
             richTextBox1.SelectionColor = Color.Green;
          
             richTextBox1.AppendText("\n");
             richTextBox1.AppendText(c);
             richTextBox1.AppendText("\n");
//              if (c == "send you a picture")
//              {
//                  Thread.Sleep(1000);
//                  Graphics g = richTextBox1.CreateGraphics();
//                  System.Drawing.Image myimg = Image.FromFile(@"C://srkl.jpg", false);
//                  g.DrawImage(myimg, 0, 0, 400, 400);
//               }
            
        }
        public void showmepic(string a, string b, string c)   //显示消息
        {

//             Graphics g = richTextBox1.CreateGraphics();
//             System.Drawing.Image myimg = Image.FromFile(c, false);
//             g.DrawImage(myimg, 0, 0, 400, 400);

        }
        private void button1_Click(object sender, EventArgs e)
        {
           
         //   Toserver.Connect("166.111.180.60", 8000);       
            string sendmessager = "q";                              //以下部分确认对方是否在线
            sendmessager += Fname;
            byte[] ToSend = Encoding.ASCII.GetBytes(sendmessager);

            Toserver.Send(ToSend);
            byte[] ToRec = new byte[1024];
            int bytelength = Toserver.Receive(ToRec);
            string Recmessage = Encoding.ASCII.GetString(ToRec, 0, bytelength);
           
            if (Recmessage == "n")
            {
                MessageBox.Show("you firend is not online", "", 0, MessageBoxIcon.None); //不在线不进行发送
                return;
            }
            else
            {
                string message;                           //在线  发送消息
                message = textBox2.Text;
                if(message=="") return;
                FIP = Recmessage;                          //与对方建立TCP连接
                TcpClient tcptoF;
               // tcptoF.
               
                tcptoF = new TcpClient(FIP,1111);                      
                string sendMessage = Myname + "|" + message;
                StreamWriter sw = new StreamWriter(tcptoF.GetStream());                 
                sw.Write(sendMessage);
                sw.Flush(); sw.Close();
                showme(DateTime.Now.ToString(), Myname, message);         //发送信息
                textBox2.Text = "";               //清空输入框
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chat_Load(object sender, EventArgs e)
        {
        }

        private void close(object sender, FormClosedEventArgs e)
        {
            chatlist1.chats.Remove(this);        //关闭时,删除在主窗口中的连接
           
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
           OpenFileDialog openfiledialog1=new OpenFileDialog();
           openfiledialog1.ShowDialog();
         
           string Name = openfiledialog1.FileName;
           if (Name == "") return;
           FileStream fStream = new FileStream(Name, FileMode.Open);//在本地打开一个

            byte[] imgData = new byte[fStream.Length];//创建一个字节流
            fStream.Read(imgData, 0, imgData.Length);//读取文件流
            fStream.Close();//关闭文件
            string sendmessager = "q";                              //以下部分确认对方是否在线
            sendmessager += Fname;
            byte[] ToSend = Encoding.ASCII.GetBytes(sendmessager);

            Toserver.Send(ToSend);
            byte[] ToRec = new byte[1024];
            int bytelength = Toserver.Receive(ToRec);
            string Recmessage = Encoding.ASCII.GetString(ToRec, 0, bytelength);

            if (Recmessage == "n")
            {
                MessageBox.Show("you firend is not online", "", 0, MessageBoxIcon.None); //不在线不进行发送
                return;
            }
            else
            {
                
               // FIP = Recmessage;                          //与对方建立TCP连接
                FIP = Recmessage;                          //与对方建立TCP连接
                TcpClient tcptoF1;
                // tcptoF.

                tcptoF1 = new TcpClient(FIP, 1111);
                string sendMessage = Myname + "|" + "send you a picture";
                StreamWriter sw = new StreamWriter(tcptoF1.GetStream());
                sw.Write(sendMessage);
                sw.Flush(); 
                sw.Close();
                tcptoF1.Close();
                showme(DateTime.Now.ToString(), Myname, "you send a picture"); 
                TcpClient tcptoF;
                tcptoF = new TcpClient(FIP, 2222);
                // tcptoF.
                NetworkStream imgStream = tcptoF.GetStream();
                imgStream.Write(imgData, 0, imgData.Length);//发送图片
                showmepic(DateTime.Now.ToString(), Myname, Name);         //发送信息
                textBox2.Text = "";               //清空输入框
                tcptoF.Close();
            }
            
        }
        private void send()
        {

            string sendmessager = "q";                              //以下部分确认对方是否在线
            sendmessager += Fname;
            byte[] ToSend = Encoding.ASCII.GetBytes(sendmessager);

            Toserver.Send(ToSend);
            byte[] ToRec = new byte[1024];
            int bytelength = Toserver.Receive(ToRec);
            string Recmessage = Encoding.ASCII.GetString(ToRec, 0, bytelength);

            if (Recmessage == "n")
            {
                MessageBox.Show("you firend is not online", "", 0, MessageBoxIcon.None); //不在线不进行发送
                return;
            }
            else
            {
                string message;                           //在线  发送消息
                message = textBox2.Text;
                if (message == "") return;
                FIP = Recmessage;                          //与对方建立TCP连接
                TcpClient tcptoF;
                // tcptoF.

                tcptoF = new TcpClient(FIP, 1111);
                string sendMessage = Myname + "|" + message;
                StreamWriter sw = new StreamWriter(tcptoF.GetStream());
                sw.Write(sendMessage);
                sw.Flush(); sw.Close();
                showme(DateTime.Now.ToString(), Myname, message);         //发送信息
                textBox2.Text = "";               //清空输入框
            }
        }

        private void send(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Enter) send(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
             string sendmessager = "q";                              //以下部分确认对方是否在线
            sendmessager += Fname;
            byte[] ToSend = Encoding.ASCII.GetBytes(sendmessager);

            Toserver.Send(ToSend);
            byte[] ToRec = new byte[1024];
            int bytelength = Toserver.Receive(ToRec);
            string Recmessage = Encoding.ASCII.GetString(ToRec, 0, bytelength);
             sendmessager = "q";                              //以下部分确认对方是否在线
            sendmessager += Myname;
           ToSend = Encoding.ASCII.GetBytes(sendmessager);

            Toserver.Send(ToSend);
           ToRec = new byte[1024];
           bytelength = Toserver.Receive(ToRec);
            string myIP = Encoding.ASCII.GetString(ToRec, 0, bytelength);

            if (Recmessage == "n")
            {
                MessageBox.Show("you firend is not online", "", 0, MessageBoxIcon.None); //不在线不进行发送
                return;
            }
            else
            {
                string message;                           //在线  发送消息
                message = "您的好友"+Myname+"向您发起了视频请求！请打开摄像头";
                
                FIP = Recmessage;                          //与对方建立TCP连接
                TcpClient tcptoF;
                // tcptoF.

                tcptoF = new TcpClient(FIP, 1111);
                string sendMessage = Myname + "|" + message;
                StreamWriter sw = new StreamWriter(tcptoF.GetStream());
                sw.Write(sendMessage);
                sw.Flush(); sw.Close();
                shipin shipin1 = new shipin(Recmessage,myIP);
                shipin1.Show();
                shipin1.Activate();
            }
            showme(DateTime.Now.ToString(), Myname, "您已发送视频请求，等待好友同意");  
        } 
    }
}
