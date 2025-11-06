<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="CashFlowTypeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.CashFlowTypeEntry" %>

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
                var filterExpression = "IsDeleted = 0 AND IsHeader = 1";
                return filterExpression;
            }

            $('#lblCashFlowTypeParentID.lblLink').click(function () {
                openSearchDialog('cashflowtype', onGetParentFilterExpression(), function (value) {
                    $('#<%=txtCashFlowTypeParentCode.ClientID %>').val(value);
                    onTxtCashFlowTypeParentChanged(value);
                });
            });

            $('#<%=txtCashFlowTypeParentCode.ClientID %>').change(function () {
                onTxtCashFlowTypeParentChanged($(this).val());
            });

            function onTxtCashFlowTypeParentChanged(value) {
                var filterExpression = onGetParentFilterExpression() + " AND CashFlowTypeCode = '" + value + "'";
                Methods.getObject('GetCashFlowTypeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnCashFlowTypeParentID.ClientID %>').val(result.CashFlowTypeID);
                        $('#<%=txtCashFlowTypeParentName.ClientID %>').val(result.CashFlowTypeName);
                        $('#<%=txtTypeLevel.ClientID %>').val(result.TypeLevel + 1);
                        $('#<%=chkIsHeader.ClientID %>').prop('checked',false);
                    }
                    else {
                        $('#<%=hdnCashFlowTypeParentID.ClientID %>').val('');
                        $('#<%=txtCashFlowTypeParentCode.ClientID %>').val('');
                        $('#<%=txtCashFlowTypeParentName.ClientID %>').val('');
                        $('#<%=txtTypeLevel.ClientID %>').val('0');
                        $('#<%=chkIsHeader.ClientID %>').prop('checked', true);
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Tipe Arus Kas")%></label></td>
                        <td><asp:TextBox ID="txtCashFlowTypeCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Tipe Arus Kas")%></label></td>
                        <td><asp:TextBox ID="txtCashFlowTypeName" Width="406px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblCashFlowTypeParentID"><%=GetLabel("Parent Tipe Arus Kas")%></label></td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnCashFlowTypeParentID" />
                            <table style="width:100%" cellpadding="1" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtCashFlowTypeParentCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtCashFlowTypeParentName" Width="300px" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Level")%></label></td>
                        <td><asp:TextBox runat="server" ID="txtTypeLevel" Width="100px" CssClass="number" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"></td>
                        <td><asp:CheckBox runat="server" ID="chkIsHeader" Text="Induk" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
