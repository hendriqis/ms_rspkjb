<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="GLAccountPayableEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLAccountPayableEntry" %>

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

            //#region AP
            $('#lblAP.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAPGLAccountNo.ClientID %>').val(value);
                    onTxtAPGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAPGLAccountNo.ClientID %>').change(function () {
                onTxtAPGLAccountCodeChanged($(this).val());
            });

            function onTxtAPGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAPID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAPSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAPSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAPIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAPCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAPDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAPMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAPFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAPSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAPID.ClientID %>').val('');
                        $('#<%=txtAPGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAPSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAPSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAPIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAPCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAPDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAPMethodName.ClientID %>').val('');
                        $('#<%=hdnAPFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAPSubLedger.ClientID %>').val('');
                    $('#<%=txtAPSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAPSubLedgerName.ClientID %>').val('');
                });
            }

            function onAPSubLedgerIDChanged() {
                if ($('#<%=hdnAPSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAPSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAPSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAPSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAPSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAPSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region AP Sub Ledger
            function onGetAPSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAPFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAPSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAPSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnAPSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAPSearchDialogTypeName.ClientID %>').val(), onGetAPSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAPSubLedgerCode.ClientID %>').val(value);
                        onTxtAPSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAPSubLedgerCode.ClientID %>').change(function () {
                onTxtAPSubLedgerCodeChanged($(this).val());
            });

            function onTxtAPSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAPSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAPSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAPCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAPMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAPSubLedger.ClientID %>').val(result[$('#<%=hdnAPIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAPSubLedgerName.ClientID %>').val(result[$('#<%=hdnAPDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAPSubLedger.ClientID %>').val('');
                            $('#<%=txtAPSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAPSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            
            onAPSubLedgerIDChanged();
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jenis")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboGCAccountPayableType" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tipe Item")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboGCItemType" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top:5px"><label><%=GetLabel("Keterangan")%></label></td>
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
                                    <td class="tdLabel"><label class="lblLink" id="lblAP"><%=GetLabel("Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPID" runat="server" />
                                        <input type="hidden" id="hdnAPSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAPSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAPIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPMethodName" runat="server" />
                                        <input type="hidden" id="hdnAPFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblAPSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPSubLedgerName" Width="100%" /></td>
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