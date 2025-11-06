<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionOrderEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.PrescriptionOrderEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtPrescriptionDate.ClientID %>');
    $('#<%=txtPrescriptionDate.ClientID %>').datepicker('option', 'minDate', getDateNow());
    $('#<%=txtPrescriptionDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    $('#<%=lblPhysician.ClientID %>.lblLink').click(function () {
        var filterExpression = 'IsDeleted = 0';
        openSearchDialog('paramedic', filterExpression, function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').change(function () {
        onTxtPhysicianCodeChanged($(this).val());
    });

    function onTxtPhysicianCodeChanged(value) {
        var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
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
</script>
<input type="hidden" runat="server" id="hdnDefaultDispensaryServiceUnitID" />
<div style="height: 310px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 110px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label id="lblPrescriptionOrderNo">
                                <%=GetLabel("Order No.")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrescriptionOrderNo" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Date") %> - <%=GetLabel("Time") %>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <colgroup>
                                    <col width="145px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPrescriptionDate" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>                                                                       
                                    <td>
                                        <asp:TextBox ID="txtPrescriptionTime" Width="100%" CssClass="time" runat="server"
                                            Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>                            
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Prescription Type") %>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                                Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 110px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory" runat="server" id="lblPhysician">
                                <%=GetLabel("Dokter")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Farmasi") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDispensaryUnit" ClientInstanceName="cboDispensaryUnit" runat="server" Width="100%">
                                <ClientSideEvents ValueChanged="function() { cboLocation.PerformCallback(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>                    
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Location")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                                Width="100%" OnCallback="cboLocation_Callback">
                                <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>

                    <tr style="display:none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Aturan Refill") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboRefillInstruction" ClientInstanceName="cboRefillInstruction"
                                Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left:5px">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 110px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label>
                                <%=GetLabel("Order Resep")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" Width="99%" TextMode="MultiLine" Height="150px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
