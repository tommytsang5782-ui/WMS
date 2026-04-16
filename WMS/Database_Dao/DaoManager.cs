using System;
using WMS;

namespace WMS.Database_Dao
{
    /// <summary>
    /// 單一取得各 DAO 的入口；已自 Form1 抽離，便於測試與重用。
    /// </summary>
    public static class DaoManager
    {
        private static readonly Lazy<Dao> _dao = new Lazy<Dao>(() => new Dao());
        private static readonly Lazy<Dao_ClosedPrescan> _daoClosedPrescan = new Lazy<Dao_ClosedPrescan>(() => new Dao_ClosedPrescan());
        private static readonly Lazy<Dao_ClosedPrescanInnerCarton> _daoClosedPrescanInnerCarton = new Lazy<Dao_ClosedPrescanInnerCarton>(() => new Dao_ClosedPrescanInnerCarton());
        private static readonly Lazy<Dao_ClosedPrescanOuterCarton> _daoClosedPrescanOuterCarton = new Lazy<Dao_ClosedPrescanOuterCarton>(() => new Dao_ClosedPrescanOuterCarton());
        private static readonly Lazy<Dao_Connection> _daoConnection = new Lazy<Dao_Connection>(() => new Dao_Connection());
        private static readonly Lazy<Dao_CustomerGroup> _daoCustomerGroup = new Lazy<Dao_CustomerGroup>(() => new Dao_CustomerGroup());
        private static readonly Lazy<Dao_InnerCarton> _daoInnerCarton = new Lazy<Dao_InnerCarton>(() => new Dao_InnerCarton());
        private static readonly Lazy<Dao_Item> _daoItem = new Lazy<Dao_Item>(() => new Dao_Item());
        private static readonly Lazy<Dao_LabelHeader> _daoLabelHeader = new Lazy<Dao_LabelHeader>(() => new Dao_LabelHeader());
        private static readonly Lazy<Dao_LabelLine> _daoLabelLine = new Lazy<Dao_LabelLine>(() => new Dao_LabelLine());
        private static readonly Lazy<Dao_Mapping> _daoMapping = new Lazy<Dao_Mapping>(() => new Dao_Mapping());
        private static readonly Lazy<Dao_OuterCarton> _daoOuterCarton = new Lazy<Dao_OuterCarton>(() => new Dao_OuterCarton());
        private static readonly Lazy<Dao_PackingHeader> _daoPackingHeader = new Lazy<Dao_PackingHeader>(() => new Dao_PackingHeader());
        private static readonly Lazy<Dao_PackingLine> _daoPackingLine = new Lazy<Dao_PackingLine>(() => new Dao_PackingLine());
        private static readonly Lazy<Dao_PackingMapping> _daoPackingMapping = new Lazy<Dao_PackingMapping>(() => new Dao_PackingMapping());
        private static readonly Lazy<Dao_Prescan> _daoPrescan = new Lazy<Dao_Prescan>(() => new Dao_Prescan());
        private static readonly Lazy<Dao_PrescanInnerCarton> _daoPrescanInnerCarton = new Lazy<Dao_PrescanInnerCarton>(() => new Dao_PrescanInnerCarton());
        private static readonly Lazy<Dao_PrescanOuterCarton> _daoPrescanOuterCarton = new Lazy<Dao_PrescanOuterCarton>(() => new Dao_PrescanOuterCarton());
        private static readonly Lazy<Dao_Printer> _daoPrinter = new Lazy<Dao_Printer>(() => new Dao_Printer());
        private static readonly Lazy<Dao_ScanLabelString> _daoScanLabelString = new Lazy<Dao_ScanLabelString>(() => new Dao_ScanLabelString());
        private static readonly Lazy<Dao_ScannedPackingHeader> _daoScannedPackingHeader = new Lazy<Dao_ScannedPackingHeader>(() => new Dao_ScannedPackingHeader());
        private static readonly Lazy<Dao_ScannedPackingLine> _daoScannedPackingLine = new Lazy<Dao_ScannedPackingLine>(() => new Dao_ScannedPackingLine());
        private static readonly Lazy<Dao_ScannedPackingMapping> _daoScannedPackingMapping = new Lazy<Dao_ScannedPackingMapping>(() => new Dao_ScannedPackingMapping());
        private static readonly Lazy<Dao_Synchronize> _daoSynchronize = new Lazy<Dao_Synchronize>(() => new Dao_Synchronize());
        private static readonly Lazy<Dao_Company> _daoCompany = new Lazy<Dao_Company>(() => new Dao_Company());
        private static readonly Lazy<Dao_User> _daoUser = new Lazy<Dao_User>(() => new Dao_User());
        private static readonly Lazy<Dao_ODataSetup> _daoODataSetup = new Lazy<Dao_ODataSetup>(() => new Dao_ODataSetup());
        private static readonly Lazy<Dao_Setup> _daoSetup = new Lazy<Dao_Setup>(() => new Dao_Setup());

        public static Dao dao => _dao.Value;
        public static Dao_ClosedPrescan daoClosedPrescan => _daoClosedPrescan.Value;
        public static Dao_ClosedPrescanInnerCarton daoClosedPrescanInnerCarton => _daoClosedPrescanInnerCarton.Value;
        public static Dao_ClosedPrescanOuterCarton daoClosedPrescanOuterCarton => _daoClosedPrescanOuterCarton.Value;
        public static Dao_Connection daoConnection => _daoConnection.Value;
        public static Dao_CustomerGroup daoCustomerGroup => _daoCustomerGroup.Value;
        public static Dao_InnerCarton daoInnerCarton => _daoInnerCarton.Value;
        public static Dao_OuterCarton daoOuterCarton => _daoOuterCarton.Value;
        public static Dao_Item daoItem => _daoItem.Value;
        public static Dao_LabelHeader daoLabelHeader => _daoLabelHeader.Value;
        public static Dao_LabelLine daoLabelLine => _daoLabelLine.Value;
        public static Dao_Mapping daoMapping => _daoMapping.Value;
        public static Dao_PackingHeader daoPackingHeader => _daoPackingHeader.Value;
        public static Dao_PackingLine daoPackingLine => _daoPackingLine.Value;
        public static Dao_PackingMapping daoPackingMapping => _daoPackingMapping.Value;
        public static Dao_Prescan daoPrescan => _daoPrescan.Value;
        public static Dao_PrescanInnerCarton daoPrescanInnerCarton => _daoPrescanInnerCarton.Value;
        public static Dao_PrescanOuterCarton daoPrescanOuterCarton => _daoPrescanOuterCarton.Value;
        public static Dao_Printer daoPrinter => _daoPrinter.Value;
        public static Dao_ScanLabelString daoScanLabelString => _daoScanLabelString.Value;
        public static Dao_ScannedPackingHeader daoScannedPackingHeader => _daoScannedPackingHeader.Value;
        public static Dao_ScannedPackingLine daoScannedPackingLine => _daoScannedPackingLine.Value;
        public static Dao_ScannedPackingMapping daoScannedPackingMapping => _daoScannedPackingMapping.Value;
        public static Dao_Synchronize daoSynchronize => _daoSynchronize.Value;
        public static Dao_Company daoCompany => _daoCompany.Value;
        public static Dao_User daoUser => _daoUser.Value;
        public static Dao_ODataSetup daoODataSetup => _daoODataSetup.Value;
        public static Dao_Setup daoSetup => _daoSetup.Value;
    }
}
