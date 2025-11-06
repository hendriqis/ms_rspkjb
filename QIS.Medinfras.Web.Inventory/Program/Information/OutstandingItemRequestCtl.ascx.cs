using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program.Information
{
    public partial class OutstandingItemRequestCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] temp = param.Split('|');
                hdnItemID.Value = temp[0];
                hdnLocationID.Value = temp[1];
                //vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
                //ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", param)).FirstOrDefault();
                
                BindGridView();
            }
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvItemRequestDtList(string.Format("ItemID = {0} AND FromLocationID = {2} AND IsDeleted = 0 AND GCItemDetailStatus <> '{1}' AND GCTransactionStatus <> '{1}'", hdnItemID.Value, Constant.TransactionStatus.VOID, hdnLocationID.Value));
            grdView.DataBind();
        }
    }
}