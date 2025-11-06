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
    public abstract class BasePagePatientOrderSatuSehatSudahIntegrasi : BasePagePatientOrderSatuSehatDalamIntegrasi
    {
        public abstract string GetFilterExpressionSatuSehatSudahIntegrasi();
        public abstract string GetSortingSatuSehatSudahIntegrasi();
        public abstract void OnGrdRowClickSatuSehatSudahIntegrasi(string transactionNo, string TestOrderID, string VisitID);
    }
}
