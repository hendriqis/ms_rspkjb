<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientMedicalDeviceEntryCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientMedicalDeviceEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_PatientMedicalDeviceEntryCtl">
    $(function () {
        setDatePicker('<%=txtImplantDate.ClientID %>');
        setDatePicker('<%=txtReviewDate.ClientID %>');
        $('#<%=txtImplantDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        $('#<%=txtReviewDate.ClientID %>').datepicker('option', 'minDate', '0');

        $('#lblItemCode').removeClass('lblLink');
        $('#<%=txtItemCode.ClientID %>').attr("disabled", "disabled");
        $('#<%=txtItemName.ClientID %>').removeAttr("disabled");

        //#region Transaction No
        $('#lblTransactionNo.lblLink').click(function () {
            var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCTransactionStatus NOT IN ('X121^001','X121^999') ";
            openSearchDialog('patientchargesdtImplant', filterExpression, function (value) {
                $('#<%=hdnTransactionDtID.ClientID %>').val(value);
                onTxtTransactionNoChanged(value);
            });
        });

        $('#<%=txtTransactionNo.ClientID %>').change(function () {
            onTxtTransactionNoChanged($(this).val());
        });

        function onTxtTransactionNoChanged(value) {
            var filterExpression = "ID = " + value;
            Methods.getObject('GetvPatientChargesDtCustom1List', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                }
                else {
                    $('#<%=txtTransactionNo.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('#<%=chkIsUsingMaster.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#lblItemCode').addClass('lblLink');
                $('#<%=txtItemCode.ClientID %>').removeAttr("disabled");
                $('#<%=txtItemName.ClientID %>').attr("disabled", "disabled");
            }
            else {
                $('#lblItemCode').removeClass('lblLink');
                $('#<%=txtItemCode.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtItemName.ClientID %>').removeAttr("disabled");
            }
        });
    });


    //#region Item
    function onGetItemFilterExpression() {
        var filterExpression = " IsImplant = 1";

        filterExpression += " AND GCItemStatus != 'X181^999'";
        return filterExpression;
    }

    $('#lblItemCode.lblLink').die('click');
    $('#lblItemCode.lblLink').live('click', function () {
        openSearchDialog('itemproduct', onGetItemFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtItemCodeChanged($(this).val());
    });

    function onTxtItemCodeChanged(value) {
        var filterExpression = onGetItemFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvItemProductList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=hdnItemName.ClientID %>').val(result.ItemName1);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=hdnItemName.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCboPhysicianChanged(s) {
        if (s.GetValue() != null) {
            $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
        }
        else
            $('#<%=hdnParamedicID.ClientID %>').val('');
    }
</script>
<style type="text/css">
    #ulVitalSign
    {
        margin: 0;
        padding: 0;
    }
    #ulVitalSign li
    {
        list-style-type: none;
        display: inline-block;
        padding-left: 5px;
        width: 48%;
    }
</style>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnItemID" value="" />
    <input type="hidden" runat="server" id="hdnItemName" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnUserID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionDtID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory"><%=GetLabel("Tanggal Pemasangan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtImplantDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsUsingMaster" runat="server" Checked = "false" Text= " Menggunakan Kode Master Item" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblLink" id="lblItemCode">
                                    <%=GetLabel("Kode Item Implant")%></label></div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label id="lblItemName">
                                    <%=GetLabel("Nama Item Implant")%></label></div>
                        </td>
                        <td colspan="2">                            
                            <asp:TextBox ID="txtItemName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory"><%=GetLabel("Tanggal Review ")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtReviewDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Serial Number")%></label></div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSerialNo" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblLink" id="lblTransactionNo">
                                    <%=GetLabel("No. Transaksi")%></label></div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan") %></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline"
                                    Height="250px" />
                            </td>
                        </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
