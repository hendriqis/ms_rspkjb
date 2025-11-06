using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.EMR.Controls
{
    public partial class DeleteRecordCtl : BaseMPPopupEntry
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override Control GetPanelEntryPopup()
        {
            throw new NotImplementedException();
        }

        public override void SetIsAdd(bool IsAdd)
        {
            throw new NotImplementedException();
        }
    }
}