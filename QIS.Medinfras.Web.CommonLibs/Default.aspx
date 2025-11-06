<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPBase.Master" AutoEventWireup="true" 
    CodeBehind="Default.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Default" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPBase" runat="server">
    <script type="text/javascript">
        function openModule() {
            var link = '';
            var url = ResolveUrl('~/Libs/Login.aspx');
            var windowID = 'Map' + link;
            var win = window.open(url, windowID, 'status=1,toolbar=0,menubar=0,resizable=1,location=0,scrollbars=1,fullscreen=1');
            if (win) {
                win.moveTo(0, 0);
                win.resizeTo(screen.width, screen.height - 20);
                win.focus();
            } else {
                showToast('Warning', 'You must allow popups for this map to work.');
            }
        }
    </script>
    <style type="text/css">
        .lblEnter                   { font-size: 14px; color: #4800FE; cursor: pointer; }
        .lblEnter:hover             { text-decoration: underline; }
    </style>
    <div class="loginBg borderBox" style="padding:0;margin:0; height:650px;">
        <div class="loginContainerLoginInfo" style="padding:0;margin:0px; height:80px; ">
            <img src='<%=GetModuleImage()%>' alt="" width="80px" style="float:left;margin-right:19px;" />
            <div style="padding-top: 10px;">Medinfras: <%=moduleName%></div>
        </div>
        <center>
            <div style="text-align:center;border:1px solid transparent;">
                <img src='<%=ResolveUrl("~/Libs/Images/healthcarelogo_qis.png")%>' alt="" style="margin:80px 0 30px 0;"/>
                <div class="lblEnter" onclick="openModule();">Enter</div>
                <div id="loginFooter">
                    <div>2013 © PT. Quantum Infra Solusindo</div>
                </div>
            </div>
        </center>
    </div>   
</asp:Content>
