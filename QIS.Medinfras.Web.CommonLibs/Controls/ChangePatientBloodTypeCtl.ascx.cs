using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangePatientBloodTypeCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            SetControlProperties();

          
        }
        private void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}') AND IsActive = 1 AND IsDeleted = 0",
                                                                            Constant.StandardCode.TITLE, //0
                                                                            Constant.StandardCode.SALUTATION, //1
                                                                            Constant.StandardCode.SUFFIX, //2
                                                                            Constant.StandardCode.BLOOD_TYPE, //3
                                                                            Constant.StandardCode.IDENTITY_NUMBERY_TYPE, //4
                                                                            Constant.StandardCode.GENDER, //5
                                                                            Constant.StandardCode.CUSTOMER_TYPE, //6
                                                                            Constant.StandardCode.VIP_PATIENT_GROUP, //7
                                                                            Constant.StandardCode.PATIENT_BLACKLIST_REASON, //8
                                                                            Constant.StandardCode.COMINICATION, //9
                                                                            Constant.StandardCode.PHYSICAL_LIMITATION_TYPE //10
                                                                        ));


            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboBloodType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.BLOOD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            List<Variable> lstBloodRhesus = new List<Variable>();
            lstBloodRhesus.Add(new Variable { Code = "+", Value = "+" });
            lstBloodRhesus.Add(new Variable { Code = "-", Value = "-" });
            Methods.SetComboBoxField<Variable>(cboBloodRhesus, lstBloodRhesus, "Value", "Code");



            int mrn = AppSession.RegisteredPatient.MRN;
            Patient oPatient = BusinessLayer.GetPatient(mrn);
            if (oPatient != null)
            {
                txtMedicalNo.Text = oPatient.MedicalNo;
                txtPatientName.Text = oPatient.FullName;
                cboBloodRhesus.Value = oPatient.BloodRhesus;
                cboBloodType.Value = oPatient.GCBloodType;
            }
             

        }
        protected void cbpPatientBlood_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "update")
            {
                if (OnSaveUpdateRecord(ref errMessage))
                {
                    result += string.Format("success");
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
                
            }

            panel.JSProperties["cpResult"] = result;
        }

        public bool OnSaveUpdateRecord(ref string errMessage)
        {

            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao entityDao = new PatientDao(ctx);
            try {

                int mrn = AppSession.RegisteredPatient.MRN;
                Patient oPatient = BusinessLayer.GetPatient(mrn);
                if (oPatient != null)
                {
                    oPatient.BloodRhesus = cboBloodRhesus.Value.ToString();
                    oPatient.GCBloodType = cboBloodType.Value.ToString();
                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(oPatient);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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