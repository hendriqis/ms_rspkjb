<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true"
    CodeBehind="Pasien1.aspx.cs" Inherits="QIS.Medinfras.Web.Dashboard.Program.Pasien1" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
<div class="page-flex">
        <!-- ! Sidebar -->
        <aside class="sidebar">
    <div class="sidebar-start">
        <div class="sidebar-head">
            <a href="/" class="logo-wrapper" title="Home">
                <span class="sr-only">Home</span>
                <span class="icon logo" aria-hidden="true"></span>
                <div class="logo-text">
                </div>

            </a>
            <button class="sidebar-toggle transparent-btn" title="Menu" type="button">
                <span class="sr-only">Toggle menu</span>
                <span class="icon menu-toggle" aria-hidden="true"></span>
            </button>
        </div>
        <div class="sidebar-body">
            <ul class="sidebar-body-menu">
                <li>
                    <a class="active" href="#"><span class="icon home" aria-hidden="true"></span>Dashboard</a>
                </li>
                <li>
                    <a class="show-cat-btn" href="##">
                        <span class="icon document" aria-hidden="true"></span>Posts
                        <span class="category__btn transparent-btn" title="Open list">
                            <span class="sr-only">Open list</span>
                            <span class="icon arrow-down" aria-hidden="true"></span>
                        </span>
                    </a>
                    <ul class="cat-sub-menu">
                        <li>
                            <a href="posts.html">All Posts</a>
                        </li>
                        <li>
                            <a href="new-post.html">Add new post</a>
                        </li>
                    </ul>
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
    <main class="main users chart-page" id="skip-target">
        <div class="containerdb">
            <div class="d-sm-flex align-items-center justify-content-between m-2"></div>
        <div class="row stat-cards">
            <div class="col-md-6 col-xl-3">
                <article class="stat-cards-item">
                    <div class="stat-cards-icon primary">
                        <img id="imgPatient" class="img-circle" runat="server" title="Patient" alt="Patient" style="width:100% ; height: 100%;"/>
                    </div>
                    <div class="stat-cards-info">
                        <p class="stat-cards-info__title2">Kunjungan Bulan Ini</p>
                        <p class="stat-cards-info__num2">
                            <label class="lblPatientCount" runat="server" id="lblPatientCount">
                            </label>
                        </p>
                    </div>
                </article>
            </div>
            <div class="col-md-6 col-xl-3">
                <article class="stat-cards-item shadow">
                  <div class="stat-cards-info col primary">
                    <p class="stat-cards-info__title2">Pasien Lama</p>
                    <p class="stat-cards-info__num2"><label class="lblOldPatient" runat="server" id="lblOldPatient">
                                        </label></p>
                  </div>
                  <div class="stat-cards-info col primary">
                    <p class="stat-cards-info__title2">Pasien Baru</p>
                    <p class="stat-cards-info__num2">
                        <label class="lblNewPatient" runat="server" id="lblNewPatient"></label>
                    </p>
                  </div>
                </article>
            </div>
            <div class="col-md-6 col-xl-3">
                <article class="stat-cards-item">
                    <div class="stat-cards-icon primary">
                        <img id="imgToday" class="img-circle" runat="server" title="Today" alt="Today" style="width:100% ; height: 100%;"/>
                    </div>
                    <div class="stat-cards-info">
                        <p class="stat-cards-info__title2">Pasien Hari Ini</p>
                        <p class="stat-cards-info__num2">
                            <label class="lblTodayPatient" runat="server" id="lblTodayPatient"></label>
                        </p>
                    </div>
                </article>
            </div>
            <div class="col-md-6 col-xl-3">
                <article class="stat-cards-item shadow">
                    <div class="stat-cards-icon success">
                        <img id="imgStatus" class="img-circle" runat="server" title="Status" alt="Status" style="width:100% ; height: 100%;"/>
                    </div>
                    <div class="stat-cards-info">
                        <p class="stat-cards-info__title2">Status Karyawan</p>
                        <p class="stat-cards-info__progress2">
                            <label class="lblEmployee" runat="server" id="lblEmployee"></label>
                        </p>
                        <p class="stat-cards-info__progress3" runat="server" id="lblStatus"></p>
                        <p class="stat-cards-info__progress3"  runat="server" id="lblYear"></p>
                    </div>
                </article>
             </div>
        </div>
        <div class="row">
            <div id="Graph" class="col-xl-8 col-lg-7">
                <div class="card shadow mb-4">
                    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                        <h6 class="m-0 font-weight-bold title-card-header">
                            Jumlah Kunjungan Per Unit Pelayanan</h6>
                        <div class="dropdown no-arrow">
                            <a class="dropdown-toggle" data-bs-toggle="collapse" href="#collapseExample" role="button"
                                aria-expanded="false" aria-controls="collapseExample"></a>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col">
                                    <dxe:ASPxComboBox CssClass="form-control" runat="server" ID="cboDepartment" ClientInstanceName="cboDepartment"
                                        Width="300px" OnCallback="cboDepartment_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCboDepartmentEndCallBack(); }" ValueChanged="function(s,e){ onCboDepartmentChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="chart">
                        <div class="chart-area">
                            <canvas id="ChartDiagramLayout"></canvas>
                        </div>
                    </div>
                </div>
            </div>
            <div id="PieChart" class="col-xl-4 col-lg-5">
                <div class="card shadow mb-4">
                    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                        <h6 class="m-0 font-weight-bold title-card-header">
                            Jumlah Jenis Kelamin Per Kunjungan</h6>
                        <div class="dropdown no-arrow">
                            <a class="dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown"
                                aria-haspopup="true" aria-expanded="false"><i class="fa-sm fa-fw text-gray-400">
                                </i></a>
                            <div class="dropdown-menu dropdown-menu-right shadow animated--fade-in" aria-labelledby="dropdownMenuLink">
                                <div class="dropdown-header">
                                    Dropdown Header:</div>
                                <a class="dropdown-item" href="#">Action</a> <a class="dropdown-item" href="#">Another
                                    action</a>
                                <div class="dropdown-divider">
                                </div>
                                <a class="dropdown-item" href="#">Something else here</a>
                            </div>
                        </div>
                    </div>
                    <!-- Card Body -->
                    <div class="card-body">
                        <div class="chart-pie">
                            <canvas id="myPieChart"></canvas>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </main>
    </div>
