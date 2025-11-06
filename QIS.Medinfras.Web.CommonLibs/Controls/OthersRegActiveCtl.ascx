﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OthersRegActiveCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.OthersRegActiveCtl" %>
<div align="center">
    <asp:PlaceHolder ID="plInfo" runat="server">
        <table class="tblEntryContent" style="width: 100%">
            <tr>
                <td valign="top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 870px" />
                            <col />
                        </colgroup>
                        <tr id="trMRN" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblMRN">
                                    <%:GetLabel("No. Rekam Medis (No.RM)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMRN" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Nama Pasien")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPatientName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Informasi Registrasi Aktif")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtInformation" Width="100%" ReadOnly="true" textMode="MultiLine" Rows="5" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>                
            </tr>
        </table>
    </asp:PlaceHolder>
</div>
