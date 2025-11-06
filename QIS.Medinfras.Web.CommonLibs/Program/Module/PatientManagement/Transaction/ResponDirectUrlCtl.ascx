<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResponDirectUrlCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ResponDirectUrlCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        var url = ResolveUrl($('#<%:hdnID.ClientID %>').val());
        showLoadingPanel();
        window.location.href = url;
    });
</script>
<input type="hidden" runat="server" id="hdnID" value="" />