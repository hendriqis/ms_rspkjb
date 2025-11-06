<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="VoidPatientDischarge.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VoidPatientDischarge" %>

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
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
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
            $('#<%=btnCancelDischarge.ClientID %>').click(function () {
                var errMessage = { text: "" };
                var filterExpression = { text: "" };

                getCheckedRegistration();
                if ($('#<%=hdnSelectedRegistration.ClientID %>').val() == '')
                    showToast('Warning', 'Please Select Registration First');
                else
                    cbpCancelPatientDischarge.PerformCallback();
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
            var lstSelectedHealthcareServiceUnitID = '';
            var lstSelectedRoomID = '';
            var lstSelectedClassID = '';
            var result = '';
            $('#<%=grdView.ClientID %> .chkPatient input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var hsuID = $(this).closest('tr').find('.HealthcareServiceUnitID').val();
                    var roomID = $(this).closest('tr').find('.RoomID').val();
                    var classID = $(this).closest('tr').find('.ClassID').val();

                    if (lstSelectedRegistration != '')
                        lstSelectedRegistration += ',';
                    lstSelectedRegistration += key;

                    if (lstSelectedHealthcareServiceUnitID != '')
                        lstSelectedHealthcareServiceUnitID += ',';
                    lstSelectedHealthcareServiceUnitID += hsuID;

                    if (lstSelectedRoomID != '')
                        lstSelectedRoomID += ',';
                    lstSelectedRoomID += roomID;

                    if (lstSelectedClassID != '')
                        lstSelectedClassID += ',';
                    lstSelectedClassID += classID;
                }
            });
            $('#<%=hdnSelectedRegistration.ClientID %>').val(lstSelectedRegistration);
            $('#<%=hdnParamHealthcareServiceUnitID.ClientID %>').val(lstSelectedHealthcareServiceUnitID);
            $('#<%=hdnParamRoomID.ClientID %>').val(lstSelectedRoomID);
            $('#<%=hdnParamClassID.ClientID %>').val(lstSelectedClassID);
        }

        function UpdateRoomAplicares() {
            var str = $('#<%:hdnParamHealthcareServiceUnitID.ClientID %>').val();
            var str1 = $('#<%:hdnParamRoomID.ClientID %>').val();
            var str2 = $('#<%:hdnParamClassID.ClientID %>').val();
            var str_array = str.split(',');
            var str_array1 = str1.split(',');
            var str_array2 = str2.split(',');

            for (var z = 0; z < str_array.length; z++) {
                var paramHSUID = str_array[z];
                var paramRoomID = str_array1[z];
                var paramClassID = str_array2[z];

                var filterSUR = "HealthcareServiceUnitID = " + paramHSUID + " AND RoomID = " + paramRoomID + " AND ClassID = " + paramClassID + " AND IsDeleted = 0 AND IsAplicares = 1";
                Methods.getListObject('GetServiceUnitRoomList', filterSUR, function (resultSUR) {
                    for (i = 0; i < resultSUR.length; i++) {
                        var hsuID = resultSUR[i].HealthcareServiceUnitID;
                        var classID = resultSUR[i].ClassID;
                        var filterExpression = "HealthcareServiceUnitID = " + hsuID + " AND ClassID = " + classID;
                        Methods.getObject('GetvServiceUnitAplicaresList', filterExpression, function (result) {
                            if (result != null) {
                                var kodeKelas = result.AplicaresClassCode;
                                var kodeRuang = result.ServiceUnitCode;
                                var namaRuang = result.ServiceUnitName;
                                var jumlahKapasitas = result.CountBedAll;
                                var jumlahKosong = result.CountBedEmpty;
                                var jumlahKosongPria = 0;
                                var jumlahKosongWanita = 0;
                                var jumlahKosongPriaWanita = result.CountBedEmpty;

                                if (result.CountIsSendToAplicares == 0) {
                                    AplicaresService.createRoom(kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, function (resultRoom) {
                                        if (resultRoom != null) {
                                            try {
                                                AplicaresService.updateStatusSendToAplicares(result.HealthcareServiceUnitID, result.ClassID, function (resultUpdate) {
                                                    if (resultUpdate != null) {
                                                        try {
                                                            var resultUpdate = resultUpdate.split('|');
                                                            if (resultUpdate[0] == "1") {
                                                                showToast('INFORMATION', "SUCCESS");
                                                            }
                                                            else {
                                                                showToast('FAILED', resultUpdate[2]);
                                                            }
                                                        } catch (error) {
                                                            showToast('FAILED', error);
                                                        }
                                                    }
                                                });
                                            } catch (err) {
                                                showToast('FAILED', err);
                                            }
                                        }
                                    });
                                } else {
                                    AplicaresService.updateRoomStatus(kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, function (resultRoom) {
                                        if (resultRoom != null) {
                                            try {
                                                AplicaresService.updateStatusSendToAplicares(result.HealthcareServiceUnitID, result.ClassID, function (resultUpdate) {
                                                    if (resultUpdate != null) {
                                                        try {
                                                            var resultUpdate = resultUpdate.split('|');
                                                            if (resultUpdate[0] == "1") {
                                                                showToast('INFORMATION', "SUCCESS");
                                                            }
                                                            else {
                                                                showToast('FAILED', resultUpdate[2]);
                                                            }
                                                        } catch (error) {
                                                            showToast('FAILED', error);
                                                        }
                                                    }
                                                });
                                            } catch (err) {
                                                showToast('FAILED', err);
                                            }
                                        }
                                    });
                                }
                            }
                        });
                    }
                });
            }
        }

        function onCbpCancelPatientDischargeEndCallback(s) {
            var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                if (isBridgingAplicares == "1") {
                    UpdateRoomAplicares();
                }

                var param = result[1].split(',');
                var tempText = "";
                for (var a = 0; a < param.length - 1; a++) {
                    if (tempText != '')
                        tempText += "<br />";
                    tempText += "Bed <b>" + param[a] + "</b> Cannot Be Used";
                }
//////                showToast('Cancellation Patient Discharge Success', tempText);

                cbpView.PerformCallback('refresh');
            }
            else {
                if (result[1] != '')
                    showToast('Cancellation Patient Discharge Failed', 'Error Message : ' + result[1]);
                else
                    showToast('Cancellation Patient Discharge Failed', '');
            }
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnParamHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnParamRoomID" runat="server" />
    <input type="hidden" value="" id="hdnParamClassID" runat="server" />
    <input type="hidden" value="" id="hdnHealthCareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnSelectedRegistration" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToAplicares" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowCancelDischargeForDischargeDead" runat="server" />
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
                                    <%=GetLabel("Discharge Date")%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDischargeDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr id="trServiceUnit" runat="server">
                                <td>
                                    <label>
                                        <%=GetLabel("Unit Pelayanan") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="200px"
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
                                                    <input type="hidden" class="HealthcareServiceUnitID" value='<%#:Eval("HealthcareServiceUnitID") %>' />
                                                    <input type="hidden" class="RoomID" value='<%#:Eval("RoomID") %>' />
                                                    <input type="hidden" class="ClassID" value='<%#:Eval("ClassID") %>' />
                                                    <asp:CheckBox ID="chkPatient" runat="server" CssClass="chkPatient" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="Registration No" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MedicalNo" HeaderText="Medical No" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Service Unit" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RoomName" HeaderText="Room" HeaderStyle-Width="120px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ClassName" HeaderText="Class" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="BedCode" HeaderText="Bed" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:CheckBoxField DataField="cfPatientDie" HeaderText="Die" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
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
                        <input type="hidden" runat="server" id="hdnSelectedRoomID" value="" />
                        <input type="hidden" runat="server" id="hdnSelectedClassID" value="" />
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
