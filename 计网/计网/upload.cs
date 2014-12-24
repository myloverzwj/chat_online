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

namespace 计网
{
    public partial class upload : Form          //登录功能
    {
        Socket Toserver;        //与服务器的TCP连接
        public upload()
        {
        Toserver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //初始化TCP
           InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void 记住密码_CheckedChanged(object sender, EventArgs e)
        {
             //在此处添加  用户数据库的是否 记住密码功能
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            string name = textBox1.Text;          //获取用户名
            string password = textBox2.Text;       //获取密码
            string sendmessager=name+'_'+password;
            Toserver.Connect("166.111.180.60", 8000);        //与服务器进行TCP连接
           
            byte[] ToSend = Encoding.ASCII.GetBytes(sendmessager);

            Toserver.Send(ToSend);       //发送用户名及密码
            byte[] ToRec = new byte[1024];
            int bytelength = Toserver.Receive(ToRec);   //接收客户端返回的字符串
            string Recmessage = Encoding.ASCII.GetString(ToRec, 0, bytelength);   //类型转化

           // Toserver.Shutdown(SocketShutdown.Both);
           // Toserver.Close(0);
            if (Recmessage != "lol")   //输入错误
            {
                MessageBox.Show("There is something wrong about usernmae or code!", "WARNING", 0, MessageBoxIcon.Warning);//提示消息
                
            }
            else
            {
                if (Recmessage == "lol")   //输入正确
                {
                    MessageBox.Show("successful!", "", 0, MessageBoxIcon.None);       //提示消息

                    chatlist chat1 = new chatlist(name, Toserver,this);     //新建主窗口
                    this.Hide();                                    //影藏当前的登录串口
                  
                    chat1.Show();                                  //显示主窗口
                    chat1.Activate();                             //激活主窗口
                   
                    
                    return;
                }
                MessageBox.Show("There is something wrong about usernmae or code!", "WARNING", 0, MessageBoxIcon.Warning);

            }
           
        }

        private void upload_Load(object sender, EventArgs e) //初始化登录窗口
        {
            textBox1.Text = "2012011422";              //在此处建立数据库连接,访问历史登录信息 
            textBox2.Text = "net2014"; 

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void close1(object sender, FormClosingEventArgs e)   //关闭
        {
             //在此处添加注销功能
            Toserver.Shutdown(SocketShutdown.Both);       //断开TCP连接
            Toserver.Close();
        }
    }
}
