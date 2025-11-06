﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientInfoByMRNCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientInfoByMRNCtl" %>
<div align="center">
    <asp:PlaceHolder ID="plInfo" runat="server">
        <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
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
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Kartu Identitas")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentityNoType" Width="100%" MaxLength="20" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblNormal" id="lblSSN" runat="server">
                                    <%=GetLabel("No. Kartu Identitas")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentityCardNo" Width="100%" MaxLength="20" runat="server" ReadOnly="true" />
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
                <td valign="top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 220px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%:GetLabel("Alamat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("No. Telepon")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhoneNo" Width="60%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("No. Handphone")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhoneNo" Width="60%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Email")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Nama Pasangan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSpouseName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Nama Ibu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMotherName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Nama Ayah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFatherName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Pekerjaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWork" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Kategori Pasien")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientCategory" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Pengkajian Awal Terakhir : Akut")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastAcuteInitialAssessmentDate" Width="100%" runat="server"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Pengkajian Awal Terakhir : Kronis")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastChronicInitialAssessmentDate" Width="100%" runat="server"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</div>
