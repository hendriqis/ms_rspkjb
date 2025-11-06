<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganKontrolRSARCCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganKontrolRSARCCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    setDatePicker('<%=txtValueDate.ClientID %>');

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    $('#<%=txtValueDate.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + '-' + MM + '-' + DD;
        $('#<%=hdnTanggal.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalString.ClientID %>').val(date);
    });

    //#region Physician
    function getPhysicianFilterExpression() {
        var polyclinicID = cboClinic.GetValue();
        var filterExpression = '';
        if (polyclinicID != '' && polyclinicID != null)
            filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
        return filterExpression;
    }

    $('#lblPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtPatientVisitPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPatientVisitPhysicianCodeChanged($(this).val());
    });

    function onTxtPatientVisitPhysicianCodeChanged(value) {
        var filterExpression = getPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onReprintPatientPaymentReceiptSuccess() {
        var controlDate = $('#<%=txtControlDate.ClientID %>').val();
        var date = $('#<%=hdnTanggal.ClientID %>').val();
        var time1 = $('#<%=txtDeathTime1.ClientID %>').val();
        var time2 = $('#<%=txtDeathTime2.ClientID %>').val();
        var documentText = $('#<%=txtDocumentText.ClientID %>').val();
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var serviceUnitID = cboClinic.GetValue();
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();

        var filterExpression = visitID + "|" + controlDate + "|" + date + "|" + time1 + "|" + time2 + "|" + documentText + "|" + serviceUnitID + "|" + paramedicID;
        var reportCode = "PM-00582";
        if (reportCode != "") {
            openReportViewer(reportCode, filterExpression);
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');
    }

</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnPrint" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <input type="hidden" id="hdnTanggal" runat="server" value="" />
        <input type="hidden" id="hdnTanggalString" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Pada Hari")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtControlDate" Width="300px" CssClass="required" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal Kontrol")%></label>
                </td>
                <td class="tdCustomDate">
                    <asp:TextBox ID="txtValueDate" CssClass="txtValueDate datepicker" runat="server"
                        Width="150px" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory" />
                    <%=GetLabel("Jam Kontrol")%>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtDeathTime1" Width="80px" CssClass="number" runat="server" Style="text-align: center"
                                    MaxLength="2" max="24" min="0" />
                            </td>
                            <td>
                                <label class="lblNormal" />
                                <%=GetLabel(":")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeathTime2" Width="80px" CssClass="number" runat="server" Style="text-align: center"
                                    MaxLength="2" max="59" min="0" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Dipergunakan untuk")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtDocumentText" Width="300px" CssClass="required" runat="server"
                        TextMode="Multiline" Rows="2" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Klinik")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox runat="server" ID="cboClinic" ClientInstanceName="cboClinic" Width="100%" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink" id="lblPhysician">
                        <%=GetLabel("Dokter")%></label>
                </td>
                <td>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpMedicalSickLeave" runat="server" Width="100%" ClientInstanceName="cbpMedicalSickLeave"
            ShowLoadingPanel="false" OnCallback="cbpMedicalSickLeave_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
