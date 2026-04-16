using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WMSClient
{
    public partial class ListPage : Form
    {
        public ListPage()
        {
            InitializeComponent();
        }
        void BuildListPage(ListPageConfig listPage)
        {
            // 建 Column
            dataGridView1.Columns.Clear();
            foreach (var col in listPage.Columns)
            {
                Type type = Type.GetType(col.Type) ?? typeof(string);
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = col.Field;
                c.HeaderText = col.Label;
                dataGridView1.Columns.Add(c);
            }

            // 建按钮
            flowLayoutPanel1.Controls.Clear();
            foreach (var btn in listPage.Buttons)
            {
                Button b = new Button();
                b.Text = btn.Text;
                b.Name = btn.Name;
                flowLayoutPanel1.Controls.Add(b);
            }
        }
    }
}
