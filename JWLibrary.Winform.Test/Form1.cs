using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JWLibrary.Winform.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Data d = new Data
            {
                Id = 1,
                Name = "test",
                Phone = "010",
                EDate = DateTime.Now
            };

            this.jwFlowLayoutPanel1.DataSource = d;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.jwFlowLayoutPanel1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Data d = (Data)this.jwFlowLayoutPanel1.DataSource;

            MessageBox.Show(string.Format($"Id:{d.Id}, Name : {d.Name}, Phone : {d.Phone}, EDate : {d.EDate}"));
        }
    }

    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime EDate { get; set; }
    }
}
