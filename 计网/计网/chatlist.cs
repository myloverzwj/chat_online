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
namespace 计网
{
    public partial class chatlist : Form
    {

        public string user;        //用户名
      //  public  firend;
        public string[] firendname;     //好友名称 以后可改为链表
        public int num;                   //好友数目
        public int chatnum;             //已打开的聊天窗口数目
        public string chatnow;           //当前选择的好友名称
        public List<chat> chats;         //所有打开的聊天窗口
        public string time_now;              //当前时间
       // public chat[] chats;
        public Socket Toserver;            //与服务器的TCP连接
          private TcpListener MyListener;      //监听端口
          private TcpListener MyListener_p; 
        public bool m_IsListen;            //是否监听
        private bool newchat;              //是否有新的好友
        private string mesnew;             //监听端口监听到信息
        public upload MM;                 //登录窗口 用于关闭程序
        private Thread m_MyThread;          //监听线程
        private Thread m_MyThread1;        //文件传输线程 不一定在这儿进行
        private string newname;          //监听端口监听到的好友名称
        private string newmes;           ////监听端口监听到信息
        public chatlist(string name1,Socket Toserver1,upload M)
        {
            InitializeComponent();        //初始化一些变量
            MM = M;
            user = name1;
            firendname = new string[100];
            chats = new List<chat>(100);
            Toserver = Toserver1;
         
            IPAddress myIPAddress = (IPAddress)Dns.GetHostAddresses(Dns.GetHostName()).GetValue(0);

            MyListener = new TcpListener( 1111);    //建立监听
                MyListener.Start();
          //  MyListener.s
                MyListener_p = new TcpListener(2222);    //建立监听
                MyListener_p.Start();
            m_IsListen = true;             
            Control.CheckForIllegalCrossThreadCalls = false;
            m_MyThread = new Thread(new ThreadStart(recv));        //打开监听的线程
            m_MyThread.Start();
            num = 0;
            newchat = false;
            m_MyThread1=new Thread(new ThreadStart(Main));
            m_MyThread1.Start();
        }
        private void Main()
        {

            while (m_IsListen)
            {
                bool connected = true;
                Socket sock = MyListener_p.AcceptSocket();
                NetworkStream newStream = new NetworkStream(sock);   
               // byte[] data = new byte[1024*1024*100];
               // int dataLength = newStream.Read(data, 0, data.Length);//读取客户端数据
                string name=user;//="C:\\Users\\mylover\\Pictures\\Camera Roll\\";
                string time1 = DateTime.Now.ToString();
               time1= time1.Replace('/', '_');
               time1 = time1.Replace(' ', '_');
               time1 = time1.Replace(':', '_');
                name+=time1;//DateTime.Now.ToString();
                name += ".jpg";

               
                while (connected)
                {
                    byte[] data = new byte[1024];
                    int dataLength = newStream.Read(data, 0, data.Length);//读取客户端数据
                    FileStream fStream = new FileStream(name, FileMode.Append);
                    string str = data.ToString();
                    fStream.Write(data, 0, data.Length);

                    if (dataLength < 1)
                    {
                        newStream.Close();
                        connected = false;
                      
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();
                        break;
                    }
                    fStream.Close();
                }



            }
        }
        private void recv()
        {
           
            while (m_IsListen)
            {

                Socket sock;
             
                sock = MyListener.AcceptSocket();
             
                

             
                NetworkStream NetStream = new NetworkStream(sock);                 //读取对方传递过来的信息               
                StreamReader sr = new StreamReader(NetStream);             
                string tempChat = sr.ReadToEnd();              //如果读取到，则触发事件将传来的信息回调                
              
                recvmessage(tempChat);        //消息处理
                mesnew = tempChat;

                sr.Close();
                
            }
        }
        private void recvmessage(string mes)
        {
         
            time_now = DateTime.Now.ToString();//以下是分解消息  消息类型为  学号+'|'+消息
            string Fname;
            string Fmessage;
            int i = 0;
            for (; mes[i] != '|'; i++) ;
            Fname = mes.Remove(i);       //得到学号
            string oldst;
            oldst = Fname + '|';
            Fmessage = mes;
            newmes = Fmessage.Replace(oldst, "");    //得到消息
            i = 0;
            newname = Fname;
           // newmes = Fmessage;
            for (i = 0; i < chats.Count; i++)//查找当前所有的聊天窗口,看是否已经有与该好友的聊天窗口
            {
                if (chats[i].Fname == Fname)      //若有  则激活该窗口,并显示消息       
                {
                    refresh();   
                    // chats[i].Show();
                    chats[i].Activate();
                    chats[i].showme(time_now,Fname,newmes);
                    return;
                }
            }
            firendname[i] = Fname;     //若没有 则提示主窗口有新的  好友的信息    以后可以加一些动画效果
            num++;
            newchat= true;
            chatnum++;
            refresh();
            //MessageBox.Show("you firend "+Fname+" sent you a short message", "", 0, MessageBoxIcon.None);
            
         //   
            //mes.First('|');
        }
        public void insertchat(string Fname)
        {
          
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
     
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)  //刷新
        {
            refresh();
           // listView2.Items.Add(mesnew);
        }

