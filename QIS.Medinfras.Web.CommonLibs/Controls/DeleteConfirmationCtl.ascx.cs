using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class DeleteConfirmationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<StandardCode> lstDeleteReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboDeleteReason, lstDeleteReason, "StandardCodeName", "StandardCodeID");
            cboDeleteReason.SelectedIndex = 0;
        }
    }
}