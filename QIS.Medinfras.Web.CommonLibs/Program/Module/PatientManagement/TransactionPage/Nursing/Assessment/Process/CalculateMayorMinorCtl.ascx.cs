using System;
using System.Collections.Generic;
using System.Data;
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
//using QIS.Medinfras.Common;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Linq;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CalculateMayorMinorCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTransactionIDCtl.Value = paramInfo[0];
            txtNursingTransactionNo.Text = paramInfo[1];
            hdnNursingDiagnoseIDCtl.Value = paramInfo[2];
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingTransactionHdDao entityHdDao = new NursingTransactionHdDao(ctx);
            NursingTransactionDtDao entityDtDao = new NursingTransactionDtDao(ctx);
            try
            {
                NursingTransactionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionIDCtl.Value));

                string filterDt = string.Format("TransactionID = '{0}'", hdnTransactionIDCtl.Value);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<NursingTransactionDt> lstDt = BusinessLayer.GetNursingTransactionDtList(filterDt, ctx);
                if (lstDt.Count > 0)
                {
                    string lstNursingDiagnoseItemID = "";
                    foreach (NursingTransactionDt e in lstDt)
                    {
                        if (!String.IsNullOrEmpty(lstNursingDiagnoseItemID))
                        {
                            lstNursingDiagnoseItemID += string.Format(",{0}", e.NursingDiagnoseItemID);
                        }
                        else
                        {
                            lstNursingDiagnoseItemID = e.NursingDiagnoseItemID.ToString();
                        }
                    }

                    string filterDiagnoseItem = string.Format("NursingDiagnoseID = '{0}'  AND IsDeleted = 0 AND NursingDiagnoseItemID IN ({1}) AND IsSubjectiveObjectiveData = 1", hdnNursingDiagnoseIDCtl.Value, lstNursingDiagnoseItemID);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<vNursingDiagnoseItem> lstDiagnoseItem = BusinessLayer.GetvNursingDiagnoseItemList(filterDiagnoseItem, ctx);

                    List<vNursingDiagnoseItem> lstDiagnoseItemDistinct = lstDiagnoseItem.GroupBy(test => test.GCNursingEvaluation).Select(grp => grp.First()).ToList().OrderBy(x => x.GCNursingEvaluation).ToList();
                    foreach (vNursingDiagnoseItem e in lstDiagnoseItemDistinct)
                    {
                        Decimal jumlahMayor = lstDiagnoseItem.Where(t => t.IsMajorData && t.GCNursingEvaluation == e.GCNursingEvaluation).ToList().Count();
                        Decimal jumlahMinor = lstDiagnoseItem.Where(t => t.IsMinorData && t.GCNursingEvaluation == e.GCNursingEvaluation).ToList().Count();
                        Decimal total = jumlahMayor + jumlahMinor;

                        Decimal PersentaseMayor = 0;
                        Decimal PersentaseMinor = 0;

                        if (jumlahMayor > 0)
                        {
                            PersentaseMayor = ((jumlahMayor / total) * 100);
                        }

                        if (jumlahMinor > 0)
                        {
                            PersentaseMinor = ((jumlahMinor / total) * 100);
                        }

                        if (e.GCNursingEvaluation == Constant.NursingEvaluation.SUBJECTIVE)
                        {
                            entityHd.PercentageSubjectiveMayor = PersentaseMayor;
                            entityHd.PercentageSubjectiveMinor = PersentaseMinor;
                        }
                        else if (e.GCNursingEvaluation == Constant.NursingEvaluation.OBJECTIVE)
                        {
                            entityHd.PercentageObjectiveMayor = PersentaseMayor;
                            entityHd.PercentageObjectiveMinor = PersentaseMinor;
                        }
                    }

                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);
                    ctx.CommitTransaction();

                    retval = entityHd.TransactionID.ToString();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Tidak ada data yang bisa diproses.");
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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