<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FixedAssetBannerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.FixedAssetBannerCtl" %>
<script type="text/javascript" id="dxss_FixedAssetBannerCtl">
    
</script>
<div style="height: 80px;">
    <table class="tblPatientBannerInfo" style="margin-left: 10px" cellpadding="0" cellspacing="0">
        <col style="width: 120px" />
        <col style="width: 250px" />
        <col style="width: 120px" />
        <col style="width: 250px" />
        <col style="width: 120px" />
        <col style="width: 250px" />
        <tr style="font-size: 1.2em">
            <td colspan="4">
                <label id="lblFixedAssetName" runat="server" style="font-weight: bold; font-size: larger; color: Black">
                </label>
            </td>
        </tr>
        <tr>
            <td class="tdPatientBannerLabel">
                <%=GetLabel("Kode")%>
            </td>
            <td class="tdPatientBannerValue">
                <label id="lblFixedAssetCode" runat="server" style="font-weight: bold; color: Black">
                </label>
            </td>
            <td class="tdPatientBannerLabel">
                <%=GetLabel("Serial No")%>
            </td>
            <td class="tdPatientBannerValue">
                <label id="lblSerialNo" runat="server" style="font-weight: bold;">
                </label>
            </td>
        </tr>
        <tr>
            <td class="tdPatientBannerLabel">
                <%=GetLabel("Kelompok")%>
            </td>
            <td class="tdPatientBannerValue">
                <label id="lblFAItemGroup" runat="server" style="font-weight: bold;">
                </label>
            </td>
            <td class="tdPatientBannerLabel">
                <%=GetLabel("Lokasi")%>
            </td>
            <td class="tdPatientBannerValue">
                <label id="lblFALocation" runat="server" style="font-weight: bold;">
                </label>
            </td>
        </tr>
    </table>
</div>
