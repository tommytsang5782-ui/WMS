using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient
{
    class KeyFields
    {
        public string[] KeyField_PrescanOuterCarton()
        {
            string[] keyFields = { "DocumentNo", "LineNo" };
            return keyFields;
        }
    }
}
