<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true"
    CodeBehind="RumahSakit2.aspx.cs" Inherits="QIS.Medinfras.Web.Dashboard.Program.RumahSakit2" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Program/RumahSakit/SurveyInformationCtl.ascx" TagName="SurveyInformationCtl"
    TagPrefix="uc1" %>
<%@ Register Src="~/Program/RumahSakit/MasterRS2Ctl.ascx" TagName="MasterRS2Ctl"
    TagPrefix="uc2" %>
<%@ Register Src="~/Program/RumahSakit/BPJSInformationCtl.ascx" TagName="BPJSInformationCtl"
    TagPrefix="uc3" %>
<%@ Register Src="~/Program/RumahSakit/AntrianOnlineCtl.ascx" TagName="AntrianOnlineCtl"
    TagPrefix="uc4" %>
<%@ Register Src="~/Program/RumahSakit/PelayananFarmasiCtl.ascx" TagName="PelayananFarmasiCtl"
    TagPrefix="uc5" %>
<%@ Register Src="~/Program/RumahSakit/DurasiTungguRJCtl.ascx" TagName="DurasiTungguRJCtl"
    TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <div class="page-flex">
        <!-- ! Sidebar -->
        <aside class="sidebar position-sticky">
    <div class="sidebar-start">
        <div class="sidebar-head">
            <a href="/" class="logo-wrapper" title="Home">
                <span class="sr-only">Home</span>
                <span class="icon logo" aria-hidden="true"></span>
            </a>
            <button class="sidebar-toggle transparent-btn" title="Menu" type="button">
                <span class="sr-only">Toggle menu</span>
                <span class="icon menu-toggle" aria-hidden="true"></span>
            </button>
        </div>
        <div class="sidebar-body" id="leftPageNavPanelDashboard">
            <ul class="sidebar-body-menu">
                <li class="nav-item" contentid="masterRS">
                    <a href="#" contentid="masterRS"><span class="icon home" aria-hidden="true"></span>Dashboard</a>
                </li>
                <li class="nav-item" contentid="surveyInformation">
                    <a href="#" contentid="surveyInformation"><span class="icon home" aria-hidden="true"></span>Survey Pasien</a>
                </li>
                <li class="nav-item" contentid="bpjsInformation">
                    <a href="#" contentid="bpjsInformation"><span class="icon home" aria-hidden="true"></span>Pasien BPJS</a>
                </li>
                <li class="nav-item" contentid="antrianonline">
                    <a href="#" contentid="antrianonline"><span class="icon home" aria-hidden="true"></span>Antrian Online</a>
                </li>
                <li class="nav-item" contentid="pelayananFarmasi">
                    <a href="#" contentid="pelayananFarmasi"><span class="icon home" aria-hidden="true"></span>Pelayanan Farmasi</a>
                </li>
                <li class="nav-item" contentid="durasiTungguRJ">
                    <a href="#" contentid="durasiTungguRJ"><span class="icon home" aria-hidden="true"></span>Durasi Tunggu RJ</a>
                </li>
                <li>
                    <a class="show-cat-btn" href="##">
                        <span class="icon folder" aria-hidden="true"></span>Categories
                        <span class="category__btn transparent-btn" title="Open list">
                            <span class="sr-only">Open list</span>
                            <span class="icon arrow-down" aria-hidden="true"></span>
                        </span>
                    </a>
                    <ul class="cat-sub-menu">
                        <li>
                            <a href="categories.html">All categories</a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a class="show-cat-btn" href="##">
                        <span class="icon image" aria-hidden="true"></span>Media
                        <span class="category__btn transparent-btn" title="Open list">
                            <span class="sr-only">Open list</span>
                            <span class="icon arrow-down" aria-hidden="true"></span>
                        </span>
                    </a>
                    <ul class="cat-sub-menu">
                        <li>
                            <a href="media-01.html">Media-01</a>
                        </li>
                        <li>
                            <a href="media-02.html">Media-02</a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a class="show-cat-btn" href="##">
                        <span class="icon paper" aria-hidden="true"></span>Pages
                        <span class="category__btn transparent-btn" title="Open list">
                            <span class="sr-only">Open list</span>
                            <span class="icon arrow-down" aria-hidden="true"></span>
                        </span>
                    </a>
                    <ul class="cat-sub-menu">
                        <li>
                            <a href="pages.html">All pages</a>
                        </li>
                        <li>
                            <a href="new-page.html">Add new page</a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href="comments.html">
                        <span class="icon message" aria-hidden="true"></span>
                        Comments
                    </a>
                    <span class="msg-counter">7</span>
                </li>
            </ul>
            <span class="system-menu__title">system</span>
            <ul class="sidebar-body-menu">
                <li>
                    <a href="appearance.html"><span class="icon edit" aria-hidden="true"></span>Appearance</a>
                </li>
                <li>
                    <a class="show-cat-btn" href="##">
                        <span class="icon category" aria-hidden="true"></span>Extentions
                        <span class="category__btn transparent-btn" title="Open list">
                            <span class="sr-only">Open list</span>
                            <span class="icon arrow-down" aria-hidden="true"></span>
                        </span>
                    </a>
                    <ul class="cat-sub-menu">
                        <li>
                            <a href="extention-01.html">Extentions-01</a>
                        </li>
                        <li>
                            <a href="extention-02.html">Extentions-02</a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a class="show-cat-btn" href="##">
                        <span class="icon user-3" aria-hidden="true"></span>Users
                        <span class="category__btn transparent-btn" title="Open list">
                            <span class="sr-only">Open list</span>
                            <span class="icon arrow-down" aria-hidden="true"></span>
                        </span>
                    </a>
                    <ul class="cat-sub-menu">
                        <li>
                            <a href="users-01.html">Users-01</a>
                        </li>
                        <li>
                            <a href="users-02.html">Users-02</a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href="##"><span class="icon setting" aria-hidden="true"></span>Settings</a>
                </li>
            </ul>
        </div>
    </div>
