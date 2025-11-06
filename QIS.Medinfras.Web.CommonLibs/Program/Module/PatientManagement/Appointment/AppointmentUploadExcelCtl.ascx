<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentUploadExcelCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentUploadExcelCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_uploadexcelctl">
    $('#<%=FileUpload1.ClientID %>').change(function () {
        readURL(this);
        var fileName = $('#<%=FileUpload1.ClientID %>').val().replace("C:\\fakepath\\", "");
        $('#txtUploadFile').val(fileName);
        $('#<%=hdnFileName.ClientID %>').val(fileName);
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#<%=hdnUploadedFile1.ClientID %>').val(e.target.result);
                cbpViewDt.PerformCallback('upload');
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $('#btnUploadFile').click(function () {
        $('#<%=hdnExcelToJSON.ClientID %>').val('');
        $('#<%=FileUpload1.ClientID %>').val('');
        cboSheetsName.SetText('');
        document.getElementById('<%= FileUpload1.ClientID %>').click();
    });

    $('#btnUpload').click(function () {
        cbpViewDt.PerformCallback('upload');
    });

    $('#btnProcessData').click(function () {
        if ($('#<%=hdnParamedicID.ClientID %>').val() != '') {
            cbpViewDt.PerformCallback('process');
        }
        else {
            displayMessageBox('WARNING', 'Harap isi dokter MCU terlebih dahulu sebelum proses permintaan perjanjian');
        }
    });


    $('#btnLoadExcel').click(function () {
        if (cboSheetsName.GetValue() != null) {
            cbpViewDt.PerformCallback('load');
        }
        else {
            displayMessageBox('WARNING', 'Harap Pilih Sheet');
        }
    });


    function oncbpViewDtEndCallback(s) {
        $('#containerImgLoadingView').hide();
        var param = s.cpResult.split('|');
        if (param[0] == 'process') {

            if (param[1] == 'fail') {
                displayMessageBox('ERROR', param[2]);
            }
            else {
                displayMessageBox('SUCCESS', 'Upload Pasien Berhasil');
                pcRightPanelContent.Hide()
                onRefreshPage();
            }
        }
        else if (param[0] == 'upload') {
            if (param[1] == 'failed') {
                displayMessageBox('ERROR', param[2]);
            }
            else {
                $('#trPhysician').attr("style", "display:none");
                $('#trVisitType').attr("style", "display:none");
                $('#<%=hdnSheetsName.ClientID %>').val(param[2]);
                cbpChangeSheet.PerformCallback();
            }
            $('#trPhysician').attr("style", "display:none");
        }
        else if (param[0] == 'load') {
            if (param[1] == 'failed') {
                var message = param[2] + '<br /><b><i>Harap perbaiki data kemudian upload ulang kembali<b /><i />';
                displayMessageBox('ERROR', message);
                $('#trPhysician').attr("style", "display:none");
                $('#trVisitType').attr("style", "display:none");
            }
            else if (param[1] == 'confirmation') {
                var message = param[2];
                displayConfirmationMessageBox('Konfirmasi', message, function (result) {
                    if (result) {
                        $('#<%=hdnIsReplaceWithExistingPatient.ClientID %>').val("1");
                        cbpViewDt.PerformCallback("replace");
                    }
                    else {
                        $('#trPhysician').removeAttr("style");
                        $('#trVisitType').removeAttr("style");
                    }
                });
            }
            else {
                $('#trPhysician').removeAttr("style");
                $('#trVisitType').removeAttr("style");
            }
        }
        else {
            $('#trPhysician').removeAttr("style");
            $('#trVisitType').removeAttr("style");
        }
    }

    function oncbpChangeSheetEndCallback(s) {
        $('#containerImgLoadingView').hide();
        onRefreshPage();
    }

    //#region Physician
    function onGetPhysicianFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID == '')
            serviceUnitID = '0';
        var filterExpression = 'IsDeleted = 0 AND IsHasPhysicianRole = 1';
        if (serviceUnitID != '0')
            filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnit WHERE HealthcareServiceUnitID = " + serviceUnitID + " AND DepartmentID = 'MCU')) AND IsDeleted = 0 AND (IsHasPhysicianRole = 1)";
        return filterExpression;
    }

    $('#<%:lblPhysician.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%:txtPhysicianCode.ClientID %>').val(value);
            onTxtPhysicianCodeChanged(value);
        });
    });

    $('#<%:txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPhysicianCodeChanged($(this).val());
    });

    function onHdnPhysicianIDChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicID = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%:hdnParamedicID.ClientID %>').val('');
                $('#<%:txtPhysicianCode.ClientID %>').val('');
                $('#<%:txtPhysicianName.ClientID %>').val('');
            }
        });
    }

    function onTxtPhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%:hdnParamedicID.ClientID %>').val('');
                $('#<%:txtPhysicianCode.ClientID %>').val('');
                $('#<%:txtPhysicianName.ClientID %>').val('');
            }
        });
    }

    $('#<%:lblVisitType.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('paramedicvisittype', onGetVisitTypeFilterExpression(), function (value) {
            $('#<%:txtVisitTypeCode.ClientID %>').val(value);
            onTxtVisitTypeCodeChanged(value);
        });
    });
    function onGetVisitTypeFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID == '')
            serviceUnitID = '0';
        var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
        if (paramedicID == '')
            paramedicID = '0';
        var filterExpression = serviceUnitID + ';' + paramedicID + ';';
        return filterExpression;
    }
    $('#<%:txtVisitTypeCode.ClientID %>').live('change', function () {
        onTxtVisitTypeCodeChanged($(this).val());
    });

    function onTxtVisitTypeCodeChanged(value) {
        var filterExpression = onGetVisitTypeFilterExpression() + "VisitTypeCode = '" + value + "'";
        Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
            }
            else {
                $('#<%:hdnVisitTypeID.ClientID %>').val('');
                $('#<%:txtVisitTypeCode.ClientID %>').val('');
                $('#<%:txtVisitTypeName.ClientID %>').val('');
            }
        });
    }

