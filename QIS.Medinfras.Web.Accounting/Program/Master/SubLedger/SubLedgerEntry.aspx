<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="SubLedgerEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.SubLedgerEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Sub Ledger Type
            function onGetSubLedgerTypeFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblSubLedgerType.lblLink').click(function () {
                openSearchDialog('subledgertype', onGetSubLedgerTypeFilterExpression(), function (value) {
                    $('#<%=txtSubLedgerTypeCode.ClientID %>').val(value);
                    onTxtSubLedgerTypeCodeChanged(value);
                });
            });

            $('#<%=txtSubLedgerTypeCode.ClientID %>').change(function () {
                onTxtSubLedgerTypeCodeChanged($(this).val());
            });

            function onTxtSubLedgerTypeCodeChanged(value) {
                var filterExpression = onGetSubLedgerTypeFilterExpression() + " AND SubLedgerTypeCode = '" + value + "'";
                Methods.getObject('GetSubLedgerTypeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSubLedgerTypeID.ClientID %>').val(result.SubLedgerTypeID);
                        $('#<%=txtSubLedgerTypeName.ClientID %>').val(result.SubLedgerTypeName);
                    }
                    else {
                        $('#<%=hdnSubLedgerTypeID.ClientID %>').val('');
                        $('#<%=txtSubLedgerTypeCode.ClientID %>').val('');
                        $('#<%=txtSubLedgerTypeName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:25%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Sub Perkiraan")%></label></td>
                        <td><asp:TextBox ID="txtSubLedgerCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Sub Perkiraan")%></label></td>
                        <td><asp:TextBox ID="txtSubLedgerName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory lblLink" id="lblSubLedgerType"><%=GetLabel("Tipe Sub Perkiraan")%></label></td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerTypeID" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtSubLedgerTypeCode" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtSubLedgerTypeName" Width="100%" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
