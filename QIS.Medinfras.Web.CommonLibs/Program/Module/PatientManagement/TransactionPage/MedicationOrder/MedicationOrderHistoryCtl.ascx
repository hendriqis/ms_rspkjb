<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationOrderHistoryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationOrderHistoryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_prescriptioncompoundentryctl">
    function onCbpViewMedicationOrderHistoryEndCallback(s) {
        hideLoadingPanel();
    }

    $('#<%=grdView.ClientID %> .chkIsSelectedMedication input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelectedMedication input').live('change', function () {
        $tr = $(this).closest('tr');
        if ($(this).is(':checked')) {
            $tr.find('.txtDosingMedicationOrderNow').removeAttr('readonly');
            $tr.find('.txtQtyMedicationOrderNow').removeAttr('readonly');
        }
        else {
            $tr.find('.txtDosingMedicationOrderNow').attr('readonly', 'readonly');
            $tr.find('.txtQtyMedicationOrderNow').attr('readonly', 'readonly');
        }
    });

    function getCheckedMember(errMessage) {
        var lstSelectedMember = [];
        var lstSelectedMemberDrugID = [];
        var lstSelectedMemberSignaID = [];
        var lstSelectedMemberCoenamRuleID = [];
        var lstSelectedMemberDosingDuration = [];
        var lstSelectedMemberGCDosingFrequency = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberGCItemUnit = [];
        var result = '';

        $('#<%=grdView.ClientID %> .chkIsSelectedMedication input:checked').each(function () {
            $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').html();
            var signa = $tr.find('.divSignaID').html();
            var coenamRule = $tr.find('.divCoenamRuleID').html();
            var dosingDuration = $tr.find('.txtDosingMedicationOrderNow').val();
            var GCDosingFrequency = $tr.find('.hdnDosingFrequency').val();
            var qty = parseFloat($tr.find('.txtQtyMedicationOrderNow').val());
            var GCItemUnit = $tr.find('.hdnGCItemUnit').val();

            lstSelectedMember.push(key);
            lstSelectedMemberSignaID.push(signa);
            lstSelectedMemberCoenamRuleID.push(coenamRule);
            lstSelectedMemberDosingDuration.push(dosingDuration);
            lstSelectedMemberGCDosingFrequency.push(GCDosingFrequency);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberGCItemUnit.push(GCItemUnit);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberSignaID.ClientID %>').val(lstSelectedMemberSignaID.join(','));
        $('#<%=hdnSelectedMemberCoenamRuleID.ClientID %>').val(lstSelectedMemberCoenamRuleID.join(','));
        $('#<%=hdnSelectedMemberDosingDuration.ClientID %>').val(lstSelectedMemberDosingDuration.join(','));
        $('#<%=hdnSelectedMemberGCDosingFrequency.ClientID %>').val(lstSelectedMemberGCDosingFrequency.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberGCItemUnit.ClientID %>').val(lstSelectedMemberGCItemUnit.join(','));
    }

    function onCboSignaNameValueChanged(s) {
        $tr = $(s.GetInputElement()).closest('tr').parent().closest('tr');
        $tr.find('.divSignaID').html(s.GetValue());

        var filterExpression = "SignaID = '" + s.GetValue() + "' AND IsDeleted = 0";
        Methods.getObject('GetSignaList', filterExpression, function (result) {
            if (result != null) {
                $('.hdnDose').val(result.Dose);
                $('.hdnFrequency').val(result.Frequency);
            }
            else {
                $('.hdnDose').val('0');
                $('.hdnFrequency').val('0');
            }
        });
    }

    function onCboCoenamRuleValueChanged(s) {
        $tr = $(s.GetInputElement()).closest('tr').parent().closest('tr');
        $tr.find('.divCoenamRuleID').html(s.GetValue());
    }

    $('.txtDosingMedicationOrderNow').live('change', function () {
        var dosing = $(this).val();
        $tr = $(this).parent().parent();
        var dose = $tr.find('.hdnDose').val();
        var frequency = $tr.find('.hdnFrequency').val();

        var qty = dosing * frequency * dose;
        $tr.find('.txtQtyMedicationOrderNow').val(qty);
    });

    $('.txtQtyMedicationOrderNow').live('change', function () {
        var qty = $(this).val();
        $tr = $(this).parent().parent();
        var dose = $tr.find('.hdnDose').val();
        var frequency = $tr.find('.hdnFrequency').val();

        var dosing = qty / (frequency * dose);
        $tr.find('.txtDosingMedicationOrderNow').val(dosing);
    });

    function onBeforeSaveRecord(errMessage) {
        errMessage.text = '';
        if (IsValid(null, 'fsMedicationOrderHistory', 'mpMedicationOrderHistoryDosing')) {
            if (IsValid(null, 'fsMedicationOrderHistory', 'mpMedicationOrderHistoryQty')) {
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
    
</script>

<input type="hidden" id="hdnVisitID" runat="server" />
<input type="hidden" id="hdnParamedicID" runat="server" />
<input type="hidden" id="hdnClassID" runat="server" />
<input type="hidden" id="hdnPrescriptionType" runat="server" />
<input type="hidden" id="hdnDispensaryUnit" runat="server" />
<input type="hidden" id="hdnLocationID" runat="server" />
<input type="hidden" id="hdnPrescriptionOrderID" runat="server" />
<input type="hidden" id="hdnDepartmentID" runat="server" value="" />
<input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnRegistrationID" runat="server" value="" />
<input type="hidden" id="hdnMRN" runat="server" value="" />
<input type="hidden" id="hdnParam" runat="server" value="" />
<input type="hidden" id="hdnReturnType" runat="server" value="" />
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberSignaID" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberCoenamRuleID" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberDosingDuration" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberGCDosingFrequency" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberGCItemUnit" runat="server" value="" />
<input type="hidden" id="hdnIsDrugChargesJustDistributionHS" runat="server" value="0" />

<div style="height: 500px; overflow-y: auto">
    <div style="height: 450px; overflow-y: auto">
        <fieldset id="fsMedicationOrderHistory">
            <dxcp:ASPxCallbackPanel ID="cbpViewMedicationOrderHistory" runat="server" Width="100%" ClientInstanceName="cbpViewMedicationOrderHistory"
                ShowLoadingPanel="false" OnCallback="cbpViewMedicationOrderHistory_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}"
                    EndCallback="function(s,e){ onCbpViewMedicationOrderHistoryEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelectedMedication" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Obat/Alkes" >
                                        <ItemTemplate>
                                            <b><%#:Eval("ItemName1")%></b>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="170px" HeaderText="Signa" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div style="display:none" class="divSignaID" runat="server" id="divSignaID"></div>
                                            <dxe:ASPxComboBox ID="cboSignaName" CssClass="cboSignaName" runat="server" Width="150px">
                                                <ClientSideEvents ValueChanged="function(s,e){ onCboSignaNameValueChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                            <input type="hidden" class="hdnDose" />
                                            <input type="hidden" class="hdnFrequency" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="170px" HeaderText="Coenam Rule" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div style="display:none" class="divCoenamRuleID" runat="server" id="divCoenamRuleID"></div>
                                            <dxe:ASPxComboBox ID="cboCoenamRule" CssClass="cboCoenamRule" runat="server" Width="150px">
                                                <ClientSideEvents ValueChanged="function(s,e){ onCboCoenamRuleValueChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderText="Durasi Terakhir" >
                                        <ItemTemplate>
                                            <input type="number" value='<%#:Eval("DosingDuration")%>' class="txtLastDuration" min="0" readonly="readonly" style="width:40px" /> <%#:Eval("DosingFrequency") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderText="Jumlah Terakhir" >
                                        <ItemTemplate>
                                            <input type="number" value='<%#:Eval("UsedQuantity")%>' class="txtUsedQuantity" min="0" readonly="readonly" style="width:40px" /> <%#:Eval("ItemUnit") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderText="Durasi Resep" >
                                        <ItemTemplate>
                                            <input type="hidden" value='<%#:Eval("GCDosingFrequency")%>' class="hdnDosingFrequency" />
                                            <input type="number" value='<%#:Eval("DosingDuration")%>' validationgroup="mpMedicationOrderHistoryDosing" class="txtDosingMedicationOrderNow" min="0" value="0" readonly="readonly" style="width:40px" /> <%#:Eval("DosingFrequency") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderText="Jumlah Obat" >
                                        <ItemTemplate>
                                            <input type="hidden" value='<%#:Eval("GCItemUnit")%>' class="hdnGCItemUnit" />
                                            <input type="number" value='<%#:Eval("UsedQuantity")%>' validationgroup="mpMedicationOrderHistoryQty" class="txtQtyMedicationOrderNow" min="0" value="0" readonly="readonly" style="width:40px" /> <%#:Eval("ItemUnit") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Tidak ada history resep untuk pasien ini.")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </fieldset>
    </div>
</div>
   