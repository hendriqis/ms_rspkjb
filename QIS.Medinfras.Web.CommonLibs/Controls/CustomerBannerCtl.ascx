<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerBannerCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.CustomerBannerCtl" %>

<script type="text/javascript" id="dxss_patientbannerctl">
    $(function () {
        Methods.checkImageError('imgBusinessPartnerLogo', 'businesspartner', 'customer');
    });
</script>
<div style="height: 100px;">
    <div style="float:left;margin:5px 0 0 5px;position: relative;">
        <img id="imgBusinessPartnerLogo" class="imgBusinessPartnerLogo" runat="server" src='' alt="" width="55" />
    </div>
    <table class="tblPatientBannerInfo" style="margin-left: 50px" cellpadding="0" cellspacing="0" >
        <col style="width:130px"/>
        <col style="width:250px"/>
        <col style="width:100px"/>
        <col style="width:150px"/>
        <col style="width:100px"/>
        <col style="width:450px"/>
        <tr style="font-size: 1.2em">
            <td colspan="6" style="padding-left: 20px"><label id="lblCustomerName" runat="server" style="font-weight: bold"></label>
                &nbsp;[<label id="lblCustomerCode" runat="server"></label>]
            </td>
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Kelompok")%></td>
            <td valign="top"><label id="lblCustomerGroupName" runat="server"></label>&nbsp;[<label id="lblCustomerGroupCode" runat="server"></label>]</td>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Telpon Tagih")%></td>
            <td valign="top"><label id="lblBillToPhone" runat="server"></label></td>
            <td valign="top" class="tdPatientBannerLabel" rowspan="4"><%=GetLabel("Alamat Tagih")%></td>
            <td valign="top" rowspan="4"><label id="lblBillToAddressInfo" runat="server"></label></td>            
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Skema Tarif")%></td>
            <td valign="top"><label id="lblTariffScheme" runat="server"></label></td>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Fax Tagih")%></td>
            <td valign="top"><label id="lblBillToFax" runat="server"></label></td>
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Tipe Rekanan")%></td>
            <td valign="top"><label id="lblCustomerType" runat="server"></label></td>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Email Tagih")%></td>
            <td valign="top"><label id="lblBillToEmail" runat="server"></label></td>
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Customer Line")%></td>
            <td valign="top"><label id="lblCustomerLineName" runat="server"></label></td>
        </tr>
    </table>
</div>