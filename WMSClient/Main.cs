using System;
using System.Windows.Forms;
using WMSClient.Labelfolder;
using WMSClient.Prescan_;
using WMSClient.ScanLabelString_;
using WMSClient.ClosedPrescanfolder;
using WMSClient.ScannedPackingListfolder;
using WMSClient.Itemfolder;
using WMSClient.CustomerGroupfolder;
using WMSClient.Printerfolder;
using WMSClient.PackingListfolder;
using WMSClient.Class;
using Newtonsoft.Json;
using WMSClient.Base;
using static WMSClient.Class.SocketConnect;

namespace WMSClient
{
    public partial class Main : BaseBusinessForm
    {
        private readonly string _userID;
        public Main()
        {
            //InitializeComponent();
        }

        public Main(SocketConnect socketConnect, String userID) : 
            base(socketConnect)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(userID))
            {
                throw new ArgumentNullException(nameof(userID), "User ID cannot be empty.");
            }

            _userID = userID;
            listBox1.Items.Add("Welcome " + _userID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var userList = new UserList(_socketConnect, _userID);
            userList.Show();
            //userList.MdiParent = this;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var mappingList = new MappingList(_socketConnect, _userID);
            mappingList.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var labelList = new LabelList(_socketConnect);
            labelList.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var scanLabelStringList = new ScanLabelStringList(_socketConnect);
            scanLabelStringList.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var prescanList = new PrescanList(_socketConnect, _userID);
            prescanList.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var packingList = new PackingList.Packing_List(_socketConnect))
            {
                DialogResult result = packingList.ShowDialog();
                // 自动释放资源，无需手动置空
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            ScannedPackingList scannedPackingList = new ScannedPackingList(_socketConnect, _userID);
            scannedPackingList.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ClosedPrescanList closedPrescanList = new ClosedPrescanList(_socketConnect, _userID);
            closedPrescanList.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ItemList itemList = new ItemList(_socketConnect, _userID);
            itemList.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
        }

        private void button12_Click(object sender, EventArgs e)
        {
            CustomerGroupList customerGroupList = new CustomerGroupList(_socketConnect, _userID);
            customerGroupList.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PrinterList printerList = new PrinterList(_socketConnect, _userID);
            printerList.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            PackingMappingList packingMappingList = new PackingMappingList(_socketConnect, _userID);
            packingMappingList.Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ScannedPackingMappingList scannedPackingMappingList = new ScannedPackingMappingList(_socketConnect, _userID);
            scannedPackingMappingList.Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ScanLabelString scanLabelString = new ScanLabelString();
            scanLabelString.DocumentNo = "abcd";
            String a = _socketConnect.SendMessage(SQLOption.Detele,scanLabelString);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            CompanyList companyList = new CompanyList(_socketConnect, _userID);
            companyList.Show();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            ODataSetupPage oDataSetupPage = new ODataSetupPage(_socketConnect, _userID);
            oDataSetupPage.Show();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            DialogResult result = form3.ShowDialog();
            if (result == DialogResult.OK)
            {
                form3 = null;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            SetupPage setupPage = new SetupPage(_socketConnect);
            setupPage.Show();
        }

        /// <summary>Open Menu2 (list mode, like Android).</summary>
        private void btnMenu2_Click(object sender, EventArgs e)
        {
            var menu2 = new Menu2Form(_socketConnect, _userID);
            menu2.Show();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
