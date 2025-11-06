<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true" CodeBehind="IHSIntegration.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.IHSIntegration" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Src="~/Program/Integration/SatuSEHAT/DashboardContent/OutpatientListCtl1.ascx" TagName="OutpatientListCtl"
    TagPrefix="uc1" %>
<%@ Register Src="~/Program/Integration/SatuSEHAT/DashboardContent/UnderDevelopmentCtl1.ascx" TagName="UnderdevelopmentCtl"
    TagPrefix="uc99" %>	

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            //#region Left Navigation Panel
            $('#leftPageNavPanelDashboard ul li').first().addClass('nav-link active');
            $('#leftPageNavPanelDashboard ul li a').first().addClass('active');

            $('#leftPageNavPanelDashboard ul li').click(function () {
                $('#leftPageNavPanelDashboard ul li.selected').removeClass('nav-item active');
                $(this).addClass('nav-link active');
                var name = $(this).attr('contentid');
                $('#' + name).removeAttr('style');
                $('#leftPageNavPanelDashboard ul li').each(function () {
                    var tempNameContainer = $(this).attr('contentid');
                    if (tempNameContainer != name) {
                        $(this).removeClass('nav-item active');
                        $(this).removeClass('nav-link active');
                        $('#' + tempNameContainer).attr('style', 'display:none');
                    }
                });

                var contentID = $(this).attr('contentID');
                showContent(contentID);
            });

            $('#leftPageNavPanelDashboard ul li a').click(function () {
                $('#leftPageNavPanelDashboard ul li a.selected').removeClass('nav-item active');
                $(this).addClass('active');
                var name = $(this).attr('contentid');
                $('#' + name).removeAttr('style');
                $('#leftPageNavPanelDashboard ul li a').each(function () {
                    var tempNameContainer = $(this).attr('contentid');
                    if (tempNameContainer != name) {
                        $(this).removeClass('active');
                    }
                });
            });

            function showContent(contentID) {
                var i, x, tablinks;
                x = document.getElementsByClassName("divPageNavPanelContent");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(contentID).style.display = "block";
            }
            //#endregion   
        });

        function onCbpViewTimeEndCallback(s) {
            var textNow = $('#<%=hdnTimeNow.ClientID %>').val();
            $('#<%=lblDateTime.ClientID %>').html(textNow);
        }

        var interval = 1000;
        var intervalID = window.setInterval(function () {
            onRefreshDateTime();
        }, interval);

        function onRefreshDateTime() {
            window.clearInterval(intervalID);
            cbpViewTime.PerformCallback('refreshHour');
            intervalID = window.setInterval(function () {
                onRefreshDateTime();
            }, interval);
        }
    </script>
    <div class="page-flex">
        <aside class="sidebar">
            <div class="sidebar-start">      
                <div class="sidebar-head">
                    <a href="/" class="logo-wrapper" title="Home">
                        <span class="sr-only">SatuSEHAT</span>
                        <span class="icon logo" aria-hidden="true"></span>
                    </a>
                    <button class="sidebar-toggle transparent-btn" title="Menu" type="button">
                        <span class="sr-only">Toggle menu</span>
                        <span class="icon menu-toggle" aria-hidden="true"></span>
                    </button>
                </div>
                <div class="sidebar-body" id="leftPageNavPanelDashboard">
                    <ul class="sidebar-body-menu">  
                        <li class="nav-item" contentid="outpatientList">
                            <a href="#" contentid="outPatientList"><span class="icon user-3" aria-hidden="true"></span>Rawat Jalan</a>
                        </li>    
                        <li class="nav-item" contentid="inpatientList">
                            <a href="#" contentid="inpatientList"><span class="icon user-3" aria-hidden="true"></span>Rawat Inap</a>
                        </li>                                                             
                    </ul>
                </div>
            </div> 
        </aside> 
        <div class="main-wrapper">    
            <nav class="main-nav--bg">
                <div class="main-nav-start">
                    <div class="search-wrapper p-6">
                        <h5 class="main-title2" style="padding:3px">
                            <label class="lblHeader" runat="server" id="lblHeader"> HEADER
                            </label>
                        </h5>
                    </div>
                </div>
                <div class="main-nav-end" style="padding: 3px;">
                    <a class="text-time">
                        <label id="lblDateTime" runat="server"> dd-MMM-yyyy
                        </label>
                    </a>
                </div>
            </nav>
            <div class="btn-toolbar mb-2 mb-md-0" style="padding: 3px;">
                <div id="containerPageContent" style="background-color: Silver">
                </div>
                <div id="divPanelContentLoading" style="position:absolute;top:30%;left:48%;display:none">
                    <div style="margin:0 auto">
                        <img src="<%= ResolveUrl("~/Libs/Images/Loading.gif")%>" alt="" />
                    </div>
                </div>  
                <div id="outpatientList" contentID="outpatientList" class="divPageNavPanelContent w3-animate-left">
                    <uc1:OutpatientListCtl ID="ctlOutpatientList" runat="server" />
                </div>
            </div>
        </div>       
    </div>
<dxcp:ASPxCallbackPanel ID="cbpViewTime" runat="server" Width="100%" ClientInstanceName="cbpViewTime"
    ShowLoadingPanel="false" OnCallback="cbpViewTime_Callback">
    <ClientSideEvents BeginCallback="function(s,e){}" EndCallback="function(s,e){ onCbpViewTimeEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent3" runat="server">
            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                position: relative; font-size: 0.95em;">
                <input type="hidden" value="" id="hdnTimeNow" runat="server" />
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
</asp:Content>