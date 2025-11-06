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

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class MCUResultEditCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] localParam = param.Split('|');
                hdnVisitID.Value = localParam[0].ToString();
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(String.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                hdnGCGender.Value = entity.GCGender;
                txtRegistrationNo.Text = entity.RegistrationNo;
                txtPatientName.Text = entity.PatientName;
                SetControlProperties();
                BindGridView();

            }
        }

        private void SetControlProperties()
        {
            string filterExpression = "";

            if (hdnGCGender.Value == Constant.Gender.FEMALE)
            {
                filterExpression = string.Format("StandardCodeID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}') AND IsDeleted = 0 ORDER BY CONVERT(INT,TagProperty) ASC",
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_1, //0
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_2, //1
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_3, //2
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_4, //3
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_5, //4
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_6, //5
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_7, //6
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_8, //7
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_9, //8
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_10, //9
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_11, //10
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_12, //11
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_14, //12
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_15, //13
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_16, //14
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_17, //15
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_18, //16
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_19, //17
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_20, //18
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_21, //19
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_22, //20
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_23, //21
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_24 //22
                                                    );
            }
            else
            {
                filterExpression = string.Format("StandardCodeID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}') AND IsDeleted = 0 ORDER BY CONVERT(INT,TagProperty) ASC",
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_1, //0
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_2, //1
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_3, //2
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_4, //3
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_5, //4
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_6, //5
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_7, //6
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_8, //7
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_9, //8
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_10, //9
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_11, //10
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_12, //11
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_14, //12
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_15, //13
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_16, //14
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_17, //15
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_18, //16
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_19, //17
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_20, //18
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_21, //19
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_22, //20
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_23, //21
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_24 //22
                                                     );
            }

            List<StandardCode> listSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboTestResultSearchCode, listSc, "StandardCodeName", "StandardCodeID");
            cboTestResultSearchCode.SelectedIndex = 0;
        }

        private void BindGridView()
        {
            string filterExpression = String.Format("VisitID = {0}", hdnVisitID.Value);
            if (cboTestResultSearchCode.Value != null)
            {
                if (cboTestResultSearchCode.Value.ToString() != "")
                {
                    filterExpression += string.Format(" AND ParentID = '{0}'", cboTestResultSearchCode.Value.ToString());
                }
            }

            List<vMCUResult> lstEntity = BusinessLayer.GetvMCUResultList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpChangeResultCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnMCUResultID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                //else
                //{
                //    if (OnSaveAddRecord(ref errMessage))
                //        result += "success";
                //    else
                //        result += string.Format("fail|{0}", errMessage);
                //}
            }
            else if (e.Parameter == "refresh")
            {
                result += "success";
                BindGridView();
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

        private void ControlToEntity(MCUResult entity)
        {
            entity.Result = chkIsSelected.Checked;
            entity.Remarks = txtRemarks.Text;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                MCUResultDao entityDao = new MCUResultDao(ctx);
                MCUResult entity = BusinessLayer.GetMCUResult(Convert.ToInt32(hdnMCUResultID.Value));
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                ControlToEntity(entity);
                entityDao.Update(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                MCUResultDao entityDao = new MCUResultDao(ctx);
                MCUResult entity = BusinessLayer.GetMCUResult(Convert.ToInt32(hdnMCUResultID.Value));
                entityDao.Delete(entity.ID);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            return result;
        }
    }
}