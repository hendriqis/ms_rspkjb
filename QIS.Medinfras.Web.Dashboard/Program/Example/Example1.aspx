<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true"
    CodeBehind="Example1.aspx.cs" Inherits="QIS.Medinfras.Web.Dashboard.Program.Example1" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Program/Example/KunjunganCtl.ascx" TagName="KunjunganCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <div class="container-fluid">
        <div class="row">
            <nav id="sidebarMenu" class="col-md-3 col-lg-2 d-md-block bg-light sidebar collapse">
      <div class="position-sticky pt-3" id="leftPageNavPanelDashboard">
        <ul class="nav flex-column nav-pills">
          <li class="nav-item" contentid="kunjungan">
            <a class="nav-link link-dark" aria-current="page" href="#">
              <span class="fa fa-bar-chart"></span>
              Diagram Kunjungan
            </a>
          </li>
          <li class="nav-item" contentid="kunjungan2">
            <a class="nav-link link-dark" href="#">
              <span class="fa fa-bar-chart"></span>
              Diagram 2
            </a>
          </li>
          <li class="nav-item" contentid="kunjungan3">
            <a class="nav-link link-dark" href="#">
              <span class="fa fa-bar-chart"></span>
              Diagram 3
            </a>
          </li>
          <li class="nav-item" contentid="kunjungan4">
            <a class="nav-link link-dark" href="#">
              <span class="fa fa-bar-chart"></span>
              Diagram 4
            </a>
          </li>
          <li class="nav-item" contentid="kunjungan5">
            <a class="nav-link link-dark" href="#">
              <span class="fa fa-bar-chart"></span>
              Diagram 5
            </a>
          </li>
          <li class="nav-item" contentid="kunjungan6">
            <a class="nav-link link-dark" href="#">
              <span class="fa fa-bar-chart"></span>
              Diagram 6
            </a>
          </li>
        </ul>
      </div>
    </nav>
     <main class="col-md-9 ms-sm-auto col-lg-10 px-md-4">
      <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3">
        <div class="btn-toolbar mb-2 mb-md-0">
                    <div id="kunjungan" class="divPageNavPanelContent w3-animate-left">
                        <uc1:KunjunganCtl ID="ctlKunjungan" runat="server" />
                    </div>
        </div>
        </div>

    <script type="text/javascript">
        $(function () {
            //#region Left Navigation Panel
            $('#leftPageNavPanelDashboard ul li').first().addClass('nav-link active');
            $('#leftPageNavPanelDashboard ul li').click(function () {
                $('#leftPageNavPanelDashboard ul li.selected').removeClass('nav-item nav-link active');
                $(this).addClass('nav-link active');

                var name = $(this).attr('contentid');
                $('#' + name).removeAttr('style');
                $('#leftPageNavPanelDashboard ul li').each(function () {
                    var tempNameContainer = $(this).attr('contentid');
                    if (tempNameContainer != name) {
                        $(this).removeClass('nav-link active');
                        $('#' + tempNameContainer).attr('style', 'display:none');
                    }
                });

                var contentID = $(this).attr('contentID');
                showContent(contentID);
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
        </script>
</asp:Content>
