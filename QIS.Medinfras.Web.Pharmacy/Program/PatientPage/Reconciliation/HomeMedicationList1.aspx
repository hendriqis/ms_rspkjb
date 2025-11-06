<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="HomeMedicationList1.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.HomeMedicationList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table>
        <tr>
            <td><%=GetLabel("View Type") %></td>
            <td>
                <asp:DropDownList ID="ddlViewType" runat="server">
                    <asp:ListItem Text="All" Value="0" Selected="True" />
                    <asp:ListItem Text="Current Medication" Value="1" />
                    <asp:ListItem Text="Past Medication" Value="2" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnCurrentMedicationDiscontinue" runat="server" CRUDMode="U"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div><%=GetLabel("Discontinue")%></div></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.focus').removeClass('focus');
                $(this).addClass('focus');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtStartDate.ClientID %>');

            $('#<%=chkIsUntilNow.ClientID %>').change(function () {
                var isEnabled = !$(this).is(":checked");
                if (isEnabled)
                    $('#<%=txtDuration.ClientID %>').removeAttr('readonly');
                else
                    $('#<%=txtDuration.ClientID %>').attr('readonly', 'readonly');
            });
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.focus'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.focus');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=txtGenericName.ClientID %>').focus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=txtGenericName.ClientID %>').val(entity.GenericName);
                $('#<%=hdnDrugID.ClientID %>').val(entity.ItemID);
                $('#<%=hdnDrugName.ClientID %>').val(entity.DrugName);
                cboForm.SetValue(entity.GCDrugForm);
                $('#<%=txtPurposeOfMedication.ClientID %>').val(entity.MedicationPurpose);
                $('#<%=txtStrengthAmount.ClientID %>').val(entity.Dose);
                cboStrengthUnit.SetValue(entity.GCDoseUnit);
                cboFrequencyTimeline.SetValue(entity.GCDosingFrequency);
                $('#<%=txtFrequencyNumber.ClientID %>').val(entity.Frequency);
                $('#<%=txtDosingDose.ClientID %>').val(entity.NumberOfDosage);
                cboDosingUnit.SetValue(entity.GCDosingUnit);
                $('#<%=txtDuration.ClientID %>').val(entity.DosingDuration);
                cboMedicationRoute.SetValue(entity.GCRoute);
                $('#<%=txtStartDate.ClientID %>').val(entity.StartDate);
                if (entity.DosingDuration == "0")
                    $('#<%=chkIsUntilNow.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsUntilNow.ClientID %>').prop('checked', false);
                if (entity.ItemID != '0')
                    ledDrugName.SetValue(entity.ItemID);
                else
                    ledDrugName.SetText(entity.DrugName);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=txtGenericName.ClientID %>').val('');
                $('#<%=hdnDrugID.ClientID %>').val('');
                $('#<%=hdnDrugName.ClientID %>').val('');
                cboForm.SetValue('');
                $('#<%=txtPurposeOfMedication.ClientID %>').val('');
                $('#<%=txtStrengthAmount.ClientID %>').val('');
                cboStrengthUnit.SetValue('');
                cboFrequencyTimeline.SetValue('');
                $('#<%=txtFrequencyNumber.ClientID %>').val('');
                $('#<%=txtDosingDose.ClientID %>').val('');
                cboDosingUnit.SetValue('');
                $('#<%=txtDuration.ClientID %>').val('');
                $('#<%=chkIsUntilNow.ClientID %>').prop('checked', false);
                cboMedicationRoute.SetValue('');
                $('#<%=txtStartDate.ClientID %>').val('');
                ledDrugName.SetValue('');
            }
            $('#<%=chkIsUntilNow.ClientID %>').change();
        }
        //#endregion

        function onLedDrugNameLostFocus(led) {
            var drugID = led.GetValueText();
            $('#<%=hdnDrugID.ClientID %>').val(drugID);
            $('#<%=hdnDrugName.ClientID %>').val(led.GetDisplayText());

            if (drugID != '') {
                var filterExpression = "ItemID = " + drugID;
                Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
                    $('#<%=txtStrengthAmount.ClientID %>').val(result.Dose);
                    cboStrengthUnit.SetValue(result.GCDoseUnit);
                    cboForm.SetValue(result.GCDrugForm);
                    $('#<%=txtDosingDose.ClientID %>').val('1');
                    cboDosingUnit.SetValue(result.GCItemUnit);
                });
            }
        }

        $(function () {
            $('#<%=ddlViewType.ClientID %>').change(function () {
                onPatientListEntryCancelEntryRecord();
                cbpView.PerformCallback('refresh');
            });

            $('#<%=btnCurrentMedicationDiscontinue.ClientID %>').click(function () {
                $tr = getSelectedRow();
                if ($tr.find('input[bindingfield=IsCurrentMedication]').val() == 'True') {
                    var id = $tr.find('.keyField').html();
                    var url = ResolveUrl("~/Program/PatientPage/Subjective/CurrentMedication/CurrentMedicationDiscontinueCtl.ascx");
                    openUserControlPopup(url, id, 'Discontinue Medication', 800, 280);
                }
                else
                    showToast('Warning', 'Cannot Discontinue This Record.');
            });
        });

        function onTxtQuickEntrySearchClick(s) {
            onPatientPageListEntryQuickEntrySave(s.GetValue());
        }

        function onAfterSaveQuickEntryRecord(val) {
            txtQuickEntry.SetText('');
        }
    </script>
