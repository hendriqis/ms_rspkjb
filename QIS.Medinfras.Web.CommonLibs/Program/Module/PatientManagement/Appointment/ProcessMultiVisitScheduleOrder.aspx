<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master"
    AutoEventWireup="true" CodeBehind="ProcessMultiVisitScheduleOrder.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProcessMultiVisitScheduleOrder" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh") %></div>
    </li>
    <li id="btnCancel" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Cancel Appointment") %></div>
    </li>
    <li id="btnBack" crudmode="R" runat="server" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbback.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
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
        
        
        .highlight
        {
            background-color: #FE5D15;
            color: White;
        }
        .btnMedicationStatus
        {
            background-color: Green;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromOrderDate.ClientID %>');
            setDatePicker('<%=txtToOrderDate.ClientID %>');
            $('#<%=txtFromOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtToOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnTestOrderID.ClientID %>').val($(this).find('.keyField').html());
                    $('#<%=hdnRegistrationID.ClientID %>').val($(this).find('.hiddenColumn').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=btnRefresh.ClientID %>').click(function () {
                onRefreshList();
            });

            $('#<%=btnCancel.ClientID %>').click(function () {
                if ($('#<%=txtMRN.ClientID %>').val() != '') {
                    $('#<%=btnRefresh.ClientID %>').attr("style", "display:none");
                    $('#<%=btnCancel.ClientID %>').attr("style", "display:none");
                    $('#<%=btnBack.ClientID %>').removeAttr("style");

                    $('.btnPrintApm').attr("style", "display:none");
                    $('.btnCancelApm').removeAttr("style");
                }
                else {
                    ShowSnackbarWarning("Harap pilih No. Rekam Medis");
                }
            });

            $('#<%=btnBack.ClientID %>').click(function () {
                $('#<%=btnRefresh.ClientID %>').removeAttr("style");
                $('#<%=btnCancel.ClientID %>').removeAttr("style");
                $('#<%=btnBack.ClientID %>').attr("style", "display:none");

                $('.btnPrintApm').removeAttr("style");
                $('.btnCancelApm').attr("style", "display:none");
            });

            $('.imgEditOrderSchedule').live('click', function () {
                var id = $('#<%=hdnTestOrderID.ClientID %>').val();
                OnCustomProcessButtonClick('editOrder', id);
            });

            $('.btnInfoApm').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('info', id);
            });

            $('.btnProcess').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('process', id);
            });

            $('.btnPrintApm').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('print', id);
            });

            $('.btnCancelApm').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('cancel', id);
            });

            $('.btnProcess').live('click', function () {
                var id = $(this).attr("alt");
                OnCustomProcessButtonClick('processRegis', id);
            });
        });

        var customButtonClickHandler = null;
        function OnCustomProcessButtonClick(type, id) {
            OpenPopupWindow(type, id);
        }

        var isProcessApm = false;
        function OpenPopupWindow(type, id) {
            switch (type) {
                case 'process':
                    isProcessApm = true;
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/Appointment/ProcessMultiVisitScheduleOrderCtl.ascx");
                    var param = 'process|' + id;
                    openUserControlPopup(url, param, 'Proses Penjadwalan', 1200, 800);
                    break;
                case 'processRegis':
                    break;
                case 'info':
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/Appointment/AppointmentMultiVisitScheduleOrderInfoCtl.ascx");
                    var param = id;
                    openUserControlPopup(url, param, 'Informasi Appointment Penjadwalan', 500, 400);
                    break;
                case 'print':
                    cbpViewDt.PerformCallback('print|' + id);
                    break;
                case 'cancel':
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/Appointment/MultiVisitAppointmentVoidCtl.ascx");
                    var param = id;
                    openUserControlPopup(url, param, 'Batal Penjadwalan Multi Kunjungan', 600, 400);
                    break;
                case 'editOrder':
                    var url = ResolveUrl("~/libs/Program/Module/PatientManagement/Appointment/MultiVisitScheduleOrderEditCtl.ascx");
                    var param = id;
                    openUserControlPopup(url, param, 'Edit Order Multi Kunjungan', 600, 400);
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

        function onAfterSaveAddRecordEntryPopup(param) {
            cbpViewDt.PerformCallback('updateResult|' + param);
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpViewDt.PerformCallback('updateResult|' + param);
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
                    $('#<%=hdnItemID.ClientID %>').val('0');
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

        //#region Patient
        $('#lblMRN.lblLink').live('click', function () {
            openSearchDialog($('#<%:hdnPatientSearchDialogType.ClientID %>').val(), "IsDeleted = 0", function (value) {
                $('#<%:txtMRN.ClientID %>').val(value);
                ontxtMRNChanged(value);
            });
        });

        $('#<%=txtMRN.ClientID %>').live('change', function () {
            ontxtMRNChanged($(this).val());
        });

        function ontxtMRNChanged(value) {
            var mrn = FormatMRN(value);
            var filterExpression = "MedicalNo = '" + mrn + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnMRN.ClientID %>').val(result.MRN);
                    $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                    $('#<%:txtPatientName.ClientID %>').val(result.PatientName);
                }
                else {
                    $('#<%:hdnMRN.ClientID %>').val('');
                    $('#<%:txtMRN.ClientID %>').val('');
                    $('#<%:txtPatientName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var regID = $('#<%=hdnRegistrationID.ClientID %>').val();
            filterExpression.text = "RegistrationID = " + regID;
            return true;
        }
    </script>
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnPastMedicationID" runat="server" />
    <input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="0" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="0" id="hdnTestOrderID" runat="server" />
    <input type="hidden" value="0" id="hdnMRN" runat="server" />
    <input type="hidden" value="0" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnSequenceForMedicationList" runat="server" />
    <input type="hidden" value="0" id="hdnParamNR0001" runat="server" />
    <input type="hidden" value="0" id="hdnPatientSearchDialogType" runat="server" />
    <div>
        <table style="width: 100%;">
            <colgroup>
                <col style="width: 5%" />
                <col style="width: 70%" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Order")%></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 145px" />
                            <col style="width: 3px" />
                            <col style="width: 145px" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtFromOrderDate" Width="120px" CssClass="datepicker" runat="server"/>
                            </td>
                            <td>
                                <%=GetLabel("s/d") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtToOrderDate" Width="120px" CssClass="datepicker" runat="server"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Unit Pelayanan")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="15%"
                        runat="server">
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink" id="lblMRN">
                        <%=GetLabel("No. Rekam Medis")%></label>
                </td>
                <td>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtMRN" Width="120px" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientName" ReadOnly="true" Width="50%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 30%" />
            <col style="width: 70%" />
        </colgroup>
        <tr>
            <td valign="top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage2">
                                <asp:GridView runat="server" ID="grdView" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="FromTestOrderID" HeaderStyle-CssClass="keyField"
                                            ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="FromRegistrationID" HeaderStyle-CssClass="hiddenColumn"
                                            ItemStyle-CssClass="hiddenColumn" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Nama Pasien")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr id="trItemName" runat="server">
                                                        <td width="95%">
                                                            <div style="font-size: 14pt; color: #0066FF; font-weight: bold; padding-bottom: 5px">
                                                                <%#: Eval("PatientName")%></div>
                                                        </td>
                                                        <td><img class="imgEditOrderSchedule imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" /></td>
                                                    </tr>
                                                </table>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <colgroup>
                                                        <col style="width: 100px" />
                                                        <col style="width: 5px" />
                                                        <col />
                                                        <col style="width: 80px" />
                                                        <col style="width: 5px" />
                                                        <col style="width: 100px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("No. RM")%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="tdDrugInfoValue" colspan="5">
                                                            <%#: Eval("MedicalNo")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("Registrasi Asal")%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="tdDrugInfoValue" colspan="5">
                                                            <%#: Eval("FromRegistrationNo")%> (<%#: Eval("cfFromRegistrationDateInString")%>)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("Penjamin Asal")%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="tdDrugInfoValue" colspan="5">
                                                            <%#: Eval("FromBusinessPartnerName")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("No. Order Asal")%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="tdDrugInfoValue" colspan="5">
                                                            <%#: Eval("FromTestOrderNo")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdDrugInfoLabel">
                                                            <%=GetLabel("Tujuan Order")%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="tdDrugInfoValue" colspan="5">
                                                            <%#: Eval("OrderServiceUnitName")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="font-weight: bold; color: red; font-size: 1.1em; padding-top: 7px">
                                            <%=GetLabel("Tidak ada order item multi kunjungan untuk pasien ini")%></div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerImgLoadingView" id="containerImgLoadingView" style="display: none">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging" style="display: none">
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
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage7 pnlContainerGridMedicationSchedule">
                                <table id="grdViewDt" class="grdSelected grdPatientPage" cellspacing="0" rules="all"
                                    style="overflow-x: scroll;" width="100%">
                                    <tr>
                                        <th style="width: 20px">
                                            <<
                                        </th>
                                        <asp:Repeater ID="rptItemNameHeader" runat="server">
                                            <ItemTemplate>
                                                <th style="width: 90px; font-weight: bold; font-size: 8pt" align="center">
                                                    <%#: Eval("ItemName")%>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <th>
                                            &nbsp;
                                        </th>
                                        <th style="width: 20px">
                                            >>
                                        </th>
                                    </tr>
                                    <asp:ListView ID="lvwViewDt" runat="server" OnItemDataBound="lvwViewDt_ItemDataBound"
                                        Style="width: 100%">
                                        <LayoutTemplate>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="background-color: Lightgray; text-align: center; ;
                                                    vertical-align: middle">
                                                    <%#: Eval("SequenceNo")%>
                                                    <input type="hidden" value="<%#:Eval("SequenceNo") %>" class="SequenceNo" />
                                                </td>
                                                <asp:Repeater ID="rptMedicationTimeDetail" runat="server">
                                                    <ItemTemplate>
                                                        <td align="left">
                                                            <table class="rptMedicationTimeDetail" border="0" cellpadding="0" cellspacing="0"
                                                                width="100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <div class="divDetailInfo">
                                                                            <%#: Eval("cfScheduleSequence") %>
                                                                        </div>
                                                                        <div class="divMultiVisitSTATUS divMultiVisitOPEN" <%# Eval("DiagnosticScheduleStatus").ToString() != "OPEN" ? "Style='display:none'":"" %>>
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width: 80%">
                                                                                        <%#: Eval("cfScheduleSequence") %>
                                                                                    </td>
                                                                                    <td style="width: 20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusInfo">
                                                                                                        <img class="btnProcess" src='<%# ResolveUrl("~/Libs/Images/Status/scheduled.png") %>'
                                                                                                            alt="<%#:Eval("ID") %>" style="cursor: pointer" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="divMultiVisitSTATUS divMultiVisitSchedule" <%# Eval("DiagnosticScheduleStatus").ToString() != "STARTED" ? "Style='display:none'":"" %>>
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width: 80%">
                                                                                        <%#: Eval("cfScheduleSequence") %>
                                                                                    </td>
                                                                                    <td style="width: 20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusInfo">
                                                                                                        <img class="btnInfoApm" src='<%# ResolveUrl("~/Libs/Images/Button/info.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div class="divMedicationStatusInfo">
                                                                                                        <img class="btnPrintApm" src='<%# ResolveUrl("~/Libs/Images/Button/print.png") %>' alt="<%#:Eval("ID") %>" />
                                                                                                        <img class="btnCancelApm" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png") %>' alt="<%#:Eval("ID") %>" style="display:none" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="divMultiVisitSTATUS divMultiVisitTaken" <%# Eval("DiagnosticScheduleStatus").ToString() != "COMPLETED" ? "Style='display:none'":"" %>>
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="width: 80%">
                                                                                        <%#: Eval("cfScheduleSequence") %>
                                                                                    </td>
                                                                                    <td style="width: 20%" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                    <%=GetLabel("Tidak ada tindakan") %>
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
                <div class="containerPaging" style="display: none">
                    <div class="wrapperPaging">
                        <div id="pagingDt">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
