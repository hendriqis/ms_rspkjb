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
    public abstract class BasePagePatientOrderSatuSehatBelumIntegrasi : BasePagePatientOrder
    {
        public abstract string GetFilterExpressionSatuSehatBelumIntegrasi();
        public abstract string GetSortingSatuSehatBelumIntegrasi();
        public abstract void OnGrdRowClickSatuSehatBelumIntegrasi(string transactionNo, string TestOrderID, string VisitID);

    }
}
