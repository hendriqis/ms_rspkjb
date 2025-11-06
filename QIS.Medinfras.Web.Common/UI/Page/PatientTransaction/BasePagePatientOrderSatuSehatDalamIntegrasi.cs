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
    public abstract class BasePagePatientOrderSatuSehatDalamIntegrasi : BasePagePatientOrderSatuSehatBelumIntegrasi
    {
        public abstract string GetFilterExpressionSatuSehatDalamIntegrasi();
        public abstract string GetSortingSatuSehatDalamIntegrasi();
        public abstract void OnGrdRowClickSatuSehatDalamIntegrasi(string transactionNo, string TestOrderID, string VisitID);
    }
}
