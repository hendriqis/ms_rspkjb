<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutritionOrderEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NutritionOrderEditCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">

    //#region Meal Plan
    function getItemMasterFilterExpression() {
        var cboValue = cboMealTime.GetValue();
        var filterExpression = "GCMealTime = '" + cboValue + "' AND IsDeletedDetail = 0 AND IsDeleted = 0";
        return filterExpression;
    }

    function OnGetMealPlanFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblMealPlan.lblLink').die('click');
    $('#lblMealPlan.lblLink').live('click', function () {
        openSearchDialog('mealplan1', getItemMasterFilterExpression(), function (value) {
            $('#<%=txtMealPlanCode.ClientID %>').val(value);
            ontxtMealPlanCodeChanged(value);
        });
    });

    $('#<%=txtMealPlanCode.ClientID %>').live('die');
    $('#<%=txtMealPlanCode.ClientID %>').live('change', function () {
        ontxtMealPlanCodeChanged($(this).val());
    });

    function ontxtMealPlanCodeChanged(value) {
        var filterExpression = OnGetMealPlanFilterExpression() + " AND MealPlanCode = '" + value + "'";
        Methods.getObject('GetvMealPlanList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnMealPlanID.ClientID%>').val(result.MealPlanID);
                $('#<%=txtMealPlanName.ClientID %>').val(result.MealPlanName);
            }
            else {
                $('#<%=hdnMealPlanID.ClientID%>').val('');
                $('#<%=txtMealPlanCode.ClientID%>').val('');
                $('#<%=txtMealPlanName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#btnPopupSave').die('click');
    $('#btnPopupSave').live('click', function () {
        if (IsValid(null, 'fsTrx', 'mpTrx'))
            cbpPopupProcess.PerformCallback('save');
    });

    $('#btnPopupCancel').die('click');
    $('#btnPopupCancel').live('click', function () {
        $('#containerEntry').hide();
        $('#<%=txtMealPlanCode.ClientID%>').val('');
        $('#<%=txtMealPlanCode.ClientID %>').val('');
    });

    $('#<%=grdPopupView.ClientID %> .imgPopupEdit.imgLink').die('click');
    $('#<%=grdPopupView.ClientID %> .imgPopupEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);

        cboMealTime.SetValue(entity.GCMealTime);
        cboMealStatus.SetValue(entity.GCMealStatus);
        $('#<%=hdnEntryID.ClientID %>').val(entity.NutritionOrderDtID);
        $('#<%=txtMealDay.ClientID%>').val(entity.MealDay);
        $('#<%=hdnParamedicID.ClientID%>').val(entity.ParamedicID);
        $('#<%=txtParamedicCode.ClientID%>').val(entity.ParamedicCode);
        $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);
        $('#<%=hdnMealPlanID.ClientID%>').val(entity.MealPlanID);
        $('#<%=txtMealPlanCode.ClientID%>').val(entity.MealPlanCode);
        $('#<%=txtMealPlanName.ClientID %>').val(entity.MealPlanName);
        $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
        $('#containerEntry').show();
    });

    $('#<%=grdPopupView.ClientID %> .imgPopupDelete.imgLink').die('click');
    $('#<%=grdPopupView.ClientID %> .imgPopupDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.NutritionOrderDtID);
                cbpPopupProcess.PerformCallback('delete');
            }
        });
    });

    //#region first date load
    var scheDate = $('#<%=hdnScheduleDate.ClientID %>').val().substring(0, 2);
    if (scheDate == '01' || scheDate == '11' || scheDate == '21') {
        $('#<%=hdnMealDay.ClientID %>').val('1');
    } else if (scheDate == '02' || scheDate == '12' || scheDate == '22') {
        $('#<%=hdnMealDay.ClientID %>').val('2');
    } else if (scheDate == '03' || scheDate == '13' || scheDate == '23') {
        $('#<%=hdnMealDay.ClientID %>').val('3');
    } else if (scheDate == '04' || scheDate == '14' || scheDate == '24') {
        $('#<%=hdnMealDay.ClientID %>').val('4');
    } else if (scheDate == '05' || scheDate == '15' || scheDate == '25') {
        $('#<%=hdnMealDay.ClientID %>').val('5');
    } else if (scheDate == '06' || scheDate == '16' || scheDate == '26') {
        $('#<%=hdnMealDay.ClientID %>').val('6');
    } else if (scheDate == '07' || scheDate == '17' || scheDate == '27') {
        $('#<%=hdnMealDay.ClientID %>').val('7');
    } else if (scheDate == '08' || scheDate == '18' || scheDate == '28') {
        $('#<%=hdnMealDay.ClientID %>').val('8');
    } else if (scheDate == '09' || scheDate == '19' || scheDate == '29') {
        $('#<%=hdnMealDay.ClientID %>').val('9');
    } else if (scheDate == '10' || scheDate == '20' || scheDate == '30') {
        $('#<%=hdnMealDay.ClientID %>').val('10');
    } else {
        $('#<%=hdnMealDay.ClientID %>').val('11');
    }
    //#endregion

    $('#lblEntryPopupAddData').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        var day = $('#<%=hdnMealDay.ClientID %>').val();
        var physicianID = $('#<%=hdnParamedicIDDt.ClientID %>').val();
        var physicianCode = $('#<%=hdnParamedicCodeDt.ClientID %>').val();
        var physicianName = $('#<%=hdnParamedicNameDt.ClientID %>').val();
        cboMealTime.SetValue('');
        $('#<%=hdnEntryID.ClientID %>').val('');
        $('#<%=txtMealDay.ClientID%>').val(day);
        $('#<%=hdnParamedicID.ClientID%>').val(physicianID);
        $('#<%=txtParamedicCode.ClientID%>').val(physicianCode);
        $('#<%=txtParamedicName.ClientID %>').val(physicianName);
        $('#<%=hdnMealPlanID.ClientID%>').val('');
        $('#<%=txtMealPlanCode.ClientID%>').val('');
        $('#<%=txtMealPlanName.ClientID %>').val('');
        $('#<%=txtRemarks.ClientID %>').val('');
        $('#<%=hdnClassID.ClientID %>').val(entity.ClassID);
        $('#containerEntry').show();
    });

    function onCbpPopupProcessEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var NutritionOrderID = s.cpNutritionOrderID;
                $('#containerEntry').hide();
                cbpPopupView.PerformCallback('refresh');
            }
        }
        if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var NutritionOrderID = s.cpNutritionOrderID;
                cbpPopupView.PerformCallback('refresh');
            }
        }
    }
    
