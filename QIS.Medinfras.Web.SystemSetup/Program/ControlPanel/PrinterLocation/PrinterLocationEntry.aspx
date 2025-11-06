<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" CodeBehind="PrinterLocationEntry.aspx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.PrinterLocationEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnParentID" runat="server" value="" />
    <input type="hidden" id="hdnTagProperty" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:12%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode")%></label></td>
                        <td><asp:TextBox ID="txtStandardCodeID" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama")%></label></td>
                        <td><asp:TextBox ID="txtStandardCodeName" Width="500px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Printer Label")%></label></td>
                        <td><asp:TextBox ID="txtPrinterLabel" Width="500px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Printer Gelang :")%></label></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Dewasa")%></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:50px"/>
                                    <col style="width:250px"/>
                                    <col style="width:30px"/>
                                    <col style="width:250px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td>&nbsp;Laki - laki&nbsp;</td>
                                    <td><asp:TextBox ID="txtPrinterGelangDewasaL" Width="90%" runat="server" /></td>
                                    <td>&nbsp;Perempuan&nbsp;</td>
                                    <td><asp:TextBox ID="txtPrinterGelangDewasaP" Width="90%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Anak")%></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:50px"/>
                                    <col style="width:250px"/>
                                    <col style="width:30px"/>
                                    <col style="width:250px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td>&nbsp;Laki - laki&nbsp;</td>
                                    <td><asp:TextBox ID="txtPrinterGelangAnakL" Width="90%" runat="server" /></td>
                                    <td>&nbsp;Perempuan&nbsp;</td>
                                    <td><asp:TextBox ID="txtPrinterGelangAnakP" Width="90%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Printer Bayi Baru Lahir")%></label></td>
                        <td><asp:TextBox ID="txtPrinterBayiBaruLahir" Width="500px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Printer Bukti Pendaftaran")%></label></td>
                        <td><asp:TextBox ID="txtPrinterBuktiPendaftaran" Width="500px" runat="server" /></td>
                    </tr>
                    <tr>                        
                        <td class="tdLabel" valign="top" style="margin-top:5px"><label><%=GetLabel("Notes")%></label></td>
                        <td><asp:TextBox ID="txtNotes" Width="500px" runat="server" TextMode="MultiLine" Rows="3" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
