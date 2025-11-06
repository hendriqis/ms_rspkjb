using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicationScheduleInfoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnDispensaryServiceUnitID.Value = paramInfo[0];

            SetControlProperties();

            if (paramInfo.Length>1)
            {
                hdnSelectedID.Value = paramInfo[1];
                int scheduleID = Convert.ToInt32(paramInfo[1]);
                string filterExp = string.Format("ID = {0}",scheduleID);
                vMedicationSchedule oSchedule = BusinessLayer.GetvMedicationScheduleList(filterExp).FirstOrDefault();
                if (oSchedule != null)
                {
                    txtMedicationDate.Text = oSchedule.MedicationDateInString;
                    txtSequenceNo.Text = oSchedule.SequenceNo;
                    txtItemName.Text = oSchedule.DrugName;
                    txtMedicationTime.Text = oSchedule.MedicationTime;
                    txtProceedTime.Text = oSchedule.ProceedTime;
                    txtMedicationStatus.Text = oSchedule.MedicationStatus;
                    if (oSchedule.GCMedicationStatus != Constant.MedicationStatus.DISCONTINUE)
                    {                        
                        txtParamedicName.Text = oSchedule.ParamedicName;
                        txtOtherMedicationStatus.Text = oSchedule.OtherMedicationStatus;

                        if (oSchedule.IsPharmacistVerified)
                        {
                            txtVerifiedPharmacistDateTime.Text = oSchedule.VerifiedPharmacistDateTime.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                            txtVerifiedPharmacistName.Text = oSchedule.VerifiedPharmacistName;
                        }

                        if (oSchedule.IsPhysicianVerified)
                        {
                            txtVerifiedPhysicianDateTime.Text = oSchedule.VerifiedPhysicianDateTime.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                            txtVerifiedPhysicianName.Text = oSchedule.VerifiedPhysicianName;
                        }
                    }
                    else
                    {
                        StringBuilder remarks = new StringBuilder();
                        if (string.IsNullOrEmpty(oSchedule.OtherDiscontinueReason))
                            remarks.AppendLine(string.Format("{0}",oSchedule.DiscontinueReasonType));
                        else
                            remarks.AppendLine(string.Format("{0} ({1})", oSchedule.DiscontinueReasonType, oSchedule.OtherDiscontinueReason));
                        remarks.AppendLine(string.Format("{0}, Discontinued Date : {1}", oSchedule.DiscontinueByUserName, oSchedule.DiscontinueDate.ToString(Constant.FormatString.DATE_FORMAT)));
                        
                        txtParamedicName.Text = oSchedule.DiscontinuePhysician;
                        txtOtherMedicationStatus.Text = remarks.ToString();

                        if (oSchedule.IsPharmacistVerified)
                        {
                            txtVerifiedPharmacistDateTime.Text = oSchedule.VerifiedPharmacistDateTime.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                            txtVerifiedPharmacistName.Text = oSchedule.VerifiedPharmacistName;
                        }

                        if (oSchedule.IsPhysicianVerified)
                        {
                            txtVerifiedPhysicianDateTime.Text = oSchedule.VerifiedPhysicianDateTime.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                            txtVerifiedPhysicianName.Text = oSchedule.VerifiedPhysicianName;
                        }
                    }

                    if (oSchedule.IsNeedConfirmation)
                    {
                        chkIsNeedConfirmation.Checked = oSchedule.IsNeedConfirmation;
                        txtParamedicName2.Text = oSchedule.NeedConfirmationParamedicName;

                        if (oSchedule.IsConfirmed)
                        {
                            txtConfirmationDateTime.Text = oSchedule.NeedConfirmationDateTime.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                        }
                    }

                    rblIsPatientFamily.SelectedValue = oSchedule.IsPatientFamily ? "1" : "0";
                    if (oSchedule.IsPatientFamily)
                    {
                        trFamilyInfo.Style.Remove("display");
                        txtPatientFamilyName.Text = oSchedule.PatientFamilyName;
                        txtFamilyRelation.Text = oSchedule.FamilyRelation;
                    }
                }
            }
        }

        private void SetControlProperties()
        {
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //if (e.Item.ItemType == ListViewItemType.DataItem)
            //{
            //    CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsProcessItem");
            //    //HtmlTextInput chkIsSelected = (CheckBox)e.Item.FindControl("chkIsProcessItem");
            //    vMedicationSchedule item = (vMedicationSchedule)e.Item.DataItem;
            //    if (item.GCMedicationStatus != Constant.MedicationStatus.OPEN)
            //    {
            //        chkIsSelected.Visible = false;
            //    }
            //}
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}