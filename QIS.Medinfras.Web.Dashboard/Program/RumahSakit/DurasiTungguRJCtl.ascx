<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DurasiTungguRJCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Dashboard.Program.DurasiTungguRJCtl" %>
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
            <div class="row" id="contentRow">
                <div>
                    <div class="card shadow mb-4">
                        <div class="card">
                            <img src='<%=ResolveUrl("~/Libs/Images/Dashboard/dashboardDurasiTungguRJ.png") %>' class="card-img-top" alt="...">
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div class="waitRow">
                    <div class="waitCol">
                        <div class="waitChartHeader">
                            <p><b>WAKTU TUNGGU UMUM</b></p>
                        </div>
                        <div class="chartPieWait">
                            <canvas id="WaitGlobalPieChart"></canvas>
                        </div>
                    </div>
                    <div class="waitCol">
                        <div class="waitChartHeader">
                            <p><b>WAKTU TUNGGU LAKI-LAKI</b></p>
                        </div>
                        <div class="chartPieWait">
                            <canvas id="WaitMalePieChart"></canvas>
                        </div>
                    </div>
                    <div class="waitCol">
                        <div class="waitChartHeader">
                            <p><b>WAKTU TUNGGU PEREMPUAN</b></p>
                        </div>
                        <div class="chartPieWait">
                            <canvas id="WaitFemalePieChart"></canvas>
                        </div>
                    </div>
                    <div class="waitColEnd">
                        <div class="waitChartHeader">
                            <p><b>WAKTU TUNGGU UNSPECIFIED</b></p>
                        </div>
                        <div class="chartPieWait">
                            <canvas id="WaitUnknownPieChart"></canvas>
                        </div>
                    </div>
                </div>
            </div>
            <div class="WaitTImeRowContainer">
                <div class="waitTimeContentContainer shadow mb-4" style="display: flex;">
                    <div class="waitGeneralTimeHeader bg-primary">
                        <p>
                            WAKTU TUNGGU
                        </p>
                    </div>
                    <div class="waitTimeContainerCategory">
                        <div class="waitGeneralCategory bg-warning">
                            <div class="generalCtgHeader">
                                <p>
                                    UMUM
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="waitDuration">
                                <p class="waitTimeCategory"><60 Menit</p>
                            </div>
                            <div class="waitDuration">
                                <p class="waitTimeCategory"><label class="lblWaitUnder60m" runat="server" id="lblWaitUnder60m"></label></p>
                            </div>
                        </div>
                    </div>  
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="generalWaitDuration">
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><90 Menit</p>
                                </div>
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><label class="lblWaitUnder90m" runat="server" id="lblWaitUnder90m"></label></p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="generalWaitDuration">
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><120 Menit</p>
                                </div>
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><label class="lblWaitUnder120m" runat="server" id="lblWaitUnder120m"></label></p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="generalWaitDuration">
                                <div class="waitDuration">
                                    <p class="waitTimeCategory">>120 Menit</p>
                                </div>
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><label class="lblWaitUnderXm" runat="server" id="lblWaitUnderXm"></label></p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="waitTimeContentContainer shadow mb-4" style="display: flex;">
                    <div class="waitGeneralTimeHeader bg-primary">
                        <p>
                            WAKTU TUNGGU
                        </p>
                    </div>
                    <div class="waitTimeContainerCategory">
                        <div class="waitGeneralCategory bg-warning">
                            <div class="generalCtgHeader">
                                <p>
                                    LAKI-LAKI
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="waitDuration">
                                <p class="waitTimeCategory"><60 Menit</p>
                            </div>
                            <div class="waitDuration">
                                <p class="waitTimeCategory"><label class="lblMWaitUnder60m" runat="server" id="lblMWaitUnder60m"></label></p>
                            </div>
                        </div>
                    </div>  
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="generalWaitDuration">
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><90 Menit</p>
                                </div>
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><label class="lblMWaitUnder90m" runat="server" id="lblMWaitUnder90m"></label></p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="generalWaitDuration">
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><120 Menit</p>
                                </div>
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><label class="lblMWaitUnder120m" runat="server" id="lblMWaitUnder120m"></label></p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="generalWaitDuration">
                                <div class="waitDuration">
                                    <p class="waitTimeCategory">>120 Menit</p>
                                </div>
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><label class="lblMWaitUnderXm" runat="server" id="lblMWaitUnderXm"></label></p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="waitTimeContentContainerEnd shadow mb-4" style="display: flex;">
                    <div class="waitGeneralTimeHeader bg-primary">
                        <p>
                            WAKTU TUNGGU
                        </p>
                    </div>
                    <div class="waitTimeContainerCategory">
                        <div class="waitGeneralCategory bg-warning">
                            <div class="generalCtgHeader">
                                <p>
                                    PEREMPUAN
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="waitDuration">
                                <p class="waitTimeCategory"><60 Menit</p>
                            </div>
                            <div class="waitDuration">
                                <p class="waitTimeCategory"><label class="lblFMWaitUnder60m" runat="server" id="lblFMWaitUnder60m"></label></p>
                            </div>
                        </div>
                    </div>  
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="generalWaitDuration">
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><90 Menit</p>
                                </div>
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><label class="lblFMWaitUnder90m" runat="server" id="lblFMWaitUnder90m"></label></p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="generalWaitDuration">
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><120 Menit</p>
                                </div>
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><label class="lblFMWaitUnder120m" runat="server" id="lblFMWaitUnder120m"></label></p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="GeneralWaitTimeContainer bg-white">
                        <div class="generalWaitDuration">
                            <div class="generalWaitDuration">
                                <div class="waitDuration">
                                    <p class="waitTimeCategory">>120 Menit</p>
                                </div>
                                <div class="waitDuration">
                                    <p class="waitTimeCategory"><label class="lblFMWaitUnderXm" runat="server" id="lblFMWaitUnderXm"></label></p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
