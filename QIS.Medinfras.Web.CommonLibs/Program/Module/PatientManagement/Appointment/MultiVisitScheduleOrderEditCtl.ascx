<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiVisitScheduleOrderEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MultiVisitScheduleOrderEditCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_printprescriptionlist">
    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $(function () {
        setDatePickerGrid();

        $('#btnSave').click(function () {
            var param = getDetailOrder();
            cbpProcess.PerformCallback("update|" + param);
        });

        $('.txtScheduleEndDate').each(function () {
            setDatePickerElement($(this));
            $(this).datepicker('option', 'minDate', '0');
        });
    });

    function getDetailOrder() {
        var lstSelectedMember = "";
        var tbl = $("[id$=lvwView]");
        var rows = tbl.find('tr');
        for (var index = 1; index < rows.length; index++) {
            var row = rows[index];
            var key = $(row).find(".keyField").html();
            if (key != null && key != "null") {
                var endDate = $(row).find(".txtScheduleEndDate").val();
                lstSelectedMember += key + "~" + endDate + "&";
            }
        }
        return lstSelectedMember;
    }

    function setDatePickerGrid() {
        var tbl = $("[id$=lvwView]");
        var rows = tbl.find('tr');
        for (var index = 1; index < rows.length; index++) {
            var row = rows[index];
            var endDate = $(row).find(".txtScheduleEndDate");
            setDatePickerElement(endDate);
            endDate.datepicker('option', 'minDate', '0');
        }
    }

    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedDate = "";
        $('.grdPrescriptionOrderDt .chkIsSelected input:checked').each(function () {
            var prescriptionOrderDtID = $(this).closest('tr').find('.keyField').html();

            var expiredDate = $(this).closest('tr').find('.txtExpiredDate').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedDate += ",";
            }
            tempSelectedID += prescriptionOrderDtID;
            tempSelectedDate += expiredDate;
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedDate.ClientID %>').val(tempSelectedDate);
            return true;
        }
        else return false;
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedDate" value="" />
<input type="hidden" runat="server" id="hdnTestOrderID" value="" />
<div>
    <table width="100%">
        <tr>
            <td>
                <div style="height: 400px; overflow: auto">
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="position: relative; width: 100%">
                                    <asp:GridView ID="lvwView" runat="server" CssClass="grdSelected grdPatientPage"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="lvwViewPrint_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Nama Tindakan")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("ItemName1")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Tanggal Mulai")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtScheduleStartDate" CssClass="datepicker txtScheduleStartDate" 
                                                    ReadOnly="true" Width="120px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Tanggal Akhir")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtScheduleEndDate" CssClass="datepicker txtScheduleEndDate" Width="120px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada item")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" id="btnSave" value='<%= GetLabel("Simpan")%>' style="width: 100px" />
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
            ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e) {showLoadingPanel();}" EndCallback="function(s,e) {hideLoadingPanel();}" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