</script>
<div style="padding: 5px">
    <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
    <input type="hidden" runat="server" id="hdnAppointmentRequestID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnFileName" value="" />
    <input type="hidden" runat="server" id="hdnUploadedFile1" value="" />
    <input type="hidden" runat="server" id="hdnExcelToJSON" value="" />
    <input type="hidden" runat="server" id="hdnSheetsName" value="" />
    <input type="hidden" runat="server" id="hdnIsReplaceWithExistingPatient" value="0" />
    <input type="hidden" runat="server" id="hdnIsAutoCreatePatient" value="" />
    <input type="hidden" runat="server" id="hdnFlagToUpperPatientName" value="" />
    <input type="hidden" runat="server" id="hdnFlagToUpperPatientAddress" value="" />
    
    <table border="0" cellpadding="0" cellspacing="0">
        <tr style="width: auto">
            <td class="tdLabel" style="width: 100px">
                <label class="lblMandatory">
                    <%=GetLabel("File to upload") %></label>
            </td>
            <td style="padding-left: 5px; width: 250px">
                <input type="text" id="txtUploadFile" style="width: 100%" readonly="readonly" />
            </td>
            <td style="padding-left: 10px">
                <input type="button" id="btnUploadFile" value="Pilih File" style="" />
            </td>
        </tr>
        <tr style="display: none">
            <td style="padding-top: 5px">
                <div style="display: none">
                    <asp:FileUpload ID="FileUpload1" runat="server"/>
                </div>
                <input type="button" id="btnUpload" value="Upload Data" style="padding: 5px" />
            </td>
        </tr>
        <tr style="padding-top: 20px; width: auto">
            <td class="tdLabel" style="width: 100px">
                <label class="lblMandatory">
                    <%=GetLabel("Pilih Sheets") %></label>
            </td>
            <td style="padding-left: 5px; width: 250px">
                <dxcp:ASPxCallbackPanel ID="cbpChangeSheet" runat="server" ClientInstanceName="cbpChangeSheet"
                    ShowLoadingPanel="false" OnCallback="cbpChangeSheet_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ oncbpChangeSheetEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <dxe:ASPxComboBox ID="cboSheetsName" ClientInstanceName="cboSheetsName" Width="100%"
                                runat="server" />
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
            <td style="padding-left: 10px">
                <input type="button" id="btnLoadExcel" value="Load Data" style="" />
            </td>
        </tr>
        <tr id="trVisitType" style="display: none; width: auto">
            <td class="tdLabel" style="width: 150px;">
                <label class="lblLink lblMandatory" runat="server" id="lblVisitType">
                    <%:GetLabel("Jenis Kunjungan")%></label>
            </td>
            <td style="padding-left: 5px;">
                <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtVisitTypeName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trPhysician" style="display: none; width: auto">
            <td class="tdLabel" style="padding-top: 5px; width: 100px">
                <label class="lblMandatory lblLink" runat="server" id="lblPhysician">
                    <%=GetLabel("Dokter MCU")%></label>
            </td>
            <td style="padding-left: 5px;">
                <input type="hidden" id="hdnParamedicID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col />
                    </colgroup>
                    <tr>
                        <td style="display: none">
                            <asp:TextBox ID="txtPhysicianCode" Width="120px" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding-left: 10px">
                <input type="button" id="btnProcessData" value="Proses Permintaan" style="" />
            </td>
        </tr>
    </table>
</div>
<div>
    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="1200px" ClientInstanceName="cbpViewDt"
        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
            EndCallback="function(s,e){ oncbpViewDtEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                        <Columns>
                            <%--<asp:BoundField DataField="No" HeaderText="No" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" />--%>
                            <asp:BoundField DataField="PatientName" HeaderText="Pasien" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="MedicalNo" HeaderText="No. RM" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="DateOfBirthInString" HeaderText="Tanggal Lahir" ItemStyle-Width="150px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Address" HeaderText="Alamat" ItemStyle-Width="150px" />
                            <asp:BoundField DataField="EmailAddress" HeaderText="Email" ItemStyle-Width="150px" />
                            <asp:BoundField DataField="IdentityCard" HeaderText="Kartu Identitas" ItemStyle-Width="150px" />
                            <asp:BoundField DataField="CorporateAccount" HeaderText="Informasi Karyawan" ItemStyle-Width="150px" />
                            <asp:BoundField DataField="AppointmentDateInString" HeaderText="Tanggal Perjanjian"
                                ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="MCUItemDetail" HeaderText="Paket Pemeriksaan" ItemStyle-Width="200px" />
                        </Columns>
                        <EmptyDataTemplate>
                            <%=GetLabel("No patient list for medical checkup") %>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
<div class="imgLoadingGrdView" id="containerImgLoadingView">
    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
</div>
