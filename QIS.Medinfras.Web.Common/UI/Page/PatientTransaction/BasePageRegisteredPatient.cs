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
    public abstract class BasePageRegisteredPatient : BasePageContent
    {
        public abstract string GetFilterExpression();
        public abstract void LoadAllWords();
        public abstract void OnGrdRowClick(string transactionNo);
    }
}