</div>
<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                position: relative; font-size: 0.95em;">
                <input type="hidden" value="" id="JsonChartData" runat="server" />
                <input type="hidden" value="" id="JsonChartPieData" runat="server" />
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
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
    function onLoad() {
        ChartBar();
        PieBar();
    }

    function onCboDepartmentEndCallBack() { }
    function onCboDepartmentChanged() {
        cbpView.PerformCallback('refresh');
        }

    $('#btnSave').click(function (evt) {
        cbpView.PerformCallback('refresh');
    });

    function onCbpViewEndCallback() {
        hideLoadingPanel();
        ChartBar();
    }

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

    function ChartBar() {
        var Data = JSON.parse($('#<%=JsonChartData.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Kunjungan'],
                    data: Data,
                    backgroundColor: ['#1E90FF']
                }]
            },
            options: {
                responsive: true,
                parsing: {
                    xAxisKey: 'ID',
                    yAxisKey: 'Value'
                }
            },
        }

        $('#ChartDiagramLayout').replaceWith($('<canvas id="ChartDiagramLayout" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('ChartDiagramLayout').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function PieBar() {
        var Data = JSON.parse($('#<%=JsonChartPieData.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['Male','Female','Unspecified'],
                datasets: [{
                    data: Data,
                    backgroundColor: ['#00008B','#FF69B4','#808080'],
                }]
            },
            options: {
                animation: {
                    animateScale: true
                },
                responsive: true,
                parsing: {
                    xAxisKey: 'ID',
                    yAxisKey: 'Value'
                }
            }
        }

        $('#myPieChart').replaceWith($('<canvas id="myPieChart" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('myPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }
</script>
</asp:Content>
