<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubLedgerDtReportInfoCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Accounting.Program.SubLedgerDtReportInfoCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    //#region GL Account
    function onGetGLAccountFilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblGLAccount.lblLink').live('click', function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtGLAccountCode.ClientID %>').val(value);
            onTxtGLAccountCodeChanged(value);
        });
    });

    $('#<%=txtGLAccountCode.ClientID %>').live('change', function () {
        onTxtGLAccountCodeChanged($(this).val());
    });

    function onTxtGLAccountCodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);

                $('#<%=hdnSubLedgerID.ClientID %>').val(result.SubLedgerID);
                $('#<%=hdnSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                $('#<%=hdnFilterExpression.ClientID %>').val(result.FilterExpression);
                $('#<%=hdnIDFieldName.ClientID %>').val(result.IDFieldName);
                $('#<%=hdnCodeFieldName.ClientID %>').val(result.CodeFieldName);
                $('#<%=hdnDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                $('#<%=hdnMethodName.ClientID %>').val(result.MethodName);
            }
            else {
                $('#<%=hdnGLAccountID.ClientID %>').val('');
                $('#<%=txtGLAccountCode.ClientID %>').val('');
                $('#<%=txtGLAccountName.ClientID %>').val('');

                $('#<%=hdnSubLedgerID.ClientID %>').val('');
                $('#<%=hdnSearchDialogTypeName.ClientID %>').val('');
                $('#<%=hdnFilterExpression.ClientID %>').val('');
                $('#<%=hdnIDFieldName.ClientID %>').val('');
                $('#<%=hdnCodeFieldName.ClientID %>').val('');
                $('#<%=hdnDisplayFieldName.ClientID %>').val('');
                $('#<%=hdnMethodName.ClientID %>').val('');
            }
            onSubLedgerIDChanged();
            $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
            $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
            $('#<%=txtSubLedgerDtName.ClientID %>').val('');
        });
    }

    function onSubLedgerIDChanged() {
        if ($('#<%=hdnSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID.ClientID %>').val() == '') {
            $('#<%=lblSubLedgerDt.ClientID %>').attr('class', 'lblDisabled');
            $('#<%=txtSubLedgerDtCode.ClientID %>').attr('readonly', 'readonly');
        }
        else {
            $('#<%=lblSubLedgerDt.ClientID %>').attr('class', 'lblLink lblMandatory');
            $('#<%=txtSubLedgerDtCode.ClientID %>').removeAttr('readonly');
        }
    }
    //#endregion

    //#region Sub Ledger
    function onGetSubLedgerDtFilterExpression() {
        var filterExpression = $('#<%=hdnFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID.ClientID %>').val());
        return filterExpression;
    }

    $('#<%=lblSubLedgerDt.ClientID %>.lblLink').live('click', function () {
        if ($('#<%=hdnSearchDialogTypeName.ClientID %>').val() != '') {
            openSearchDialog($('#<%=hdnSearchDialogTypeName.ClientID %>').val(), onGetSubLedgerDtFilterExpression(), function (value) {
                $('#<%=txtSubLedgerDtCode.ClientID %>').val(value);
                onTxtSubLedgerDtCodeChanged(value);
            });
        }
    });

    $('#<%=txtSubLedgerDtCode.ClientID %>').live('change', function () {
        onTxtSubLedgerDtCodeChanged($(this).val());
    });

    function onTxtSubLedgerDtCodeChanged(value) {
        if ($('#<%=hdnSearchDialogTypeName.ClientID %>').val() != '') {
            var filterExpression = onGetSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnCodeFieldName.ClientID %>').val() + " = '" + value + "'";
            Methods.getObject($('#<%=hdnMethodName.ClientID %>').val(), filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnSubLedgerDtID.ClientID %>').val(result[$('#<%=hdnIDFieldName.ClientID %>').val()]);
                    $('#<%=txtSubLedgerDtName.ClientID %>').val(result[$('#<%=hdnDisplayFieldName.ClientID %>').val()]);
                }
                else {
                    $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
                    $('#<%=txtSubLedgerDtName.ClientID %>').val('');
                }
            });
        }
    }
    //#endregion
</script>

<table>
    <colgroup>
        <col style="width:150px"/>
        <col />
    </colgroup>
    <tr>
        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblGLAccount"><%=GetLabel("Perkiraan")%></label></td>
        <td>
            <input type="hidden" id="hdnGLAccountID" runat="server" />
            <input type="hidden" id="hdnSubLedgerID" runat="server" />
            <input type="hidden" id="hdnSearchDialogTypeName" runat="server" />
            <input type="hidden" id="hdnIDFieldName" runat="server" />
            <input type="hidden" id="hdnCodeFieldName" runat="server" />
            <input type="hidden" id="hdnDisplayFieldName" runat="server" />
            <input type="hidden" id="hdnMethodName" runat="server" />
            <input type="hidden" id="hdnFilterExpression" runat="server" />
            <table style="width:100%" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:30%"/>
                    <col style="width:3px"/>
                    <col/>
                </colgroup>
                <tr>
                    <td><asp:TextBox runat="server" ID="txtGLAccountCode" Width="100%" /></td>
                    <td>&nbsp;</td>
                    <td><asp:TextBox runat="server" ID="txtGLAccountName" ReadOnly="true" Width="100%" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblSubLedgerDt"><%=GetLabel("Sub Perkiraan")%></label></td>
        <td>
            <input type="hidden" id="hdnSubLedgerDtID" runat="server" />
            <table style="width:100%" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:30%"/>
                    <col style="width:3px"/>
                    <col/>
                </colgroup>
                <tr>
                    <td><asp:TextBox runat="server" ID="txtSubLedgerDtCode" Width="100%" /></td>
                    <td>&nbsp;</td>
                    <td><asp:TextBox runat="server" ID="txtSubLedgerDtName" ReadOnly="true" Width="100%" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tdLabel"><label><%=GetLabel("Tahun")%></label></td>
        <td><dxe:ASPxComboBox ID="cboYear" Width="120px" ClientInstanceName="cboYear" runat="server" /></td>
    </tr>
    <tr>
        <td class="tdLabel"><label><%=GetLabel("Bulan")%></label></td>
        <td><dxe:ASPxComboBox ID="cboMonth" Width="120px" ClientInstanceName="cboMonth" runat="server" /></td>
    </tr>
</table>