<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FetalMeasurementEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.FetalMeasurementEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_obstetricHistoryEntryctl">
    setDatePicker('<%=txtMeasurementDate.ClientID %>');
    $('#<%=txtMeasurementDate.ClientID %>').datepicker('option', 'maxDate', '0');
    calculateGestationalWeek();
    $('#<%=txtPregnancyNo.ClientID %>').focus();

    function calculateGestationalWeek() {
        //Rumus Naegele : Tanggal HPHT ditambah 7, Bulan dikurang 3, dan tahun ditambah 1
        var hphtDate = $('#<%=txtLMP.ClientID %>').val();
        var measurementDate = $('#<%=txtMeasurementDate.ClientID %>').val();

        var from = hphtDate.split("-");
        var fromDate = new Date(parseInt(from[2]), parseInt(from[1]) - 1, parseInt(from[0]));

        var to = measurementDate.split("-");
        var toDate = new Date(parseInt(to[2]), parseInt(to[1]) - 1, parseInt(to[0]));

        //convert milliseconds to week
        var gestationalWeek = parseInt((toDate - fromDate) / 604800000);
        $('#<%=txtGestationalWeek.ClientID %>').val(gestationalWeek);
    }

    $('#<%=txtMeasurementDate.ClientID %>').die('change');
    $('#<%=txtMeasurementDate.ClientID %>').live('change', function (evt) {
        calculateGestationalWeek();
    });
</script>
<div style="height: 450px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnFetusID" value="" />
    <input type="hidden" value="" id="hdnLMPDate" runat="server" />
    <table>
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <table style="width: 100%" id="tblEntryContent">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kehamilan Ke-")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPregnancyNo" Width="60px" runat="server" ReadOnly="True" CssClass="number" />
                        </td>
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Menstruasi Terakhir (HPHT)")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLMP" Width="120px" runat="server" ReadOnly = "True" style="text-align:center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Janin Ke-")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtFetusNo" Width="60px" runat="server" ReadOnly="True" CssClass="number"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam Pemeriksaan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMeasurementDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMeasurementTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Usia Kehamilan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtGestationalWeek" Width="60px" runat="server" CssClass="number" ReadOnly="True" /><%=GetLabel(" minggu")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Biparietal Diameter (BPD)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtBPD" Width="60px" CssClass="number" runat="server"/> mm
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Abdominal Circumference (AC)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtAC" Width="60px" CssClass="number" runat="server"/> mm
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Estimated Fetal Weight (EFW)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtEFW" Width="60px" CssClass="number" runat="server"/> mm
                        </td>
                    </tr>   
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Head Circumference (HC)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtHC" Width="60px" CssClass="number" runat="server"/> mm
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Femur Length (FL)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtFL" Width="60px" CssClass="number" runat="server"/> mm
                        </td>
                    </tr>      
                    <tr style="display:none">
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Humerus Length (HL)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtHL" Width="60px" CssClass="number" runat="server"/> mm
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Crown Crump Length (CRL)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtCRL" Width="60px" CssClass="number" runat="server"/> mm
                        </td>
                    </tr>       
                    <tr style="display:none">
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Gestational Sac (GS)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtGS" Width="60px" CssClass="number" runat="server"/> mm
                        </td>
                    </tr>   
                    <tr style="display:none">
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Fetal Heart Rate (FHR)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtFHR" Width="60px" CssClass="number" runat="server"/>
                        </td>
                    </tr>     
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Occipitofrontal Diameter (OFD)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtOFD" Width="60px" CssClass="number" runat="server"/> mm
                        </td>
                    </tr>                                                                                                                                          
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Tambahan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
