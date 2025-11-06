<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="CancelPatientDischargeList.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.CancelPatientDischargeList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                if (IsValid(evt, 'fsBedStatus', 'mpBedStatus'))
                    refreshGrdRegisteredPatient();
            });

            $('#btnConfirm').click(function () {
                getCheckedRegistration();
                if ($('#<%=hdnSelectedRegistration.ClientID %>').val() == '')
                    showToast('Warning', 'Please Select Registration First');
                else
                    cbpCancelPatientDischarge.PerformCallback();
            });

            setDatePicker('<%=txtDischargeDate.ClientID %>');
            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                refreshGrdRegisteredPatient();
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function getCheckedRegistration() {
            var lstSelectedRegistration = '';
            var result = '';
            $('#<%=grdView.ClientID %> .chkPatient input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstSelectedRegistration != '')
                        lstSelectedRegistration += ',';
                    lstSelectedRegistration += key;
                }
            });
            $('#<%=hdnSelectedRegistration.ClientID %>').val(lstSelectedRegistration);
        }

        $('#chkSelectAllPatient').die('change');
        $('#chkSelectAllPatient').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkPatient').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });

        function onCbpCancelPatientDischargeEndCallback(s) {
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                checkBridgingAplicares();
            var param = result[1].split(',');
            var tempText = "";
            for (var a = 0; a < param.length - 1; a++) {
                if (tempText != '')
                    tempText += "<br />";
                tempText += "Bed <b>" + param[a] + "</b> Cannot Be Used";
            }
            showToast('Save Success', tempText);

                cbpView.PerformCallback('refresh');
            }
            else {
                if (result[1] != '')
                    showToast('Save Failed', 'Error Message : ' + result[1]);
                else
                    showToast('Save Failed', '');
            }
            hideLoadingPanel();
        }

        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }

        function checkBridgingAplicares() {
            if ($('#<%:hdnIsBridgingToAplicares.ClientID %>').val() == '1') {
                var str = $('#<%:hdnSelectedHealthcareServiceUnitID.ClientID %>').val();
                var str2 = $('#<%:hdnSelectedClassID.ClientID %>').val();
                var str_array = str.split('|');
                var str_array2 = str2.split('|');
                for (var i = 0; i < str_array.length; i++) {
                    // Trim the excess whitespace.
                    str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
                    // Add additional code here, such as:

                    var id = str_array[i];
                    var classId = str_array2[i];
                    var filterExpression = "HealthcareServiceUnitID = '" + id + "'";
                    if (id != $('#<%:hdnHealthcareServiceUnitICUID.ClientID %>').val() &&
                    id != $('#<%:hdnHealthcareServiceUnitNICUID.ClientID %>').val() &&
                    id != $('#<%:hdnHealthcareServiceUnitPICUID.ClientID %>').val()) {
                        filterExpression += " AND ClassID = '" + classId + "'";
                    }
                    Methods.getObject('GetvServiceUnitRoomStatusList', filterExpression, function (result) {
                        if (result != null) {
                            var kodeKelas = result.ApplicaresClassCode;
                            var jumlahKosong = result.BedEmpty;
                            var jumlahKapasitas = result.BedCount;
                            var kodeRuang = result.ServiceUnitCode;
                            var namaRuang = result.ServiceUnitName;
                            AplicaresService.updateRoomStatus(kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, 0, 0, 0, function (result2) {
                                try {
                                    var obj = jQuery.parseJSON(result2);
                                    if (obj.metadata.code == '1') {
                                        showToast('Sukses', obj.metadata.message);
                                    }
                                    else {
                                        showToast('Gagal', obj.metadata.message);
                                    }
                                } catch (err) {
                                    alert(err);
                                }
                            });
                        }
                    });
                }
            }
        }
    </script>
    <input type="hidden" runat="server" id="hdnErrorInfo" value="" />
    <input type="hidden" runat="server" id="hdnIsBridgingToAplicares"  />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitICUID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitPICUID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitNICUID" value="" />
    <input type="hidden" runat="server" id="hdnSelectedRegistration" value="" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetMenuCaption()%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsBedStatus">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Pulang")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDischargeDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <input type="button" id="btnConfirm" value='<%=GetLabel("Proses") %>' />
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                    </div>
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
                                            <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="MedicalNo" HeaderText="No.RM" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Ruang Perawatan" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="RoomName" HeaderText="Kamar" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="ClassName" HeaderText="Kelas" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="BedCode" HeaderText="Tempat Tidur" HeaderStyle-Width="100px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
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
