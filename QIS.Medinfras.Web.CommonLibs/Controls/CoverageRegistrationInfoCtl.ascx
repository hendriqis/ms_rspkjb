﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoverageRegistrationInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CoverageRegistrationInfoCtl" %>
<div align="center">
    <asp:PlaceHolder ID="plInfo" runat="server">
        <table class="tblEntryContent" style="width: 100%">
            <tr>
                <td valign="top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 170px" />
                            <col style="width: 170px" />
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
                        <tr id="trSEP" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="Label1">
                                    <%:GetLabel("No. SEP")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSEPNo" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trNHSRegistrationNo" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblIdentityNo">
                                    <%:GetLabel("No. Kartu Identitas")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentityNo" Width="100%" runat="server" ReadOnly="true" />
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
                                    <%:GetLabel("Nama Pasien (Khusus)")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPatientName2" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Nama Panggilan")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPreferredName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Jenis Kelamin")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGender" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Golongan Darah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBloodType" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBloodRhesus" Width="100%" runat="server" Style="margin-right: 3px; text-align: left"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Agama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReligion" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Kewarganegaraan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNationality" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Tempat / Tanggal Lahir")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBirthPlace" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDOB" Width="100%" runat="server" Style="margin-right: 3px; text-align: left"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Umur")%></label>
                            </td>
                            <td colspan="3">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 50px" />
                                        <col style="width: 50px" />
                                        <col style="width: 50px" />
                                        <col style="width: 50px" />
                                        <col style="width: 50px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtAgeInYear" Width="50px" runat="server" Style="margin-right: 3px;
                                                text-align: right" ReadOnly="true" />
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <%:GetLabel("Tahun")%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeInMonth" Width="50px" runat="server" Style="margin-right: 3px;
                                                text-align: right" ReadOnly="true" />
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <%:GetLabel("Bulan")%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeInDay" Width="50px" runat="server" Style="margin-right: 3px;
                                                text-align: right" ReadOnly="true" />
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <%:GetLabel("Hari")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>                
            </tr>
        </table>
    </asp:PlaceHolder>
</div>
