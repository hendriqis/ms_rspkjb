using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;


namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class ChangeStatusMCUList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.CHANGE_STATUS_MCU;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID IN ('{0}','{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.CLOSED));
            Methods.SetComboBoxField<StandardCode>(cboMCUStatus, lst, "StandardCodeName", "StandardCodeID");
            cboMCUStatus.SelectedIndex = 0;

            txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                result = "refresh|" + PageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterList = string.Format("GCRegistrationStatus NOT IN ('{0}','{1}')", Constant.VisitStatus.CLOSED, Constant.VisitStatus.CANCELLED);

            filterList += string.Format(" AND GCMCUStatus = '{0}'", cboMCUStatus.Value.ToString());
            filterList += string.Format(" AND RegistrationDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            List<vRegistrationMCU1> lstEntity = BusinessLayer.GetvRegistrationMCU1List(filterList);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao regDao = new RegistrationDao(ctx);
            RegistrationInfoDao regInfoDao = new RegistrationInfoDao(ctx);

            try
            {
                if (hdnSelectedRegistrationID.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedRegistrationID.Value = hdnSelectedRegistrationID.Value.Substring(1);
                }

                string filterReg = string.Format("RegistrationID IN ({0})", hdnSelectedRegistrationID.Value);
                List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(filterReg, ctx);
                foreach (Registration reg in lstRegistration)
                {
                    RegistrationInfo regInfo = regInfoDao.Get(reg.RegistrationID);
                    if (type == "close")
                    {
                        if (regInfo != null)
                        {
                            if (regInfo.GCMCUStatus ==  "" || regInfo.GCMCUStatus == Constant.TransactionStatus.OPEN)
                            {
                                regInfo.GCMCUStatus = Constant.TransactionStatus.CLOSED;
                                regInfo.MCUClosedBy = AppSession.UserLogin.UserID;
                                regInfo.MCUClosedDateTime = DateTime.Now;
                                regInfoDao.Update(regInfo);
                            }
                            else
                            {
                                result = false;
                                errMessage = string.Format("Registrasi MCU di nomor {0} tidak dapat ditutup karena statusnya sudah CLOSED.", reg.RegistrationNo);
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                            }
                        }
                        else
                        {
                            RegistrationInfo regInfoNew = new RegistrationInfo();
                            regInfoNew.RegistrationID = reg.RegistrationID;
                            regInfoNew.GCMCUStatus = Constant.TransactionStatus.CLOSED;
                            regInfoNew.MCUClosedBy = AppSession.UserLogin.UserID;
                            regInfoNew.MCUClosedDateTime = DateTime.Now;
                            regInfoDao.Insert(regInfoNew);
                        }
                    }
                    else if (type == "reopen")
                    {
                        if (regInfo != null)
                        {
                            if (regInfo.GCMCUStatus == Constant.TransactionStatus.CLOSED)
                            {
                                regInfo.GCMCUStatus = Constant.TransactionStatus.OPEN;
                                regInfo.MCUReopenBy = AppSession.UserLogin.UserID;
                                regInfo.MCUReopenDateTime = DateTime.Now;
                                regInfoDao.Update(regInfo);
                            }
                            else
                            {
                                result = false;
                                errMessage = string.Format("Registrasi MCU di nomor {0} tidak dapat dibuka kembali karena statusnya sudah OPEN.", reg.RegistrationNo);
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                            }
                        }
                        else
                        {
                            RegistrationInfo regInfoNew = new RegistrationInfo();
                            regInfoNew.RegistrationID = reg.RegistrationID;
                            regInfoNew.GCMCUStatus = Constant.TransactionStatus.OPEN;
                            regInfoNew.MCUReopenBy = AppSession.UserLogin.UserID;
                            regInfoNew.MCUReopenDateTime = DateTime.Now;
                            regInfoDao.Insert(regInfoNew);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
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
    }
}