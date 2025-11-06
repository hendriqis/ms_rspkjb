<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutritionOrderHistoryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NutritionOrderHistoryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_prescriptioncompoundentryctl">
    function oncbpViewMealOrderHistoryEndCallback(s) {
        hideLoadingPanel();
    }

    function getCheckedMember(errMessage) {
        var lstSelectedMember = [];
        var lstSelectedMemberHd = [];
        var result = '';

        $('#<%=grdView.ClientID %> .chkIsSelectedMealOrder input:checked').each(function () {
            $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').html();
            var hdId = $(this).closest('tr').find('.orderHdId').val();
            lstSelectedMember.push(key);
            lstSelectedMemberHd.push(hdId);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberHd.ClientID %>').val(lstSelectedMemberHd.join(','));
    }

    function onBeforeSaveRecord(errMessage) {
        errMessage.text = '';
        if (IsValid(null, 'fsMealOrderHistory', 'mpMedicationOrderHistoryDosing')) {
            if (IsValid(null, 'fsMealOrderHistory', 'mpMedicationOrderHistoryQty')) {
                getCheckedMember(errMessage);
                if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                    if (errMessage.text != '')
                        return false;
                    return true;
                }
                else {
                    errMessage.text = 'Please Select Item First';
                    return false;
                }
                return false;
            }
        }
    }

    //#region schedule date
    setDatePicker('<%=txtScheduleDate.ClientID %>');
    $('#<%=txtScheduleDate.ClientID %>').datepicker('option', '', '0');

    $('#<%=txtScheduleDate.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + '-' + MM + '-' + DD;
        $('#<%=hdnScheduleDate.ClientID %>').val(dateALL);
    });
    //#endregion

    //#region filter grid view
    $('#<%:trMealDay.ClientID %>').removeAttr('style');
    $('#<%:trScheduleDate.ClientID %>').attr('style', 'display:none');

    $('#<%=chkUseScheduleDate.ClientID %>').change(function () {
        onChangeChkScheduleDate($(this).val());
    });

    function onChangeChkScheduleDate(value) {
        var isCheck = $('#<%:chkUseScheduleDate.ClientID %>').is(':checked');
        if (isCheck) {
            $('#<%:trScheduleDate.ClientID %>').removeAttr('style');
            $('#<%:trMealDay.ClientID %>').attr('style', 'display:none');
        }
        else {
            $('#<%:trMealDay.ClientID %>').removeAttr('style');
            $('#<%:trScheduleDate.ClientID %>').attr('style', 'display:none');
        }
    }

    $('#<%=chkIgnoreMealTime.ClientID %>').die();
    $('#<%=chkIgnoreMealTime.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            cboMealTimeHistory.SetEnabled(false);
        }
        else {
            cboMealTimeHistory.SetEnabled(true);
        }
        onRefreshGridView();
    });
    //#endregion

    $('#btnRefresh1').live('click', function () {
        cbpViewMealOrderHistory.PerformCallback('refresh');
    });
    $('#btnRefresh2').live('click', function () {
        cbpViewMealOrderHistory.PerformCallback('refresh');
    });
    
</script>
<input type="hidden" id="hdnVisitID" runat="server" />
<input type="hidden" id="hdnParamedicID" runat="server" />
<input type="hidden" id="hdnNutritionOrderHdID" runat="server" />
<input type="hidden" id="hdnRegistrationID" runat="server" value="" />
<input type="hidden" id="hdnScheduleDate" runat="server" value="" />
<input type="hidden" id="hdnScheduleTime" runat="server" value="" />
<input type="hidden" id="hdnClassID" runat="server" />
<input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnMRN" runat="server" value="" />
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberHd" runat="server" value="" />
<input type="hidden" id="hdnMealTime" runat="server" value="" />
<input type="hidden" id="hdnOrderDate" runat="server" value="" />
<input type="hidden" id="hdnOrderTime" runat="server" value="" />
<input type="hidden" id="hdnGCMealDayCtlOH" runat="server" value="" />
<input type="hidden" id="hdnParam" runat="server" value="" />
<input type="hidden" id="hdnGCDietType" runat="server" value="" />
<div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 20%" />
            <col style="width: auto" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label id="lblMealTime">
                    <%=GetLabel("Jadwal Makan") %>
                </label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <dxe:ASPxComboBox ID="cboMealTimeHistory" ClientInstanceName="cboMealTimeHistory"
                                runat="server" Width="80%" ReadOnly="false">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:CheckBox ID="chkIgnoreMealTime" runat="server" />
                <%=GetLabel("Abaikan Jadwal Makan")%>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Filter") %>
                </label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkUseScheduleDate" runat="server" />
                            <%=GetLabel("Berdasarkan Jadwal Tgl Makan")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trMealDay" runat="server">
            <td class="tdLabel">
                <label id="lblMealDay">
                    <%=GetLabel("Menu Hari ke -") %>
                </label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <dxe:ASPxComboBox ID="cboMealDayHistory" ClientInstanceName="cboMealDayHistory" runat="server"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <input type="button" id="btnRefresh1" title='<%:GetLabel("Ambil Data") %>' value="Ambil Data" />
            </td>
        </tr>
        <tr id="trScheduleDate" runat="server">
            <td class="tdLabel">
                <label id="lblScheduleDate">
                    <%=GetLabel("Jadwal Tanggal Makan") %>
                </label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="padding-right: 1px; width: 145px">
                            <asp:TextBox ID="txtScheduleDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <input type="button" id="btnRefresh2" title='<%:GetLabel("Ambil Data") %>' value="Ambil Data" />
            </td>
        </tr>
    </table>
</div>
<div style="height: 500px; overflow-y: auto">
    <div style="height: 450px; overflow-y: auto">
        <fieldset id="fsMealOrderHistory">
            <dxcp:ASPxCallbackPanel ID="cbpViewMealOrderHistory" runat="server" Width="100%"
                ClientInstanceName="cbpViewMealOrderHistory" ShowLoadingPanel="false" OnCallback="cbpViewMealOrderHistory_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ oncbpViewMealOrderHistoryEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="NutritionOrderDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <input type="hidden" class="orderHdId" value='<%#: Eval("NutritionOrderHdID")%>' />
                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelectedMealOrder" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                        HeaderText="Menu Makan">

                                        <ItemTemplate>
                                            <b>
                                                <%#:Eval("MealPlanName")%></b>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" HeaderText="Jadwal Tanggal">
                                        <ItemTemplate>
                                            <b>
                                                <%#:Eval("ScheduleDateInString")%></b>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" HeaderText="Jadwal Makan">
                                        <ItemTemplate>
                                            <b>
                                                <%#:Eval("MealTime")%></b>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" HeaderText="Menu Hari Ke -">
                                        <ItemTemplate>
                                            <b>
                                                <%#:Eval("MealDay")%></b>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Tidak ada history order menu makan untuk pasien ini.")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </fieldset>
    </div>
</div>
