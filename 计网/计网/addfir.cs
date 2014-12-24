using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 计网
{
    public partial class addfir : Form
    {
        public string name;
        public chatlist chat1;
        public int num;
        public addfir(chatlist M,int t)
        {
             
            InitializeComponent();
            chat1 = M;
            num = t;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
           // addname1 = name;
            chat1.firendname[num] = name;
            this.Close();
            chat1.refresh();
        }
    }
}