</script>
<input type="hidden" value="" id="hdnNutritionOrderID" runat="server" />
<input type="hidden" value="" id="hdnWatermarkText" runat="server" />
<input type="hidden" value="" id="hdnOrderDtID" runat="server" />
<input type="hidden" value="" id="hdnMealDay" runat="server" />
<input type="hidden" value="" id="hdnDefaultChargeClassID" runat="server" />
<input type="hidden" value="" id="hdnClassID" runat="server" />
<input type="hidden" value="" id="hdnParamedicIDDt" runat="server" />
<input type="hidden" value="" id="hdnParamedicCodeDt" runat="server" />
<input type="hidden" value="" id="hdnParamedicNameDt" runat="server" />
<input type="hidden" value="" id="hdnScheduleDate" runat="server" />
<div class="pageTitle">
    <%=GetLabel("Detail Order Makanan")%></div>
<div style="max-height: 500px; overflow-y: auto" id="containerPopup">
    <table style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table width="100%">
                    <tr>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label id="Label1" class="lblNormal">
                                            <%=GetLabel("No. Order")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNutritionOrderNo" Width="190px" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="75px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblDiagnose">
                                            <%=GetLabel("Diagnose")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDiagnoseID" Width="120px" ReadOnly="true" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDiagnoseName" ReadOnly="true" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <%=GetLabel("Tanggal") %>
                                        /
                                        <%=GetLabel("Jam") %>
                                        Order
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td style="padding-right: 1px">
                                                    <asp:TextBox ID="txtOrderDate" Width="100px" CssClass="datepicker" runat="server"
                                                        ReadOnly="true" />
                                                </td>
                                                <td style="width: 5px">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                        ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="75px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" style="width: 55px">
                                        <%=GetLabel("Agama") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReligion" Width="80px" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="containerEntry" style="margin-top: 4px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsTrx" style="margin: 0">
                        <input type="hidden" value="" id="hdnEntryID" runat="server" />
                        <table class="tblEntryDetail">
                            <colgroup>
                                <col width="10px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Meal Time")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboMealTime" ClientInstanceName="cboMealTime"
                                        Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <label class="lblLink lblMandatory" id="lblMealPlan">
                                        <%=GetLabel("Meal Plan") %></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="hidden" value="" id="hdnMealPlanID" runat="server" />
                                                <asp:TextBox runat="server" ID="txtMealPlanCode" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMealPlanName" Width="200px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Meal Status")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboMealStatus" ClientInstanceName="cboMealStatus"
                                        Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Meal Day")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtMealDay" Width="70px" CssClass="number" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <label class="lblLink lblMandatory" id="lblParamedic">
                                        <%=GetLabel("Paramedic") %></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="hidden" value="" id="hdnParamedicID" runat="server" />
                                                <asp:TextBox runat="server" ID="txtParamedicCode" ReadOnly="true" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtParamedicName" Width="200px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%;" valign="top">
                                    <%=GetLabel("Remarks") %>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" Width="300px" Rows="2" ID="txtRemarks" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <input type="button" id="btnPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                        ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdPopupView" runat="server" CssClass="grdView notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgPopupEdit <%# Eval("GCItemDetailStatus").ToString() != "X121^002" ? "imgDisabled" : "imgLink"%>"
                                                        title='<%=GetLabel("Edit")%>' src='<%# Eval("GCItemDetailStatus").ToString() != "X121^002" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" />
                                                    <img class="imgPopupDelete <%# Eval("GCItemDetailStatus").ToString() != "X121^002" ? "imgDisabled" : "imgLink"%>"
                                                        title='<%=GetLabel("Delete")%>' src='<%# Eval("GCItemDetailStatus").ToString() != "X121^002" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" value="<%#Eval("NutritionOrderDtID") %>" bindingfield="NutritionOrderDtID" />
                                                    <input type="hidden" value="<%#Eval("GCMealTime") %>" bindingfield="GCMealTime" />
                                                    <input type="hidden" value="<%#Eval("GCMealStatus") %>" bindingfield="GCMealStatus" />
                                                    <input type="hidden" value="<%#Eval("MealDay") %>" bindingfield="MealDay" />
                                                    <input type="hidden" value="<%#Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                    <input type="hidden" value="<%#Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                    <input type="hidden" value="<%#Eval("MealPlanID") %>" bindingfield="MealPlanID" />
                                                    <input type="hidden" value="<%#Eval("MealPlanCode") %>" bindingfield="MealPlanCode" />
                                                    <input type="hidden" value="<%#Eval("MealPlanName") %>" bindingfield="MealPlanName" />
                                                    <input type="hidden" value="<%#Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="NutritionOrderDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="MealTime" HeaderText="Meal Time" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="MealPlanName" HeaderText="Meal Plan" HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="MealDay" HeaderText="Hari Ke-" HeaderStyle-Width="70px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="600px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                                        <span class="lblLink" id="lblEntryPopupAddData">
                                            <%= GetLabel("Tambah Data")%></span>
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
                    ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
                    <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcessEndCallback(s); }" />
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