</asp:Content>

<asp:Content ID="ctnQuickEntry" ContentPlaceHolderID="plhQuickEntry" runat="server">
    <table>
        <tr>
            <td style="width:100px" class="tdLabel">
                <%=GetLabel("Quick Entry")%>
            </td>
            <td>
                <qis:QISQuickEntry runat="server" ClientInstanceName="txtQuickEntry" ID="txtQuickEntry" Width="1000px">
                    <ClientSideEvents SearchClick="function(s){ onTxtQuickEntrySearchClick(s); }" />
                    <QuickEntryHints>
                        <qis:QISQuickEntryHint Text="Drug Name" ValueField="ItemID" TextField="ItemName1" Description="Item Name" FilterExpression="IsDeleted = 0" MethodName="GetvDrugInfoList">
                            <Columns>
                                <qis:QISQuickEntryHintColumn Caption="Item Code" FieldName="ItemCode" Width="200px" />
                                <qis:QISQuickEntryHintColumn Caption="Item Name" FieldName="ItemName1" Width="600px" />
                            </Columns>
                        </qis:QISQuickEntryHint>
                        <qis:QISQuickEntryHint Text="Dose" Description="Dose" />
                        <qis:QISQuickEntryHint Text="Dose Unit" ValueField="StandardCodeID" TextField="StandardCodeName" Description="i.e. Ampul / Buah / Mg / etc" MethodName="GetStandardCodeList" FilterExpression="ParentID = 'X003'">
                            <Columns>
                                <qis:QISQuickEntryHintColumn Caption="Unit" FieldName="StandardCodeName" Width="300px" />
                            </Columns>
                        </qis:QISQuickEntryHint> 
                        <qis:QISQuickEntryHint Text="Signa" ValueField="SignaID" TextField="SignaLabel" Description="i.e. S 1 dd 1 tab / S 1 dd 1 tab ac / S 1 dd 1 tab pc / etc" MethodName="GetSignaList" FilterExpression="IsDeleted = 0">
                            <Columns>
                                <qis:QISQuickEntryHintColumn Caption="Signa Label" FieldName="SignaLabel" Width="100px" />
                                <qis:QISQuickEntryHintColumn Caption="Text" FieldName="SignaName1" Width="300px" />
                            </Columns>
                        </qis:QISQuickEntryHint>   
                        <qis:QISQuickEntryHint Text="Start Date" Description="Format : yyyyMMdd" />   
                        <qis:QISQuickEntryHint Text="Duration" Description="Until Now : 0" />     
                    </QuickEntryHints>
                </qis:QISQuickEntry>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />

    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <table style="width:100%">
                    <colgroup>
                        <col style="width:150px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Generic Name")%></label></td>
                        <td><asp:TextBox runat="server" ID="txtGenericName" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Drug Name")%></label></td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnDrugID" runat="server" />
                            <input type="hidden" value="" id="hdnDrugName" runat="server" />
                            <qis:QISSearchTextBox ID="ledDrugName" ClientInstanceName="ledDrugName" runat="server" Width="300px" IsAllowOtherValue="True"
                                ValueText="ItemID" FilterExpression="IsDeleted = 0" DisplayText="ItemName1" MethodName="GetvDrugInfoList" >
                                <ClientSideEvents ValueChanged="function(s){ onLedDrugNameLostFocus(s); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Generic Name" FieldName="GenericName" Description="i.e. paracetamol" Width="100px" />
                                    <qis:QISSearchTextBoxColumn Caption="Item Name" FieldName="ItemName1" Description="i.e. Panadol" Width="300px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Strength")%></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtStrengthAmount" runat="server" Width="100%" CssClass="number" /></td>
                                    <td>&nbsp;</td>
                                    <td><dxe:ASPxComboBox ID="cboStrengthUnit" ClientInstanceName="cboStrengthUnit" runat="server" Width="100%" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Form")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboForm" ClientInstanceName="cboForm" runat="server" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Purpose Of Medication")%></label></td>
                        <td><asp:TextBox ID="txtPurposeOfMedication" Width="300px" runat="server" TextMode="MultiLine" /></td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table style="width:100%">
                    <colgroup>
                        <col style="width:150px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Frequency")%></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col style="width:100px"/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" /></td>
                                    <td>&nbsp;</td>
                                    <td><dxe:ASPxComboBox ID="cboFrequencyTimeline" ClientInstanceName="cboFrequencyTimeline" runat="server" Width="100%" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dosing")%></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col style="width:100px"/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" /></td>
                                    <td>&nbsp;</td>
                                    <td><dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server" Width="100%" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Medication Route")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute" runat="server" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Start Date")%></label></td>
                        <td><asp:TextBox runat="server" ID="txtStartDate" CssClass="datepicker" Width="120px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Until Now")%></label></td>
                        <td><asp:CheckBox runat="server" ID="chkIsUntilNow" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Duration")%></label></td>
                        <td><asp:TextBox runat="server" ID="txtDuration" Width="100px" CssClass="number" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                        <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" />
                                        <input type="hidden" value="<%#:Eval("GCDrugForm") %>" bindingfield="GCDrugForm" />
                                        <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                        <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
                                        <input type="hidden" value="<%#:Eval("GCDosingFrequency") %>" bindingfield="GCDosingFrequency" />
                                        <input type="hidden" value="<%#:Eval("Frequency") %>" bindingfield="Frequency" />
                                        <input type="hidden" value="<%#:Eval("NumberOfDosage") %>" bindingfield="NumberOfDosage" />
                                        <input type="hidden" value="<%#:Eval("GCDosingUnit") %>" bindingfield="GCDosingUnit" />
                                        <input type="hidden" value="<%#:Eval("DosingDuration") %>" bindingfield="DosingDuration" />
                                        <input type="hidden" value="<%#:Eval("GCRoute") %>" bindingfield="GCRoute" />
                                        <input type="hidden" value="<%#:Eval("MedicationPurpose") %>" bindingfield="MedicationPurpose" />
                                        <input type="hidden" value="<%#:Eval("StartDateInDatePickerFormat") %>" bindingfield="StartDate" />                                        
                                        <input type="hidden" value="<%#:Eval("IsCurrentMedication") %>" bindingfield="IsCurrentMedication" />                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div><%=GetLabel("Generic")%> - <%=GetLabel("Product")%> - <%=GetLabel("Strength")%> - <%=GetLabel("Form")%></div>
                                        <div>
                                            <div style="color: Blue; width: 35px; float: left;"><%=GetLabel("DOSE")%></div>
                                            <%=GetLabel("Dose")%> - <%=GetLabel("Route")%> - <%=GetLabel("Frequency")%>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("InformationLine1")%></div>
                                        <div>
                                            <div style="color: Blue; width: 35px; float: left;"><%=GetLabel("DOSE")%></div>
                                            <%#: Eval("NumberOfDosage")%>
                                            <%#: Eval("DosingUnit")%>
                                            -
                                            <%#: Eval("Route")%>
                                            -
                                            <%#: Eval("cfDoseFrequency")%>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="StartDateInString" HeaderText="Start Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                <asp:BoundField DataField="DiscontinueDateInString" HeaderText="Discontinue Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>
