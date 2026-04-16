using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ClientType
{
    class WindowsClient
    {
        private void recievemsg(object soc)
        {
            Socket socketClient = (Socket)soc;
            CommuForm commuForm = new CommuForm("", "", "", "");
            NetworkStream ns = new NetworkStream(socketClient);
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true;
            Dao dao = new Dao();
            Dao_ClosedPrescan daoClosedPrescan = new Dao_ClosedPrescan();
            Dao_ClosedPrescanInnerCarton daoClosedPrescanInnerCarton = new Dao_ClosedPrescanInnerCarton();
            Dao_ClosedPrescanOuterCarton daoClosedPrescanOuterCarton = new Dao_ClosedPrescanOuterCarton();
            Dao_Connection daoConnection = new Dao_Connection();
            Dao_CustomerGroup daoCustomerGroup = new Dao_CustomerGroup();
            Dao_DirectPrint daoDirectPrint = new Dao_DirectPrint();
            Dao_DirectPrintInnerCarton daoDirectPrintInnerCarton = new Dao_DirectPrintInnerCarton();
            Dao_DirectPrintOuterCarton daoDirectPrintOuterCarton = new Dao_DirectPrintOuterCarton();
            Dao_InnerCarton daoInnerCarton = new Dao_InnerCarton();
            Dao_Item daoItem = new Dao_Item();
            Dao_LabelHeader daoLabelHeader = new Dao_LabelHeader();
            Dao_LabelLine daoLabelLine = new Dao_LabelLine();
            Dao_Mapping daoMapping = new Dao_Mapping();
            Dao_OuterCarton daoOuterCarton = new Dao_OuterCarton();
            Dao_PackingHeader daoPackingHeader = new Dao_PackingHeader();
            Dao_PackingLine daoPackingLine = new Dao_PackingLine();
            Dao_PackingMapping daoPackingMapping = new Dao_PackingMapping();
            Dao_Prescan daoPrescan = new Dao_Prescan();
            Dao_PrescanInnerCarton daoPrescanInnerCarton = new Dao_PrescanInnerCarton();
            Dao_PrescanOuterCarton daoPrescanOuterCarton = new Dao_PrescanOuterCarton();
            Dao_Printer daoPrinter = new Dao_Printer();
            Dao_ScanLabelString daoScanLabelString = new Dao_ScanLabelString();
            Dao_ScannedPackingHeader daoScannedPackingHeader = new Dao_ScannedPackingHeader();
            Dao_ScannedPackingLine daoScannedPackingLine = new Dao_ScannedPackingLine();
            Dao_ScannedPackingMapping daoScannedPackingMapping = new Dao_ScannedPackingMapping();
            Dao_Synchronize daoSynchronize = new Dao_Synchronize();
            Dao_User daoUser = new Dao_User();
            MessageText MsgTxt = new MessageText();

            DataTable dt;
            string json;
            string msg;
            int i;
            String MACAddress = "";

            while (true)
            {
                try
                {

                    msg = sr.ReadLine();

                    commuForm = JsonConvert.DeserializeObject<CommuForm>(msg);
                    showmsg(socketClient.RemoteEndPoint.ToString() + ":\r\n" +
                           "  --->  Command: " + commuForm.Command + "\r\n" +
                           "  --->  Action      : " + commuForm.Action + "\r\n" +
                           "  --->  Table       : " + commuForm.Table + "\r\n" +
                           "  --->  Str            : " + commuForm.Str);
                    switch (commuForm.Command)
                    {
                        case "Open":
                            switch (commuForm.Action)
                            {
                                case "List":
                                    switch (commuForm.Table)
                                    {
                                        case "UserList":
                                            UserList userList = new UserList();
                                            userList.OpenUserList(socketClient, this);
                                            break;
                                        case "UserCard":
                                            User_Control userCardControl = new User_Control();
                                            userCardControl.OpenUserCard();
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case "New":
                            switch (commuForm.Action)
                            {
                                case "Device":
                                    dao.InitSynchronize(commuForm.Str, DateTime.MinValue);
                                    break;
                            }
                            break;
                        case "SQL_W":
                            int effectedRows = 0;
                            switch (commuForm.Action)
                            {
                                case "PCConnect":
                                    try
                                    {
                                        Console.WriteLine("For key = " + socketClient.RemoteEndPoint.ToString() + " , value = {0}.",
                                            dic[socketClient.RemoteEndPoint.ToString()]);
                                        String value;
                                        if (dic2.TryGetValue(socketClient.RemoteEndPoint.ToString(), out value))
                                        {
                                            dic2[socketClient.RemoteEndPoint.ToString()] = "PC";
                                        }

                                    }
                                    catch (KeyNotFoundException)
                                    {
                                        Console.WriteLine("Key = " + socketClient.RemoteEndPoint.ToString() + " is not found.");
                                    }
                                    break;
                                case "Select":
                                    switch (commuForm.Table)
                                    {
                                        case "User":
                                            //string iString = "2021-01-01 22:12 PM";
                                            //DateTime oDate = DateTime.ParseExact(iString, "yyyy-MM-dd HH:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                                            //dt = dao.GetSynchronizeData("User", oDate);
                                            //List<User> data = new List<User>();
                                            //foreach (DataRow dr in dt.Rows)
                                            //{
                                            //    User cUser = daoUser.GetItem<User>(dr);
                                            //    data.Add(cUser);
                                            //}

                                            User user = JsonConvert.DeserializeObject<User>(commuForm.Str.Remove(0, 1));
                                            List<User> userList = daoUser.Select(user);
                                            json = JsonConvert.SerializeObject(userList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "Item":
                                            Item item = JsonConvert.DeserializeObject<Item>(commuForm.Str.Remove(0, 1));
                                            List<Item> itemList = daoItem.Select(item);
                                            json = JsonConvert.SerializeObject(itemList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "CustomerGroup":
                                            CustomerGroup customerGroup = JsonConvert.DeserializeObject<CustomerGroup>(commuForm.Str.Remove(0, 1));
                                            List<CustomerGroup> customerGroupList = daoCustomerGroup.Select(customerGroup);
                                            json = JsonConvert.SerializeObject(customerGroupList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "Printer":
                                            Printer printer = JsonConvert.DeserializeObject<Printer>(commuForm.Str.Remove(0, 1));
                                            List<Printer> printerList = daoPrinter.SelectPrinter(printer);
                                            json = JsonConvert.SerializeObject(printerList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "PackingHeader":
                                            PackingHeader packingHeader = JsonConvert.DeserializeObject<PackingHeader>(commuForm.Str.Remove(0, 1));
                                            List<PackingHeader> packingHeaderList = daoPackingHeader.SelectPackingHeader(packingHeader);
                                            json = JsonConvert.SerializeObject(packingHeaderList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "PackingLine":
                                            PackingLine packingLine = JsonConvert.DeserializeObject<PackingLine>(commuForm.Str.Remove(0, 1));
                                            List<PackingLine> packingLineList = daoPackingLine.SelectPackingLine(packingLine);
                                            json = JsonConvert.SerializeObject(packingLineList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "Mapping":
                                            Mapping mapping = JsonConvert.DeserializeObject<Mapping>(commuForm.Str.Remove(0, 1));
                                            List<Mapping> mappingLine = daoMapping.Select(mapping);
                                            json = JsonConvert.SerializeObject(mappingLine, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "LabelHeader":
                                            LabelHeader labelHeader = JsonConvert.DeserializeObject<LabelHeader>(commuForm.Str.Remove(0, 1));
                                            List<LabelHeader> labelHeaderList = daoLabelHeader.Select(labelHeader);
                                            json = JsonConvert.SerializeObject(labelHeaderList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "LabelLine":
                                            LabelLine labelLine = JsonConvert.DeserializeObject<LabelLine>(commuForm.Str.Remove(0, 1));
                                            List<LabelLine> labelLineList = daoLabelLine.Select(labelLine);
                                            json = JsonConvert.SerializeObject(labelLineList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "ScanLabelString":
                                            ScanLabelString scanLabelString = JsonConvert.DeserializeObject<ScanLabelString>(commuForm.Str.Remove(0, 1));
                                            List<ScanLabelString> scanLabelStringList = daoScanLabelString.Select(scanLabelString);
                                            json = JsonConvert.SerializeObject(scanLabelStringList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "Prescan":
                                            Prescan prescan = JsonConvert.DeserializeObject<Prescan>(commuForm.Str.Remove(0, 1));
                                            List<Prescan> prescanList = daoPrescan.Select(prescan);
                                            json = JsonConvert.SerializeObject(prescanList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "OuterCarton":
                                            OuterCarton outerCarton = JsonConvert.DeserializeObject<OuterCarton>(commuForm.Str.Remove(0, 1));
                                            List<OuterCarton> outerCartonList = daoOuterCarton.SelectOuterCarton(outerCarton);
                                            json = JsonConvert.SerializeObject(outerCartonList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "InnerCarton":
                                            InnerCarton innerCarton = JsonConvert.DeserializeObject<InnerCarton>(commuForm.Str.Remove(0, 1));
                                            List<InnerCarton> innerCartonList = daoInnerCarton.SelectInnerCarton(innerCarton);
                                            json = JsonConvert.SerializeObject(innerCartonList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "PrescanOuterCarton":
                                            PrescanOuterCarton prescanOuterCarton = JsonConvert.DeserializeObject<PrescanOuterCarton>(commuForm.Str.Remove(0, 1));
                                            List<PrescanOuterCarton> prescanOuterCartonList = daoPrescanOuterCarton.Select(prescanOuterCarton);
                                            json = JsonConvert.SerializeObject(prescanOuterCartonList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "PrescanInnerCarton":
                                            PrescanInnerCarton prescaninnerCarton = JsonConvert.DeserializeObject<PrescanInnerCarton>(commuForm.Str.Remove(0, 1));
                                            List<PrescanInnerCarton> prescaninnerCartonList = daoPrescanInnerCarton.Select(prescaninnerCarton);
                                            json = JsonConvert.SerializeObject(prescaninnerCartonList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "PackingMapping":
                                            PackingMapping packingMapping = JsonConvert.DeserializeObject<PackingMapping>(commuForm.Str.Remove(0, 1));
                                            List<PackingMapping> packingMappingList = daoPackingMapping.SelectPackingMapping(packingMapping);
                                            json = JsonConvert.SerializeObject(packingMappingList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "DirectPrint":
                                            DirectPrint directPrint = JsonConvert.DeserializeObject<DirectPrint>(commuForm.Str.Remove(0, 1));
                                            List<DirectPrint> directPrintList = daoDirectPrint.SelectDirectPrint(directPrint);
                                            json = JsonConvert.SerializeObject(directPrintList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "DirectPrintOuterCarton":
                                            DirectPrintOuterCarton directPrintOuterCarton = JsonConvert.DeserializeObject<DirectPrintOuterCarton>(commuForm.Str.Remove(0, 1));
                                            List<DirectPrintOuterCarton> directPrintOuterCartonList = daoDirectPrintOuterCarton.SelectDirectPrintOuterCarton(directPrintOuterCarton);
                                            json = JsonConvert.SerializeObject(directPrintOuterCarton, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "DirectPrintInnerCarton":
                                            DirectPrintInnerCarton directPrintInnerCarton = JsonConvert.DeserializeObject<DirectPrintInnerCarton>(commuForm.Str.Remove(0, 1));
                                            List<DirectPrintInnerCarton> directPrintInnerCartondList = daoDirectPrintInnerCarton.SelectDirectPrintInnerCarton(directPrintInnerCarton);
                                            json = JsonConvert.SerializeObject(directPrintInnerCartondList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "ScannedPackingHeader":
                                            ScannedPackingHeader scannedPackingHeader = JsonConvert.DeserializeObject<ScannedPackingHeader>(commuForm.Str.Remove(0, 1));
                                            List<ScannedPackingHeader> scannedPackingHeaderList = daoScannedPackingHeader.SelectScannedPackingHeader(scannedPackingHeader);
                                            json = JsonConvert.SerializeObject(scannedPackingHeaderList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "ScannedPackingLine":
                                            ScannedPackingLine scannedPackingLine = JsonConvert.DeserializeObject<ScannedPackingLine>(commuForm.Str.Remove(0, 1));
                                            List<ScannedPackingLine> scannedPackingLineList = daoScannedPackingLine.SelectScannedPackingLine(scannedPackingLine);
                                            json = JsonConvert.SerializeObject(scannedPackingLineList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "ScannedPackingMapping":
                                            ScannedPackingMapping scannedPackingMapping = JsonConvert.DeserializeObject<ScannedPackingMapping>(commuForm.Str.Remove(0, 1));
                                            List<ScannedPackingMapping> scannedPackingMappingList = daoScannedPackingMapping.SelectScannedPackingMapping(scannedPackingMapping);
                                            json = JsonConvert.SerializeObject(scannedPackingMappingList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "ClosedPrescan":
                                            ClosedPrescan closedPrescan = JsonConvert.DeserializeObject<ClosedPrescan>(commuForm.Str.Remove(0, 1));
                                            List<ClosedPrescan> closedPrescanList = daoClosedPrescan.SelectClosedPrescan(closedPrescan);
                                            json = JsonConvert.SerializeObject(closedPrescanList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "ClosedPrescanOuterCarton":
                                            ClosedPrescanOuterCarton closedPrescanOuterCarton = JsonConvert.DeserializeObject<ClosedPrescanOuterCarton>(commuForm.Str.Remove(0, 1));
                                            List<ClosedPrescanOuterCarton> closedPrescanOuterCartonList = daoClosedPrescanOuterCarton.SelectClosedPrescanOuterCarton(closedPrescanOuterCarton);
                                            json = JsonConvert.SerializeObject(closedPrescanOuterCartonList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                        case "ClosedPrescanInnerCarton":
                                            ClosedPrescanInnerCarton closedPrescanInnerCarton = JsonConvert.DeserializeObject<ClosedPrescanInnerCarton>(commuForm.Str.Remove(0, 1));
                                            List<ClosedPrescanInnerCarton> closedPrescanInnerCartonList = daoClosedPrescanInnerCarton.SelectClosedPrescanInnerCarton(closedPrescanInnerCarton);
                                            json = JsonConvert.SerializeObject(closedPrescanInnerCartonList, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                    }
                                    break;
                                case "Update":
                                    switch (commuForm.Table)
                                    {
                                        case "User":
                                            List<User> users = JsonConvert.DeserializeObject<List<User>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoUser.Update(users[0].UserID, users[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(users[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "User");
                                            break;
                                        case "Item":
                                            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoItem.Update(items[0].No, items[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(items[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "Item");
                                            break;
                                        case "CustomerGroup":
                                            List<CustomerGroup> customerGroups = JsonConvert.DeserializeObject<List<CustomerGroup>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoCustomerGroup.Update(customerGroups[0].Code, customerGroups[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(customerGroups[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "CustomerGroup");
                                            break;
                                        case "Printer":
                                            List<Printer> printers = JsonConvert.DeserializeObject<List<Printer>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoPrinter.Update(printers[0].Code, printers[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(printers[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "Printer");
                                            break;
                                        case "PackingHeader":
                                            break;
                                        case "PackingLine":
                                            break;
                                        case "Mapping":
                                            List<Mapping> mappings = JsonConvert.DeserializeObject<List<Mapping>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoMapping.Update(mappings[0].No, mappings[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(mappings[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "Mapping");
                                            break;
                                        case "LabelHeader":
                                            List<LabelHeader> labelHeaders = JsonConvert.DeserializeObject<List<LabelHeader>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoLabelHeader.Update(labelHeaders[0].Code, labelHeaders[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(labelHeaders[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "LabelHeader");
                                            break;
                                        case "LabelLine":
                                            List<LabelLine> labelLines = JsonConvert.DeserializeObject<List<LabelLine>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoLabelLine.Update(labelLines[0].Code, labelLines[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(labelLines[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "LabelLine");
                                            break;
                                        case "ScanLabelString":
                                            List<ScanLabelString> scanLabelStrings = JsonConvert.DeserializeObject<List<ScanLabelString>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoScanLabelString.Update(scanLabelStrings[0].EntryNo, scanLabelStrings[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(scanLabelStrings[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "ScanLabelString");
                                            break;
                                        case "OuterCarton":
                                            List<OuterCarton> outerCartons = JsonConvert.DeserializeObject<List<OuterCarton>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoOuterCarton.UpdateOuterCarton(outerCartons[0], outerCartons[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(outerCartons[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "OuterCarton");
                                            break;
                                        case "InnerCarton":
                                            List<InnerCarton> innerCartons = JsonConvert.DeserializeObject<List<InnerCarton>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoInnerCarton.UpdateInnerCarton(innerCartons[0], innerCartons[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(innerCartons[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "InnerCarton");
                                            break;
                                        case "Prescan":
                                            List<Prescan> prescans = JsonConvert.DeserializeObject<List<Prescan>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoPrescan.Update(prescans[0], prescans[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(prescans[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "Prescan");
                                            break;
                                        case "PrescanOuterCarton":
                                            List<PrescanOuterCarton> prescanOuterCartons = JsonConvert.DeserializeObject<List<PrescanOuterCarton>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoPrescanOuterCarton.Update(prescanOuterCartons[0], prescanOuterCartons[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(prescanOuterCartons[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "PrescanOuterCarton");
                                            break;
                                        case "PrescanInnerCarton":
                                            List<PrescanInnerCarton> prescaninnerCartons = JsonConvert.DeserializeObject<List<PrescanInnerCarton>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoPrescanInnerCarton.Update(prescaninnerCartons[0], prescaninnerCartons[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(prescaninnerCartons[1], new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "PrescanInnerCarton");
                                            break;
                                        case "PackingMapping":
                                            List<PackingMapping> packingMappings = JsonConvert.DeserializeObject<List<PackingMapping>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoPackingMapping.UpdatePackingMapping(packingMappings[0], packingMappings[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(packingMappings, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "PackingMapping");
                                            break;
                                        case "DirectPrint":
                                            List<DirectPrint> directPrints = JsonConvert.DeserializeObject<List<DirectPrint>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoDirectPrint.UpdateDirectPrint(directPrints[0], directPrints[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(directPrints, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "DirectPrint");
                                            break;
                                        case "DirectPrintOuterCarton":
                                            List<DirectPrintOuterCarton> directPrintOuterCartons = JsonConvert.DeserializeObject<List<DirectPrintOuterCarton>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoDirectPrintOuterCarton.UpdateDirectPrintOuterCarton(directPrintOuterCartons[0], directPrintOuterCartons[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(directPrintOuterCartons, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "DirectPrintOuterCarton");
                                            break;
                                        case "DirectPrintInnerCarton":
                                            List<DirectPrintInnerCarton> directPrintInnerCartonds = JsonConvert.DeserializeObject<List<DirectPrintInnerCarton>>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoDirectPrintInnerCarton.UpdateDirectPrintInnerCarton(directPrintInnerCartonds[0], directPrintInnerCartonds[1]);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(directPrintInnerCartonds, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "DirectPrintInnerCarton");
                                            break;
                                        case "ScannedPackingHeader":
                                            break;
                                        case "ScannedPackingLine":
                                            break;
                                        case "ScannedPackingMapping":
                                            break;
                                        case "ClosedPrescan":
                                            break;
                                        case "ClosedPrescanOuterCarton":
                                            break;
                                        case "ClosedPrescanInnerCarton":
                                            break;
                                    }
                                    break;
                                case "Insert":
                                    switch (commuForm.Table)
                                    {
                                        case "User":
                                            User user = JsonConvert.DeserializeObject<User>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoUser.Insert(user);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(user, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Insert", "User");
                                            break;
                                        case "CustomerGroup":
                                            CustomerGroup customerGroup = JsonConvert.DeserializeObject<CustomerGroup>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoCustomerGroup.Insert(customerGroup);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(customerGroup, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Insert", "CustomerGroup");
                                            break;
                                        case "Printer":
                                            Printer printer = JsonConvert.DeserializeObject<Printer>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoPrinter.Insert(printer);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(printer, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Insert", "Printer");
                                            break;
                                        case "PackingHeader":
                                            break;
                                        case "PackingLine":
                                            break;
                                        case "Item":
                                            Item item = JsonConvert.DeserializeObject<Item>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoItem.Insert(item);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(item, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "Item");
                                            break;
                                        case "Mapping":
                                            Mapping mapping = JsonConvert.DeserializeObject<Mapping>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoMapping.Insert(mapping);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(mapping, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Insert", "Mapping");
                                            break;
                                        case "LabelHeader":
                                            LabelHeader labelHeader = JsonConvert.DeserializeObject<LabelHeader>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoLabelHeader.Insert(labelHeader);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(labelHeader, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Insert", "LabelHeader");
                                            break;
                                        case "LabelLine":
                                            LabelLine labelLine = JsonConvert.DeserializeObject<LabelLine>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoLabelLine.Insert(labelLine);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(labelLine, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Insert", "LabelLine");
                                            break;
                                        case "ScanLabelString":
                                            break;
                                        case "OuterCarton":
                                            break;
                                        case "InnerCarton":
                                            break;
                                        case "Prescan":
                                            break;
                                        case "PrescanOuterCarton":
                                            break;
                                        case "PrescanInnerCarton":
                                            break;
                                        case "PackingMapping":
                                            break;
                                        case "DirectPrint":
                                            break;
                                        case "DirectPrintOuterCarton":
                                            break;
                                        case "DirectPrintInnerCarton":
                                            break;
                                        case "ScannedPackingHeader":
                                            break;
                                        case "ScannedPackingLine":
                                            break;
                                        case "ScannedPackingMapping":
                                            break;
                                        case "ClosedPrescan":
                                            break;
                                        case "ClosedPrescanOuterCarton":
                                            break;
                                        case "ClosedPrescanInnerCarton":
                                            break;
                                    }
                                    break;
                                case "Delete":
                                    switch (commuForm.Table)
                                    {
                                        case "User":
                                            User user = JsonConvert.DeserializeObject<User>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoUser.Delete(user);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(user, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Delete", "User");
                                            break;
                                        case "CustomerGroup":
                                            CustomerGroup customerGroup = JsonConvert.DeserializeObject<CustomerGroup>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoCustomerGroup.Delete(customerGroup);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(customerGroup, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Insert", "CustomerGroup");
                                            break;
                                        case "Printer":
                                            Printer printer = JsonConvert.DeserializeObject<Printer>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoPrinter.DeletePrinter(printer);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(printer, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Insert", "Printer");
                                            break;
                                        case "PackingHeader":
                                            break;
                                        case "PackingLine":
                                            break;
                                        case "Item":
                                            Item item = JsonConvert.DeserializeObject<Item>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoItem.DeleteItem(item);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(item, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Update", "Item");
                                            break;
                                        case "Mapping":
                                            Mapping mapping = JsonConvert.DeserializeObject<Mapping>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoMapping.Delete(mapping);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(mapping, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Insert", "Mapping");
                                            break;
                                        case "LabelHeader":
                                            LabelHeader labelHeader = JsonConvert.DeserializeObject<LabelHeader>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoLabelHeader.Delete(labelHeader);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(labelHeader, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Delete", "LabelHeader");
                                            break;
                                        case "LabelLine":
                                            LabelLine labelLine = JsonConvert.DeserializeObject<LabelLine>(commuForm.Str.Remove(0, 1));
                                            effectedRows = daoLabelLine.Delete(labelLine);
                                            SendMesageToConnectedAndroid(JsonConvert.SerializeObject(labelLine, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }), socketClient, "Delete", "LabelLine");
                                            break;
                                        case "ScanLabelString":
                                            break;
                                        case "OuterCarton":
                                            break;
                                        case "InnerCarton":
                                            break;
                                        case "Prescan":
                                            break;
                                        case "PrescanOuterCarton":
                                            break;
                                        case "PrescanInnerCarton":
                                            break;
                                        case "PackingMapping":
                                            break;
                                        case "DirectPrint":
                                            break;
                                        case "DirectPrintOuterCarton":
                                            break;
                                        case "DirectPrintInnerCarton":
                                            break;
                                        case "ScannedPackingHeader":
                                            break;
                                        case "ScannedPackingLine":
                                            break;
                                        case "ScannedPackingMapping":
                                            break;
                                        case "ClosedPrescan":
                                            break;
                                        case "ClosedPrescanOuterCarton":
                                            break;
                                        case "ClosedPrescanInnerCarton":
                                            break;
                                    }
                                    break;
                                case "Login":
                                    switch (commuForm.Table)
                                    {
                                        case "User":
                                            User loginUser = JsonConvert.DeserializeObject<User>(commuForm.Str.Remove(0, 1));
                                            dt = dao.GetUser(loginUser.UserID);
                                            String loginmsg = "";
                                            if (dt.Rows.Count > 0)
                                            {
                                                dt.TableName = "USER";
                                                List<User> userlist = new List<User>();
                                                userlist = (from DataRow dr in dt.Rows
                                                            select new User()
                                                            {
                                                                UserID = dr["User ID"].ToString(),
                                                                Password = dr["Password"].ToString()
                                                            }).ToList();
                                                foreach (User user in userlist)
                                                {
                                                    if (loginUser.Password == user.Password)
                                                    {
                                                        loginmsg = "OK";
                                                    }
                                                    else
                                                    {
                                                        loginmsg = "Incorrect Password";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                loginmsg = "User does not exist";
                                            }
                                            json = JsonConvert.SerializeObject(loginmsg, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
                                            sw.WriteLine(json);
                                            break;
                                    }
                                    break;
                            }
                            break;

                    }

                    //可在这里指定接受数据格式
                    if (socketClient.LingerState == null)
                    {
                        socketClient.Shutdown(SocketShutdown.Both);
                        socketClient.Close();
                        socketClient = null;
                        break;
                    }

                    /*
                    byte[] buffer = new byte[1024];
                    int n = socketClient.Receive(buffer);
                    //string msg = Encoding.Default.GetString(buffer, 0, n);
                    string msg = Encoding.UTF8.GetString(buffer, 0, n);
                    showmsg(socketClient.RemoteEndPoint.ToString()  + "\r\n" + ":[" + socketClient.Connected + "]");
                    
                    //可在这里指定接受数据格式
                    if (socketClient.LingerState == null)
                    {
                        socketClient.Shutdown(SocketShutdown.Both);
                        socketClient.Close();
                        socketClient = null;
                        break;
                    */
                }
                catch (SocketException _e)
                {
                    showmsg("Disconnected: error code {0}!" + _e.NativeErrorCode);
                    showmsg("收到來自 --> " + socketClient.RemoteEndPoint.ToString() + " 的指令，但執行失敗\r\n");
                    RemoveListBox(socketClient.RemoteEndPoint.ToString());
                    socketClient.Shutdown(SocketShutdown.Both);
                    socketClient.Close();
                    socketClient = null;

                    break;
                }
                catch (Exception _e1)
                {
                    showmsg("Disconnected: error code {0}!" + _e1.Message);
                    RemoveListBox(socketClient.RemoteEndPoint.ToString());
                    socketClient.Shutdown(SocketShutdown.Both);
                    socketClient.Close();
                    socketClient = null;

                    break;
                }
                catch
                {
                    //防止錯誤
                    break;
                }
            }
        }

    }
}
