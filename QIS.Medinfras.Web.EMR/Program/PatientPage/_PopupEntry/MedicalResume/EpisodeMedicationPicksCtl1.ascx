<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeMedicationPicksCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.EpisodeMedicationPicksCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<style type="text/css">
        .trSelectedItem {background-color: #ecf0f1 !important;}        
        .highlight    {  background-color:#FE5D15; color: White; }       
</style>

<script type="text/javascript" id="dxss_drugsquickpicksHistoryCtl1">
    function onBeforeProcess(param) {
        if (!getCheckedMember()) {
            return false;
        }
        else {
            return true;
        }
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    function getCheckedMember() {
        var lstSelectedMember = [];

        var result = '';
        var count = 0;

        $('.grdNormal .chkIsSelected input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').val();
            var itemName = $tr.find('.itemName').val();
            var signaRule = $tr.find('.signaRule').val();
            var dispenseQty = $tr.find('.dispenseQty').val();
            var IsHasResidualEffect = $tr.find('.IsHasResidualEffect').val();
            var medicationLineText = itemName + ";" + signaRule + ";";
            if (IsHasResidualEffect == "YA") {
                medicationLineText += "Potensi Memiliki Efek Residual :" + IsHasResidualEffect;
            }
            lstSelectedMember.push(medicationLineText);

            count += 1;
        });

        if (count == 0) {
            var messageBody = "Belum ada item yang dipilih.";
            displayMessageBox('Lookup : Riwayat Pengobatan', messageBody);
            return false;
        }
        else {
            $('#<%=hdnSelectedItem.ClientID %>').val(lstSelectedMember.join('|'));
            return true;
        }
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        hideLoadingPanel();

        setPaging($("#pagingPopup"), pageCount, function (page) {
            RefreshGrid(true, page);
        });

        $('#<%=lvwView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=lvwView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnOrderID.ClientID %>').val($(this).find('.keyField').html());
            }
        });
        $('#<%=lvwView.ClientID %> tr:eq(1)').click();
    });

    function RefreshGridDetail(mode, pageNo) {
        getCheckedMember();
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                RefreshGrid(true, page);
            });
            $('#<%=lvwView.ClientID %> tr:eq(1)').click();
        }
    }
    //#endregion

    function onAfterProcessPopupEntry(param) {
        if (typeof onAfterLookUpEpisodePrescription == 'function') {
            onAfterLookUpEpisodePrescription(param);
        }
    }

    function onRefreshList() {
        cbpView.PerformCallback('refresh');
    }
</script>

