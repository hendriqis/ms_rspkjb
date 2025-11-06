﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PatientInfoCtl">
    $(function () {
        var gender = $('#<%=hdnPatientGender.ClientID %>').val();
        Methods.checkImageError('imgPatientProfilePicture', 'patient', gender);

        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        $('#leftPanel ul li').first().click();
    });

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
</script>
<style type="text/css">
    #leftPanel
    {
        border: 1px solid #6E6E6E;
        width: 100%;
        height: 100%;
        position: relative;
    }
    #leftPanel > ul
    {
        margin: 0;
        padding: 2px;
        border-bottom: 1px groove black;
    }
    #leftPanel > ul > li
    {
        list-style-type: none;
        font-size: 15px;
        display: list-item;
        border: 1px solid #fdf5e6 !important;
        padding: 5px 8px;
        cursor: pointer;
        background-color: #87CEEB !important;
    }
    #leftPanel > ul > li.selected
    {
        background-color: #ff5722 !important;
        color: White;
    }
    .divContent
    {
        padding-left: 4px;
        min-height: 400px;
    }
</style>
<div style="width: 100%;">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <colgroup>
            <col style="width: 210px" />
            <col />
        </colgroup>
        <tr>
            <td>
                <div id="lblMedicalNo" runat="server" class="w3-lime w3-xxlarge" style="text-align: center;
                    text-shadow: 1px 1px 0 #444; width: 100%">
                </div>
                <td>
                    <div id="lblPatientName" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align: center;
                        text-shadow: 1px 1px 0 #444">
                    </div>
                </td>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentid="divPage2" title="Keluhan & Riwayat Kesehatan" class="w3-hover-red">Catatan
                            Pasien</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top; padding-left: 5px;">
            <input type="hidden" id="hdnPatientGender" runat="server" class="hdnPatientGender" />
                <div id="divPage1" class="w3-border divContent w3-animate-left" style="display: none">
                    <table style="margin-top: 5px; width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 450px;">
                                    <colgroup style="width: 175px" />
                                    <colgroup style="width: 10px; text-align: center" />
                                    <colgroup style="width: 200px" />
                                    <colgroup style="width: 10px; text-align: center" />
                                    <colgroup />
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("No. Rekam Medis (No.RM)")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblMRN" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label2">
                                                <%:GetLabel("No. Kartu Identitas")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblIdentityNo" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label3">
                                                <%:GetLabel("Nama Pasien")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPatientName2" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label4">
                                                <%:GetLabel("Nama Pasien (Khusus)")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPatientName3" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label5">
                                                <%:GetLabel("Nama Panggilan")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPreferredName" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label6">
                                                <%:GetLabel("Jenis Kelamin")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblGender" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label7">
                                                <%:GetLabel("Golongan Darah")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblBloodType" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label8">
                                                <%:GetLabel("Agama")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblReligion" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label9">
                                                <%:GetLabel("Kewarganegaraan")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblNationality" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label19">
                                                <%:GetLabel("Suku")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblEthnic" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label10">
                                                <%:GetLabel("Tempat / Tanggal Lahir")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblBirthPlace" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Umur")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td colspan="3">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 20px" />
                                                    <col style="width: 50px" />
                                                    <col style="width: 20px" />
                                                    <col style="width: 50px" />
                                                    <col style="width: 20px" />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <span id="lblAgeInYear" runat="server" width="100%" style="color: Black"></span>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-right: 5px">
                                                        <%:GetLabel("Tahun")%>
                                                    </td>
                                                    <td>
                                                        <span id="lblAgeInMonth" runat="server" width="100%" style="color: Black"></span>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-right: 5px">
                                                        <%:GetLabel("Bulan")%>
                                                    </td>
                                                    <td>
                                                        <span id="lblAgeInDay" runat="server" width="100%" style="color: Black"></span>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-right: 5px">
                                                        <%:GetLabel("Hari")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr style="margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label1">
                                                <%:GetLabel("No. SEP")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblSEPNo" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label11">
                                                <%:GetLabel("Jenis Peserta")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblJenisPeserta" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label12">
                                                <%:GetLabel("No. Kartu BPJS")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblNHSRegistrationNo" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr style="margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label17">
                                                <%:GetLabel("Pengkajian Awal Terakhir : Akut")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblLastAcuteInitialAssessmentDate" runat="server" width="100%" style="color: Black">
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label18">
                                                <%:GetLabel("Pengkajian Awal Terakhir : Kronis")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblLastChronicInitialAssessmentDate" runat="server" width="100%" style="color: Black">
                                            </span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 450px;">
                                    <colgroup style="width: 100px" />
                                    <colgroup style="width: 10px; text-align: center" />
                                    <colgroup style="width: 200px" />
                                    <colgroup style="width: 10px; text-align: center" />
                                    <colgroup />
                                    <tr>
                                        <td style="vertical-align: top">
                                            <img style="width: 110px; height: 125px" runat="server" runat="server" id="imgPatientImage" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label13">
                                                <%:GetLabel("Alamat")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblAddress" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label14">
                                                <%:GetLabel("No. Telepon")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPhoneNo" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label15">
                                                <%:GetLabel("No. Handphone")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblMobilePhoneNo" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="Label16">
                                                <%:GetLabel("Email")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblEmail" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr style="margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Nama Pasangan")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblSpouseName" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Nama Ibu")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblMotherName" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Nama Ayah")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblFatherName" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr style="margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Pendidikan")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblEducation" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Pekerjaan")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblWork" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Kategori Pasien")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPatientCategory" runat="server" width="100%" style="color: Black"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divContent w3-animate-left" style="display: none">
                    <input type="hidden" value="" runat="server" id="hdnMRN" />
                    <table class="tblContentArea">
                        <tr>
                            <td style="padding: 5px; vertical-align: top">
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <div>
                                                                    <%=GetLabel("Catatan Pasien") %></div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div>
                                                                    <b>
                                                                        <%#: Eval("cfCreatedDatePickerFormat")%>,
                                                                        <%#: Eval("cfCreatedTime") %>, <span style="color: Blue">
                                                                            <%#: Eval("CreatedByName") %></span> </b>
                                                                </div>
                                                                <div>
                                                                    <%#: Eval("Notes") %>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No Data To Display") %>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                            </td>
                        </tr>
                </div>
            </td>
            <td style="vertical-align: top">
            </td>
        </tr>
    </table>
</div>
