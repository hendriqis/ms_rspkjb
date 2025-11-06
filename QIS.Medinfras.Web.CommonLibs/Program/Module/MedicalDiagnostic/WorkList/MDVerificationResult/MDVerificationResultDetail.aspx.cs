using System;
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
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MDVerificationResultDetail : BasePageTrx
    {
        private string[] lstSelectedMember = null;
        private string lstMember = "";

        public override string OnGetMenuCode()
        {
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                return Constant.MenuCode.Imaging.IMAGING_RESULT_VERIFICATION;
            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                return Constant.MenuCode.Radiotheraphy.RADIOTERAPHY_RESULT_VERIFICATION;
            else
                return Constant.MenuCode.MedicalDiagnostic.MD_RESULT_VERIFICATION;
        }

        protected override void InitializeDataControl()
        {
            string TransactionID;
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                TransactionID = param[0];
                hdnTransactionHdID.Value = TransactionID;
                hdnVisitID.Value = Convert.ToString(AppSession.RegisteredPatient.VisitID);

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnMDResultID.Value = BusinessLayer.GetImagingResultHdList(string.Format("ChargeTransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value)).FirstOrDefault().ID.ToString();
                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);

                List<ImagingResultDt> entityImagingDT = BusinessLayer.GetImagingResultDtList(string.Format("ID = {0} AND IsDeleted = 0", hdnMDResultID.Value));
                foreach (ImagingResultDt imagingDt in entityImagingDT)
                {
                    lstMember += imagingDt.ItemID;
                    lstMember += ",";
                }
                hdnSelectedMember.Value = lstMember;

                BindGridView();

                List<ImagingResultHd> entitytemp = BusinessLayer.GetImagingResultHdList(string.Format("ChargeTransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value));
                IsLoadFirstRecord = entitytemp.Count > 0;
                if (entitytemp.Count > 0)
                {
                    IsLoadFirstRecord = true;
                    EntityToControl(entitytemp[0]);
                }
                else
                {
                    IsLoadFirstRecord = false;
                }

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnID, new ControlEntrySetting(false, false, false, "0"));
        }

        private void EntityToControl(ImagingResultHd entity)
        {
            hdnID.Value = entity.ID.ToString();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientChargesDtImagingResult entity = e.Item.DataItem as vPatientChargesDtImagingResult;
                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(Convert.ToString(entity.ItemID)))
                    chkIsSelected.Checked = true;
                else
                    chkIsSelected.Checked = false;
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0 AND IsTestItem = 1 AND ResultGCTransactionStatus = '{1}'", hdnTransactionHdID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vPatientChargesDtImagingResult> lstEntity = BusinessLayer.GetvPatientChargesDtImagingResultList(filterExpression, int.MaxValue, 1, "ItemCode");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        #region Custom Process
        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ImagingResultHdDao imagingResultHdDAO = new ImagingResultHdDao(ctx);
            ImagingResultDtDao imagingResultDtDAO = new ImagingResultDtDao(ctx);
            try
            {
                ImagingResultHd imagingHd = imagingResultHdDAO.Get(Convert.ToInt32(hdnMDResultID.Value));

                if (type == "verified")
                {
                    #region Verified

                    string filterExpression = String.Format("ID = {0}", hdnMDResultID.Value);
                    List<ImagingResultDt> lstImagingResultDt = BusinessLayer.GetImagingResultDtList(filterExpression, ctx);
                    foreach (ListViewDataItem item in lvwView.Items)
                    {
                        CheckBox chkIsSelected = (CheckBox)item.FindControl("chkIsSelected");
                        HtmlInputHidden hdnItemID = (HtmlInputHidden)item.FindControl("keyField");
                        ImagingResultDt imagingDt = lstImagingResultDt.FirstOrDefault(p => p.ItemID == Convert.ToInt32(hdnItemID.Value));
                        if (imagingDt != null)
                        {
                            if (chkIsSelected.Checked)
                            {
                                if (!imagingDt.IsVerified)
                                {
                                    imagingDt.IsVerified = true;
                                    imagingDt.VerifiedBy = AppSession.UserLogin.UserID;
                                    imagingDt.VerifiedDate = DateTime.Now;
                                    imagingDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    imagingResultDtDAO.Update(imagingDt);
                                }
                            }
                            else
                            {
                                if (imagingDt.IsVerified)
                                {
                                    imagingDt.IsVerified = false;
                                    imagingDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    imagingResultDtDAO.Update(imagingDt);
                                }
                            }
                        }
                    }

                    int rowCount = BusinessLayer.GetImagingResultDtRowCount(string.Format("ID = {0} AND IsVerified = 0 AND IsDeleted = 0", hdnMDResultID.Value), ctx);
                    if (rowCount < 1)
                    {
                        imagingHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        imagingHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        imagingResultHdDAO.Update(imagingHd);
                    }

                    ctx.CommitTransaction();

                    SendResultToRISPacs(imagingHd.ChargeTransactionID);

                    #endregion
                }
                else if (type == "unverified")
                {
                    #region Unverified

                    List<ImagingResultDt> lstImagingResultDt = BusinessLayer.GetImagingResultDtList(string.Format("ID = {0} AND IsVerified = 1 AND IsDeleted = 0", imagingHd.ID), ctx);
                    foreach (ImagingResultDt imagingDt in lstImagingResultDt)
                    {
                        imagingDt.IsVerified = false;
                        imagingDt.VerifiedBy = null;
                        imagingDt.VerifiedDate = null;
                        imagingDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        imagingResultDtDAO.Update(imagingDt);
                    }

                    int rowCount = BusinessLayer.GetLaboratoryResultDtRowCount(string.Format("ID = {0} AND IsVerified = 1 AND IsDeleted = 0", imagingHd.ID), ctx);
                    if (rowCount < 1)
                    {
                        imagingHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        imagingHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        imagingResultHdDAO.Update(imagingHd);
                    }

                    ctx.CommitTransaction();

                    #endregion
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public void SendResultToRISPacs(int transactionID)
        {
            try
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                {
                    if (AppSession.IsBridgingToRIS)
                    {
                        string[] resultInfo = "0|Unknown Protocol".Split('|');
                        switch (AppSession.RIS_BRIDGING_PROTOCOL)
                        {
                            case Constant.RIS_Bridging_Protocol.WEB_API:
                                var result1 = SendResultToMedinfrasAPI(transactionID);
                                resultInfo = ((string)result1).Split('|');
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
        }

        public string SendResultToMedinfrasAPI(int transactionID)
        {
            string result = "1|";

            try
            {
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                RISService oService = new RISService();
                //string apiResult = oService.ADT_A01(AppSession.UserLogin.HealthcareID, entity, visitInfo, patientInfo);
                string apiResult = oService.SendResultToRIS_MedinfrasApi(transactionID);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[1];
                    entityAPILog.Response = apiResult;
                    entityAPILog.ErrorMessage = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    result = string.Format("{0}|{1}", "0", apiResultInfo[1]);

                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    entityAPILog.MessageText = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                result = string.Format("{0}|{1}", "0", ex.Message);
            }

            return result;
        }
    }
}