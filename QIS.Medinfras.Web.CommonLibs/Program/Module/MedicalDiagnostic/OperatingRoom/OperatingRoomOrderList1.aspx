<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="OperatingRoomOrderList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.OperatingRoomOrderList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnNewOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("New Order")%></div>
    </li>
    <li id="btnPrint" crudmode="R" runat="server" visible="false">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Print")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <style type="text/css">
        #divRoomInfo > a
        {
            text-decoration: none;
            padding: 1px;
            color: Gray;
        }
        #divRoomInfo > a.selected
        {
            background-color: #f44336 !important;
            color: White;
        }
        
        .tdSchedule
        {
            border: 1px solid #AAA;
        }
        .tdScheduleTime
        {
            font-size: 18px !important;
            color: #4d0000 !important;
            text-align: center;
        }
        
        #contentDetailNavPane > a
        {
            margin: 0;
            font-size: 12px;
        }
        #contentDetailNavPane > a.selected
        {
            color: #fff !important;
            background-color: #f44336 !important;
        }
    </style>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setCalendar("1");

            $('#contentDetailNavPane a').click(function () {
                $('#contentDetailNavPane a.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                if (contentID != null) {
                    showDetailContent(contentID);
                    if (contentID == "contentDetailPage1") {
                        setPaging($("#paging"), pageCount, function (page) {
                            cbpView.PerformCallback('changepage|' + page);
                        });
                    }
                    else if (contentID == "contentDetailPage2") {
                        cbpPanelView3.PerformCallback('refresh');
                    }
                    else {
                        cbpPanelView4.PerformCallback('refresh');
                    }
                    $('#<%=hdnContentID.ClientID %>').val(contentID);
                }
            });

            $('#contentDetailNavPane a').first().click();

            //#region Room Code
            $('#divRoomInfo a').click(function () {
                $('#divRoomInfo a.selected').removeClass('selected');
                $(this).addClass('selected');
                var roomCode = $(this).attr('contentID');
                var roomName = $(this).attr('contentName');
                var cfDisplayRoom = $(this).attr('contentDisplay');

                $('#<%=hdnOperatingRoomCode.ClientID %>').val(roomCode);
                cboOperatingRoom.SetValue(cfDisplayRoom);
                ////// cboOperatingRoomAll.SetValue(cfDisplayRoom);
                displayRoomDetail(roomCode, roomName);
            });
            //#endregion

            $('.btnProcess').live('click', function () {
                var $tr = $(this).closest('tr').parent().closest('tr');
                var visitID = $tr.find('.visitID').html();
                var testOrderID = $tr.find('.keyField').html();

                var url = ResolveUrl("~/Libs/Controls/Process/SurgeryOrderEntryCtl1.ascx");
                var id = visitID + '|' + testOrderID;

                openUserControlPopup(url, id, 'Proses Penjadwalan Order Kamar Operasi', 900, 600);
            });

            $('.btnEdit').live('click', function () {
                var visitID = $(this).attr('visitID');
                var testOrderID = $(this).attr('orderID');
                var isEnabled = $(this).attr('enabled');
                var patientInfo = $(this).attr('patientInfo');
                var patientLocation = $(this).attr('patientLocation');

                if (isEnabled == "True") {
                    var url = ResolveUrl("~/Libs/Controls/Process/SurgeryOrderEntryCtl1.ascx");
                    var id = visitID + '|' + testOrderID;

                    openUserControlPopup(url, id, 'Proses Penjadwalan Order Kamar Operasi', 900, 600);
                }
                else {
                    displayMessageBox("JADWAL KAMAR OPERASI", "Order tidak bisa dilakukan perubahan lagi karena sudah diproses");
                }
            });

            $('.btnDelete').live('click', function () {
                var roomScheduleID = $(this).attr('roomScheduleID');
                var visitID = $(this).attr('visitID');
                var testOrderID = $(this).attr('orderID');
                var patientInfo = $(this).attr('patientInfo');
                var isEnabled = $(this).attr('enabled');

                if (isEnabled == "True") {
                    var id = roomScheduleID + '|' + visitID + '|' + testOrderID;

                    var message = "Hapus jadwal kamar operasi untuk pasien " + patientInfo + " ?";
                    displayConfirmationMessageBox("JADWAL KAMAR OPERASI", message, function (result) {
                        if (result) {
                            cbpDeleteRoomSchedule.PerformCallback(id);
                            cbpPanelView4.PerformCallback('refresh');
                        }
                    });
                }
                else {
                    displayMessageBox("JADWAL KAMAR OPERASI", "Jadwal Order tidak bisa dihapus karena sudah diproses");
                }
            });

            $('.btnDocumentChecklist').live('click', function () {
                var visitID = $(this).attr('visitID');
                var testOrderID = $(this).attr('orderID');
                var isEnabled = $(this).attr('enabled');
                var patientInfo = $(this).attr('patientInfo');
                var patientLocation = $(this).attr('patientLocation');

                if (isEnabled == "True") {
                    var url = ResolveUrl("~/Libs/Controls/Process/SurgeryOrderDocumentCheckListCtl1.ascx");
                    var id = visitID + '|' + testOrderID;

                    openUserControlPopup(url, id, 'Checklist Kelengkapan Dokumen', 900, 600);
                }
                else {
                    displayMessageBox("Checklist Kelengkapan Dokumen", "Order tidak bisa dilakukan perubahan lagi karena sudah diproses");
                }
            });

            $('.btnStartOrder').live('click', function () {
                var roomScheduleID = $(this).attr('roomScheduleID');
                var visitID = $(this).attr('visitID');
                var testOrderID = $(this).attr('orderID');
                var patientInfo = $(this).attr('patientInfo');
                var isAllowStart = $(this).attr('isAllowStart');
                var isStarted = $(this).attr('isStarted');
                var appointmentID = $(this).attr('appointmentID');
                var isAppointmentRegistered = $(this).attr('isAppointmentRegistered');
                var isDocumentCheckListCompleted = $(this).attr('isDocumentCheckListCompleted');

                var id = roomScheduleID + '|' + visitID + '|' + testOrderID;
                if (isDocumentCheckListCompleted == "False") {
                    displayMessageBox("JADWAL KAMAR OPERASI", "Checklist Kelengkapan Berkas/Dokumen Tindakan belum dilakukan. <br/> <br/> <span style='font-style:italic; font-weight:bold'>Silahkan lakukan dulu proses checklist kelengkapan berkas.</span>");
                    return;
                }

                if (isStarted == "False") {
                    if (isAllowStart == "True") {
                        var message = "Mulai jadwal kamar operasi untuk pasien " + patientInfo + " ?";
                        displayConfirmationMessageBox("JADWAL KAMAR OPERASI", message, function (result) {
                            if (result) {
                                cbpStartOrder.PerformCallback(id);
                                cbpPanelView4.PerformCallback('refresh');
                            }
                        });
                    }
                    else {
                        if (isAppointmentRegistered == "False" && appointmentID != "0") {
                            displayMessageBox("JADWAL KAMAR OPERASI", "Appointment dari jadwal kamar operasi ini belum didaftarkan kunjungannya. <br/> <br/> <span style='font-style:italic; font-weight:bold'>Silahkan lakukan dulu proses ubah link Jadwal dengan Kunjungan yang sekarang.</span>");
                        }
                        else {
                            displayMessageBox("JADWAL KAMAR OPERASI", "Hanya bisa dilakukan di tanggal yang sama sesuai dengan jadwal serta pasien sudah terregistrasi (bukan appointment)");
                        }
                    }
                }
                else {
                    var message = "Ubah status selesai jadwal kamar operasi untuk pasien " + patientInfo + " ?";
                    displayConfirmationMessageBox("JADWAL KAMAR OPERASI", message, function (result) {
                        if (result) {
                            cbpStopOrder.PerformCallback(id);
                            cbpPanelView4.PerformCallback('refresh');
                        }
                    });
                }
            });

            $('.btnUpdateRegistration').live('click', function () {
                var id = $(this).attr('roomScheduleID');
                var visitID = $(this).attr('visitID');
                var testOrderID = $(this).attr('orderID');
                var isEnabled = $(this).attr('enabled');
                var patientInfo = $(this).attr('patientInfo');
                var patientLocation = $(this).attr('patientLocation');

                if (isEnabled == "True") {
                    var url = ResolveUrl("~/Libs/Controls/Process/SurgeryOrderRegistrationCtl1.ascx");
                    var id = id + "|" + visitID + '|' + testOrderID;

                    openUserControlPopup(url, id, 'Proses Ubah Nomor Registrasi Order Kamar Operasi', 900, 600);
                }
                else {
                    displayMessageBox("Ubah Nomor Registrasi Jadwal Kamar Operasi", "Order tidak bisa dilakukan perubahan lagi karena sudah diproses");
                }
            });


            $('#<%=btnRefresh.ClientID %>').click(function () {
                if ($('#<%=hdnContentID.ClientID %>').val() == "contentDetailPage1") {
                    cbpView.PerformCallback('refresh');
                }
                else if ($('#<%=hdnContentID.ClientID %>').val() == "contentDetailPage2") {
                    cbpPanelView3.PerformCallback('refresh');
                }
                else {
                    cbpPanelView4.PerformCallback('refresh');
                }
            });

            $('#<%=btnNewOrder.ClientID %>').click(function () {
                var url = ResolveUrl("~/Libs/Controls/Process/NewSurgeryOrderEntryCtl1.ascx");
                var id = '0' + '|' + '0';

                openUserControlPopup(url, id, 'Proses Penjadwalan Order Kamar Operasi', 900, 600);
            });

            $('#<%=lblSelectedDate.ClientID %>').html($('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val());

            $('#divRoomInfo a').first().click();

            setDatePicker('<%=txtFromScheduleDate.ClientID %>');

            $('#<%=txtFromScheduleDate.ClientID %>').datepicker('option', '0');

            $('.imgViewSurgeryMR.imgLink').die('click');
            $('.imgViewSurgeryMR.imgLink').live('click', function (evt) {
                var url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewSurgeryAssessmentCtl1.ascx");
                var testOrderID = $(this).attr('testOrderID');
                var param = testOrderID + "|" + "0";
                openUserControlPopup(url, param, "Ringkasan Data Operasi", 1000, 500);
            });
        });

        function setCalendar(isAllowBackDate) {
            if (isAllowBackDate == '1') {
                $("#calAppointment").datepicker({
                    defaultDate: "w",
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: "dd-mm-yy",
                    //minDate: "0",
                    onSelect: function (dateText, inst) {
                        $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                        $('#<%=lblSelectedDate.ClientID %>').html(dateText);
                        cbpView.PerformCallback('refresh');
                        $('#divRoomInfo a').first().click();
                    }
                });
            }
            else {
                $("#calAppointment").datepicker({
                    defaultDate: "w",
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: "dd-mm-yy",
                    minDate: "0",
                    onSelect: function (dateText, inst) {
                        $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                        $('#<%=lblSelectedDate.ClientID %>').html(dateText);
                        cbpView.PerformCallback('refresh');
                        $('#divRoomInfo a').first().click();
                    }
                });
            }
        }

        //#region  Slide Show
        var slideIndex = 1;
        showDivs(slideIndex);

        function plusDivs(n) {
            showDivs(slideIndex += n);
        }

        function showDivs(n) {
            var i;
            var x = document.getElementsByClassName("mySlides");
            if (n > x.length) { slideIndex = 1 }
            if (n < 1) { slideIndex = x.length }
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            x[slideIndex - 1].style.display = "block";
        }
        //#endregion Slide Show

        $('#<%=btnPrint.ClientID %>').click(function () {
            displayMessageBox("JADWAL KAMAR OPERASI", "Maaf, fitur ini sementara belum tersedia");
            //cbpView.PerformCallback('import');
        });

        function onAfterCustomClickSuccess(type) {

            cbpView.PerformCallback('refresh');
            cbpPanelView4.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup(param) {

            cbpView.PerformCallback('refresh');
            cbpPanelView4.PerformCallback('refresh');
            $('#divRoomInfo a').first().click();
        }

        function onAfterSaveEditRecordEntryPopup(param) {

            cbpView.PerformCallback('refresh');
            cbpPanelView4.PerformCallback('refresh');
            $('#divRoomInfo a').first().click();
        }

        function onAfterProcessPopupEntry(param) {

            cbpView.PerformCallback('refresh');
            cbpPanelView4.PerformCallback('refresh');
            $('#divRoomInfo a').first().click();
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshControl();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        function displayRoomDetail(roomCode, roomName) {
            var i, x, tablinks;
            x = document.getElementsByClassName("roomDetailInfo");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            $('#<%=divRoomName.ClientID %>').html(roomName);
            cbpViewDt.PerformCallback('refresh');
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

        function onCbpPanelView3EndCallback(s) {
            $('#containerImgLoadingView3').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView3.ClientID %> tr:eq(1)').click();

                setPaging($("#paging3"), pageCount, function (page) {
                    cbpPanelView3.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView3.ClientID %> tr:eq(1)').click();
        }

        function onCbpPanelView4EndCallback(s) {
            $('#containerImgLoadingView4').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView4.ClientID %> tr:eq(1)').click();

                setPaging($("#paging4"), pageCount, function (page) {
                    cbpPanelView4.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView4.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function isValidDate(value) {
            var dateWrapper = new Date(value);
            return !isNaN(dateWrapper.getDate());
        }

        function onRefreshPage() {
            cbpView.PerformCallback('refresh');
        }

        function onCbpRoomScheduleStatusEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpView.PerformCallback('refresh');
                $('#divRoomInfo a').first().click();
            }
            else {
                displayErrorMessageBox("Jadwal Kamar Operasi", param[2]);
            }
        }

        function showDetailContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("contentDetail");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }

        function onCboRoomValueChanged() {
            if ($('#<%=hdnContentID.ClientID %>').val() == "contentDetailPage2") {
                cbpPanelView3.PerformCallback('refresh');
            }
        }

        function onCboRoom2ValueChanged() {
            if ($('#<%=hdnContentID.ClientID %>').val() == "contentDetailPage3") {
                cbpPanelView4.PerformCallback('refresh');
            }
        }       
    </script>
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestDate" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitImagingID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitLaboratoryID" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" id="hdnOperatingRoomID" runat="server" value="" />
    <input type="hidden" id="hdnOperatingRoomCode" runat="server" value="" />
    <input type="hidden" id="hdnContentID" runat="server" value="contentDetailPage1" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <div id="contentDetailNavPane" class="w3-bar w3-black">
                    <a contentid="contentDetailPage1" class="w3-bar-item w3-button tablink selected">Proses Penjadwalan</a>
                    <a contentid="contentDetailPage2" class="w3-bar-item w3-button tablink">Terjadwal Per Bulan</a> 
                    <a contentid="contentDetailPage3" class="w3-bar-item w3-button tablink">Terjadwal Per Tanggal</a>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="contentDetailPage1" class="container contentDetail  w3-animate-top">
                    <table class="tblContentArea">
                        <tr>
                            <td style="padding: 5px; vertical-align: top">
                                <table style="width: 100%;">
                                    <colgroup>
                                        <col style="width: 400px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDate" />
                                                        <div id="calAppointment">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td rowspan="2" valign="top" style="padding-bottom: 10px">
                                            <table border="0" cellpadding="0" cellspacing="1" width="100%" class="w3-table w3-border">
                                                <colgroup>
                                                    <col style="width: 120px" />
                                                    <col />
                                                    <col style="width: 120px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td colspan="4">
                                                        <div id="lblSelectedDate" runat="server" class="lblSelectedDate w3-blue w3-xlarge"
                                                            style="text-align: center; text-shadow: 1px 1px 0 #444; width: 100%; padding-bottom: 2px">
                                                            dd-MM-yyyy</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center">
                                                        <div id="divRoomInfo" class="w3-bar w3-xlarge">
                                                            <asp:Repeater ID="lstRoomCode" runat="server">
                                                                <ItemTemplate>
                                                                    <a href="#" class="lnkRoomCode w3-button" title='<%#: Eval("RoomName") %>' contentID = '<%#: Eval("RoomCode") %>' contentName='<%#: Eval("RoomName") %>' contentDisplay = '<%#: Eval("cfRoomNameDisplay") %>'><%#: Eval("RoomCode") %></a>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center">
                                                        <div id="divRoomName" runat="server" class="w3-win8-emerald w3-xlarge" style="text-align: center;
                                                            text-shadow: 1px 1px 0 #444; width: 100%; padding-bottom: 2px">
                                                            Ruang Operasi</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center">
                                                        <div class="w3-animate-left" style="text-align: center">
                                                            <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                                                ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                                                    EndCallback="function(s,e){ $('#containerImgLoadingViewDt').hide(); }" />
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid1">
                                                                            <div id="containerAppointment" class="containerAppointment">
                                                                                <asp:GridView ID="grdAppointment" runat="server" CssClass="grdSelected grdAppointment"
                                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                                    OnRowDataBound="grdAppointment_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                                                            ItemStyle-HorizontalAlign="Center">
                                                                                            <HeaderTemplate>
                                                                                                <div class="tdTime" style="text-align: center">
                                                                                                    <%=GetLabel("No") %>
                                                                                                </div>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <div class="tdTime w3-large" style="text-align: center">
                                                                                                    <%#: Eval("Number") %></div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField ItemStyle-CssClass="tdAppointment">
                                                                                            <HeaderTemplate>
                                                                                                <div class="tdTime" style="text-align: left">
                                                                                                    <%=GetLabel("DATA JADWAL") %>
                                                                                                </div>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Repeater ID="rptOrderInformation" runat="server">
                                                                                                    <HeaderTemplate>
                                                                                                        <ul style="list-style-type: none;">
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <li style="padding-bottom: 2px">
                                                                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #e6e6e6">
                                                                                                                <colgroup>
                                                                                                                    <col style="width: 100px;" />
                                                                                                                    <col style="width: 77px" />
                                                                                                                    <col />
                                                                                                                    <col style="width: 70px" />
                                                                                                                </colgroup>
                                                                                                                <tr>
                                                                                                                    <td style="background-color: #f2f2f2; text-align: center;">
                                                                                                                        <div class="w3-bar w3-large" style="color: #990000;">
                                                                                                                            <%#: Eval("ScheduledTime") %>
                                                                                                                            -
                                                                                                                            <%#: Eval("cfEndTime") %></div>
                                                                                                                        <div class="w3-bar w3-small" style="color: #990000;">
                                                                                                                            <%#: Eval("TestOrderNo") %></div>
                                                                                                                        <div>
                                                                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                                <colgroup>
                                                                                                                                    <col style="width: 24px;" />
                                                                                                                                    <col style="width: 24px;" />
                                                                                                                                    <col style="width: 24px;" />
                                                                                                                                    <col style="width: 24px;" />
                                                                                                                                    <col />
                                                                                                                                </colgroup>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <div <%# Eval("cfIsCompleted").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                                                            <img class="btnEdit imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# Eval("cfIsAllowEdit").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                                                                patientinfo='<%#Eval("PatientName") %>' enabled='<%#Eval("cfIsAllowEdit") %>'
                                                                                                                                                patientlocation='<%#Eval("cfPatientLocation") %>' />
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <div <%# Eval("cfIsCompleted").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                                                            <img class="btnDelete imgLink" title='<%=GetLabel("Hapus Jadwal")%>' src='<%# Eval("cfIsAllowEdit").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                                                                patientinfo='<%#Eval("PatientName") %>' enabled='<%#Eval("cfIsAllowEdit") %>' />
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <div <%# Eval("cfIsCompleted").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                                                            <img class="btnStartOrder imgLink" src='<%# Eval("cfIsStarted").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/start.png") : ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                                                                                title='<%# Eval("cfIsStarted").ToString() == "False" ? "Mulai Operasi" : "Selesai Operasi" %>'
                                                                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                                                                patientinfo='<%#Eval("PatientName") %>' isallowstart='<%#Eval("cfIsAllowSTART") %>'
                                                                                                                                                isstarted='<%#Eval("cfIsStarted") %>' appointmentid='<%#Eval("appointmentID") %>'
                                                                                                                                                isappointmentregistered='<%#Eval("cfIsAppointmentRegistered") %>' isdocumentchecklistcompleted='<%#Eval("IsDocumentCheckListCompleted") %>' />
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <div <%# Eval("cfIsCompleted").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                                                            <img class="btnDocumentChecklist imgLink" title='<%=GetLabel("Checklist Dokumen")%>'
                                                                                                                                                src='<%# Eval("cfIsAllowEdit").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/list_disabled.png") : ResolveUrl("~/Libs/Images/Button/list.png")%>'
                                                                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                                                                patientinfo='<%#Eval("PatientName") %>' enabled='<%#Eval("cfIsAllowEdit") %>'
                                                                                                                                                patientlocation='<%#Eval("cfPatientLocation") %>' />
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <div <%# Eval("AppointmentID").ToString() == "0" ? "Style='display:none'":"" %>>
                                                                                                                                            <img class="btnUpdateRegistration imgLink" title='<%=GetLabel("Ubah Nomor Registrasi")%>'
                                                                                                                                                src='<%# Eval("cfIsAppointmentRegistered").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/link_disabled.png") : ResolveUrl("~/Libs/Images/Button/link.png")%>'
                                                                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                                                                patientinfo='<%#Eval("PatientName") %>' enabled='<%#Eval("cfIsAllowEdit") %>'
                                                                                                                                                patientlocation='<%#Eval("cfPatientLocation") %>' />
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </td>
                                                                                                                    <td style="background-color: #f2f2f2;">
                                                                                                                        <div style="text-align: left">
                                                                                                                            <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="77px"
                                                                                                                                width="45px" style="float: left; margin-right: 10px;" /></div>
                                                                                                                    </td>
                                                                                                                    <td style="background-color: #f2f2f2; width: 100%">
                                                                                                                        <div>
                                                                                                                            <span style="font-weight: bold; font-size: 11pt">
                                                                                                                                <%#: Eval("PatientName") %></span></div>
                                                                                                                        <div>
                                                                                                                            <%#: Eval("MedicalNo") %>,
                                                                                                                            <%#: Eval("RegistrationNo")%> <i>(<%#: Eval("RegistrationStatus")%>)</i>,
                                                                                                                            <%#: Eval("cfDateOfBirth")%></div>
                                                                                                                        <div style="font-style: italic">
                                                                                                                            <%#: Eval("cfPatientLocation")%></div>
                                                                                                                        <div>
                                                                                                                            <%#: Eval("DokterOperator")%></div>
                                                                                                                        <div>
                                                                                                                            <%#: Eval("BusinessPartnerName")%></div>
                                                                                                                        <div <%# Eval("AppointmentID").ToString() == "0" ? "Style='display:none'":"" %> class="blink">
                                                                                                                            <label id="lblAppointmentInfo" class="lblLink" title="Pasien Appointment" style="font-weight: bold">
                                                                                                                                <%#: Eval("AppointmentNo")%></label></div>
                                                                                                                    </td>
                                                                                                                    <td style="background-color: #f2f2f2">
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </li>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        </ul>
                                                                                                    </FooterTemplate>
                                                                                                </asp:Repeater>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <EmptyDataTemplate>
                                                                                        <%=GetLabel("Belum ada penjadwalan kamar operasi untuk tanggal yang dipilih")%>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dxcp:ASPxCallbackPanel>
                                                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top">
                                            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpViewEndCallback(s);}" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid2">
                                                            <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                                ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn visitID" ItemStyle-CssClass="hiddenColumn visitID" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                        <HeaderTemplate>
                                                                            <%=GetLabel("Order Kamar Operasi")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #e6e6e6">
                                                                                <colgroup>
                                                                                    <col style="width: 70px" />
                                                                                    <col style="width: 77px" />
                                                                                    <col />
                                                                                    <col style="width: 70px" />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td style="background-color: #f2f2f2">
                                                                                        <div class="w3-bar w3-xlarge" style="color: #990000; padding-right: 5px">
                                                                                            <%#: Eval("ScheduledTime") %></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="text-align: left">
                                                                                            <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="77px"
                                                                                                width="45px" style="float: left; margin-right: 10px;" /></div>
                                                                                    </td>
                                                                                    <td style="width: 100%">
                                                                                        <div class="w3-bar w3-small" style="color: #990000;">
                                                                                            <%#: Eval("TestOrderNo") %>
                                                                                            <i>(<%#: Eval("TransactionStatusWatermark")%>)</i></div>
                                                                                        <div>
                                                                                            <span style="font-weight: bold; font-size: 11pt">
                                                                                                <%#: Eval("PatientName") %></span></div>
                                                                                        <div>
                                                                                            <%#: Eval("MedicalNo") %>,
                                                                                            <%#: Eval("RegistrationNo")%>
                                                                                            <i>(<%#: Eval("RegistrationStatus")%>)</i></div>
                                                                                        <div style="font-style: italic">
                                                                                            <%#: Eval("cfPatientLocation")%></div>
                                                                                        <div>
                                                                                            <%#: Eval("ParamedicName")%></div>
                                                                                        <div>
                                                                                            <%#: Eval("BusinessPartnerName")%></div>
                                                                                    </td>
                                                                                    <td style="background-color: #f2f2f2">
                                                                                        <img class="btnProcess imgLink" title='<%=GetLabel("Penjadwalan Order")%>' src='<%= ResolveUrl("~/Libs/Images/Button/process.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada informasi order jadwal kamar operasi di tanggal ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
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
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="contentDetailPage2" class="container contentDetail  w3-animate-top" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                        <colgroup>
                            <col width="150px" />
                            <col width="60px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tahun - Bulan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboYear" ClientInstanceName="cboYear" Width="60px" />
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboMonth" ClientInstanceName="cboMonth" Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Ruang Kamar Operasi")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboOperatingRoom" ClientInstanceName="cboOperatingRoom"
                                    Width="250px">
                                    <ClientSideEvents ValueChanged="function (s,e) { onCboRoomValueChanged();}" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <dxcp:ASPxCallbackPanel ID="cbpPanelView3" runat="server" Width="100%" ClientInstanceName="cbpPanelView3"
                                    ShowLoadingPanel="false" OnCallback="cbpPanelView3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView3').show(); }"
                                        EndCallback="function(s,e){ onCbpPanelView3EndCallback(s);}" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage1">
                                                <asp:GridView ID="grdView3" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable"
                                                    EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView3_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn visitID" ItemStyle-CssClass="hiddenColumn visitID" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="40px">
                                                            <HeaderTemplate>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div <%# Eval("GCScheduleStatus").ToString() == "X449^03" ? "class='blink-alert'":"" %>>
                                                                    <img id="imgStatusImageUri1" runat="server" width="24" height="24" alt="" visible="true" src="" />    
                                                                </div>    
                                                            </ItemTemplate>      
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="cfScheduledDateInString" HeaderText="Tanggal" HeaderStyle-CssClass="cfScheduledDate"
                                                            ItemStyle-CssClass="cfScheduledDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="ScheduledTime" HeaderText="Jam" HeaderStyle-CssClass="scheduledTime"
                                                            ItemStyle-CssClass="scheduledTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="50px" />
                                                        <asp:BoundField DataField="RoomCode" HeaderText="Ruang Operasi" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Informasi Pasien")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <colgroup>
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td style="width: 100%">
                                                                            <div>
                                                                                <span style="font-weight: bold; font-size: 11pt">
                                                                                    <%#: Eval("PatientName") %></span></div>
                                                                            <div>
                                                                                <%#: Eval("MedicalNo") %>,
                                                                                <%#: Eval("RegistrationNo")%></div>
                                                                            <div style="font-style: italic">
                                                                                <%#: Eval("cfPatientLocation")%></div>
                                                                            <div>
                                                                                <%#: Eval("ParamedicName")%></div>
                                                                            <div>
                                                                                <%#: Eval("BusinessPartnerName")%></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Team Pelaksana" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div>
                                                                    <textarea style="border: 0; width: 99%; min-height: 100px; max-height:150px; background-color: transparent;"
                                                                        readonly><%#: DataBinder.Eval(Container.DataItem, "cfTeamMemberList") %> </textarea>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Jenis Tindakan" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div>
                                                                    <textarea style="border: 0; width: 99%; min-height: 100px; max-height:150px; background-color: transparent;"
                                                                        readonly><%#: DataBinder.Eval(Container.DataItem, "cfProcedureLineList") %> </textarea>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="EstimatedDuration" HeaderText="Durasi (menit)" HeaderStyle-CssClass="duration"
                                                            ItemStyle-CssClass="duration" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                            HeaderStyle-Width="75px" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="150px">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Riwayat Infeksi")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <colgroup>
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td style="width: 100%">
                                                                            <div>
                                                                                <span>
                                                                                    <%#: Eval("cfIsHasInfectious") %></span></div>
                                                                            <div <%# Eval("IsHasInfectiousDisease").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                class="blink-alert">
                                                                                <br />
                                                                                <%#: Eval("cfInfectiousDisease") %></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="150px">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Komorbid")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <colgroup>
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td style="width: 100%">
                                                                            <div>
                                                                                <span>
                                                                                    <%#: Eval("cfIsHasComorbidities") %></span></div>
                                                                            <div <%# Eval("IsHasComorbidities").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                class="blink-alert">
                                                                                <br />
                                                                                <%#: Eval("cfComorbidities") %></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada informasi order jadwal kamar operasi di periode ini")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView3">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging" style="display:none">
                                    <div class="wrapperPaging">
                                        <div id="paging3">
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="contentDetailPage3" class="container contentDetail  w3-animate-top" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal")%></label>
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
                                            <asp:TextBox ID="txtFromScheduleDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
<%--                                        <td>
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToScheduleDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>--%>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Ruang Kamar Operasi")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboOperatingRoomAll" ClientInstanceName="cboOperatingRoomAll"
                                    Width="250px">
                                    <ClientSideEvents ValueChanged="function (s,e) { onCboRoom2ValueChanged();}" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="3">
                                <dxcp:ASPxCallbackPanel ID="cbpPanelView4" runat="server" Width="100%" ClientInstanceName="cbpPanelView4"
                                    ShowLoadingPanel="false" OnCallback="cbpPanelView4_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView4').show(); }"
                                        EndCallback="function(s,e){ onCbpPanelView4EndCallback(s);}" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                            <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGridPatientPage1">
                                                <asp:GridView ID="grdView4" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView4_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn visitID" ItemStyle-CssClass="hiddenColumn visitID" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="40px">
                                                            <HeaderTemplate>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div <%# Eval("GCScheduleStatus").ToString() == "X449^03" ? "class='blink-alert'":"" %>>
                                                                    <img id="imgStatusImageUri" runat="server" width="24" height="24" alt="" visible="true" src="" />    
                                                                </div>    
                                                            </ItemTemplate>      
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="cfScheduledDateInString" HeaderText="Tanggal" HeaderStyle-CssClass="cfScheduledDate"
                                                            ItemStyle-CssClass="cfScheduledDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="ScheduledTime" HeaderText="Jam" HeaderStyle-CssClass="scheduledTime"
                                                            ItemStyle-CssClass="scheduledTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="50px" />
                                                        <asp:BoundField DataField="RoomCode" HeaderText="Ruang Operasi" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Informasi Pasien")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <colgroup>
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td style="width: 100%">
                                                                            <div>
                                                                                <span style="font-weight: bold; font-size: 11pt">
                                                                                    <%#: Eval("PatientName") %></span></div>
                                                                            <div>
                                                                                <%#: Eval("MedicalNo") %>,
                                                                                <%#: Eval("RegistrationNo")%></div>
                                                                            <div style="font-style: italic">
                                                                                <%#: Eval("cfPatientLocation")%></div>
                                                                            <div>
                                                                                <%#: Eval("ParamedicName")%></div>
                                                                            <div>
                                                                                <%#: Eval("BusinessPartnerName")%></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Team Pelaksana" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div>
                                                                    <textarea style="border: 0; width: 99%; min-height: 100px; max-height:150px; background-color: transparent;"
                                                                        readonly><%#: DataBinder.Eval(Container.DataItem, "cfTeamMemberList") %> </textarea>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Jenis Tindakan" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div>
                                                                    <textarea style="border: 0; width: 99%; min-height: 100px; max-height:150px; background-color: transparent;"
                                                                        readonly><%#: DataBinder.Eval(Container.DataItem, "cfProcedureLineList") %> </textarea>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="EstimatedDuration" HeaderText="Durasi (menit)" HeaderStyle-CssClass="duration"
                                                            ItemStyle-CssClass="duration" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                            HeaderStyle-Width="75px" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="150px">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Riwayat Infeksi")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <colgroup>
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td style="width: 100%">
                                                                            <div>
                                                                                <span>
                                                                                    <%#: Eval("cfIsHasInfectious") %></span></div>
                                                                            <div <%# Eval("IsHasInfectiousDisease").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                class="blink-alert">
                                                                                <br />
                                                                                <%#: Eval("cfInfectiousDisease") %></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="150px">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Komorbid")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <colgroup>
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td style="width: 100%">
                                                                            <div>
                                                                                <span>
                                                                                    <%#: Eval("cfIsHasComorbidities") %></span></div>
                                                                            <div <%# Eval("IsHasComorbidities").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                class="blink-alert">
                                                                                <br />
                                                                                <%#: Eval("cfComorbidities") %></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <colgroup>
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td style="text-align:center">
                                                                            <div>
                                                                                <img class="imgViewSurgeryMR imgLink" title='<%=GetLabel("Lihat Pengkajian Kamar Operasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                    alt="" testOrderID='<%#Eval("TestOrderID") %>' style="display:none"/>      
                                                                            </div>                    
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="display:none">
                                                                                <colgroup>
                                                                                    <col style="width: 30px;" />
                                                                                    <col style="width: 30px;" />
                                                                                    <col style="width: 30px;" />
                                                                                    <col style="width: 30px;" />
                                                                                    <col />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div <%# Eval("cfIsCompleted").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                            <img class="btnEdit imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# Eval("cfIsAllowEdit").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                patientinfo='<%#Eval("PatientName") %>' enabled='<%#Eval("cfIsAllowEdit") %>'
                                                                                                patientlocation='<%#Eval("cfPatientLocation") %>' />
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div <%# Eval("cfIsCompleted").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                            <img class="btnDelete imgLink" title='<%=GetLabel("Hapus Jadwal")%>' src='<%# Eval("cfIsAllowEdit").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                patientinfo='<%#Eval("PatientName") %>' enabled='<%#Eval("cfIsAllowEdit") %>' />
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div <%# Eval("cfIsCompleted").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                            <img class="btnStartOrder imgLink" src='<%# Eval("cfIsStarted").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/start.png") : ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                                title='<%# Eval("cfIsStarted").ToString() == "False" ? "Mulai Operasi" : "Selesai Operasi" %>'
                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                patientinfo='<%#Eval("PatientName") %>' isallowstart='<%#Eval("cfIsAllowSTART") %>'
                                                                                                isstarted='<%#Eval("cfIsStarted") %>' appointmentid='<%#Eval("appointmentID") %>'
                                                                                                isappointmentregistered='<%#Eval("cfIsAppointmentRegistered") %>' isdocumentchecklistcompleted='<%#Eval("IsDocumentCheckListCompleted") %>' />
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div <%# Eval("cfIsCompleted").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                            <img class="btnDocumentChecklist imgLink" title='<%=GetLabel("Checklist Dokumen")%>'
                                                                                                src='<%# Eval("cfIsAllowEdit").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/list_disabled.png") : ResolveUrl("~/Libs/Images/Button/list.png")%>'
                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                patientinfo='<%#Eval("PatientName") %>' enabled='<%#Eval("cfIsAllowEdit") %>'
                                                                                                patientlocation='<%#Eval("cfPatientLocation") %>' />
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div <%# Eval("AppointmentID").ToString() == "0" ? "Style='display:none'":"" %>>
                                                                                            <img class="btnUpdateRegistration imgLink" title='<%=GetLabel("Ubah Nomor Registrasi")%>'
                                                                                                src='<%# Eval("cfIsAppointmentRegistered").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/link_disabled.png") : ResolveUrl("~/Libs/Images/Button/link.png")%>'
                                                                                                alt="" roomscheduleid='<%#Eval("ID") %>' visitid='<%#Eval("VisitID") %>' orderid='<%#Eval("TestOrderID") %>'
                                                                                                patientinfo='<%#Eval("PatientName") %>' enabled='<%#Eval("cfIsAllowEdit") %>'
                                                                                                patientlocation='<%#Eval("cfPatientLocation") %>' />
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                                <div style="display: none;">
                                                                                    <input type="button" class="btnProcess" id="btnConfirm" value='<%=GetLabel("Confirm") %>'
                                                                                        class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-medium" />
                                                                                </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada informasi order jadwal kamar operasi di periode ini")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="Div2">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="paging4">
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteRoomSchedule" runat="server" Width="100%" ClientInstanceName="cbpDeleteRoomSchedule"
            ShowLoadingPanel="false" OnCallback="cbpDeleteRoomSchedule_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpRoomScheduleStatusEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpStartOrder" runat="server" Width="100%" ClientInstanceName="cbpStartOrder"
            ShowLoadingPanel="false" OnCallback="cbpStartOrder_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpRoomScheduleStatusEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpStopOrder" runat="server" Width="100%" ClientInstanceName="cbpStopOrder"
            ShowLoadingPanel="false" OnCallback="cbpStopOrder_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpRoomScheduleStatusEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
