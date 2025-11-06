<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="MedicationList1.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.MedicationList1" %>
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
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <div class="menuTitle">
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh") %></div>
    </li>
    <li id="btnVerification" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Verifikasi Pemberian") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhList" runat="server">
    <style>
        .pnlContainerGridMedicationSchedule
        {
            width: 900px !important;
            overflow-x: auto;
        }
        
        .tdDrugInfoLabel
        {
            text-align: right;
            vertical-align: top;
            font-size: 10pt;
            font-style: italic;
        }
        
        .tdDrugInfoValue
        {
            vertical-align: top;
            font-size: 11pt;
            font-weight: bold;
        }
        
        .txtMedicationTime
        {
            width: 70px;
            text-align: center;
            color: Blue;
            font-size: larger;
        }
        
        .btnMedicationStatus
        {
            background-color: Green;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').click(function () {
                onRefreshList();
            })

            $('#<%=btnVerification.ClientID %>').click(function () {
                if ($('#<%=hdnUserParamedicType.ClientID %>').val() == Constant.ParamedicType.Pharmacist) {
                    var serviceUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
                    var url = ResolveUrl('~/Program/PatientPage/Medication/Administration/VerifyMedicationAdministrationCtl1.ascx');
                    var param = serviceUnitID + '|' + $('#<%=hdnMRN.ClientID %>').val();
                    openUserControlPopup(url, param, 'Verifikasi Pemberian Obat', 800, 450);
                }
                else {
                    displayErrorMessageBox('Verifikasi Pemberian Obat', 'Verifikasi Pemberian Obat hanya bisa dilakukan oleh seorang Apoteker.');
                }
            });


            $('.imgCompound').live('click', function () {
                var url = ResolveUrl("~/Program/Information/CompoundDetailCtl.ascx");
                var param = $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val();
                openUserControlPopup(url, param, 'UDD - Compound Detail', 650, 300);
            });
        });

        $('.chkIsProcessItem input').live('change', function () {
            var $cell = $(this).closest("td");
            var $tr = $cell.closest('tr');
            var isChecked = $(this).is(":checked");
            if ($(this).is(':checked')) {
                $cell.addClass('highlight');
            }
            else {
                $cell.removeClass('highlight');
            }
        });

        function getSelectedCheckbox() {
            var tempSelectedID = "";

            $('.grdNormal .chkIsProcessItem input:checked').each(function () {
                var $tr = $(this).closest('tr').closest('tr');
                var id = $tr.find('.hdnOrderDetailID').val();

                if (tempSelectedID != "") {
                    tempSelectedID += ",";
                }
                tempSelectedID += id;
            });

            if (tempSelectedID != "") {
                $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
                return true;
            }
            else return false;
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
                    $('.grdNormal tr:eq(1)').click();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdNormal tr:eq(1)').click();
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
                var itemID = $tr.find('.hdnItemID').val();
                var itemName = $tr.find('.hdnDrugName').val();
                var paramedicName = $tr.find('.hdnParamedicName').val();
                var param = orderDetailID + '|' + itemID + '|' + itemName + '|' + paramedicName;
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationList/MedicationTakenInfoCtl.ascx");
                openUserControlPopup(url, param, 'Medication - Administration Detail', 800, 500);
            }
        });

        function onRefreshList() {
            cbpView.PerformCallback('refresh');
        }
    </script>
    <input type="hidden" value="" id="hdnPrescriptionOrderDtID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnUserParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnUserParamedicType" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="0" id="hdnDisplayMode" runat="server" />
    <table style="width: 100%">
        <tr>
            <td>
                <table border="0" style="width: 100%">
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
                            <dxe:ASPxComboBox ID="cboMedicationStatus" ClientInstanceName="cboMedicationStatus"
                                runat="server" Width="100%">
                                <ClientSideEvents ValueChanged="function() { onRefreshList();}" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
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
                                <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th rowspan="2" align="center" style="width: 30px">
                                                </th>
                                                <th rowspan="2" align="center" style="width: 30px">
                                                    <div>
                                                        <%=GetLabel("UDD")%></div>
                                                </th>
                                                <th rowspan="2" align="center" style="width: 30px">
                                                </th>
                                                <th rowspan="2" align="left">
                                                    <div>
                                                        <%=GetLabel("Nama Obat")%>
                                                        <br />
                                                        <span style="color: Blue;">
                                                            <%=GetLabel("Generik")%></span>
                                                        <br />
                                                        <span style="font-style: italic">
                                                            <%=GetLabel("Dokter")%></span>
                                                        <div>
                                                </th>
                                                <th colspan="6" align="center">
                                                    <div>
                                                        <%=GetLabel("Aturan Pemberian")%></div>
                                                </th>
                                                <th colspan="6">
                                                    <div>
                                                        <%=GetLabel("Waktu Pemberian/Sequence") %></div>
                                                </th>
                                                <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Mulai Diberikan")%></div>
                                                </th>
                                                <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Sampai Dengan")%></div>
                                                </th>
                                                <th colspan="3">
                                                    <div style="text-align: left; font-weight: bold">
                                                        <%=GetLabel("Jumlah") %></div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 40px;">
                                                    <div style="text-align: right">
                                                        <%=GetLabel("Frekuensi") %></div>
                                                </th>
                                                <th style="width: 40px; text-align: left">
                                                    <div>
                                                        <%=GetLabel("Periode") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div style="text-align: right">
                                                        <%=GetLabel("Dosis") %></div>
                                                </th>
                                                <th style="width: 50px;">
                                                    <div style="text-align: left">
                                                        <%=GetLabel("Satuan") %></div>
                                                </th>
                                                <th align="center" style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("PRN")%></div>
                                                </th>
                                                <th style="width: 100px;">
                                                    <div style="text-align: left">
                                                        <%=GetLabel("Rute") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("1") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("2") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("3") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("4") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("5") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("6") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Dikirim Farmasi") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Diberikan ke Pasien") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Sisa Obat") %></div>
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
                                                <th rowspan="2" align="center" style="width: 30px;display:none">
                                                </th>
                                                <th rowspan="2" align="center" style="width: 30px">
                                                    <div>
                                                        <%=GetLabel("UDD")%></div>
                                                </th>
                                                <th rowspan="2" align="center" style="width: 30px">
                                                </th>
                                                <th rowspan="2" align="left">
                                                    <div>
                                                        <%=GetLabel("Nama Obat")%>
                                                        <br />
                                                        <span style="color: Blue;">
                                                            <%=GetLabel("Generik")%></span>
                                                        <br />
                                                        <span style="font-style: italic">
                                                            <%=GetLabel("Dokter")%></span>
                                                        <div>
                                                </th>
                                                <th colspan="6" align="center">
                                                    <div>
                                                        <%=GetLabel("Aturan Pemberian")%></div>
                                                </th>
                                                <th colspan="6">
                                                    <div>
                                                        <%=GetLabel("Waktu Pemberian/Sequence") %></div>
                                                </th>
                                                <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Mulai Diberikan")%></div>
                                                </th>
                                                <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Sampai Dengan")%></div>
                                                </th>
                                                <th colspan="3">
                                                    <div style="text-align: center; font-weight: bold">
                                                        <%=GetLabel("Jumlah") %></div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 40px;">
                                                    <div style="text-align: right">
                                                        <%=GetLabel("Frekuensi") %></div>
                                                </th>
                                                <th style="width: 40px; text-align: left">
                                                    <div>
                                                        <%=GetLabel("Periode") %></div>
                                                </th>
                                                <th style="width: 40px;">
                                                    <div style="text-align: right">
                                                        <%=GetLabel("Dosis") %></div>
                                                </th>
                                                <th style="width: 50px;">
                                                    <div style="text-align: left">
                                                        <%=GetLabel("Satuan") %></div>
                                                </th>
                                                <th align="center" style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("PRN")%></div>
                                                </th>
                                                <th style="width: 100px;">
                                                    <div style="text-align: left">
                                                        <%=GetLabel("Rute") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("1") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("2") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("3") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("4") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("5") %></div>
                                                </th>
                                                <th style="width: 30px;">
                                                    <div>
                                                        <%=GetLabel("6") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Dikirim Farmasi") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Diberikan ke Pasien") %></div>
                                                </th>
                                                <th style="width: 60px;">
                                                    <div>
                                                        <%=GetLabel("Sisa Obat") %></div>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRow" runat="server">
                                            <td align="center" style="width: 30px; vertical-align: middle;display:none">
                                                <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" />
                                            </td>
                                            <td align="center" style="width: 30px; background: #ecf0f1; vertical-align: middle">
                                                <asp:CheckBox ID="chkIsUsingUDD" runat="server" Enabled="false" CssClass="chkIsUsingUDD"
                                                    Checked='<%# Eval("IsUsingUDD")%>' />
                                            </td>
                                            <td align="center" style="background: #ecf0f1; vertical-align: middle;">
                                                <div <%# Eval("IsUsingUDD").ToString() != "True" ? "Style='display:none'":"" %>>
                                                    <img id="imgStatusImageUri" runat="server" width="24" height="24" alt="" visible="true" />
                                                </div>
                                                <div>
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                        min-width: 30px; float: left;' />
                                                </div>
                                            </td>
                                            <td>
                                                <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                                <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID"
                                                    class="hdnOrderDetailID" />
                                                <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" class="hdnItemID" />
                                                <input type="hidden" value="" bindingfield="GCItemUnit" />
                                                <input type="hidden" value="" bindingfield="GCBaseUnit" />
                                                <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" class="hdnDrugName" />
                                                <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName"
                                                    class="hdnParamedicName" />
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
                                                <input type="hidden" value="<%#:Eval("cfDispenseQuantity") %>" bindingfield="cfDispenseQuantity"
                                                    class="hdnCfDispenseQuantity" />
                                                <input type="hidden" value="<%#:Eval("cfTakenQuantity") %>" bindingfield="cfTakenQuantity"
                                                    class="hdnCfTakenQuantity" />
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("DrugName")%></b></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="color: Blue; float: left;">
                                                                <%#: Eval("GenericName")%></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="font-style: italic; float: left;">
                                                                <%#: Eval("ParamedicName")%></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right">
                                                <div>
                                                    <%#: Eval("Frequency")%></div>
                                            </td>
                                            <td align="left">
                                                <div>
                                                    <%#: Eval("DosingFrequency")%></div>
                                            </td>
                                            <td align="right">
                                                <div>
                                                    <%#: Eval("cfNumberOfDosage")%></div>
                                            </td>
                                            <td align="left">
                                                <div>
                                                    <%#: Eval("DosingUnit")%></div>
                                            </td>
                                            <td align="center" style="vertical-align: middle;">
                                                <div>
                                                    <asp:CheckBox ID="chkIsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' />
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%#: Eval("Route")%></div>
                                            </td>
                                            <td align="center">
                                                <div style="text-align: center;">
                                                    <%#: Eval("Sequence1Time")%>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div style="text-align: center;">
                                                    <%#: Eval("Sequence2Time")%>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div style="text-align: center;">
                                                    <%#: Eval("Sequence3Time")%>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div style="text-align: center;">
                                                    <%#: Eval("Sequence4Time")%>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div style="text-align: center;">
                                                    <%#: Eval("Sequence5Time")%>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div style="text-align: center;">
                                                    <%#: Eval("Sequence6Time")%>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div>
                                                    <%#: Eval("cfStartDate")%></div>
                                            </td>
                                            <td align="center">
                                                <label id="lblEndDate" runat="server">
                                                    <%#: Eval("cfEndDate")%></label>
                                                <div class="blink-alert" style="text-align: left; color: Red; font-size: 0.9em; font-style: italic">
                                                    <%#: Eval("cfDiscontinueDate")%></div>
                                            </td>
                                            <td valign="middle" style="background: #ecf0f1">
                                                <div style="text-align: right; color: Black">
                                                    <label class="lblDispenseQuantity lblLink">
                                                        <%#:Eval("cfDispenseQuantity", "{0:N}")%></label>
                                                </div>
                                            </td>
                                            <td valign="middle" style="background: #ecf0f1">
                                                <div style="text-align: right; color: blue">
                                                    <label class="lblTakenQuantity lblLink">
                                                        <%#:Eval("cfTakenQuantity", "{0:N}")%></label>
                                                </div>
                                            </td>
                                            <td valign="middle" style="background: #ecf0f1">
                                                <div style="text-align: right; color: Black">
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
