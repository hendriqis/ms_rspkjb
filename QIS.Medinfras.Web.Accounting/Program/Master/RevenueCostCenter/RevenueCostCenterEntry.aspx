<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="RevenueCostCenterEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.RevenueCostCenterEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Revenue Cost Center Parent
            function onGetParentFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblRevenueCostCenterParentID.lblLink').click(function () {
                openSearchDialog('revenuecostcenter', onGetParentFilterExpression(), function (value) {
                    $('#<%=txtRevenueCostCenterParentCode.ClientID %>').val(value);
                    onTxtRevenueCostCenterParentChanged(value);
                });
            });

            $('#<%=txtRevenueCostCenterParentCode.ClientID %>').change(function () {
                onTxtRevenueCostCenterParentChanged($(this).val());
            });

            function onTxtRevenueCostCenterParentChanged(value) {
                var filterExpression = onGetParentFilterExpression() + " AND RevenueCostCenterCode = '" + value + "'";
                Methods.getObject('GetRevenueCostCenterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnRevenueCostCenterParentID.ClientID %>').val(result.RevenueCostCenterID);
                        $('#<%=txtRevenueCostCenterParentName.ClientID %>').val(result.RevenueCostCenterName);
                    }
                    else {
                        $('#<%=hdnRevenueCostCenterParentID.ClientID %>').val('');
                        $('#<%=txtRevenueCostCenterParentCode.ClientID %>').val('');
                        $('#<%=txtRevenueCostCenterParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Kelompok Rincian Transaksi")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:60%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Revenue Cost Center")%></label></td>
                        <td><asp:TextBox ID="txtRevenueCostCenterCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Revenue Cost Center")%></label></td>
                        <td><asp:TextBox ID="txtRevenueCostCenterName" Width="406px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblRevenueCostCenterParentID"><%=GetLabel("Parent Revenue Cost Center")%></label></td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnRevenueCostCenterParentID" />
                            <table style="width:100%" cellpadding="1" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtRevenueCostCenterParentCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtRevenueCostCenterParentName" Width="300px" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
