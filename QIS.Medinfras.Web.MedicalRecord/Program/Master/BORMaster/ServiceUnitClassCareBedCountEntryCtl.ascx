<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceUnitClassCareBedCountEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.ServiceUnitClassCareBedCountEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ServiceUnitClassCareBedCountEntryCtl">
    $(function () {
        setDatePicker('<%:txtStartingDate.ClientID %>');
        $('#<%:txtStartingDate.ClientID %>').datepicker('option', 'minDate', 0);
    });

    //#region Class Care
    function getClassCareFilterExpression() {
        var filterExpression = 'IsDeleted = 0';
        return filterExpression;
    }

    $('#<%:lblClass.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('classcare', getClassCareFilterExpression(), function (value) {
            $('#<%:txtClassCode.ClientID %>').val(value);
            onTxtClassCodeChanged(value);
        });
    });

    $('#<%:txtClassCode.ClientID %>').live('change', function () {
        onTxtClassCodeChanged($(this).val());
    });

    function onTxtClassCodeChanged(value) {
        var filterExpression = getClassCareFilterExpression() + " AND ClassCode = '" + value + "'";
        Methods.getObject('GetClassCareList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnClassID.ClientID %>').val(result.ClassID);
                $('#<%:txtClassName.ClientID %>').val(result.ClassName);

                var filterBedCount = "ClassCode = '" + value + "' AND ServiceUnitID = " + $('#<%:hdnServiceUnitID.ClientID %>').val();
                Methods.getObject('GetvServiceUnitClassCareBedCountList', filterBedCount, function (result2) {
                    if (result2 != null) {
                        $('#<%:txtStartingDate.ClientID %>').val(result2.cfStartingDateInStringDatePicker);
                        $('#<%:txtBedCount.ClientID %>').val(result2.BedCount);
                        $('#<%:txtRemarks.ClientID %>').val(result2.Remarks);
                    }
                    else {
                        $('#<%:txtStartingDate.ClientID %>').val('');
                        $('#<%:txtBedCount.ClientID %>').val('0');
                        $('#<%:txtRemarks.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%:hdnClassID.ClientID %>').val('');
                $('#<%:txtClassCode.ClientID %>').val('');
                $('#<%:txtClassName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%=txtBedCount.ClientID %>').keydown(function (e) {
        var key = e.charCode || e.keyCode || 0;
        return (
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
    });

</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnClassID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 80px" />
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Unit Pelayanan")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="300px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" runat="server" id="lblClass">
                    <%:GetLabel("Kelas Perawatan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtClassCode" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtClassName" Width="90%" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal Mulai Belaku")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtStartingDate" Width="120px" runat="server" CssClass="datepicker" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Jumlah Bed")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtBedCount" runat="server" Width="100px" CssClass="numeric">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Catatan")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRemarks" runat="server" Width="300px">
                </asp:TextBox>
            </td>
        </tr>
    </table>
</div>
