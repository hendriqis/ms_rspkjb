using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class MCUResultDetail : BasePageTrx
    {
        protected string GetPageTitle()
        {
            string filterMenu = string.Format("MenuCode = '{0}'", OnGetMenuCode());
            MenuMaster menu = BusinessLayer.GetMenuMasterList(filterMenu).FirstOrDefault();
            hdnPageTitle.Value = menu.MenuCaption;

            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.MCU_RESULT_EXTRENAL;
        }

        List<PatientChargesDt> lstEntityDt = new List<PatientChargesDt>();

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                string param = Page.Request.QueryString["id"];
                hdnVisitID.Value = param;
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnHealthcareServiceUnitID.Value = Convert.ToString(entity.HealthcareServiceUnitID);

                hdnParamedicID.Value = entity.ParamedicID.ToString();
                txtPhysicianCode.Text = entity.ParamedicCode;
                txtPhysicianName.Text = entity.ParamedicName;

                vMCUResult MCUResult = BusinessLayer.GetvMCUResultList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                if (MCUResult != null)
                {
                    if (MCUResult.ParamedicID != 0 || MCUResult.ParamedicID.ToString() != "0")
                    {
                        hdnParamedicID.Value = MCUResult.ParamedicID.ToString();
                    }

                    txtPhysicianCode.Text = MCUResult.ParamedicCode;
                    txtPhysicianName.Text = MCUResult.ParamedicName;
                }

                ctlPatientBanner.InitializePatientBanner(entity);

                string filterExpression = "";
                if (entity.GCGender == Constant.Gender.FEMALE)
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
                    filterExpression = string.Format("StandardCodeID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}') AND IsDeleted = 0 ORDER BY CONVERT(INT,TagProperty) ASC",
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_1, //0
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_2, //1
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_4, //2
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_5, //3
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_6, //4
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_7, //5
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_8, //6
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_9, //7
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_10, //8
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_11, //9
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_12, //10
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_14, //11
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_15, //12
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_16, //13
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_17, //14
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_18, //15
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_19, //16
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_20, //17
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_21, //18
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_22, //19
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_23, //20
                                                            Constant.StandardCode.EXTRENAL_MCU_RESULT_24 //21
                                                        );
                }

                List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
                rptHeader.DataSource = lstEntity;
                rptHeader.DataBind();

                hdnStandardCodeID.Value = lstEntity.FirstOrDefault().StandardCodeID;

                SetControlEntrySetting(txtActualVisitDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                SetControlEntrySetting(txtActualVisitTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

                BindGridView();
            }
        }

        private void BindGridView()
        {
            ConsultVisit cv = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitID.Value));
            txtActualVisitDate.Text = cv.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtActualVisitTime.Text = cv.ActualVisitTime;
            txtQueueNo.Text = Convert.ToString(cv.QueueNo);

            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", hdnStandardCodeID.Value);
            filterExpression += string.Format(" AND StandardCodeID NOT IN (SELECT GCTestResult FROM MCUResult WHERE VisitID = {0}) ORDER BY CONVERT(INT,TagProperty) ASC", hdnVisitID.Value);

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                StandardCode entity = e.Item.DataItem as StandardCode;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                HtmlInputHidden hdnKey = (HtmlInputHidden)e.Item.FindControl("hdnKey");
                hdnKey.Value = entity.StandardCodeID.ToString();
            }
        }

        private void SaveMCUResult(ref string result)
        {
            String[] listKey = hdnListKey.Value.Split('|');
            String[] listIsChecked = hdnListIsChecked.Value.Split('|');
            String[] listRemarks = hdnListRemarks.Value.Split('|');

            IDbContext ctx = DbFactory.Configure(true);
            MCUResultDao entityDao = new MCUResultDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisitDao visitDao = new ConsultVisitDao(ctx);

            try
            {
                if (!String.IsNullOrEmpty(hdnListKey.Value))
                {
                    for (int a = 0; a < listKey.Length; a++)
                    {
                        MCUResult entity = new MCUResult();
                        entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        entity.GCTestResult = listKey[a];
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        bool isTrue = true;
                        if (listIsChecked[a].ToString() == "0")
                        {
                            isTrue = false;
                        }

                        entity.Result = isTrue;
                        entity.Remarks = listRemarks[a];

                        if (!String.IsNullOrEmpty(hdnParamedicID.Value) || hdnParamedicID.Value != "0")
                        {
                            entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                        }
                        else
                        {
                            entity.ParamedicID = null;
                        }

                        entityDao.Insert(entity);
                    }
                }

                ConsultVisit visit = visitDao.Get(Convert.ToInt32(hdnVisitID.Value));
                visit.ActualVisitDate = Helper.GetDatePickerValue(txtActualVisitDate);
                visit.ActualVisitTime = txtActualVisitTime.Text;
                visit.QueueNo = Convert.ToInt16(txtQueueNo.Text);
                if (!String.IsNullOrEmpty(hdnParamedicID.Value) || hdnParamedicID.Value != "0")
                {
                    visit.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                }
                visitDao.Update(visit);

                //**Ditutup Oleh RA 20200909
                //* Karena LastUpdatedDate selalu ketimpa setelah proses insert.

                //String filterExpression = String.Format("VisitID = {0}", visit.VisitID);
                //List<MCUResult> lstMcuResult = BusinessLayer.GetMCUResultList(filterExpression, ctx);
                //foreach (MCUResult e in lstMcuResult)
                //{
                //    if (!String.IsNullOrEmpty(hdnParamedicID.Value))
                //    {
                //        e.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                //        entityDao.Update(e);
                //    }
                //    else {
                //        e.ParamedicID = null;
                //        entityDao.Update(e);
                //    }
                //}

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                result = string.Format("fail|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changeTab" || param[0] == "refresh")
                {
                    BindGridView();
                    result = "success|refresh";
                }
                else if (param[0] == "save")
                {
                    result = "success|save";
                    SaveMCUResult(ref result);
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}