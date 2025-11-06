<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="EpisodeMedicationList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.EpisodeMedicationList" EnableViewState="false" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMPFrame" runat="server">
    <style>
        .pnlContainerGridMedicationSchedule {
            width: 900px !important;
            overflow-x: auto;
        }
        
        .tdDrugInfoLabel 
        {
            text-align:right;
            vertical-align:top;
            font-size:10pt;
            font-style:italic
        }
        
        .tdDrugInfoValue 
        {
            vertical-align:top;
            font-size:11pt;
            font-weight:bold;            
        }        
        
        .txtMedicationTime
        {
            width:70px;
            text-align:center;
            color: Blue;
            font-size:larger
        }
        
        .btnMedicationStatus {background-color:Green;}
        
        .highlight    {  background-color:#FE5D15; color: White; }       
    </style>
    <script type="text/javascript">
        $(function () {
            $('.btnEdit').live('click', function () {
                alert('Update Medication Status : This feature is not available yet.');
            });

            $('.btnInfo').live('click', function () {
                alert('Discontinue Information : This feature is not available yet.');
            });

            $('.imgCompound').live('click', function () {
                var url = ResolveUrl("~/Program/Information/CompoundDetailCtl.ascx");
                var param = $('#<%=hdnPrescriptionOrderDtIDCBCtl.ClientID %>').val();
                openUserControlPopup(url, param, 'UDD - Compound Detail', 650, 300);
            });
        });

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
                    $('.grdPrescriptionOrderDt tr:eq(1)').click();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdPrescriptionOrderDt tr:eq(1)').click();
        }
        //#endregion

        $('.lblDispenseQuantity.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var value = $tr.find('.hdnCfDispenseQuantity').val();
            if (value != '0') {
                var orderDetailID = $tr.find('.hdnOrderDetailID').val();
                var itemID = $tr.find('.hdnItemID').val();
                var itemName = $tr.find('.hdnDrugName').val();
                var paramedicName = $tr.find('.hdnParamedicName').val();
                var param = orderDetailID + '|' + itemID + '|' + itemName + '|' + paramedicName;
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationList/MedicationDispensedInfoCtl.ascx");
                openUserControlPopup(url, param, 'Medication - Dispensed Detail', 800, 500);
            }
        });

        $('.lblTakenQuantity.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var value = $tr.find('.hdnCfTakenQuantity').val();
            if (value != '0') {
                var orderDetailID = $tr.find('.hdnOrderDetailID').val();
                var pastMedicationID = $tr.find('.hdnPastMedicationID').val();
                var itemID = $tr.find('.hdnItemID').val();
                var itemName = $tr.find('.hdnDrugName').val();
                var paramedicName = $tr.find('.hdnParamedicName').val();
                var param = orderDetailID + '|' + pastMedicationID + '|' + itemID + '|' + itemName + '|' + paramedicName;
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationList/MedicationTakenInfoCtl.ascx");
                openUserControlPopup(url, param, 'Medication - Administration Detail', 800, 500);
            }
        });

        function onRefreshList() {
            cbpView.PerformCallback('refresh');
        }
    </script>
    <input type="hidden" value="" id="hdnPrescriptionOrderDtIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnVisitIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="1" id="hdnDisplayMode" runat="server" />
    <input type="hidden" value="1" id="hdnMedicationStatus" runat="server" />
    <table style="width: 100%" >
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
            <td valign="top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
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
                                                <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                    <div>
                                                        <%=GetLabel("End Date")%></div>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <div style="font-weight:bold">
                                                        <%=GetLabel("QUANTITY") %></div>
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
                                                <th style="width: 60px;">
                                                    <div style="font-weight:bold">
                                                        <%=GetLabel("Dispensed") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div style="font-weight:bold">
                                                        <%=GetLabel("Taken") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div style="font-weight:bold">
                                                        <%=GetLabel("Balance") %></div>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center" style="width:30px;background:#ecf0f1; vertical-align:middle"><asp:CheckBox ID="chkIsUsingUDD" runat="server" Enabled="false" CssClass="chkIsUsingUDD" Checked='<%# Eval("IsUsingUDD")%>' /></td>
                                            <td align="center" style="background:#ecf0f1; vertical-align:middle">
                                                <div <%# Eval("IsUsingUDD").ToString() != "True" ? "Style='display:none'":"" %>>
                                                    <img id="imgStatusImageUri" runat="server" width="24" height="24" alt="" visible="true" src="" />    
                                                </div>
                                            </td>
                                            <td>
                                                <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                                <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" class="hdnOrderDetailID"  />
                                                <input type="hidden" value="<%#:Eval("PastMedicationID") %>" bindingfield="PastMedicationID" class="hdnPastMedicationID"  />
                                                <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" class = "hdnItemID" />
                                                <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" class="hdnDrugName"  />
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
                                                <input type="hidden" value="<%#:Eval("cfDispenseQuantity") %>" bindingfield="cfDispenseQuantity" class="hdnCfDispenseQuantity" />
                                                <input type="hidden" value="<%#:Eval("cfTakenQuantity") %>" bindingfield="cfTakenQuantity" class="hdnCfTakenQuantity" />
                                                <table>
                                                    <tr>
                                                        <td><div><b><%#: Eval("DrugName")%></b></div></td>
                                                        <td rowspan="2">&nbsp;</td>
                                                        <td rowspan="2">
                                                            <div><img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px; min-width: 30px; float: left;' /></div>
                                                            <div><img class="imgIsHAM blink-alert" src='<%# ResolveUrl("~/Libs/Images/Status/ham.png")%>' alt="" style='<%# Eval("IsHAM").ToString() == "False" ? "display:none;": "" %> max-width:30px; cursor:pointer; min-width: 30px; float: left;' /></div>
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
                                            <td align="center">
                                                <div><%#: Eval("cfEndDate")%></div>
                                            </td>
                                            <td valign="middle" style="background:#ecf0f1">
                                                <div style="text-align: right;color:Black">
                                                    <label class="lblDispenseQuantity lblLink"><%#:Eval("cfDispenseQuantity", "{0:N}")%></label>                                                    
                                                </div>
                                            </td>
                                            <td valign="middle" style="background:#ecf0f1">
                                                <div style="text-align: right;color:blue">
                                                    <label class="lblTakenQuantity lblLink"><%#:Eval("cfTakenQuantity", "{0:N}")%></label>                                                    
                                                </div>
                                            </td>
                                            <td valign="middle" style="background:#ecf0f1">
                                                <div style="text-align: right;color:Black">
                                                    <%#: Eval("cfRemainingQuantity")%>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
