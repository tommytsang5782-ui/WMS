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
    public partial class CardPage : Form
    {
        public CardPage()
        {
            InitializeComponent();

        }
        void BuildCardPage(CardPageConfig cardPage)
        {
            panel1.Controls.Clear();
            int top = 10;

            foreach (var field in cardPage.Fields)
            {
                // Label
                Label lbl = new Label();
                lbl.Text = field.Label;
                lbl.Top = top;
                lbl.Left = 10;
                panel1.Controls.Add(lbl);

                // TextBox
                TextBox txt = new TextBox();
                txt.Name = field.Field;
                txt.Top = top;
                txt.Left = 100;
                txt.Width = 200;
                panel1.Controls.Add(txt);

                top += 35;
            }
        }
    }
}