<div style="padding:3px;">
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnOrderID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnPopupVisitID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedItem" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />    
    <input type="hidden" value="1" id="hdnDisplayMode" runat="server" />
    <input type="hidden" value="1" id="hdnMedicationStatus" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <table border="0" style="width:100%">
                    <colgroup>
                        <col width="150px" />
                        <col width="200px" />
                        <col width="150px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Jenis Obat")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                                Width="100%">
                                <ClientSideEvents ValueChanged="function() { onRefreshList();}" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Status Obat")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboMedicationStatus" ClientInstanceName="cboMedicationStatus" runat="server"
                                Width="100%">
                                <ClientSideEvents ValueChanged="function() { onRefreshList();}" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding:2px;vertical-align:top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="1300px" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage2">
                                <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th rowspan="2" align="center" style="width:30px">
                                                    <div>
                                                        <%=GetLabel("UDD")%></div>
                                                </th>
                                                <th rowspan="2" align="center" style="width:30px">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th rowspan="2" align="left">
                                                    <div>
                                                        <%=GetLabel("Drug Name")%>
                                                        -
                                                        <%=GetLabel("Form")%></div>
                                                    <div>
                                                        <div style="color: Blue; float: left;">
                                                            <%=GetLabel("Generic Name")%></div>
                                                            </div>
                                                </th>
                                                <th colspan="6" align="center">
                                                    <div>
                                                        <%=GetLabel("Signa")%></div>
                                                </th>
                                                <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Start Date")%></div>
                                                </th>
                                                <th colspan="4">
                                                    <div>
                                                        <%=GetLabel("Medication Time") %></div>
                                                </th>
                                                <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                    <div>
                                                        <%=GetLabel("End Date")%></div>
                                                </th>
                                                <th colspan="3">
                                                    <div style="text-align:left; font-weight:bold">
                                                        <%=GetLabel("QUANTITY") %></div>
                                                </th>
                                                <th  align="center" style="padding: 3px; width: 110px;"> <%=GetLabel("Potensi Memiliki Efek Residual") %> </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 40px;">
                                                    <div style="text-align:right">
                                                        <%=GetLabel("Frequency") %></div>
                                                </th>
                                                <th style="width: 40px;text-align:left">
                                                    <div>
                                                        <%=GetLabel("Timeline") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div style="text-align:right">
                                                        <%=GetLabel("Dose") %></div>
                                                </th>
                                                <th style="width: 50px;">
                                                    <div style="text-align:left">
                                                        <%=GetLabel("Unit") %></div>
                                                </th>
                                                <th align="center" style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("PRN")%></div>
                                                </th>
                                                <th style="width: 100px;">
                                                    <div style="text-align:left">
                                                        <%=GetLabel("Route") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div>
                                                        <%=GetLabel("Morning") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div>
                                                        <%=GetLabel("Noon") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div>
                                                        <%=GetLabel("Evening") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div>
                                                        <%=GetLabel("Night") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Dispensed") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Taken") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Balance") %></div>
                                                </th>
                                                <th>
                                                    <div> </div>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th rowspan="2" align="center" style="width:30px">
                                                    <div>
                                                        <%=GetLabel("UDD")%></div>
                                                </th>
                                                <th rowspan="2" align="center" style="width:30px">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th rowspan="2" align="left">
                                                    <div>
                                                        <%=GetLabel("Drug Name")%>
                                                        -
                                                        <%=GetLabel("Form")%></div>
                                                    <div>
                                                        <div style="color: Blue; float: left;">
                                                            <%=GetLabel("Generic Name")%></div>
                                                            </div>
                                                </th>
                                                <th colspan="6" align="center">
                                                    <div>
                                                        <%=GetLabel("Signa")%></div>
                                                </th>
                                                <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Start Date")%></div>
                                                </th>
                                                <th colspan="4">
                                                    <div>
                                                        <%=GetLabel("Medication Time") %></div>
                                                </th>
                                                <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                    <div>
                                                        <%=GetLabel("End Date")%></div>
                                                </th>
                                                <th colspan="3">
                                                    <div style="text-align:left; font-weight:bold">
                                                        <%=GetLabel("QUANTITY") %></div>
                                                </th>
                                                <th  align="center" style="padding: 3px; width: 110px;">
                                                    <div style="text-align:left; font-weight:bold">
                                                        <%=GetLabel("Potensi Memiliki Efek Residual") %></div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 40px;">
                                                    <div style="text-align:right">
                                                        <%=GetLabel("Frequency") %></div>
                                                </th>
                                                <th style="width: 40px;text-align:left">
                                                    <div>
                                                        <%=GetLabel("Timeline") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div style="text-align:right">
                                                        <%=GetLabel("Dose") %></div>
                                                </th>
                                                <th style="width: 50px;">
                                                    <div style="text-align:left">
                                                        <%=GetLabel("Unit") %></div>
                                                </th>
                                                <th align="center" style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("PRN")%></div>
                                                </th>
                                                <th style="width: 100px;">
                                                    <div style="text-align:left">
                                                        <%=GetLabel("Route") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div>
                                                        <%=GetLabel("Morning") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div>
                                                        <%=GetLabel("Noon") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div>
                                                        <%=GetLabel("Evening") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div>
                                                        <%=GetLabel("Night") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Dispensed") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Taken") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Balance") %></div>
                                                </th>
                                                <th>
                                                    <div>   </div>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <div align="center" style="width:30px; align:center">
                                                    <asp:CheckBox ID="chkIsUsingUDD" runat="server" Enabled="false" Checked='<%# Eval("IsUsingUDD")%>' />
                                                </div>
                                            </td>
                                            <td align="center" style="width:30px;background:#ecf0f1; vertical-align:middle">
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </td>
                                            <td>
                                                <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                                <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" class="hdnOrderDetailID"  />
                                                <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" class = "hdnItemID" />
                                                <input type="hidden" value="" bindingfield="GCItemUnit" />
                                                <input type="hidden" value="" bindingfield="GCBaseUnit" />
                                                <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" class="itemName"  /> 
                                                <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" class="hdnParamedicName"  />
                                                <input type="hidden" value="<%#:Eval("GCDosingFrequency") %>" bindingfield="GCDosingFrequency" />
                                                <input type="hidden" value="<%#:Eval("Frequency") %>" bindingfield="Frequency" />
                                                <input type="hidden" value="<%#:Eval("NumberOfDosage") %>" bindingfield="NumberOfDosage" />
                                                <input type="hidden" value="<%#:Eval("DosingDuration") %>" bindingfield="DosingDuration" />
                                                <input type="hidden" value="<%#:Eval("StartDate") %>" bindingfield="StartDate" />
                                                <input type="hidden" value="<%#:Eval("StartTime") %>" bindingfield="StartTime" />                                            
                                                <input type="hidden" value="<%#:Eval("IsMorning") %>" bindingfield="IsMorning" />
                                                <input type="hidden" value="<%#:Eval("IsNoon") %>" bindingfield="IsNoon" />
                                                <input type="hidden" value="<%#:Eval("IsEvening") %>" bindingfield="IsEvening" />
                                                <input type="hidden" value="<%#:Eval("IsNight") %>" bindingfield="IsNight" />
                                                <input type="hidden" value="<%#:Eval("IsAsRequired") %>" bindingfield="IsAsRequired" />
                                                 <input type="hidden" value="<%#:Eval("cfStrIsHasResidualEffect") %>" bindingfield="IsHasResidualEffect" class="IsHasResidualEffect" />
                                                <input type="hidden" value="<%#:Eval("cfDispenseQuantity") %>" bindingfield="cfDispenseQuantity" class="dispenseQty" />
                                                <input type="hidden" value="<%#:Eval("cfSignaRule") %>" bindingfield="cfSignaRule" class="signaRule" />
                                                <input type="hidden" value="<%#:Eval("cfTakenQuantity") %>" bindingfield="cfTakenQuantity" class="hdnCfTakenQuantity" />
                                                <table>
                                                    <tr>
                                                        <td><div><b><%#: Eval("DrugName")%></b></div></td>
                                                        <td rowspan="2">&nbsp;</td>
                                                        <td rowspan="2">
                                                            <div><img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px; min-width: 30px; float: left;' /></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="color: Blue; float: left;"><%#: Eval("GenericName")%></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="float: left; font-style:italic"><%#: Eval("ParamedicName")%></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right">           
                                                <div><%#: Eval("Frequency")%></div>                                             
                                            </td>
                                            <td align="left">           
                                                <div><%#: Eval("DosingFrequency")%></div>                                             
                                            </td>
                                            <td align="right">
                                                <div><%#: Eval("cfNumberOfDosage")%></div>
                                            </td>
                                            <td align="left">
                                                <div> <%#: Eval("DosingUnit")%></div>
                                            </td>
                                            <td align="right">
                                                <div style="text-align: center;">
                                                    <asp:CheckBox ID="chkIsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' />
                                                </div>
                                            </td>
                                            <td>
                                                <div><%#: Eval("Route")%></div>
                                            </td>
                                            <td align="center">
                                                <div><%#: Eval("cfStartDate")%></div>
                                            </td>
                                            <td align="right">
                                                <div style="text-align: center;">
                                                    <asp:CheckBox ID="chkIsMorning" runat="server" Enabled="false" Checked='<%# Eval("IsMorning")%>' />
                                                </div>
                                            </td>
                                            <td align="right">
                                                <div style="text-align: center;">
                                                    <asp:CheckBox ID="chkIsNoon" runat="server" Enabled="false" Checked='<%# Eval("IsNoon")%>' />
                                                </div>
                                            </td>
                                            <td align="right">
                                                <div style="text-align: center;">
                                                    <asp:CheckBox ID="chkIsEvening" runat="server" Enabled="false" Checked='<%# Eval("IsEvening")%>' />
                                                </div>
                                            </td>
                                            <td align="right">
                                                <div style="text-align: center;">
                                                    <asp:CheckBox ID="chkIsNight" runat="server" Enabled="false" Checked='<%# Eval("IsNight")%>' />
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div><%#: Eval("cfEndDate")%></div>
                                            </td>
                                            <td valign="middle" style="background:#ecf0f1">
                                                <div style="text-align: right;color:Black">
                                                    <label class="lblDispenseQuantity lblLink"><%#:Eval("cfDispenseQuantity", "{0:N}")%></label>                                                    
                                                </div>
                                            </td>
                                            <td align="right">           
                                                <div><%#: Eval("cfTakenQuantity")%></div>                                             
                                            </td>
                                            <td align="right">           
                                                <div><%#: Eval("cfRemainingQuantity")%></div>                                             
                                            </td>
                                            <td>
                                                <div><%#: Eval("cfStrIsHasResidualEffect")%> </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>