</aside>
<div class="main-wrapper">
    <nav class="main-nav--bg">
        <div class="containerdb main-nav">
            <div class="main-nav-start">
                <div class="search-wrapper p-6">
                    <h5 class="main-title2">
                        <label class="lblHeader" runat="server" id="lblHeader">
                        </label>
                    </h5>
                </div>
            </div>
            <div class="main-nav-end">
                <button class="sidebar-toggle transparent-btn" title="Menu" type="button">
                    <span class="sr-only">Toggle menu</span>
                    <span class="icon menu-toggle--gray" aria-hidden="true"></span>
                </button>
                <a class="text-time">
                    <label id="lblDateTime" runat="server">
                    </label>
                </a>
                <button class="theme-switcher gray-circle-btn" type="button" title="Switch theme">
                    <span class="sr-only">Switch theme</span>
                    <i class="sun-icon" data-feather="sun" aria-hidden="true"></i>
                    <i class="moon-icon" data-feather="moon" aria-hidden="true"></i>
                </button>
                </div>
            </div>
        </nav>
            <div class="btn-toolbar mb-2 mb-md-0">
        <div id="surveyInformation" class="divPageNavPanelContent w3-animate-left" style='display:none'>
            <uc1:surveyInformationCtl ID="ctlsurveyInformation" runat="server" />
        </div>
        <div id="masterRS" class="divPageNavPanelContent w3-animate-left">
            <uc2:MasterRS2Ctl ID="ctlMasterRS2" runat="server" />
        </div>
        <div id="bpjsInformation" class="divPageNavPanelContent w3-animate-left" style='display:none'>
            <uc3:BPJSInformationCtl ID="ctlBPJSInformation" runat="server" />
        </div>
        <div id="antrianonline" class="divPageNavPanelContent w3-animate-left" style='display:none'>
            <uc4:AntrianOnlineCtl ID="CtlAntrianOnline" runat="server" />
        </div>
        <div id="pelayananFarmasi" class="divPageNavPanelContent w3-animate-left" style='display:none'>
            <uc5:PelayananFarmasiCtl ID="ctlPelayananFarmasi" runat="server" />
        </div>
        <div id="durasiTungguRJ" class="divPageNavPanelContent w3-animate-left" style='display:none'>
            <uc6:DurasiTungguRJCtl ID="CtlDurasiTungguRJ" runat="server" />
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

        function onCbpViewEndCallback() {
            hideLoadingPanel();
        }

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
</asp:Content>
