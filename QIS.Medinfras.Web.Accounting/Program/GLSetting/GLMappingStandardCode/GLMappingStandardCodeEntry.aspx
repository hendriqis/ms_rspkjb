<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="GLMappingStandardCodeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLMappingStandardCodeEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        //#region COA
        function onGetGLAccountFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblCOA.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                $('#<%=txtGLAccountNo.ClientID %>').val(value);
                ontxtGLAccountNoChanged(value);
            });
        });

        $('#<%=txtGLAccountNo.ClientID %>').live('change', function () {
            ontxtGLAccountNoChanged($(this).val());
        });

        function ontxtGLAccountNoChanged(value) {
            var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);
                }
                else {
                    $('#<%=hdnGLAccountID.ClientID %>').val('');
                    $('#<%=txtGLAccountNo.ClientID %>').val('');
                    $('#<%=txtGLAccountName.ClientID %>').val('');
                }
            });
        }
        //#endregion

    </script>
    <input type="hidden" id="hdnStandardCodeID" runat="server" value="" />
    <input type="hidden" id="hdnGLAccountID_ORI" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Standard Code ID")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStandardCodeID" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Standard Code Name")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStandardCodeName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trCOA" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblCOA">
                                <%=GetLabel("COA")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccountID" runat="server" value="" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtGLAccountNo" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGLAccountName" Width="99%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>