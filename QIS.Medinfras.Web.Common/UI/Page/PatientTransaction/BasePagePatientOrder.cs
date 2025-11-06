using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml.Linq;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BasePagePatientOrder : BasePageRegisteredPatient
    {
        public abstract string GetFilterExpressionTestOrder();
        public abstract string GetSortingTestOrder();
        public abstract void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID);
    }
}
