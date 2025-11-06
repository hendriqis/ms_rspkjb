<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="VoidPhysicianDischarge.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VoidPhysicianDischarge" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li runat="server" id="btnCancelDischarge">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onBeforeProcess(filterExpression, errMessage) {
            var registrationID = getCurrentID();
            if (registrationID == '') {
                errMessage.text = 'Please Select Patient First!';
                return false;
            }
            else {
                filterExpression.text = "RegistrationID = " + registrationID;
                return true;
            }
        }

        $(function () {
            $('#<%=btnCancelDischarge.ClientID %>').click(function (evt) {
                var errMessage = { text: "" };
                var filterExpression = { text: "" };

                getCheckedRegistration();
                if ($('#<%=hdnSelectedRegistration.ClientID %>').val() == '') {
                    showToast('Warning', 'Please Select Registration First');
                }
                else {
                    var lstSelectedMember = $('#<%=hdnSelectedVisit.ClientID %>').val().split(',');
                    var id = '';

                    for (var i = 0; i < lstSelectedMember.length; i++) {
                        if (id == '') {
                            id = lstSelectedMember[i];
                        }
                        else {
                            id += "," + lstSelectedMember[i];
                        }
                    }

                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        var filterExpression = "VisitID IN (" + id + ") AND IsDeleted = 0";
                        var apptRequestNo = '';
                        Methods.getListObject('GetAppointmentRequestList', filterExpression, function (resultCheck) {
                            for (i = 0; i < resultCheck.length; i++) {
                                if (apptRequestNo == '') {
                                    apptRequestNo = resultCheck[i].AppointmentRequestNo;
                                }
                                else {
                                    apptRequestNo += ", " + resultCheck[i].AppointmentRequestNo;
                                }
                            }
                        });

                        var message = "No. Registrasi ini sudah memiliki permintaan perjanjian dengan No. <b>" + apptRequestNo +"</b>, Apakah tetap lanjutkan proses discharge?";
                        if (apptRequestNo != '') {
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    cbpCancelPatientDischarge.PerformCallback();
                                }
                            });
                        }
                        else {
                            cbpCancelPatientDischarge.PerformCallback();
                        }
                    }
                }
            });

            setDatePicker('<%=txtDischargeDate.ClientID %>');
            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtDischargeDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            refreshGrdRegisteredPatient();
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
            }, 0);
        }

        function onCboServiceUnitValueChanged(s) {
            onRefreshGridView();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }

        $('#chkSelectAllPatient').die('change');
        $('#chkSelectAllPatient').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkPatient').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });

        function getCheckedRegistration() {
            var lstSelectedRegistration = '';
            var lstSelectedVisit = '';
            var result = '';
            $('#<%=grdView.ClientID %> .chkPatient input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var registrationID = $(this).closest('tr').find('.hiddenColumn').html();
                    if (lstSelectedRegistration != '')
                        lstSelectedRegistration += ',';
                    lstSelectedRegistration += registrationID;

                    if (lstSelectedVisit != '')
                        lstSelectedVisit += ',';
                    lstSelectedVisit += key;
                }
            });
            $('#<%=hdnSelectedRegistration.ClientID %>').val(lstSelectedRegistration);
            $('#<%=hdnSelectedVisit.ClientID %>').val(lstSelectedVisit);
        }

        function onCbpCancelPatientDischargeEndCallback(s) {
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                if (result[1] != '')
                    showToast('Cancellation Physician Discharge Failed', 'Error Message : ' + result[1]);
                else
                    showToast('Cancellation Physician Discharge Failed', '');
            }
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnHealthCareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnSelectedRegistration" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedVisit" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToAplicares" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitICUID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitPICUID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitNICUID" runat="server" />
    <div style="padding: 15px">
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table>
                            <colgroup>
                                <col style="width: 150px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <%=GetLabel("Tanggal Discharge")%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDischargeDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr id="trServiceUnit" runat="server">
                                <td>
                                    <label>
                                        <%=GetLabel("Unit Pelayanan ") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="300px"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Search")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                            <%=GetLabel("Halaman Ini Akan")%>
                            <span class="lblLink" id="lblRefresh">[refresh]</span>
                            <%=GetLabel("Setiap")%>
                            <%=GetRefreshGridInterval() %>
                            <%=GetLabel("Menit")%>
                        </div>
                        <hr />
                    </fieldset>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAllPatient" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkPatient" runat="server" CssClass="chkPatient" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="No. Registrasi" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MedicalNo" HeaderText="No.RM" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi pasien yang sudah di-discharge oleh Dokter")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <dxcp:ASPxCallbackPanel ID="cbpCancelPatientDischarge" runat="server" Width="100%"
            ClientInstanceName="cbpCancelPatientDischarge" ShowLoadingPanel="false" OnCallback="cbpCancelPatientDischarge_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpCancelPatientDischargeEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server">
                    <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                        <input type="hidden" runat="server" id="hdnSelectedHealthcareServiceUnitID" value="" />
                        <input type="hidden" runat="server" id="hdnSelectedClassID" value="" />
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
