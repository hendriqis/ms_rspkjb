using System;
using System.Collections.Generic;
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
    public partial class INCBGNaikKelas : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;
                txtMRN.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                hdnVisitID.Value = entity.VisitID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                BindGridView();
                SetControlProperties();
            } 
        }

        private void SetControlProperties()
        {
    
            txtNilaiHakKelas.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            
        }
        
        private void BindGridView()
        {
            
            string filter = string.Format("RegistrationID = '{0}' AND IsDeleted = 0 ORDER BY ID DESC", hdnRegistrationID.Value);
            grdVisitNotes.DataSource = BusinessLayer.GetvRegistrationJKNList(filter);
            grdVisitNotes.DataBind();
        }

        protected void cbpRegistrationJkn_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnID.Value != "" && Convert.ToInt32(hdnID.Value) > 0 )
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(RegistrationJKN entity)
        {
            entity.IsNilaiIur = chkIuran.Checked; 
            entity.NilaiHakKelas = Convert.ToDecimal(txtNilaiHakKelas.Text);
            entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            entity.ClassID = Convert.ToInt32(hdnClassID.Value);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                RegistrationJKN entity = new RegistrationJKN();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertRegistrationJKN(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                RegistrationJKN entity = BusinessLayer.GetRegistrationJKN(Convert.ToInt32(hdnID.Value));
                if (entity != null) {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateRegistrationJKN(entity);
                }
              
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                RegistrationJKN entity = BusinessLayer.GetRegistrationJKN(Convert.ToInt32(hdnID.Value));
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateRegistrationJKN(entity);
                }
                 
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }
    }
}