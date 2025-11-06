<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BloodBankOrderProcessCtl1.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.BloodBankOrderProcessCtl1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_bloodBankEntryctl">
    $(function () {
        setDatePicker('<%=txtRealizationDate.ClientID %>');
        setDatePicker('<%=txtOrderDate.ClientID %>');
        setDatePicker('<%=txtPMIPickupDate.ClientID %>');

        $('#<%=txtRealizationDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');

        //#region Room

        function getRoomFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var filterExpression = '';

            if (serviceUnitID != '') {
                filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
            }

            if (filterExpression != '') {
                filterExpression += " AND ";
            }
            filterExpression += "IsDeleted = 0";

            return filterExpression;
        }
        //#endregion

        $('#<%=rblGCSourceType.ClientID %> input').change(function () {
            if ($(this).val() == "X533^001") {
                $('#<%=trPaymentTypeInfo.ClientID %>').removeAttr("style");
                $('#<%=trPMIReference1.ClientID %>').removeAttr("style");
                $('#<%=trPMIReference2.ClientID %>').removeAttr("style"); 
            }
            else {
                $('#<%=trPaymentTypeInfo.ClientID %>').attr("style", "display:none");
                $('#<%=trPMIReference1.ClientID %>').attr("style", "display:none");
                $('#<%=trPMIReference2.ClientID %>').attr("style", "display:none");
            }
        });

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanelContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        $('#leftPageNavPanel ul li').first().click();
    });
 
    function onBeforeSaveRecord(errMessage) {
        var resultFinal = true;
        return resultFinal;
    }

    function onAfterProcessPopupEntry(retVal) {
        if (retVal != "") onLoadObject(retVal);
    }
</script>
<style type="text/css">
</style>
<div>
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnParameterRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnParameterVisitID" value="" />
    <input type="hidden" runat="server" id="hdnParameterParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnBloodBankOrderID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" value="1" id="hdnProcedureGroupProcessMode" runat="server" />
    <input type="hidden" value="1" id="hdnParamedicTeamProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnOrderDtProcedureGroupID" value="" />
    <input type="hidden" runat="server" id="hdnOrderDtParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionID" value="" />
    <table class="tblContentArea">
            <colgroup>
                <col style="width: 20%" />
                <col style="width: 80%" />
            </colgroup>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Data Order" class="w3-hover-red">Data Order</li>
                        <li contentID="divPage2" title="Riwayat Transfusi" class="w3-hover-red">Riwayat Transfusi</li>                                                   
                    </ul>     
                </div> 
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr id="trTransactionDateTime" runat="server">
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal ") %>
                                /
                                <%=GetLabel("Jam Konfirmasi Order") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRealizationDate" Width="100px" runat="server" CssClass="datepicker"
                                    Style="text-align: center" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtRealizationTime" Width="60px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                        </tr>

                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Dokter")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" />
                            </td>
                        </tr>  
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Golongan Darah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboBloodType" ClientInstanceName="cboBloodType"
                                    Width="99%" ToolTip = "Golongan Darah">
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <label class="lblMandatory">
                                                <%=GetLabel("Rhesus")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboRhesus" ClientInstanceName="cboRhesus"
                                                Width="60px" ToolTip = "Rhesus">
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Darah/Komponen")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboBloodComponentType" ClientInstanceName="cboBloodComponentType"
                                    Width="99%" ToolTip = "Jenis Darah/Komponen">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jumlah Permintaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" Width="60px" CssClass="numeric" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" />
                            </td>
                        </tr>    
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Sumber/Asal Darah") %></td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblGCSourceType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                    <asp:ListItem Text=" PMI" Value="X533^001" />
                                    <asp:ListItem Text=" Persediaan BDRS" Value="X533^002" />
                                    <asp:ListItem Text=" Pendonor" Value="X533^003" />
                                </asp:RadioButtonList>
                            </td>    
                        </tr>  
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Cara Penyimpanan") %></td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblGCUsageType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                    <asp:ListItem Text=" Langsung digunakan" Value="X534^001" />
                                    <asp:ListItem Text=" Dititipkan di BDRS" Value="X534^002" />
                                </asp:RadioButtonList>
                            </td>    
                        </tr> 
                        <tr id="trPaymentTypeInfo" runat="server" style="display: none">
                            <td class="tdLabel"><%=GetLabel("Cara Pembayaran (Jika PMI)") %></td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblGCPaymentType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                    <asp:ListItem Text=" Dibayar langsung di PMI" Value="X535^001" />
                                    <asp:ListItem Text=" Tagihan Pasien di Rumah Sakit" Value="X534^002" />
                                </asp:RadioButtonList>
                            </td>    
                        </tr>   
                        <tr id="trPMIReference1" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor Referensi PMI")%> </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                            </td>
                            <td />
                        </tr>                          
                        <tr id="trPMIReference2" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pengambilan di PMI")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPMIPickupDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPMIPickupTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>    
                        </tr>          
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Klinis/Diagnosa") %></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline"
                                    Height="150px" />
                            </td>     
                        </tr>                                                                                                                                                                                                    
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                    <table border="0" cellpadding="1" cellspacing="0" width="99%" style="margin-top: 2px;">
                        <colgroup>
                            <col style="width: 180px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Riwayat Transfusi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalHistory" runat="server" Width="99%" TextMode="Multiline"
                                    Height="250px" />
                            </td>
                        </tr>   
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
</div>

