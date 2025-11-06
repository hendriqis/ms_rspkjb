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
    public abstract class BasePagePatientOrder2 : BasePagePatientOrder
    {
        public abstract string GetFilterExpressionCodification();
        public abstract string GetSortingCodification();
        public abstract void OnGrdRowClickCodification(string transactionNo, string TestOrderID, string VisitID);
    }
}