        private void button3_Click(object sender, EventArgs e)      //添加好友
        {
            string addname;
            addfir add1 = new addfir(this,num);
            num++;
            add1.Show();
            add1.Activate();
            addname = add1.name;
       
        }

        private void talkto(object sender, MouseEventArgs e)     //双击好友名称是打开聊天窗口
        {
            chatnow = listView1.SelectedItems[0].Text;            //获取选中好友信息
        // label1.Text = chatnow;
            int i;
            for (i = 0; i < chats.Count;i++ )       //查找当前打开的所有聊天窗口
            {
                if (chats[i].Fname == chatnow)   //若有显示并激活该窗口
                {
                    chats[i].Show();
                    chats[i].Activate();
                    return;
                }
            }

            chats.Insert(i, new chat(user, chatnow, this));         //若没有新建窗口
            chats[i].Show();
            chats[i].Activate();
           if(newchat) chats[i].showme(time_now, newname, newmes);   //若有消息发送
           newchat = false;
            chatnum++;
            

        }
      
        public void refresh()    //刷新显示   以后可改为定期刷新
        {
            listView1.Items.Clear();
         //   Toserver.Connect("166.111.180.60", 8000);
            for (int i = 0; i < num; i++)    //查询当前所有好友的在线情况
            {
                string sendmessager = "q";
                sendmessager += firendname[i];
                byte[] ToSend = Encoding.ASCII.GetBytes(sendmessager);

                Toserver.Send(ToSend);
                byte[] ToRec = new byte[1024];
                int bytelength = Toserver.Receive(ToRec);
                string Recmessage = Encoding.ASCII.GetString(ToRec, 0, bytelength);
                if (Recmessage != "n")
                {
                    listView1.Items.Add(firendname[i]);
                    listView1.Items[i].ForeColor = Color.Yellow;            //在线显示为黄色    以后可添加图标

                }
                else
                {
                    listView1.Items.Add(firendname[i]);                  //不在线显示为黑色 以后可添加图标
                    listView1.Items[i].ForeColor = Color.Black;
                }
            }
//        
            listView2.Clear();
            for (int i = 0; i < chats.Count;i++)         //显示当前所有的聊天窗口信息
            {

                listView2.Items.Add(chats[i].Fname);
            }
            if (newchat)
            {
//                 int i = chats.Count;
//                 chats.Insert(i, new chat(user, newname, this));
//                 chats[i].Show();
//                 chats[i].Activate();
//                 newchat = false;
                listView2.Items.Add(newname + " send a short message,please clik to check");   //提示用户有新的消息   以后可添加更多效果
                   
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void close(object sender, FormClosingEventArgs e)
        {
            
            MM.Close();   //关闭主程序
            Application.Exit();
        }

        private void chatlist_Load(object sender, EventArgs e)
        {

        }
    }
}
