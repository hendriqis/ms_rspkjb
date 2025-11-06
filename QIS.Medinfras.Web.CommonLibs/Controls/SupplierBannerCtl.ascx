<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierBannerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.SupplierBannerCtl" %>
<script type="text/javascript" id="dxss_SupplierBannerCtl">
    $(function () {
        Methods.checkImageError('imgBusinessPartnerLogo', 'businesspartner', 'supplier');
    });
</script>
<div style="height: 85px;">
    <div style="float: left; margin: 5px 0 0 5px; position: relative;">
        <img id="imgBusinessPartnerLogo" class="imgBusinessPartnerLogo" runat="server" src=''
            alt="" width="55" />
    </div>
    <table class="tblPatientBannerInfo" style="margin-left: 50px" cellpadding="0" cellspacing="0">
        <col style="width: 135px" />
        <col style="width: 250px" />
        <col style="width: 135px" />
        <col style="width: 250px" />
        <col style="width: 135px" />
        <col />
        <tr style="font-size: 1.2em">
            <td colspan="6" style="padding-left: 20px">
                <label id="lblBusinessPartnerName" runat="server" style="font-weight: bold; font-size: medium">
                </label>
                &nbsp;[<label id="lblBusinessPartnerCode" runat="server" style="font-weight: bold;
                    font-size: medium"></label>]
            </td>
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel">
                <%=GetLabel("Telpon")%>
            </td>
            <td valign="top">
                <label id="lblPhoneNo" runat="server" style="font-weight: bold;">
                </label>
            </td>
            <td valign="top" class="tdPatientBannerLabel">
                <%=GetLabel("Contact Person")%>
            </td>
            <td valign="top">
                <label id="lblContactPerson" runat="server" style="font-weight: bold;">
                </label>
                &nbsp;-&nbsp;
                <label id="lblContactPersonMobileNo" runat="server" style="font-weight: bold;">
                </label>
            </td>
            <td valign="top" class="tdPatientBannerLabel" rowspan="3">
                <%=GetLabel("Alamat")%>
            </td>
            <td valign="top" rowspan="3">
                <label id="lblAddress" runat="server" style="font-weight: bold;">
                </label>
            </td>
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel">
                <%=GetLabel("Email")%>
            </td>
            <td valign="top">
                <label id="lblEmail" runat="server" style="font-weight: bold;">
                </label>
            </td>
            <td valign="top" class="tdPatientBannerLabel">
                <%=GetLabel("Email (CP)")%>
            </td>
            <td valign="top">
                <label id="lblContactPersonEmail" runat="server" style="font-weight: bold;">
                </label>
            </td>
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel">
                <%=GetLabel("Tipe Partner")%>
            </td>
            <td valign="top">
                <label id="lblPartnerType" runat="server" style="font-weight: bold;">
                </label>
            </td>
            <td valign="top" class="tdPatientBannerLabel">
                <%=GetLabel("Supplier Line")%>
            </td>
            <td valign="top">
                <label id="lblSupplierLine" runat="server" style="font-weight: bold;">
                </label>
            </td>
        </tr>
    </table>
</div>