</div>
<input type="hidden" value="" id="JsonChartWaitGlobal" runat="server" />
<input type="hidden" value="" id="JsonChartWaitMale" runat="server" />
<input type="hidden" value="" id="JsonChartWaitFemale" runat="server" />
<input type="hidden" value="" id="JsonChartWaitUnknown" runat="server" />
<script type="text/javascript" id="dxss_bedinformationctl">

    $(function () {
        PieWaitGlobal();
        PieWaitMale();
        PieWaitFemale();
        PieWaitUnknown();
    });

    function dynamicColors() {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgba(" + r + "," + g + "," + b;
    }

    function poolColors(a) {
        var pool = [];
        for (i = 0; i < a; i++) {
            pool.push(dynamicColors());
        }
        return pool;
    }

    function PieWaitGlobal() {
        var Data = JSON.parse($('#<%=JsonChartWaitGlobal.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['<60 Menit','<90 Menit','<120 Menit', '>120 Menit'],
                datasets: [{
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderColor: ['rgba(0, 0, 0)'],
                    hoverOffset: 4,
                }]
            },
            options: {
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'right',
                        labels: {
                            boxWidth: 20,
                            padding: 20
                        }
                    }
                },
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

        $('#WaitGlobalPieChart').replaceWith($('<canvas id="WaitGlobalPieChart"></canvas>'));
        var ctx = document.getElementById('WaitGlobalPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function PieWaitMale() {
        var Data = JSON.parse($('#<%=JsonChartWaitMale.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['<60 Menit','<90 Menit','<120 Menit', '>120 Menit'],
                datasets: [{
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderColor: ['rgba(0, 0, 0)'],
                    hoverOffset: 4,
                }]
            },
            options: {
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'right',
                        labels: {
                            boxWidth: 20,
                            padding: 20
                        }
                    }
                },
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

        $('#WaitMalePieChart').replaceWith($('<canvas id="WaitMalePieChart"></canvas>'));
        var ctx = document.getElementById('WaitMalePieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function PieWaitFemale() {
        var Data = JSON.parse($('#<%=JsonChartWaitFemale.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['<60 Menit','<90 Menit','<120 Menit', '>120 Menit'],
                datasets: [{
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderColor: ['rgba(0, 0, 0)'],
                    hoverOffset: 4,
                }]
            },
            options: {
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'right',
                        labels: {
                            boxWidth: 20,
                            padding: 20
                        }
                    }
                },
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

        $('#WaitFemalePieChart').replaceWith($('<canvas id="WaitFemalePieChart"></canvas>'));
        var ctx = document.getElementById('WaitFemalePieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function PieWaitUnknown() {
        var Data = JSON.parse($('#<%=JsonChartWaitUnknown.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['<60 Menit','<90 Menit','<120 Menit', '>120 Menit'],
                datasets: [{
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderColor: ['rgba(0, 0, 0)'],
                    hoverOffset: 4,
                }]
            },
            options: {
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'right',
                        labels: {
                            boxWidth: 20,
                            padding: 20
                        }
                    }
                },
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

        $('#WaitUnknownPieChart').replaceWith($('<canvas id="WaitUnknownPieChart"></canvas>'));
        var ctx = document.getElementById('WaitUnknownPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }
</script>
