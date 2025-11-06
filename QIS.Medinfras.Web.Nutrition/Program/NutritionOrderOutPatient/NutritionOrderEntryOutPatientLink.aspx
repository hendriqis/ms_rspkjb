<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="NutritionOrderEntryOutPatientLink.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionOrderEntryOutPatientLink" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnResultBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnKdGudang" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultChargeClassID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtOrderDate.ClientID %>');

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
                $('#<%=txtMealPlanCode.ClientID%>').val('');
                $('#<%=txtMealPlanCode.ClientID %>').val('');
            });

            //#region Meal Plan
            function OnGetMealPlanFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblMealPlan.lblLink').click(function () {
                openSearchDialog('mealplan', OnGetMealPlanFilterExpression(), function (value) {
                    $('#<%=txtMealPlanCode.ClientID %>').val(value);
                    ontxtMealPlanCodeChanged(value);

                });
            });


            $('#<%=txtMealPlanCode.ClientID %>').change(function () {
                ontxtMealPlanCodeChanged($(this).val());
            });

            function ontxtMealPlanCodeChanged(value) {
                var filterExpression = OnGetMealPlanFilterExpression() + " AND MealPlanCode = '" + value + "'";
                Methods.getObject('GetvMealPlanList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMealPlanID.ClientID%>').val(result.MealPlanID);
                        $('#<%=txtMealPlanName.ClientID %>').val(result.MealPlanName);
                        cbpProcess.PerformCallback('setcbo')
                    }
                    else {
                        $('#<%=hdnMealPlanID.ClientID%>').val('');
                        $('#<%=txtMealPlanCode.ClientID%>').val('');
                        $('#<%=txtMealPlanName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Paramedic
            function OnGetParamedicFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblParamedic.lblLink').click(function () {
                openSearchDialog('paramedic', OnGetParamedicFilterExpression(), function (value) {
                    $('#<%=txtParamedicCode.ClientID %>').val(value);
                    ontxtParamedicCodeChanged(value);
                });
            });

            $('#<%=txtParamedicCode.ClientID %>').change(function () {
                ontxtParamedicCodeChanged($(this).val());
            });

            function ontxtParamedicCodeChanged(value) {
                var filterExpression = OnGetParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParamedicID.ClientID%>').val(result.ParamedicID);
                        $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnParamedicID.ClientID%>').val('');
                        $('#<%=txtParamedicCode.ClientID%>').val('');
                        $('#<%=txtParamedicName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Diagnose
            function OnGetDiagnoseFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblDiagnose.lblLink').click(function () {
                openSearchDialog('diagnose', OnGetDiagnoseFilterExpression(), function (value) {
                    $('#<%=txtDiagnoseID.ClientID %>').val(value);
                    ontxtDiagnoseChanged(value);
                });
            });

            $('#<%=txtDiagnoseID.ClientID %>').change(function () {
                ontxtDiagnoseChanged($(this).val());
            });

            function ontxtDiagnoseChanged(value) {
                var filterExpression = OnGetDiagnoseFilterExpression() + " AND DiagnoseID = '" + value + "'";
                Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    }

                });
            }
            //#endregion

            //#region NutritionOrderHd
            function onGetNutritionOrderFilterExpression() {
                var filterExpression = "LinkField like '" + $('#<%=hdnRegistrationNo.ClientID%>').val() + "%'";
                return filterExpression;
            }

            $('#lblNutritionOrder.lblLink').click(function () {
                openSearchDialog('nutritionorderhd', onGetNutritionOrderFilterExpression(), function (value) {
                    $('#<%=txtNutritionOrderNo.ClientID %>').val(value);
                    ontxtNutritionOrderNoChanged(value);
                });
            });

            function ontxtNutritionOrderNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            $('#<%=btnResultBack.ClientID %>').click(function () {
                document.location = document.referrer;
            });

            $('#btnSave').click(function () {
                if (IsValid(null, 'fsTrx', 'mpTrx'))
                    cbpProcess.PerformCallback('save');
            });
        }

        $('#lblAddData').live('click', function () {
            $('#<%=hdnEntryID.ClientID%>').val('');
            cboMealTime.SetValue('');
            cboMealDay.SetValue($('#<%=hdnMealDay.ClientID %>').val());
            //$('#<%=hdnMealPlanID.ClientID%>').val('');
            //$('#<%=txtMealPlanCode.ClientID%>').val('');
            //$('#<%=txtMealPlanName.ClientID %>').val('');
            $('#<%=txtRemarks.ClientID %>').val('');
            //$('#<%=hdnParamedicID.ClientID%>').val('');
            //$('#<%=txtParamedicCode.ClientID%>').val('');
            //$('#<%=txtParamedicName.ClientID %>').val('');
            $('#containerEntry').show();
        });

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.NutritionOrderDtID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);

            cboMealTime.SetValue(entity.GCMealTime);
            cboMealDay.SetValue(entity.GCMealDay);
            $('#<%=hdnEntryID.ClientID %>').val(entity.NutritionOrderDtID);
            $('#<%=hdnMealPlanID.ClientID %>').val(entity.MealPlanID);
            $('#<%=hdnParamedicID.ClientID%>').val(entity.ParamedicID);
            $('#<%=txtParamedicCode.ClientID%>').val(entity.ParamedicCode);
            $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);
            $('#<%=hdnMealPlanID.ClientID%>').val(entity.MealPlanID);
            $('#<%=txtMealPlanCode.ClientID%>').val(entity.MealPlanCode);
            $('#<%=txtMealPlanName.ClientID %>').val(entity.MealPlanName);
            $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            $('#containerEntry').show();
        });

        function onCbpProcessEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var NutritionOrderID = s.cpNutritionOrderID;
                    onAfterSaveRecordDtSuccess(NutritionOrderID);
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        function onAfterSaveRecordDtSuccess(NutritionOrderID) {
            if ($('#<%=hdnNutritionOrderID.ClientID %>').val() == '0') {
                $('#<%=hdnNutritionOrderID.ClientID %>').val(NutritionOrderID);
                var filterExpression = 'NutritionOrderHdID = ' + NutritionOrderID;

                Methods.getObject('GetNutritionOrderHdList', filterExpression, function (result) {
                    $('#<%=txtNutritionOrderNo.ClientID %>').val(result.NutritionOrderNo);
                });
            }
        }
    </script>
    <input type="hidden" value="" id="hdnNutritionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnWatermarkText" runat="server" />
    <input type="hidden" value="" id="hdnOrderDtID" runat="server" />
    <input type="hidden" value="" id="hdnMealDay" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em">
                <%=GetLabel("Nutrition Order")%></div>
        </div>
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <table class="tblEntryContent">
                                    <colgroup>
                                        <col width="150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label id="lblNutritionOrder" class="lblMandatory lblLink">
                                                <%=GetLabel("No. Order")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNutritionOrderNo" Width="150px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblLink" id="lblDiagnose">
                                                <%=GetLabel("Diagnosis")%></label>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDiagnoseID" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDiagnoseName" ReadOnly="true" Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="tdLabel" style="width: 145px">
                                <%=GetLabel("Tanggal") %>
                                /
                                <%=GetLabel("Jam Order") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px">
                                            <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table>
                        <tr>
                            <td class="tdLabel" style="width: 55px">
                                <%=GetLabel("Diagnosis Text") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiagnose" Width="320px" runat="server" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 55px">
                                <%=GetLabel("Agama") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAgama" Width="80px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </td> </tr>
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
                                <td style="width: 10%">
                                    <label class="lblLink lblMandatory" id="lblMealPlan">
                                        <%=GetLabel("Panel Menu") %></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <input type="hidden" value="" id="hdnMealPlanID" runat="server" />
                                                <asp:TextBox runat="server" ID="txtMealPlanCode" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMealPlanName" Width="200px" ReadOnly="true" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="tdLabel" width="190px" align="right">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Menu Hari Ke-")%></label>
                                            </td>
                                            <td align="right">
                                                <dxe:ASPxComboBox runat="server" ID="cboMealDay" ClientInstanceName="cboMealDay"
                                                    Width="90px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            </tr>
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
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Meal Day")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboMealStatus" ClientInstanceName="cboMealStatus"
                                        Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <label class="lblLink lblMandatory" id="lblParamedic">
                                        <%=GetLabel("Dokter") %></label>
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
                                                <asp:TextBox runat="server" ID="txtParamedicCode" />
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
                                    <%=GetLabel("Remarks")%>
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
                                                <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
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
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="NutritionOrderDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Edit")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Delete")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("NutritionOrderDtID") %>" bindingfield="NutritionOrderDtID" />
                                                    <input type="hidden" value="<%#:Eval("GCMealTime") %>" bindingfield="GCMealTime" />
                                                    <input type="hidden" value="<%#:Eval("GCMealDay") %>" bindingfield="GCMealDay" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                    <input type="hidden" value="<%#:Eval("MealPlanID") %>" bindingfield="MealPlanID" />
                                                    <input type="hidden" value="<%#:Eval("MealPlanCode") %>" bindingfield="MealPlanCode" />
                                                    <input type="hidden" value="<%#:Eval("MealPlanName") %>" bindingfield="MealPlanName" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="MealTime" HeaderText="Meal Time" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MealPlanName" HeaderText="Meal Plan" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MealDay" HeaderText="Hari Ke - " ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="300px" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-Width="300px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div style="width: 100%; text-align: center">
                                        <span class="lblLink" id="lblAddData" style="<%=IsEditable.ToString() == "False" ? "display:none": "" %>">
                                            <%= GetLabel("Add Data")%></span>
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
