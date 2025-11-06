<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="MedicationChartVerificationList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationChartVerificationList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh") %></div>
    </li>
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbmedicationdispense.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process") %></div>
    </li>
    <li id="btnDiscontinueMedicationOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbmedicationdiscontinue.png") %>' alt="" title="Discontinue/Stop Pengobatan" /><br style="clear: both" />
        <div>
            <%=GetLabel("Stop") %></div>
    </li>
    <li id="btnExtendOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbmedicationextend.png") %>' alt="" title="Extent/Tambah Durasi Pengobatan" /><br style="clear: both" />
        <div>
            <%=GetLabel("Extend") %></div>
    </li>
    <li id="btnChangeSigna" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbmedicationchangesigna.png") %>' alt="" title="Extent/Tambah Durasi Pengobatan" /><br style="clear: both" />
        <div>
            <%=GetLabel("Change Signa") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
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
        
        
        .highlight    {  background-color:#FE5D15; color: White; }     
        .btnMedicationStatus {background-color:Green;}  
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val($(this).find('.keyField').html());
                    $('#<%=hdnPastMedicationID.ClientID %>').val($(this).find('.hiddenColumn').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=chkIsUsingUDD.ClientID %>').change(function () {
                onRefreshList();
            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                onRefreshList();
            });

            $('#<%=btnProcess.ClientID %>').click(function () {
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationOrder/MedicationAdministrationCtl.ascx");
                var serviceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                var param = serviceUnitID + '|' + $('#<%=hdnMRN.ClientID %>').val();
                if ($('#<%=hdnParamNR0001.ClientID %>').val() == "0") {
                    url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationOrder/MedicationAdministration2Ctl.ascx");
                }
                openUserControlPopup(url, param, 'Medication Administration', 1200, 600);
            });

            $('#<%=btnDiscontinueMedicationOrder.ClientID %>').click(function () {
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/NurseDiscontinueMedicationCtl.ascx");
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                var param = serviceUnitID + '|';
                openUserControlPopup(url, param, 'UDD - Discontinue Medication', 1000, 600);
            });

            $('#<%=btnExtendOrder.ClientID %>').click(function () {
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/NurseExtendOrderDurationCtl.ascx");
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                var param = serviceUnitID + '|';
                openUserControlPopup(url, param, 'UDD - Extend Order Duration', 1200, 600);
            });

            $('#<%=btnChangeSigna.ClientID %>').click(function () {
                var id = $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val();
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/NurseEditMedicationSigna2Ctl.ascx");
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                var param = serviceUnitID + '|';
                openUserControlPopup(url, param, 'UDD - Change Signa', 700, 450);
            });

            $('.btnEdit').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('edit', id);
            });

            $('.btnEditSchedule').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('editSchedule', id);
            });

            $('.btnInfo').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('info', id);
            });

            $('.btnSwitch').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('switch', id);
            });

            $('.btnSwitchHome').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('switch', id);
            });

            $('.imgCompound').live('click', function () {
                var url = ResolveUrl("~/libs/Program/Information/CompoundDetailCtl.ascx");

                var param = $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val();
                openUserControlPopup(url, param, 'UDD - Compound Detail', 650, 300);
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshList();
            });
        });

        var customButtonClickHandler = null;
        function OnCustomProcessButtonClick(type, id) {
            OpenPopupWindow(type, id);
        }

        function OpenPopupWindow(type, id) {
            switch (type) {
                case 'dispense':
                    var url = ResolveUrl("~/Program/Prescription/UDD/MedicationProcess/DispenseMedicationCtl.ascx");
                    var serviceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                    var prescriptionFeeAmount = $('#<%=hdnPrescriptionFeeAmount.ClientID %>').val();
                    var param = serviceUnitID + '|' + prescriptionFeeAmount;
                    openUserControlPopup(url, param, 'UDD - Dispense Medication', 650, 550);
                    break;
                case 'extend':
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/ExtendOrderDurationCtl.ascx");
                    var serviceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                    var param = serviceUnitID + '|';
                    openUserControlPopup(url, param, 'UDD - Extend Order Duration', 1200, 600);
                    break;
                case 'discontinue':
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/DiscontinueMedicationCtl.ascx");
                    var serviceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                    var param = serviceUnitID + '|';
                    openUserControlPopup(url, param, 'UDD - Discontinue Medication', 1000, 600);
                    break;
                case 'print':
                    var url = ResolveUrl("~/Program/Prescription/UDD/MedicationProcess/PrintMedicationLabelCtl.ascx");
                    var serviceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                    var prescriptionFeeAmount = $('#<%=hdnPrescriptionFeeAmount.ClientID %>').val();
                    var param = serviceUnitID + '|' + prescriptionFeeAmount;
                    openUserControlPopup(url, param, 'UDD - Print Medication Label', 650, 550);
                    break;
                case 'edit':
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/EditMedicationAdministrationCtl.ascx");
                    var serviceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                    var param = serviceUnitID + '|' + id;
                    openUserControlPopup(url, param, 'UDD - Administrasi Pemberian Obat', 550, 350);
                    break;
                case 'editSchedule':
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/EditMedicationScheduleCtl.ascx");
                    var prescriptionOrderDtID = $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val();
                    var pastMedicationID = $('#<%=hdnPastMedicationID.ClientID %>').val();
                    var param = id + '|' + prescriptionOrderDtID + '|' + pastMedicationID;

                    openUserControlPopup(url, param, 'UDD - Edit Medication Schedule', 550, 350);
                    break;
                case 'switch':
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/ChangeScheduleSequenceCtl.ascx");
                    var prescriptionOrderDtID = $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val();
                    var pastMedicationID = $('#<%=hdnPastMedicationID.ClientID %>').val();
                    var param = id + '|' + prescriptionOrderDtID + '|' + pastMedicationID;
                    openUserControlPopup(url, param, 'UDD - Change Medication Sequence', 550, 250);
                    break;
                case 'info':
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/MedicationScheduleInfoCtl.ascx");
                    var serviceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                    var param = serviceUnitID + '|' + id;
                    openUserControlPopup(url, param, 'UDD - Medication Schedule Information', 550, 250);
                    break;
                default:
                    alert('Undefined Popup Window Type');
                    break;
            }
        }

        function onRefreshDetailList() {
            cbpViewDt.PerformCallback('refresh');
        }

        function onRefreshList() {
            cbpView.PerformCallback('refresh');
            $('.containerPaging').hide();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
            $('.containerPaging').hide();
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                else {
                    $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val('0');
                    $('#<%=hdnPastMedicationID.ClientID %>').val('0');
                    cbpViewDt.PerformCallback('refresh');
                }
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            $('.containerPaging').hide();
        }
        //#endregion

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalIDReg = window.setInterval(function () {
            onRefreshList();
        }, interval);
    </script>
    <input type="hidden" value="" id="hdnPrescriptionOrderDtID" runat="server" />
    <input type="hidden" value="" id="hdnPastMedicationID" runat="server" />
    <input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="0" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnSequenceForMedicationList" runat="server" />
    <input type="hidden" value="0" id="hdnParamNR0001" runat="server" />
    <table style="width: 100%" >
        <colgroup>
            <col style="width: 40%" />
            <col style="width: 60%" />
        </colgroup>
        <tr>
            <td colspan="2">
                <table border="0">
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Display Option")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                                Width="235px">
                                <ClientSideEvents ValueChanged="function() { onRefreshList();}" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsUsingUDD" runat="server" Text=" Yang menggunakan UDD saja" Checked="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="font-size:0.95em">
                                <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("menit")%>
                            </div>
                        </td>
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
                                <asp:GridView runat="server" ID="grdView" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PastMedicationID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Drug Name")%> 
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr id="trItemName" runat="server">
                                                        <td width="95%">
                                                            <div style="font-size:14pt;color:#0066FF;font-weight:bold;padding-bottom:5px"><%#: Eval("DrugName")%></div>
                                                        </td>
                                                        <td>
                                                            <div><img class="imgCompound" src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px; cursor:pointer; min-width: 30px; float: left;' /></div>
                                                        </td>
                                                        <td>
                                                            <div><img class="imgIsHAM blink-alert" src='<%# ResolveUrl("~/Libs/Images/Status/ham.png")%>' alt="" style='<%# Eval("IsHAM").ToString() == "False" ? "display:none;": "" %> max-width:30px; cursor:pointer; min-width: 30px; float: left;' /></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <colgroup>
                                                        <col style="width:80px"/>
                                                        <col style="width:5px"/>
                                                        <col />
                                                        <col style="width:80px"/>
                                                        <col style="width:5px"/>
                                                        <col style="width:100px"/>
                                                    </colgroup>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("Generic Name")%>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td class="tdDrugInfoValue" colspan="5">
                                                            <%#: Eval("GenericName")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("Signa")%>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td class="tdDrugInfoValue" colspan="4">
                                                            <span><%#: Eval("cfSignaRule")%></span>&nbsp;&nbsp;<asp:CheckBox ID="chkIsAsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' Text="PRN" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("Route")%>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td class="tdDrugInfoValue">
                                                            <%#: Eval("Route")%>
                                                        </td>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("Start Date")%>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td class="tdDrugInfoValue">
                                                            <%#: Eval("cfStartDate")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("Special Instruction")%>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td class="tdDrugInfoValue">
                                                            <span style="color:red"><%#: Eval("MedicationAdministration")%></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("Physician")%></div>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td class="tdDrugInfoValue" colspan="4">
                                                            <%#: Eval("ParamedicName")%>
                                                        </td>
                                                    </tr>
                                                </table>                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="font-weight:bold; color:red;font-size:1.1em;padding-top:7px"><%=GetLabel("no medication schedulled for this patient")%></div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerImgLoadingView" id="containerImgLoadingView" style="display:none">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging" style="display:none">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </td>
            <td valign="top">
                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ $('#containerImgLoadingViewDt').hide(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage2 pnlContainerGridMedicationSchedule">
                                <table id="grdViewDt" class="grdSelected grdPatientPage" cellspacing="0" rules="all" style="overflow: auto;" width="100%">
                                    <tr>
                                        <th style="width:20px">
                                            <<
                                        </th>
                                        <asp:Repeater ID="rptMedicationDateHeader" runat="server">
                                            <ItemTemplate>
                                                <th style="width: 90px; font-weight:bold; font-size: 14pt" align="center">
                                                    <%#: Eval("MedicationDate")%>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <th>
                                            &nbsp;
                                        </th>
                                        <th style="width:20px">
                                            >>
                                        </th>
                                    </tr>
                                    <asp:ListView ID="lvwViewDt" runat="server" OnItemDataBound="lvwViewDt_ItemDataBound" style="width:100%">
                                        <LayoutTemplate>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="background-color:Lightgray; text-align:center;font-size:14pt; font-weight:bold; vertical-align:middle">
                                                    <%#: Eval("SequenceNo")%>
                                                    <input type="hidden" value="<%#:Eval("SequenceNo") %>" class="SequenceNo" />
                                                </td>
                                                <asp:Repeater ID="rptMedicationTimeDetail" runat="server">
                                                    <ItemTemplate>
                                                        <td align="left">
                                                            <table class="rptMedicationTimeDetail" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <div class="divDetailInfo">
                                                                            <%#: Eval("MedicationTime") %>
                                                                        </div>
                                                                        <div class="divSTATUS divOPEN" <%# Eval("MedicationDisplayStatus").ToString() != "OPEN" ? "Style='display:none'":"" %>>
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width:80%">
                                                                                        <%#: Eval("MedicationTime") %>
                                                                                    </td>
                                                                                    <td style="width:20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusIcon">
                                                                                                          <img class="btnSwitch" src='<%# ResolveUrl("~/Libs/Images/Status/switch.png") %>' alt="<%#:Eval("ID") %>" style="cursor:pointer" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusInfo">
                                                                                                        &nbsp;
                                                                                                    </div>                                                                                                    
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>                                                                                
                                                                                </tr>
                                                                            </table>    
                                                                        </div>
                                                                        <div class="divSTATUS divSchedule" <%# Eval("MedicationDisplayStatus").ToString() != "SCHEDULE" ? "Style='display:none'":"" %>>
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width:80%">
                                                                                        <%#: Eval("MedicationTime") %>
                                                                                    </td>
                                                                                    <td style="width:20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusIcon">
                                                                                                          <img class="btnEditSchedule" src='<%# ResolveUrl("~/Libs/Images/Status/scheduled.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusInfo">
                                                                                                        <img class="btnSwitchHome" src='<%# ResolveUrl("~/Libs/Images/Status/switch.png") %>' alt="<%#:Eval("ID") %>" style="style='<%# Eval("IsHomeMedication").ToString() == "False" ? "display:none;": "" %>'" />
                                                                                                    </div>                                                                                                   
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>                                                                                
                                                                                </tr>
                                                                            </table>                                                                        
                                                                        </div>
                                                                        <div class="divSTATUS divTaken" <%# Eval("MedicationDisplayStatus").ToString() != "TAKEN" ? "Style='display:none'":"" %>>                                                                            
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width:80%">
                                                                                        <%#: Eval("MedicationTime") %>
                                                                                    </td>
                                                                                    <td style="width:20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusIcon">
                                                                                                          <img class="btnStatus" src='<%# ResolveUrl("~/Libs/Images/Status/done.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusInfo">
                                                                                                        <img class="btnInfo" src='<%# ResolveUrl("~/Libs/Images/Button/info.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>                                                                                
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="divSTATUS divRefused" <%# Eval("MedicationDisplayStatus").ToString() != "REFUSED" ? "Style='display:none'":"" %>>                                                                            
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width:80%">
                                                                                        <%#: Eval("MedicationTime") %>
                                                                                    </td>
                                                                                    <td style="width:20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusIcon">
                                                                                                          <img class="btnStatus" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusInfo">
                                                                                                        <img class="btnEdit" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>                                                                                
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="divSTATUS divPending" <%# Eval("MedicationDisplayStatus").ToString() != "PENDING" ? "Style='display:none'":"" %>>                                                                            
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width:80%">
                                                                                        <%#: Eval("MedicationTime") %>
                                                                                    </td>
                                                                                    <td style="width:20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusIcon">
                                                                                                          <input type="hidden" class="scheduleID" value = "<%#:Eval("ID") %>" /> 
                                                                                                          <img class="btnStatus" src='<%# ResolveUrl("~/Libs/Images/Status/pending.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusInfo">
                                                                                                        <img class="btnInfo" src='<%# ResolveUrl("~/Libs/Images/Button/info.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>                                                                                
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="divSTATUS divOverDue" <%# Eval("MedicationDisplayStatus").ToString() != "OVERDUE" ? "Style='display:none'":"" %>>                                                                            
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width:80%">
                                                                                        <label class="blink-medication-overdue"><%#: Eval("MedicationTime") %></label>
                                                                                    </td>
                                                                                    <td style="width:20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusIcon">
                                                                                                          <input type="hidden" class="scheduleID" value = "<%#:Eval("ID") %>" /> 
                                                                                                          <img class="btnStatus" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusInfo">
                                                                                                        <img class="btnEdit" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>                                                                                
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="divSTATUS divDiscontinue" <%# Eval("MedicationDisplayStatus").ToString() != "DISCONTINUE" ? "Style='display:none'":"" %>>                                                                            
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width:80%">
                                                                                        STOP
                                                                                    </td>
                                                                                    <td style="width:20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusIcon">
                                                                                                          <img src='<%# ResolveUrl("~/Libs/Images/Status/stop.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="padding-top:4px">
                                                                                                    <div <%# Eval("cfIsNeedToReturn").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                        <img class="btnInfo" src='<%# ResolveUrl("~/Libs/Images/Button/info.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                    <div <%# Eval("cfIsNeedToReturn").ToString() != "True" ? "Style='display:none'":"" %>>
                                                                                                        <img class="btnInfo blink-alert" src='<%# ResolveUrl("~/Libs/Images/Button/warning.png") %>' alt="<%#:Eval("ID") %>" style="width:24px" title="Obat sudah diproses oleh Farmasi (Harus diretur)"/>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>                                                                                
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("Tidak ada pasien yang sedang dalam perawatan") %>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </table>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging" style="display:none">
                    <div class="wrapperPaging">
                        <div id="pagingDt">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
