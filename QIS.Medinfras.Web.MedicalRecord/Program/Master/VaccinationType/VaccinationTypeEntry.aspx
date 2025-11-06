<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="VaccinationTypeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.VaccinationTypeEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=txtDisplayColorPicker.ClientID %>').colorPicker();
            $('#<%=txtDisplayColorPicker.ClientID %>').change(function () {
                $('#<%=txtDisplayColor.ClientID %>').val($(this).val());
            });

            $('#<%=txtDisplayColor.ClientID %>').change(function () {
                $('#<%=txtDisplayColorPicker.ClientID %>').val($(this).val());
                $('#<%=txtDisplayColorPicker.ClientID %>').change();
            });
        }
    </script>
    
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
            <col />
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width:30%"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Imunisasi")%></label></td>
                        <td><asp:TextBox ID="txtVaccinationCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Imunisasi")%></label></td>
                        <td><asp:TextBox ID="txtVaccinationName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Inisial")%></label></td>
                        <td><asp:TextBox ID="txtShortName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kelompok Imunisasi")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboVaccinationGroup" Width="300px" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("CVX Group")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboCVXGroup" Width="300px" runat="server" /></td>
                    </tr> 
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("CVX Name")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboCVXName" Width="300px" runat="server" /></td>
                    </tr> 
                </table>   
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tampilan Urutan")%></label></td>
                        <td><asp:TextBox ID="txtDisplayOrder" Width="100px" CssClass="number" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Warna")%></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0"> 
                                <tr>
                                    <td><asp:TextBox ID="txtDisplayColor" CssClass="colorpicker" Width="100px" runat="server" /></td>
                                    <td style="padding-left:5px"><asp:TextBox ID="txtDisplayColorPicker" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Catatan")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:CheckBox ID="chkIsCovid19" runat="server" /><%=GetLabel("Vaksinasi COVID 19")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

