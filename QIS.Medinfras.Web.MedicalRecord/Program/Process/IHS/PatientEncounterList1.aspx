<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="PatientEncounterList1.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientEncounterList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <style type="text/css">
        .LvColor
        {
            background-color: Silver !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtAppointmentDate.ClientID %>');
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
            }
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            }, 0);
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            var gender = $('.hdnPatientGender').val();
            Methods.checkImageError('imgPatientImage', 'patient', gender);
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var gender = $('.hdnPatientGender').val();
            Methods.checkImageError('imgPatientImage', 'patient', gender);
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else if(param[0] == 'print') {
                cbpView.PerformCallback('refresh');
            }
        }
        //#endregion

        function refreshGrdRegisteredPatient() {

            cbpView.PerformCallback('refresh');
        }

        function onGetPhysicianFilterExpression() {
            var filterExpression = "";
            var healthcareServiceUnitID = cboServiceUnit.GetValue();
            if (healthcareServiceUnitID != '0')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ") AND IsDeleted = 0";
            else
                filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        function onCboServiceUnitValueChanged() {
            $('#<%=txtPhysicianCode.ClientID %>').val('');
            $('#<%=hdnPhysicianID.ClientID %>').val('');
            $('#<%=txtPhysicianName.ClientID %>').val('');
        }

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }

        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtAppointmentDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtAppointmentDate.ClientID %>').removeAttr('readonly');
            //            onRefreshGridView();
        });

        $('.btnPrintTracerAppointment').die('click');
        $('.btnPrintTracerAppointment').live('click', function () {
            $row = $(this).closest('tr');
            var apmID = $row.find('.keyfield').html();
            cbpView.PerformCallback('print|' + apmID);
        });

        $('.btnPrintTracerRMAppointment').die('click');
        $('.btnPrintTracerRMAppointment').live('click', function () {
            $row = $(this).closest('tr');
            var apmID = $row.find('.keyfield').html();
            cbpView.PerformCallback('print_rm|' + apmID);
        });
        $('.btnDraftTransaction').die('click');
        $('.btnDraftTransaction').live('click', function () {
            $row = $(this).closest('tr');
            var id = parseInt($row.find('.keyfield').html());

            var isHasCharges = false;
            var isHasTestOrder = false;
            var isHasTestOrderHd = false;
            var isHasServiceOrder = false;
            var isHasPrescriptionOrder = false;

            var filterExpressionDraftCharges = "AppointmentID = '" + id + "' AND GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "' AND ISNULL(GCTransactionDetailStatus,'') != '" + Constant.TransactionStatus.VOID + "' AND IsDeleted = 0";
            var filterExpressionDraftTestOrderHd = "AppointmentID = '" + id + "' AND GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "'";
            var filterExpressionDraftTestOrder = "AppointmentID = '" + id + "' AND GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "' AND ISNULL(GCDraftTestOrderStatus,'') != '" + Constant.OrderStatus.VOID + "' AND IsDeleted = 0";
            var filterExpressionDraftServiceOrder = "AppointmentID = '" + id + "' AND GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "' AND ISNULL(GCDraftServiceOrderStatus,'') != '" + Constant.OrderStatus.VOID + "' AND IsDeleted = 0";
            var filterExpressionDraftPrescriptionOrder = "AppointmentID = '" + id + "' AND GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "' AND ISNULL(GCDraftPrescriptionOrderStatus,'') != '" + Constant.OrderStatus.VOID + "' AND IsDeleted = 0";

            Methods.getObject('GetvDraftPatientChargesDtList', filterExpressionDraftCharges, function (result) {
                if (result != null) {
                    isHasCharges = true;
                }
                else {
                    isHasCharges = false;
                }
            });

            Methods.getObject('GetvDraftTestOrderDtList', filterExpressionDraftTestOrder, function (result) {
                if (result != null) {
                    isHasTestOrder = true;
                }
                else {
                    isHasTestOrder = false;
                }
            });

            Methods.getObject('GetvDraftServiceOrderDtList', filterExpressionDraftServiceOrder, function (result) {
                if (result != null) {
                    isHasServiceOrder = true;
                }
                else {
                    isHasServiceOrder = false;
                }
            });

            Methods.getObject('GetvDraftPrescriptionOrderDtList', filterExpressionDraftPrescriptionOrder, function (result) {
                if (result != null) {
                    isHasPrescriptionOrder = true;
                }
                else {
                    isHasPrescriptionOrder = false;
                }
            });

            if (isHasCharges == true || isHasTestOrderHd == true || isHasTestOrder == true || isHasServiceOrder == true || isHasPrescriptionOrder == true) {
                userControlType = 'processdraft';
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/DraftAppointmentInfoCtl.ascx");
                openUserControlPopup(url, id, 'Draft Transaction And Order', 1250, 600);
            }
            else {
                showToast('INFO', "Tidak ada draft informasi order pada appointment ini");
            }
        });
        
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentID" runat="server" />
    <input type="hidden" value = "" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitIDLaboratory" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitIDRadiology" runat="server" />
    
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=HttpUtility.HtmlEncode(GetMenuCaption())%>
        </div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Perjanjian")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppointmentDate" Width="120px" runat="server" CssClass="datepicker" />
                                    <asp:CheckBox ID="chkIsPreviousEpisodePatient" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                </td>
                            </tr>
                            <tr id="trServiceUnit" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="350px"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblPhysician">
                                        <%=GetLabel("Dokter / Tenaga Medis")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                                    <table style="width: 350px" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 50px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPhysicianCode" Width="50px" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="98%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Dokter" FieldName="ParamedicName" />
                                            <qis:QISIntellisenseHint Text="Appointment Method" FieldName="AppointmentMethod" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr id="trRegistrationStatus" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        Status Registrasi</label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboRegistrationStatus" ClientInstanceName="cboRegistrationStatus"
                                        runat="server" Width="350px">
                                        <%--<ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />--%>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <%=GetLabel("Halaman Ini Akan")%>
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                        <%=GetLabel("Setiap")%>
                        <%=GetRefreshGridInterval() %>
                        <%=GetLabel("Menit")%>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s);}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyfield" style="display: none">
                                                    </th>
                                                    <th style="width: 30px">
                                                        <%=GetLabel("No. Antrian")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Informasi Kunjungan")%>
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("No. Appointment")%>
                                                    </th>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("Tanggal dan Waktu Perjanjian")%>
                                                    </th>
                                                    <th style="width: 200px">
                                                        <%=GetLabel("Informasi Pasien")%>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("Informasi Kontak")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Penjamin Bayar")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Mobile / Online")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Pasien Baru")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Registrasi")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Batal")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Print")%>
                                                    </th>
                                                     <th style="width: 40px">
                                                        <%=GetLabel("Print Tracer")%>
                                                    </th>
                                                     <th style="width: 40px">
                                                        <%=GetLabel("Rencana Kontrol")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="14">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyfield" style="display: none">
                                                    </th>
                                                    <th style="width: 30px">
                                                        <%=GetLabel("No. Antrian")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Informasi Kunjungan")%>
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("No. Appointment")%>
                                                    </th>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("Tanggal dan Waktu Perjanjian")%>
                                                    </th>
                                                    <th style="width: 200px">
                                                        <%=GetLabel("Informasi Pasien")%>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("Informasi Kontak")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Penjamin Bayar")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Mobile / Online")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Pasien Baru")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Registrasi")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Batal")%>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <%=GetLabel("Print")%>
                                                    </th>
                                                     <th style="width: 40px">
                                                        <%=GetLabel("Print Tracer")%>
                                                    </th>
                                                     <th style="width: 40px">
                                                        <%=GetLabel("Rencana Kontrol")%>
                                                    </th>
                                                </tr>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trItem">
                                                <td class="keyfield" style="display: none">
                                                    <%#: Eval("AppointmentID") %>
                                                </td>
                                                <td align="center">
                                                    <div>
                                                        <%#: Eval("QueueNo") %>
                                                    </div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        <%#: Eval("ServiceUnitName") %>
                                                        -
                                                        <%#: Eval("ParamedicName") %>
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div style="font-weight: bold">
                                                        <%#: Eval("AppointmentNo") %>
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div></div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        <%#: Eval("cfPatientNameSalutation")%></div>
                                                </td>
                                                <td align="center">
                                                    <div>
                                                        </div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        <%#: Eval("BusinessPartnerName") %></div>
                                                </td>
                                                <td align="center">
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkIsFromMobileApps" runat="server"
                                                            Enabled="false" />
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsNewPatient")%>'
                                                            Enabled="false" />
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkIsRegistration" runat="server" 
                                                            Enabled="false" />
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkDeleteAppointment" runat="server" 
                                                            Enabled="false" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <input type="button" class="btnPrintTracerAppointment" value='<%=GetLabel("PRINT") %>'
                                                        style="width: 60px; height: 25px" />
                                                </td>
                                                  <td>

                                                    <input type="button" class="btnPrintTracerRMAppointment" value='<%=GetLabel("PRINT") %>'
                                                        style="width: 60px; height: 25px" />
                                                </td>
                                                 <td>

                                                    <input type="button" class="btnDraftTransaction" value='<%=GetLabel("View") %>'
                                                        style="width: 60px; height: 25px" />
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
    </div>
</asp:Content>
