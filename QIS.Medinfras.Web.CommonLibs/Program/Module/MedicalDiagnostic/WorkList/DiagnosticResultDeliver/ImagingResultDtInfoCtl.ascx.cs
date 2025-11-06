using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ImagingResultDtInfoCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnImagingResultIDCtl.Value = param;

            string filterResultHd = string.Format("ID = {0}", param);
            vImagingResultHd entity = BusinessLayer.GetvImagingResultHdList(filterResultHd).FirstOrDefault();
            EntityToControl(entity);
            
            BindGridView();
        }

        private void EntityToControl(vImagingResultHd entity)
        {
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtRegistrationDate.Text = entity.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtRegistrationTime.Text = entity.RegistrationTime;
            txtPatientInfo.Text = entity.cfPatientInfo;
            txtTransactionNo.Text = entity.ChargesTransactionNo;
            txtResultDate.Text = entity.ResultDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtResultTime.Text = entity.ResultTime;
            if (entity.TestOrderID != null && entity.TestOrderID != 0)
            {
                txtOrderNo.Text = entity.TestOrderNo;
                txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_FORMAT);
                txtOrderTime.Text = entity.TestOrderTime;
            }            
            txtNotes.Text = entity.Remarks;
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            List<vImagingResultDt> lstEntity = BusinessLayer.GetvImagingResultDtList(string.Format("ID = {0} AND IsDeleted = 0 ORDER BY ItemName", hdnImagingResultIDCtl.Value));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}