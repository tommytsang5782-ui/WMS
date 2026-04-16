using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMSClient.Base;
using WMSClient.Class;
using static WMSClient.Class.SocketConnect;

namespace WMSClient
{
    public partial class Menu3 : BaseBusinessForm
    {
        public Menu3()
        {
            InitializeComponent();
        }
        void BuildMenu(TreeView tree, List<MenuItem> menu)
        {
            tree.Nodes.Clear();
            foreach (var item in menu)
            {
                TreeNode node = new TreeNode(item.Text);
                node.Tag = item.FormName;
                tree.Nodes.Add(node);
            }
        }
    }

}
