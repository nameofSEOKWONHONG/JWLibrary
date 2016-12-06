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
                Id = 0,
                Name = "test",
                Phone = "010",
                EDate = DateTime.Now
            };

            IList<Data> datas = new List<Data>
            {
                d
            };


            this.jwFlowLayoutPanel1.DataSource = d;
            this.jwDataGridView1.DataSource = datas;
            this.jwFlowLayoutPanel1.EndedControl += (s, e) =>
            {
                this.button2.Focus();
                //this.button2.Select();
            };
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

        private void button3_Click(object sender, EventArgs e)
        {
            Data d = (Data)this.jwFlowLayoutPanel1.DataSource;

            d.Id = 99;
            if (this.jwFlowLayoutPanel1.LockControl)
                this.jwFlowLayoutPanel1.LockControl = false;
            else
                this.jwFlowLayoutPanel1.LockControl = true;
        }
    }

    public class Data : INotifyPropertyChanged
    {
        private int _id;
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged("Id"); } }
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        private string _phone;
        public string Phone { get { return _phone; } set { _phone = value; OnPropertyChanged("Phone"); } }
        private DateTime _edate;
        public DateTime EDate { get { return _edate; } set { _edate = value; OnPropertyChanged("EDate"); } }

        public event PropertyChangedEventHandler PropertyChanged;        

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
