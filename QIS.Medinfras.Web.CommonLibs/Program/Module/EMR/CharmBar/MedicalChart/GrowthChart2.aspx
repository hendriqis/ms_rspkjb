<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPFrame.master" AutoEventWireup="true" CodeBehind="GrowthChart2.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.GrowthChart2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">

    <script type="text/javascript" id="dxss_GrowthChartCtl">      
    </script>

    <div>
        <canvas id="growthChart">
        </canvas>
    </div>
</asp:Content>
