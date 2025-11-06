<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AntenatalRecordEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AntenatalRecordEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtLogDate.ClientID %>');
    setDatePicker('<%=txtLMP.ClientID %>');
    setDatePicker('<%=txtEDB.ClientID %>');
    $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%=txtLMP.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%=txtEDB.ClientID %>').datepicker('option', 'minDate', '0');
</script>
<div style="height: 450px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnIsNutrition" value="" />
    <table class="tblContentArea">
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
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Pencatatan")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblMandatory">
                                <%=GetLabel("Kehamilan Ke-")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPregnancyNo" Width="60px" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Menstruasi Terakhir (LMP)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLMP" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblMandatory">
                                <%=GetLabel("Estimasi Tanggal Persalinan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEDB" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblMandatory">
                                <%=GetLabel("G-P-A-L")%></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="1">
                                <colgroup>
                                    <col style="width:60px" />
                                    <col style="5px" />
                                    <col style="width:60px" />
                                    <col style="5px" />
                                    <col style="width:60px" />
                                    <col style="5px" />
                                    <col />                                  
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtGravida" Width="60px" CssClass="number" runat="server" /></td>
                                    <td style="text-align:center">-</td>
                                    <td><asp:TextBox ID="txtPara" Width="60px" CssClass="number" runat="server" /></td>
                                    <td style="text-align:center">-</td>
                                    <td><asp:TextBox ID="txtAbortion" Width="60px" CssClass="number" runat="server" /></td>
                                    <td style="text-align:center">-</td>
                                    <td><asp:TextBox ID="txtLife" Width="60px" CssClass="number" runat="server" /></td>
                                </tr>
                            </table>                           
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblMandatory">
                                <%=GetLabel("Catatan Riwayat Menstruasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtMenstrualHistory" runat="server" TextMode="MultiLine" Rows="3"
                                Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <%=GetLabel("Catatan Riwayat Kehamilan") %>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtMedicalHistory" runat="server" Width="100%" TextMode="Multiline"
                                Rows="5" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
