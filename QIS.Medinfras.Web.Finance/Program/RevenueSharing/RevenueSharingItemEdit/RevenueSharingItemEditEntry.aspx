<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="RevenueSharingItemEditEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingItemEditEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessSaveRecord" crudmode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" /><div><%=GetLabel("Proses")%></div></li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

            $('#<%=btnProcessSaveRecord.ClientID %>').click(function () {
                if (IsValid(null, 'fsList', 'mpList'))
                    onCustomButtonClick('process');
            });

            //#region Item
            function onGetItemFilterExpression() {
                return "<%=OnGetItemFilterExpression() %>"
            }

            $('#lblItem.lblLink').click(function () {
                openSearchDialog('item', onGetItemFilterExpression(), function (value) {
                    $('#<%=txtItemCode.ClientID %>').val(value);
                    onTxtItemCodeChanged(value);
                });
            });

            $('#<%=txtItemCode.ClientID %>').change(function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var filterExpression = onGetItemFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                    }
                    else {
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Paramedic
            $('#lblParamedic.lblLink').click(function () {
                openSearchDialog('paramedic', 'IsDeleted = 0', function (value) {
                    $('#<%=txtParamedicCode.ClientID %>').val(value);
                    onTxtParamedicCodeChanged(value);
                });
            });

            $('#<%=txtParamedicCode.ClientID %>').change(function () {
                onTxtParamedicCodeChanged($(this).val());
            });

            function onTxtParamedicCodeChanged(value) {
                var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnParamedicID.ClientID %>').val('');
                        $('#<%=txtParamedicCode.ClientID %>').val('');
                        $('#<%=txtParamedicName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Revenue Sharing
            $('#lblRevenueSharing.lblLink').click(function () {
                openSearchDialog('revenuesharing', 'IsDeleted = 0', function (value) {
                    $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                    onTxtRevenueSharingCodeChanged(value);
                });
            });

            $('#<%=txtRevenueSharingCode.ClientID %>').change(function () {
                onTxtRevenueSharingCodeChanged($(this).val());
            });

            function onTxtRevenueSharingCodeChanged(value) {
                var filterExpression = "RevenueSharingCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                        $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                    }
                    else {
                        $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                        $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                        $('#<%=txtRevenueSharingName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        });

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Success', 'Formula Honor Dokter Berhasil Diubah');
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />

    <fieldset id="fsList" style="margin:0"> 
        <table class="tblEntryContent" style="width:50%">
            <colgroup>
                <col style="width:160px;" />
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblItem"><%=GetLabel("Pelayanan") %></label></td>
                <td>
                    <input type="hidden" id="hdnItemID" runat="server" value="" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 3px" />
                        </colgroup>
                        <tr>
                            <td><asp:TextBox ID="txtItemCode" Width="100%" runat="server" /></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>   
            <tr>
                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblParamedic"><%=GetLabel("Dokter / Paramedis") %></label></td>
                <td>
                    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 3px" />
                        </colgroup>
                        <tr>
                            <td><asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" /></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr> 
            <tr>
                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Peranan")%></label></td>
                <td><dxe:ASPxComboBox ID="cboParamedicRole" ClientInstanceName="cboParamedicRole" Width="300px" runat="server" /></td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kelas")%></label></td>
                <td><dxe:ASPxComboBox ID="cboClass" ClientInstanceName="cboClass" Width="300px" runat="server" /></td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Periode Transaksi") %></label></td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 145px" />
                            <col style="width: 3px" />
                            <col style="width: 145px" />
                        </colgroup>
                        <tr>
                            <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" /></td>
                            <td><%=GetLabel("s/d") %></td>
                            <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblRevenueSharing"><%=GetLabel("Formula Honor Dokter") %></label></td>
                <td>
                    <input type="hidden" id="hdnRevenueSharingID" runat="server" value="" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 3px" />
                        </colgroup>
                        <tr>
                            <td><asp:TextBox ID="txtRevenueSharingCode" Width="100%" runat="server" /></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox ID="txtRevenueSharingName" ReadOnly="true" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>   
        </table>
    </fieldset>
</asp:Content>