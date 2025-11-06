<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="GLAccountReceivableEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLAccountReceivableEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            function onGetGLAccountFilterExpression() {
                var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
                return filterExpression;
            }

            //#region AR
            $('#lblAR.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtARGLAccountNo.ClientID %>').val(value);
                    onTxtARGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtARGLAccountNo.ClientID %>').change(function () {
                onTxtARGLAccountCodeChanged($(this).val());
            });

            function onTxtARGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnARID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtARGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnARSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnARSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnARIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnARCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnARDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnARMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnARFilterExpression.ClientID %>').val(result.FilterExpression);
                        onARSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnARID.ClientID %>').val('');
                        $('#<%=txtARGLAccountName.ClientID %>').val('');
                        $('#<%=hdnARSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnARSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnARIDFieldName.ClientID %>').val('');
                        $('#<%=hdnARCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnARDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnARMethodName.ClientID %>').val('');
                        $('#<%=hdnARFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnARSubLedger.ClientID %>').val('');
                    $('#<%=txtARSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtARSubLedgerName.ClientID %>').val('');
                });
            }

            function onARSubLedgerIDChanged() {
                if ($('#<%=hdnARSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnARSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblARSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtARSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblARSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtARSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region AR Sub Ledger
            function onGetARSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnARFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnARSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblARSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnARSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnARSearchDialogTypeName.ClientID %>').val(), onGetARSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtARSubLedgerCode.ClientID %>').val(value);
                        onTxtARSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtARSubLedgerCode.ClientID %>').change(function () {
                onTxtARSubLedgerCodeChanged($(this).val());
            });

            function onTxtARSubLedgerCodeChanged(value) {
                if ($('#<%=hdnARSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetARSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnARCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnARMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnARSubLedger.ClientID %>').val(result[$('#<%=hdnARIDFieldName.ClientID %>').val()]);
                            $('#<%=txtARSubLedgerName.ClientID %>').val(result[$('#<%=hdnARDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnARSubLedger.ClientID %>').val('');
                            $('#<%=txtARSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtARSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            
            onARSubLedgerIDChanged();
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCAccountReceivableType" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Modul")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboGCARTransactionGroup" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jenis Instansi")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboGCCustomerType" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top:5px"><label><%=GetLabel("Notes")%></label></td>
                        <td><asp:TextBox ID="txtNotes" Width="300px" runat="server" TextMode="MultiLine" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <colgroup>
                        <col style="width: 50%"/>
                    </colgroup>
                    <tr>
                        <td>
                            <table width="100%">
                                <colgroup>
                                    <col width="190px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblAR"><%=GetLabel("Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnARID" runat="server" />
                                        <input type="hidden" id="hdnARSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnARSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnARIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnARCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnARDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnARMethodName" runat="server" />
                                        <input type="hidden" id="hdnARFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtARGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtARGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblARSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnARSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtARSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtARSubLedgerName" Width="100%" /></td>
                                            </tr>
                                        </table>
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