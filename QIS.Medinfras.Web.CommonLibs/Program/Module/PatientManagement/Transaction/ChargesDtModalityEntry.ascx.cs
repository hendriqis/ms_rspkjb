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
    public partial class ChargesDtModalityEntry : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramSplit = param.Split('|');
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", paramSplit[1])).FirstOrDefault();
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;
                txtMRN.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                hdnVisitID.Value = entity.VisitID.ToString();
                hdnTransactionIDCtl.Value = paramSplit[0];
                txtTransactionNo.Text = paramSplit[2];
                txtTransactionNo.ReadOnly = true;

                PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionIDCtl.Value));
                if (entityHd != null)
                {
                    if (entityHd.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                    {
                        divContainerAddData.Style.Add("display", "none");
                    }
                }

                BindGridView(1, true, ref PageCount);
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
            txtItemCode.Attributes.Add("validationgroup", "mpModality");
            txtItemName.Attributes.Add("validationgroup", "mpModality");

            String filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEDICAL_IMAGING_MODALITIES);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            StandardCode sc = new StandardCode();
            sc.StandardCodeID = "";
            sc.StandardCodeName = "";
            lstStandardCode.Add(sc);
            Methods.SetComboBoxField<StandardCode>(cboModalities, lstStandardCode.OrderBy(t => t.StandardCodeID).ToList(), "StandardCodeName", "StandardCodeID");
            cboModalities.SelectedIndex = 0;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TransactionID = {0}", hdnTransactionIDCtl.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesDtModalityRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            grdView.DataSource = BusinessLayer.GetvPatientChargesDtModalityList(filterExpression, 8, pageIndex, "ItemName1 ASC");
            grdView.DataBind();
        }

        protected void cbpModalityCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            if (param[0] == "save")
            {
                result = "save|";
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                    {
                        BindGridView(1, true, ref pageCount);
                        result += "success";
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                    {
                        BindGridView(1, true, ref pageCount);
                        result += "success";
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            else if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "delete")
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                {
                    BindGridView(1, true, ref pageCount);
                    result += "success";
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }

            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PatientChargesDtModality entity)
        {
            entity.PatientChargesDtID = Convert.ToInt32(hdnChargesDtID.Value);
            if (!String.IsNullOrEmpty(hdnModalityID.Value))
            {
                entity.ModalityID = Convert.ToInt32(hdnModalityID.Value);
            }
            else
            {
                entity.ModalityID = null;
            }
            if (cboModalities.Value != null)
            {
                entity.GCModality = cboModalities.Value.ToString();
            }
            else
            {
                entity.GCModality = "";
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtModalityDao entityDao = new PatientChargesDtModalityDao(ctx);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnChargesDtID.Value));
                if (!entityDt.IsDeleted && entityDt.GCTransactionDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    PatientChargesHd entityHd = entityHdDao.Get(entityDt.TransactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterCheck = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", hdnChargesDtID.Value);
                        List<PatientChargesDtModality> lstCheck = BusinessLayer.GetPatientChargesDtModalityList(filterCheck, ctx);

                        PatientChargesDtModality entity = new PatientChargesDtModality();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        if (lstCheck.Where(t => t.GCModality == entity.GCModality && t.ModalityID == entity.ModalityID).Count() <= 0)
                        {
                            entityDao.Insert(entity);
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            errMessage = string.Format("Item Ini Sudah memiliki modality / modalities seperti ini.");
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                            result = false;
                        }
                    }
                    else
                    {
                        errMessage = string.Format("Item Ini Sudah diproses oleh user lain. Harap Refresh halaman ini");
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                        result = false;
                    }
                }
                else
                {
                    errMessage = string.Format("Item Ini Sudah diproses oleh user lain. Harap Refresh halaman ini");
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtModalityDao entityDao = new PatientChargesDtModalityDao(ctx);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnChargesDtID.Value));
                if (!entityDt.IsDeleted && entityDt.GCTransactionDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    PatientChargesHd entityHd = entityHdDao.Get(entityDt.TransactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterCheck = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", hdnChargesDtID.Value);
                        List<PatientChargesDtModality> lstCheck = BusinessLayer.GetPatientChargesDtModalityList(filterCheck, ctx);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PatientChargesDtModality entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                        if (!entity.IsDeleted)
                        {
                            ControlToEntity(entity);
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;


                            if (lstCheck.Where(t => t.GCModality == entity.GCModality && t.ModalityID == entity.ModalityID && t.ID != entity.ID).Count() <= 0)
                            {
                                entityDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                errMessage = string.Format("Item Ini Sudah memiliki modality / modalities seperti ini.");
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                                result = false;
                            }
                        }
                        else
                        {
                            errMessage = string.Format("Item Ini Sudah diproses oleh user lain. Harap Refresh halaman ini");
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                            result = false;
                        }
                    }
                    else
                    {
                        errMessage = string.Format("Item Ini Sudah diproses oleh user lain. Harap Refresh halaman ini");
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                        result = false;
                    }
                }
                else
                {
                    errMessage = string.Format("Item Ini Sudah diproses oleh user lain. Harap Refresh halaman ini");
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtModalityDao entityDao = new PatientChargesDtModalityDao(ctx);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnChargesDtID.Value));
                if (!entityDt.IsDeleted && entityDt.GCTransactionDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    PatientChargesHd entityHd = entityHdDao.Get(entityDt.TransactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PatientChargesDtModality entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = string.Format("Item Ini Sudah diproses oleh user lain. Harap Refresh halaman ini");
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                        result = false;
                    }
                }
                else
                {
                    errMessage = string.Format("Item Ini Sudah diproses oleh user lain. Harap Refresh halaman ini");
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}