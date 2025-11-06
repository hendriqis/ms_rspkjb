<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPBase.Master" AutoEventWireup="true" 
    CodeBehind="ErrorPage.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPBase" runat="server">
    <script language="javascript" type="text/javascript">
        function toggleMe(a) {
            var lnkErrorDetail = document.getElementById('lnkErrorDetail');
            var e = document.getElementById(a);
            if (!e) return true;
            if (e.style.display == "none") {
                lnkErrorDetail.innerHTML = '- Hide Error Detail -';
                e.style.display = "block";
            } else {
                lnkErrorDetail.innerHTML = '- Show Error Detail -';
                e.style.display = "none";
            }
            return true;
        }

        $(function () {
            $('#divBack').click(function () {
                document.location = document.referrer;
            });
        });
    </script>
    <div style="float: left; margin-left: 20px; padding-top: 10px; vertical-align: top">
        <img src='<%=ResolveUrl("~/Libs/Images/error.png")%>' width="68px" height="68px" alt="" />
    </div>
    <div style="margin-left: 100px; padding-top: 2px;">
        <h1 style="font-weight: bold; padding-top: 10px; color: Red"><%=GetLabel("Application Error")%></h1>
        <br />
        <hr />
        <span><%=GetLabel("Please contact your Technical Supports and give them this error information.")%></span><br />
        <br />
        <label><%=GetLabel("An internal error was occured that prevents the page to be displayed properly.")%></label>
        <br />
        <br />
        
        <div id="divBack" class="lblLink"><%=GetLabel("Back")%></div>

        <div>
            <a id='lnkErrorDetail' class="lblLink" onclick="toggleMe('divErrMsg')">- Show Error Detail -</a> 
        </div>
        <br />
        <div id="divErrMsg" style="display: none; overflow-y: scroll">
            <label id="lblMessage" runat="server">
                Error Message
            </label>
        </div>
    </div>
</asp:Content>

