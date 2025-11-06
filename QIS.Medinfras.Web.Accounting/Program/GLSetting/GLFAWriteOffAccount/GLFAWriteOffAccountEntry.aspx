<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="GLFAWriteOffAccountEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLFAWriteOffAccountEntry" %>

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

            //#region FAGroup
            function onGetFAGroupFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblFAGroup').click(function () {
                openSearchDialog('fagroup', onGetFAGroupFilterExpression(), function (value) {
                    $('#<%=txtFAGroupCode.ClientID %>').val(value);
                    onTxtFAGroupCodeChanged(value);
                });
            });

            $('#<%=txtFAGroupCode.ClientID %>').change(function () {
                onTxtFAGroupCodeChanged($(this).val());
            });

            function onTxtFAGroupCodeChanged(value) {
                var filterExpression = onGetFAGroupFilterExpression() + " AND FAGroupCode = '" + value + "'";
                Methods.getObject('GetFAGroupList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnFAGroupID.ClientID %>').val(result.FAGroupID);
                        $('#<%=txtFAGroupName.ClientID %>').val(result.FAGroupName);
                    }
                    else {
                        $('#<%=hdnFAGroupID.ClientID %>').val('');
                        $('#<%=txtFAGroupCode.ClientID %>').val('');
                        $('#<%=txtFAGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region WO
            $('#lblWO.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtWOGLAccountNo.ClientID %>').val(value);
                    onTxtWOGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtWOGLAccountNo.ClientID %>').change(function () {
                onTxtWOGLAccountCodeChanged($(this).val());
            });

            function onTxtWOGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnWOID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtWOGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnWOSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnWOSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnWOIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnWOCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnWODisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnWOMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnWOFilterExpression.ClientID %>').val(result.FilterExpression);
                        onWOSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnWOID.ClientID %>').val('');
                        $('#<%=txtWOGLAccountName.ClientID %>').val('');
                        $('#<%=hdnWOSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnWOSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnWOIDFieldName.ClientID %>').val('');
                        $('#<%=hdnWOCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnWODisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnWOMethodName.ClientID %>').val('');
                        $('#<%=hdnWOFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnWOSubLedger.ClientID %>').val('');
                    $('#<%=txtWOSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtWOSubLedgerName.ClientID %>').val('');
                });
            }

            function onWOSubLedgerIDChanged() {
                if ($('#<%=hdnWOSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnWOSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblWOSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtWOSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblWOSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtWOSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region WO Sub Ledger
            function onGetWOSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnWOFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnWOSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblWOSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnWOSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnWOSearchDialogTypeName.ClientID %>').val(), onGetWOSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtWOSubLedgerCode.ClientID %>').val(value);
                        onTxtWOSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtWOSubLedgerCode.ClientID %>').change(function () {
                onTxtWOSubLedgerCodeChanged($(this).val());
            });

            function onTxtWOSubLedgerCodeChanged(value) {
                if ($('#<%=hdnWOSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetWOSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnWOCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnWOMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnWOSubLedger.ClientID %>').val(result[$('#<%=hdnWOIDFieldName.ClientID %>').val()]);
                            $('#<%=txtWOSubLedgerName.ClientID %>').val(result[$('#<%=hdnWODisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnWOSubLedger.ClientID %>').val('');
                            $('#<%=txtWOSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtWOSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion
            
            onWOSubLedgerIDChanged();
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
                        <td class="tdLabel"><label class="lblLink" id="lblFAGroup"><%=GetLabel("Kelompok Aset & Inventaris")%></label></td>
                        <td>
                            <input type="hidden" id="hdnFAGroupID" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtFAGroupCode" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtFAGroupName" Width="100%" ReadOnly="true" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tipe Penjualan/Pemusnahan")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboGCWriteOffType" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Cara Penjualan")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboGCAssetSalesType" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Bank")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboBank" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top:5px"><label><%=GetLabel("Keterangan")%></label></td>
                        <td><asp:TextBox ID="txtNotes" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblWO"><%=GetLabel("COA Penjualan/Pemusnahan")%></label></td>
                        <td>
                            <input type="hidden" id="hdnWOID" runat="server" />
                            <input type="hidden" id="hdnWOSubLedgerID" runat="server" />
                            <input type="hidden" id="hdnWOSearchDialogTypeName" runat="server" />
                            <input type="hidden" id="hdnWOIDFieldName" runat="server" />
                            <input type="hidden" id="hdnWOCodeFieldName" runat="server" />
                            <input type="hidden" id="hdnWODisplayFieldName" runat="server" />
                            <input type="hidden" id="hdnWOMethodName" runat="server" />
                            <input type="hidden" id="hdnWOFilterExpression" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtWOGLAccountNo" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtWOGLAccountName" Width="100%" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblWOSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                        <td>
                            <input type="hidden" id="hdnWOSubLedger" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtWOSubLedgerCode" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtWOSubLedgerName" Width="100%" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>