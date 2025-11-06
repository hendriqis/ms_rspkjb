<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="PatientRegistrationConfirmationList.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.PatientRegistrationConfirmationList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                if (IsValid(evt, 'fsBedStatus', 'mpBedStatus'))
                    onRefreshGridView();
            });

            $('#btnConfirm').click(function () {
                getCheckedRegistration();
                if ($('#<%=hdnSelectedRegistration.ClientID %>').val() == '')
                    showToast('Warning', 'Please Select Registration First');
                else
                    cbpRegistrationConfirmation.PerformCallback('save|');
            });

            //#region Room
            $('#<%=lblRoom.ClientID %>.lblLink').live('click', function () {
                var serviceUnitID = cboServiceUnit.GetValue();
                var filterExpression = '';
                if (serviceUnitID != '') {
                    filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
                    openSearchDialog('serviceunitroom', filterExpression, function (value) {
                        $('#<%=txtRoomCode.ClientID %>').val(value);
                        onTxtRoomCodeChanged(value);
                    });
                }
            });

            $('#<%=txtRoomCode.ClientID %>').live('change', function () {
                onTxtRoomCodeChanged($(this).val());
            });

            function onTxtRoomCodeChanged(value) {
                var filterExpression = '';
                var serviceUnitID = cboServiceUnit.GetValue();
                if (serviceUnitID != '') {
                    filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ";
                    filterExpression += "RoomCode = '" + value + "' AND IsDeleted = 0";
                    Methods.getObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnRoomID.ClientID %>').val(result.RoomID);
                            $('#<%=txtRoomName.ClientID %>').val(result.RoomName);
                        }
                        else {
                            $('#<%=hdnRoomID.ClientID %>').val('');
                            $('#<%=txtRoomCode.ClientID %>').val('');
                            $('#<%=txtRoomName.ClientID %>').val('');
                        }
                        onRefreshGridView();
                    });
                }
                else {
                    $('#<%=hdnRoomID.ClientID %>').val('');
                    $('#<%=txtRoomCode.ClientID %>').val('');
                    $('#<%=txtRoomName.ClientID %>').val('');
                }
            }
            //#endregion
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

        function onCboServiceUnitValueChanged() {
            $('#<%=hdnRoomID.ClientID %>').val('');
            $('#<%=txtRoomCode.ClientID %>').val('');
            $('#<%=txtRoomName.ClientID %>').val('');
            onRefreshGridView();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }


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

        $('#chkSelectAllPatient').die('change');
        $('#chkSelectAllPatient').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkPatient').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });

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
                                var jumlahKosongPria = result.CountBedEmptyMale;
                                var jumlahKosongWanita = result.CountBedEmptyFemale;
                                var jumlahKosongPriaWanita = result.CountBedEmptyMale + result.CountBedEmptyFemale;

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

        function onCbpRegistrationConfirmationEndCallback(s) {
            var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                if (isBridgingAplicares == "1") {
                    UpdateRoomAplicares();
                }

                onRefreshGridView();
            }
            else if (result[0] == 'refresh') {
                onRefreshGridView();
            }
            else if (result[0] == 'failed') {
                showToast('Save Failed', 'Error Message : ' + result[1]);
                hideLoadingPanel();
            }
            else if (result[0] == 'confirmation') {
                var confirmationMessage = "Pasien yang akan dikonfirmasi memiliki order makan yang akan di transfer, apakah melakukan proses transfer?";
                showToastConfirmation(confirmationMessage, function (result) {
                    if (result) {
                        cbpView.PerformCallback('confirm|true');
                    }
                    else {
                        cbpView.PerformCallback('confirm|false');
                    }
                });
            }
            else {
                if (result[1] != '')
                    showToast('Save Failed', 'Error Message : ' + result[1]);
                else
                    showToast('Save Failed', '');
                hideLoadingPanel();
            }
        }

        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }
    </script>
    <input type="hidden" value="" id="hdnIsBridgingToAplicares" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" id="hdnSelectedRegistration" runat="server" value="" />
    <input type="hidden" value="" id="hdnParamHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnParamRoomID" runat="server" />
    <input type="hidden" value="" id="hdnParamClassID" runat="server" />
    <input type="hidden" value="" id="hdnIsTransferMealOrder" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToIPTV" runat="server" />
    <div style="padding:15px">
        <div class="pageTitle"><%=GetMenuCaption()%></div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <fieldset id="fsBedStatus">  
                        <table class="tblEntryContent" style="width:60%;">
                            <colgroup>
                                <col style="width:25%"/>
                                <col/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Ruang Perawatan")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink" runat="server" id="lblRoom"><%=GetLabel("Kamar")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtRoomCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtRoomName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>

                    <div style="padding:7px 0 0 3px;font-size:0.95em">
                        <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                    </div>

                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
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
                                            <asp:CheckBoxField DataField="IsTransferred" HeaderText="Pindahan" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="No. Registrasi" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" />   
                                            <asp:BoundField DataField="MedicalNo" HeaderText="No. Rekam Medis" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfPatientName" HeaderText="Nama Pasien" HeaderStyle-HorizontalAlign="Left" />                           
                                            <asp:BoundField DataField="BedCode" HeaderText="Tempat Tidur" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="RoomFrom" HeaderText="Ruang Asal" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="BedFrom" HeaderText="Tempat Tidur Asal" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada pasien masuk atau pindahan di unit ini.")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <input type="button" id="btnConfirm" value='<%=GetLabel("K o n f i r m a s i") %>' class="btnRefresh w3-button w3-orange w3-border w3-border-blue w3-round-medium"/>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpRegistrationConfirmation" runat="server" Width="100%" ClientInstanceName="cbpRegistrationConfirmation"
            ShowLoadingPanel="false" OnCallback="cbpRegistrationConfirmation_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpRegistrationConfirmationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
