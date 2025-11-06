using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class UpdateOrderStatusCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderID.Value = paramInfo[1];
            txtTransactionNo.Text = paramInfo[2];
            if (!string.IsNullOrEmpty(hdnPrescriptionOrderID.Value))
            {
                String filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PRESCRIPTION_TYPE);
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
                lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

                if (!AppSession.IsUsedInpatientPrescriptionTypeFilter)
                {
                    Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                }
                else
                {
                    if (!string.IsNullOrEmpty(AppSession.InpatientPrescriptionTypeFilter))
                    {
                        string[] prescriptionType = AppSession.InpatientPrescriptionTypeFilter.Split(',');
                        Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).Where(x => !prescriptionType.Contains(x.StandardCodeID)).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                    else
                    {
                        Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                }

                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                EntityToControl(entity);
            }
        }

        private void EntityToControl(PrescriptionOrderHd entity)
        {
            txtOrderDateTime.Text = string.Format("{0} {1}", entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), entity.PrescriptionTime);
            cboPrescriptionType.Value = entity.GCPrescriptionType;
            txtRemarks.Text = entity.Remarks;            
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);

            try
            {
                int orderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                string referenceNo = string.Empty;

                PrescriptionOrderHd orderHd = orderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                if (orderHdDao != null)
                {
                    orderHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                    orderHd.Remarks = txtRemarks.Text;
                    orderHdDao.Update(orderHd);
                }

                ctx.CommitTransaction();
                retval = hdnPrescriptionOrderID.Value;               
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = "0|" + errMessage;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
    }
}