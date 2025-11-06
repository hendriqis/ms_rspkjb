<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="PatientAppointmentInformationPerParamedicPerDay.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientAppointmentInformationPerParamedicPerDay" %>

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
            background-color: #F7DC6F !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#calAppointment").datepicker({
                defaultDate: "w",
                changeMonth: true,
                changeYear: true,
                dateFormat: "dd-mm-yy",
                //minDate: "0",
                onSelect: function (dateText, inst) {
                    $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                    if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() != '') {
                        refreshGrdRegisteredPatient();
                    }
                }
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
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
                showCount();
            }
        }
        //#endregion

        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }
        function showCount() {
            var TotalApm = $('#<%=hdnTotalApm.ClientID %>').val();
            var TotalRegis = $('#<%=hdnTotalRegisApm.ClientID %>').val();
            var TotalGoShow = $('#<%=hdnTotalGoShow.ClientID %>').val();
            var TotalReceivingTreatment = $('#<%=hdnTotalReceivingTreatment.ClientID %>').val();
            var hdnIsTimeSlot = $('#<%=hdnIsTimeSlot.ClientID %>').val();
            var hdnTotalPatient = $('#<%=hdnTotalPatient.ClientID %>').val();
            var hdnTotalNonRegis = $('#<%=hdnTotalNonRegis.ClientID %>').val();
            $('#txtTotalApm').val(TotalApm);
            $('#txtTotalRegis').val(TotalRegis);
            $('#txtTotalGoShow').val(TotalGoShow);
            $('#txtTotalReceivingTreatment').val(TotalReceivingTreatment);
            $('#txtTotalPatient').val(hdnTotalPatient);
            $('#txtTotalNonRegis').val(hdnTotalNonRegis);
            if (hdnIsTimeSlot = "1") {
                $('.thJam').show();
                $('.thNoAntrian').hide();

            } else {
                $('.thJam').hide();
                $('.thNoAntrian').show();
            }

        }
        //#region Service Unit
        function onGetServiceUnitFilterExpression() {
            var filterExpression = "HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitParamedic WHERE ParamedicID = " + $('#<%=hdnParamedicID.ClientID %>').val() + ") AND DepartmentID IN ('" + Constant.Facility.OUTPATIENT + "','" + Constant.Facility.DIAGNOSTIC + "') AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblServiceUnit.lblLink').live('click', function () {
            if ($('#<%=hdnParamedicID.ClientID %>').val() != '') {
                openSearchDialog('serviceunitperhealthcare', onGetServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            }
            else {
                showToast('Warning', 'User anda bukan sebagai Paramedis');
            }
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            if ($('#<%=hdnParamedicID.ClientID %>').val() != '') {
                var filterExpression = onGetServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);

                        refreshGrdRegisteredPatient();
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%=txtServiceUnitCode.ClientID %>').val('');
                $('#<%=txtServiceUnitName.ClientID %>').val('');
                showToast('Warning', 'User anda bukan sebagai Paramedis');
            }
        }
        //#endregion 

        function oncboSessionValueChanged() {
            if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() != '') {
                refreshGrdRegisteredPatient();
            }
            else {
                showToast('Warning', 'Silahkan Pilih Service Unit Terlebih Dahulu');
            }
        }
    </script>
    <input type="hidden" id="hdnParamedicID" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=HttpUtility.HtmlEncode(GetMenuCaption())%>
        </div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top; border-right: 1px solid #AAA;">
                    <div style="overflow-y: hidden; overflow-x: hidden;">
                        <fieldset id="fsPatientList">
                            <table class="tblEntryContent" style="width: 60%;">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDate" />
                                        <div id="calAppointment">
                                        </div>
                                    </td>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory">
                                                        <b>
                                                            <%=GetLabel("Doctor")%></b>
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicName" Width="100%" ReadOnly="true" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblLink lblMandatory" id="lblServiceUnit">
                                                        <b>
                                                            <%=GetLabel("Service Unit")%></b></label>
                                                </td>
                                                <td colspan="3">
                                                    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceUnitCode" CssClass="required" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trSlot" runat="server">
                                                <td valign="top">
                                                    <label class="lblMandatory">
                                                        <b>
                                                            <%=GetLabel("Session")%></b>
                                                    </label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboSession" ClientInstanceName="cboSession" Width="20%" runat="server">
                                                        <ClientSideEvents ValueChanged="function(s,e) { oncboSessionValueChanged(s); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                              <table>
                                <td valign="top">
                                        <table>
                                            <tr>
                                                <td>Total Appointment</td>
                                                <td><input type="text" readonly="readonly" id="txtTotalApm" /></td>
                                                <td>Total Appointment Sudah Registrasi</td>
                                                <td><input type="text" readonly="readonly" id="txtTotalRegis" /></td>
                                                <td>Total Appointment Belum Registrasi</td>
                                                <td><input type="text" readonly="readonly" id="txtTotalNonRegis" /></td>
                                            </tr>
                                            <tr>
                                                <td>Total Registrasi Langsung</td>
                                                <td><input type="text" readonly="readonly" id="txtTotalGoShow" /></td>
                                                <td>Total Sudah Dilayani</td>
                                                <td><input type="text" readonly="readonly" id="txtTotalReceivingTreatment" /></td>
                                                 <td>Total Pasien Datang</td>
                                                <td><input type="text" readonly="readonly" id="txtTotalPatient" /></td>
                                            </tr>
                                        </table>
                                    </td>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s);}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                               <input type="hidden" id="hdnTotalApm" runat="server" />
                               <input type="hidden" id="hdnTotalRegisApm" runat="server" />
                               <input type="hidden" id="hdnTotalGoShow" runat="server" />
                               <input type="hidden" id="hdnTotalReceivingTreatment" runat="server" />
                               <input type="hidden" id="hdnIsTimeSlot" runat="server" />
                               <input type="hidden" id="hdnTotalPatient" runat="server" />
                               <input type="hidden" id="hdnTotalNonRegis" runat="server" />
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px"  class="thJam">
                                                         <%=GetLabel("Jam")%>  
                                                    </th>
                                                    <th style="width: 40px" class="thNoAntrian">
                                                        <%=GetLabel("No. Antrian")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("No. Perjanjian")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("No. Registrasi")%>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("No. RM")%>
                                                    </th>
                                                    <th style="width: 250px">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th style="width: 50px">
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="6">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px"  class="thJam">
                                                         <%=GetLabel("Jam")%>  
                                                    </th>
                                                    <th style="width: 40px" class="thNoAntrian">
                                                        <%=GetLabel("No. Antrian")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("No. Perjanjian")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("No. Registrasi")%>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("No. RM")%>
                                                    </th>
                                                    <th style="width: 250px">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th style="width: 50px">
                                                    </th>
                                                </tr>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trItem">
                                                <td align="center"  class="thJam">
                                                    <div>
                                                        <%#: Eval("StartTime") %>
                                                    </div>
                                                </td>
                                                <td align="center" class="thNoAntrian">
                                                    <div>
                                                        <%#: Eval("QueueNo") %>
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div style="font-weight: bold">
                                                        <%#: Eval("AppointmentNo") %>
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div style="font-weight: bold">
                                                        <%#: Eval("RegistrationNo") %>
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div>
                                                        <%#: Eval("MedicalNo") %></div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        <%#: Eval("PatientName")%></div>
                                                </td>
                                                <td align="center" valign="top" style="width: 20px">
                                                    <div <%# Eval("IsReceivingTreatment").ToString() == "0" ? "Style='display:none'":"" %>>
                                                        <img id="imgReceivingTreatmentUri" runat="server" width="24" height="24" alt="" visible="true" />
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
    </div>
</asp:Content>
