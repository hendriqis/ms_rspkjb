using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class PersonNameFormatList : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.PERSON_NAME;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            List<PersonNameConfiguration> lstEntity = BusinessLayer.GetPersonNameConfigurationList("IsActive = 1");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnProcessRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            List<PersonNameConfiguration> lstEntity = BusinessLayer.GetPersonNameConfigurationList("IsActive = 1");

            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                foreach (PersonNameConfiguration entity in lstEntity)
                {
                    string suffixColumn = entity.SuffixColumn.Substring(2);
                    string titleColumn = entity.TitleColumn.Substring(2);
                    ctx.CommandText = string.Format("SELECT {0},{1},{2},{3},{4} = dbo.fnGetStandardCodeName({5}),{6} = dbo.fnGetStandardCodeName({7}) FROM {8}",
                        entity.IDColumn, entity.FirstNameColumn, entity.MiddleNameColumn, entity.LastNameColumn, suffixColumn, entity.SuffixColumn, titleColumn, entity.TitleColumn, entity.TableName);
                    DataTable dataTable = DaoBase.GetDataTable(ctx);
                    for (int i = 0; i < dataTable.Rows.Count; ++i)
                    {
                        DataRow dataRow = dataTable.Rows[i];
                        string firstName = dataRow[entity.FirstNameColumn].ToString();
                        string middleName = dataRow[entity.MiddleNameColumn].ToString();
                        string lastName = dataRow[entity.LastNameColumn].ToString();
                        string suffix = dataRow[suffixColumn].ToString();
                        string title = dataRow[titleColumn].ToString();
                        string name = Helper.GenerateName(lastName, middleName, firstName);
                        string fullName = Helper.GenerateFullName(name, title, suffix);
                        int id = Convert.ToInt32(dataRow[entity.IDColumn]);

                        ctx.CommandText = string.Format("UPDATE {0} SET {1} = '{2}', {3} = '{4}' WHERE {5} = {6}", 
                            entity.TableName, entity.NameColumn, name, entity.FullNameColumn, fullName, entity.IDColumn, id);
                        DaoBase.ExecuteNonQuery(ctx);
                    }

                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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