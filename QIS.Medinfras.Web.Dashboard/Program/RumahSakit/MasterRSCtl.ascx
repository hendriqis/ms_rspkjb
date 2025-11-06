<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterRSCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Dashboard.Program.MasterRSCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="main-wrapper">
    <main class="main users chart-page" id="skip-target">
            <div class="containerdb">
                <div class="d-sm-flex align-items-center justify-content-between m-2"></div>
            <div class="row stat-cards">
                <div class="col-md-6 col-xl-3">
                    <div class="stat-cards-item">
                        <div class="stat-cards-icon primary">
                            <img id="imgDoctor" class="img-circle" runat="server" title="Doctor" alt="Doctor" style="width:100% ; height: 100%;"/>
                        </div>
                        <div class="stat-cards-info">
                            <p class="stat-cards-info__title2">Total Dokter Aktif</p>
                            <p class="stat-cards-info__num2">
                                <label class="lblParamedicCount" runat="server" id="lblParamedicCount">
                                </label>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-xl-3">
                    <article class="stat-cards-item">
                        <div class="stat-cards-icon primary">
                            <img id="imgNurse" class="img-circle" runat="server" title="Nurse" alt="Nurse" style="width:100% ; height: 100%;"/>
                        </div>
                        <div class="stat-cards-info">
                            <p class="stat-cards-info__title2">Total Perawat Aktif</p>
                            <p class="stat-cards-info__num2">
                                <label class="lblNurseCount" runat="server" id="lblNurseCount"></label>
                            </p>
                        </div>
                    </article>
                </div>
                <div class="col-md-6 col-xl-3">
                    <article class="stat-cards-item">
                        <div class="stat-cards-icon primary">
                            <img id="imgClinic" class="img-circle" runat="server" title="Clinic" alt="Clinic" style="width:100% ; height: 100%;"/>
                        </div>
                        <div class="stat-cards-info">
                            <p class="stat-cards-info__title2">Total Klinik</p>
                            <p class="stat-cards-info__num2">
                                <label class="lblClinicCount" runat="server" id="lblClinicCount"></label>
                            </p>
                        </div>
                    </article>
                </div>
                <div class="col-md-6 col-xl-3">
                    <article class="stat-cards-item">
                        <div class="stat-cards-icon primary">
                            <img id="imgBed" class="img-circle" runat="server" title="Bed" alt="Bed" style="width:100% ; height: 100%;"/>
                        </div>
                        <div class="stat-cards-info">
                            <p class="stat-cards-info__title2">Total Bed</p>
                            <p class="stat-cards-info__num2">
                                <label class="lblBedCount" runat="server" id="lblBedCount"></label>
                            </p>
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
                                Jumlah Jenis Kelamin Per Bed</h6>
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
<script type="text/javascript" id="dxss_masterrsctl">
    $(function () {
        ChartBar();
        PieBar();
    });

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
            }
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