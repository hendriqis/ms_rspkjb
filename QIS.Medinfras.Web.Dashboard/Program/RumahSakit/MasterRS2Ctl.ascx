<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterRS2Ctl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Dashboard.Program.MasterRS2Ctl" %>
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
            <p class="dashboard-title" id="dbTitle1">
                <b>Dashboard</b> > Overview
            </p>    
            </div>           
            <div class="row" id="contentRow">
                <div class="column left1 h-100" id="cl1">
                    <div class="card shadow mb-4">
                        <div class="card">
                            <img src='<%=ResolveUrl("~/Libs/Images/Dashboard/Title.png") %>' class="card-img-top" alt="...">
                        </div>
                    </div>
                </div> <%--KOLOM PERTAMA--%>
                <div class="column middle1 h-100" id="cm1">
                    <div class="card shadow mb-4">
                        <div id="carouselExampleIndicators" class="carousel slide rounded" data-bs-ride="carousel" data-bs-interval="8000">
                        <div class="carousel-indicators">
                            <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                            <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="1" aria-label="Slide 2"></button>
                            <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="2" aria-label="Slide 3"></button>
                        </div>
                            <div class="carousel-inner rounded">
                                <div class="carousel-item active">
                                    <img src='<%=ResolveUrl("~/Libs/Images/Dashboard/Guide1.png") %>' class="d-block w-100" alt="...">
                                </div>
                                <div class="carousel-item">
                                    <img src='<%=ResolveUrl("~/Libs/Images/Dashboard/Guide2.png") %>' class="d-block w-100" alt="...">
                                </div>
                                <div class="carousel-item">
                                    <img src='<%=ResolveUrl("~/Libs/Images/Dashboard/Guide3.png") %>' class="d-block w-100" alt="...">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 col-xl-3">
                            <article class="stat-cards-item">
                                <div>
                                    <div class="stat-cards-icon primary">
                                        <img id="imgDoctor" class="img-circle" runat="server" title="Doctor" alt="Doctor" style="width:100% ; height: 100%;"/>
                                    </div>
                                    <div class="stat-cards-info">
                                        <p class="stat-cards-info__title2">DOKTOR AKTIF</p>
                                        <p class="stat-cards-info__num2">
                                            <label class="lblParamedicCount" runat="server" id="lblParamedicCount">
                                            </label>
                                        </p>
                                    </div>
                                </div>
                            </article>
                        </div>
                        <div class="col-md-6 col-xl-3">
                            <article class="stat-cards-item">
                                <div>
                                    <div class="stat-cards-icon primary">
                                        <img id="imgNurse" class="img-circle" runat="server" title="Nurse" alt="Nurse" style="width:100% ; height: 100%;"/>
                                    </div>
                                    <div class="stat-cards-info">
                                        <p class="stat-cards-info__title2">PERAWAT AKTIF</p>
                                        <p class="stat-cards-info__num2">
                                            <label class="lblNurseCount" runat="server" id="lblNurseCount"></label>
                                        </p>
                                    </div>
                                </div>
                            </article>
                        </div>
                        <div class="col-md-6 col-xl-3">
                            <article class="stat-cards-item">
                                <div>
                                    <div class="stat-cards-icon primary">
                                        <img id="imgClinic" class="img-circle" runat="server" title="Clinic" alt="Clinic" style="width:100% ; height: 100%;"/>
                                    </div>
                                    <div class="stat-cards-info">
                                        <p class="stat-cards-info__title2">TOTAL KLINIK</p>
                                        <p class="stat-cards-info__num2">
                                            <label class="lblClinicCount" runat="server" id="lblClinicCount"></label>
                                        </p>
                                    </div>
                                </div>
                            </article>
                        </div>
                        <div class="col-md-6 col-xl-3">
                            <article class="stat-cards-item">
                                <div>
                                    <div class="stat-cards-icon primary">
                                        <img id="imgBed" class="img-circle" runat="server" title="Bed" alt="Bed" style="width:100% ; height: 100%;"/>
                                    </div>
                                    <div class="stat-cards-info">
                                        <p class="stat-cards-info__title2">TOTAL BED</p>
                                        <p class="stat-cards-info__num2">
                                            <label class="lblBedCount" runat="server" id="lblBedCount"></label>
                                        </p>
                                    </div>
                                </div>
                            </article>
                        </div>
                    </div>
                </div> <%--KOLOM KEDUA--%>
                <div class="column right1 h-100" id="cr1">
                    <div class="card shadow mb-4">
                        <div class="card">
                            <div class="card-body">
                                <h5 class="card-title text-primary"><b>DAFTAR GRAFIK</b></h5>
                                 <p class="card-text">Dashboard Summary Pasien terdiri dari data-data terkait pasien per bulan</p>
                                <ul class="list-group list-group-flush">
                                    <li class="list-group-item">Dashboard Detail Pasien</li>
                                    <li class="list-group-item">Dashboard Data Kunjungan</li>
                                    <li class="list-group-item">Dashboard Detail Administrasi</li>
                                </ul>
                            </div>
                            <img src='<%=ResolveUrl("~/Libs/Images/Dashboard/DaftarKonten.png") %>' class="card-img-bottom" alt="...">
                        </div>
                    </div>
                </div> <%--KOLOM KETIGA--%>     
            </div> <%--ROW PERTAMA--%>
            <div class="card shadow mb-4">
                <button type="button" class="btn btn-primary" data-bs-toggle="collapse" data-bs-target=".multi-collapse" aria-expanded="false" aria-controls="multiCollapseExample1 multiCollapseExample2"><b>TOGGLE GRAFIK</b></button>
            </div>
            <div class="row" id="contentRow2"> <%--ROW KEDUA--%>
                <div class="column left1 h-100" id="row2cl1">
                    <p class="dashboard-title" id="dbTitle2">
                        <b>Dashboard</b> > Detail Pasien
                    </p>
                    <div class="card shadow mb-4">
                        <div class="accordion" id="accordionPanelsStayOpenExample">
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="H2">
                                    <button class="accordion-button bg-primary text-white" type="button" data-bs-toggle="collapse" data-bs-target="#accorpie1" aria-expanded="true" aria-controls="accorpie1">
                                        <b>GENDER PASIEN</b>
                                    </button>
                                </h2>
                                <div id="accorpie1" class="accordion-collapse collapse show multi-collapse" aria-labelledby="panelsStayOpen-headingOne">
                                    <div id="Div5" class="col-xl-4 col-lg-5"> <%--piechartTemplate--%>
                                        <div class="card-body">
                                            <div class="chart-pie">
                                                <canvas id="myPieChart"></canvas>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="H3">
                                    <button class="accordion-button bg-primary text-white" type="button" data-bs-toggle="collapse" data-bs-target="#accorpie2" aria-expanded="true" aria-controls="accorpie2">
                                        <b>KONDISI PASIEN</b>
                                    </button>
                                </h2>
                                <div id="accorpie2" class="accordion-collapse collapse show multi-collapse" aria-labelledby="panelsStayOpen-headingOne">
                                    <div id="Div6" class="col-xl-4 col-lg-5"> <%--piechartPenjamin--%>
                                        <div class="card-body">
                                            <div class="chart-pie">
                                                <canvas id="KondisiPieChart"></canvas>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="H4">
                                    <button class="accordion-button bg-primary text-white" type="button" data-bs-toggle="collapse" data-bs-target="#accorpie3" aria-expanded="true" aria-controls="accorpie3">
                                        <b>METODE DISCHARGE</b>
                                    </button>
                                </h2>
                                <div id="accorpie3" class="accordion-collapse collapse show multi-collapse" aria-labelledby="panelsStayOpen-headingOne">
                                    <div id="Div4" class="col-xl-4 col-lg-5"> <%--piechartTemplate--%>
                                        <div class="chart">
                                            <div class="chart-area">
                                                <canvas id="ChartDiagramDM"></canvas>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div> <%--KOLOM PERTAMA--%>
                <div class="column middle1 h-100" id="row2cm1">
                    <p class="dashboard-title" id="dbTitle1">
                        <b>Dashboard</b> > Data Kunjungan
                    </p>
                <div class="card shadow mb-4">
                    <div class="accordion bg-dark" id="Div17">
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="H7">
                                <button class="accordion-button collapsed bg-primary text-white" type="button" data-bs-toggle="collapse" data-bs-target="#accorbar1" aria-expanded="true" aria-controls="accorbar1">
                                    <b>KUNJUNGAN UNIT PELAYANAN</b>
                                </button>
                            </h2>
                            <div id="accorbar1" class="accordion-collapse collapse show multi-collapse" aria-labelledby="panelsStayOpen-headingOne">
                                <div id="Div19" class="col-xl-8 col-lg-7">
                                    <div class="chart">
                                        <div class="chart-area">
                                            <canvas id="ChartDiagramLayout2"></canvas>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="H8">
                                <button class="accordion-button collapsed bg-primary text-white" type="button" data-bs-toggle="collapse" data-bs-target="#accorbar2" aria-expanded="true" aria-controls="accorbar2">
                                 <b>KUNJUNGAN DOKTER</b>
                                </button>
                            </h2>
                            <div id="accorbar2" class="accordion-collapse collapse show multi-collapse" aria-labelledby="panelsStayOpen-headingTwo">
                            <div id="Div21" class="col-xl-8 col-lg-7">
                                <div class="chart">
                                    <div class="chart-area">
                                        <canvas id="ChartDiagramDokter"></canvas>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="H9">
                                <button class="accordion-button collapsed bg-primary text-white" type="button" data-bs-toggle="collapse" data-bs-target="#accorbar3" aria-expanded="true" aria-controls="accorbar3">
                                    <b>KUNJUNGAN DEPARTEMEN</b>
                                </button>
                            </h2>
                            <div id="accorbar3" class="accordion-collapse collapse show multi-collapse" aria-labelledby="panelsStayOpen-headingThree">
                                <div id="Div23" class="col-xl-8 col-lg-7">
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
                        </div>
                    </div>
                </div>
                </div> <%--KOLOM KEDUA--%>
                <div class="column right1 h-100" id="row2cr1">
                    <p class="dashboard-title" id="P1">
                        <b>Dashboard</b> > Detail Administrasi
                    </p>
                    <div class="card shadow mb-4">
                        <div class="accordion" id="Div2">
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="H1">
                                    <button class="accordion-button bg-primary text-white" type="button" data-bs-toggle="collapse" data-bs-target="#accorpie1Row2" aria-expanded="true" aria-controls="accorpie1Row2">
                                        <b>STATUS REGISTRASI</b>
                                    </button>
                                </h2>
                                <div id="accorpie1Row2" class="accordion-collapse collapse show multi-collapse" aria-labelledby="panelsStayOpen-headingOne">
                                    <div id="Div1" class="col-xl-4 col-lg-5"> <%--piechartStatus--%>
                                        <div class="card-body">
                                            <div class="chart-pie">
                                                <canvas id="StatusPieChart"></canvas>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="H10">
                                    <button class="accordion-button bg-primary text-white" type="button" data-bs-toggle="collapse" data-bs-target="#accorpie2Row2" aria-expanded="true" aria-controls="accorpie2Row2">
                                        <b>KELAS PASIEN</b>
                                    </button>
                                </h2>
                                <div id="accorpie2Row2" class="accordion-collapse collapse show multi-collapse" aria-labelledby="panelsStayOpen-headingOne">
                                    <div id="Div11" class="col-xl-4 col-lg-5"> <%--piechartPenjamin--%>
                                        <div class="card-body">
                                            <div class="chart-pie">
                                                <canvas id="KelasPieChart"></canvas>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="H11">
                                    <button class="accordion-button bg-primary text-white" type="button" data-bs-toggle="collapse" data-bs-target="#accorpie3Row2" aria-expanded="true" aria-controls="accorpie3Row2">
                                        <b>PENJAMIN BAYAR</b>
                                    </button>
                                </h2>
                                <div id="accorpie3Row2" class="accordion-collapse collapse show multi-collapse" aria-labelledby="panelsStayOpen-headingOne">
                                    <div id="Div13" class="col-xl-4 col-lg-5"> <%--piechartTemplate--%>
                                        <div class="card-body">
                                            <div class="chart-pie">
                                                <canvas id="PenjaminPieChart"></canvas>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div> <%--KOLOM KETIGA--%>
            </div> <%--ROW KEDUA--%>
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
                <input type="hidden" value="" id="JsonDataPerDepartment" runat="server" />
                <input type="hidden" value="" id="JsonRegStatus" runat="server" />
                <input type="hidden" value="" id="JsonDataPerDokter" runat="server" />
                <input type="hidden" value="" id="JsonChartPieStatus" runat="server" />
                <input type="hidden" value="" id="JsonChartPiePenjamin" runat="server" />
                <input type="hidden" value="" id="JsonChartDischarge" runat="server" />
                <input type="hidden" value="" id="JsonChartPieKondisiPasien" runat="server" />
                <input type="hidden" value="" id="JsonChartPieKelasPasien" runat="server" />
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
<script type="text/javascript" id="dxss_masterrsctl">
    $(function () {
        ChartBar();
        PieBar();
        PieStatus();
        ChartBarDept();
        ChartBarDokt();
        PiePenjamin();
        ChartBarDisc();
        PieKondisi();
        PieKelas();
    });
    
    function dynamicColors() {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgba(" + r + "," + g + "," + b;
    }

    function poolColors(a) {
        var pool = [];
        for(i = 0; i < a; i++) {
            pool.push(dynamicColors());
        }
        return pool;
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
                    backgroundColor: poolColors(Data.length), 
                    borderColor: ['rgba(0, 0, 0)'],
                    borderWidth: 2,
                    borderRadius: 5,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            boxWidth: 20,
                            padding: 20
                        }
                    }
                },
                responsive: true,
                animations: {
                  tension: {
                    duration: 1000,
                    easing: 'linear',
                    from: 1,
                    to: 0,
                    loop: true
                  }
                },
                scales: {
                    y: {
                        suggestedMax: 10,  
                        ticks: {
                            stepSize: 1
                            }
                       },
                    x: {
                        display: false
                       }
                },
                parsing: {
                    xAxisKey: 'ID',
                    yAxisKey: 'Value'
                }
            }
        }

        $('#ChartDiagramLayout').replaceWith($('<canvas id="ChartDiagramLayout" width="300px" height="200px"></canvas>'));
        var ctx = document.getElementById('ChartDiagramLayout').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function ChartBarDept() {
        var Data = JSON.parse($('#<%=JsonDataPerDepartment.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Kunjungan'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderColor: ['rgba(0, 0, 0)'],
                    borderWidth: 2,
                    borderRadius: 5,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            boxWidth: 20,
                            padding: 20
                        }
                    }
                },
                responsive: true,
                animations: {
                  tension: {
                    duration: 1000,
                    easing: 'linear',
                    from: 1,
                    to: 0,
                    loop: true
                  }
                },
                scales: {
                    y: {
                        suggestedMax: 10,  
                        ticks: {
                            stepSize: 1
                            }
                       },
                    x: {
                        display: false
                       }
                },
                parsing: {
                    xAxisKey: 'DepartmentID',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#ChartDiagramLayout2').replaceWith($('<canvas id="ChartDiagramLayout2" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('ChartDiagramLayout2').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function ChartBarDokt() {
        var Data = JSON.parse($('#<%=JsonDataPerDokter.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderColor: ['rgba(0, 0, 0)'],
                    borderWidth: 2,
                    borderRadius: 5,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            boxWidth: 20
                        }
                    }
                },
                responsive: true,
                animations: {
                  tension: {
                    duration: 1000,
                    easing: 'linear',
                    from: 1,
                    to: 0,
                    loop: true
                  }
                },
                scales: {
                    y: {
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       },
                    x: {
                        display: false
                       }
                },
                parsing: {
                    xAxisKey: 'ParamedicName',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#ChartDiagramDokter').replaceWith($('<canvas id="ChartDiagramDokter" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('ChartDiagramDokter').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function ChartBarDisc() {
        var Data = JSON.parse($('#<%=JsonChartDischarge.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderColor: ['rgba(0, 0, 0)'],
                    borderWidth: 2,
                    borderRadius: 5,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            boxWidth: 20
                        }
                    }
                },
                responsive: true,
                animations: {
                  tension: {
                    duration: 1000,
                    easing: 'linear',
                    from: 1,
                    to: 0,
                    loop: true
                  }
                },
                scales: {
                    y: {
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       },
                    x: {
                        display: false
                       }
                },
                parsing: {
                    xAxisKey: 'DischargeMethod',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#ChartDiagramDM').replaceWith($('<canvas id="ChartDiagramDM" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('ChartDiagramDM').getContext('2d');
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
                    backgroundColor: ['rgba(125, 13, 253, 0.7)','rgba(253, 13, 177, 0.7)', 'rgba(81, 111, 122, 0.7)'],
                    borderColor: ['rgba(0, 0, 0)'],
                }]
            },
            options: {
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            boxWidth: 10,
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

        $('#myPieChart').replaceWith($('<canvas id="myPieChart" width="350px" height="150px"></canvas>'));
        var ctx = document.getElementById('myPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function PieStatus() {
        var Data = JSON.parse($('#<%=JsonChartPieStatus.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['Non-Void','Void'],
                datasets: [{
                    data: Data,
                    backgroundColor: ['rgba(0, 162, 255, 0.7)','rgba(153, 0, 255, 0.7)'],
                    borderColor: ['rgba(0, 0, 0)'],
                }]
            },
            options: {
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            boxWidth: 20,
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

        $('#StatusPieChart').replaceWith($('<canvas id="StatusPieChart" width="350px" height="150px"></canvas>'));
        var ctx = document.getElementById('StatusPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function PiePenjamin() {
        var Data = JSON.parse($('#<%=JsonChartPiePenjamin.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['Pribadi','Non-Pribadi'],
                datasets: [{
                    data: Data,
                    backgroundColor: ['rgba(13, 110, 253, 0.7)','rgba(61, 13, 253, 0.7)'],
                    borderColor: ['rgba(0, 0, 0)'],
                    hoverOffset: 4,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        position: 'bottom',
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

        $('#PenjaminPieChart').replaceWith($('<canvas id="PenjaminPieChart" width="350px" height="150px"></canvas>'));
        var ctx = document.getElementById('PenjaminPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    } 

    
    function PieKondisi() {
        var Data = JSON.parse($('#<%=JsonChartPieKondisiPasien.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['Sehat','Membaik', 'Sakit'],
                datasets: [{
                    data: Data,
                    backgroundColor: ['rgba(13, 110, 253, 0.7)','rgba(61, 13, 253, 0.7)', 'rgba(252, 186, 3, 0.7)'],
                    borderColor: ['rgba(0, 0, 0)'],
                    hoverOffset: 4,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        position: 'bottom',
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

        $('#KondisiPieChart').replaceWith($('<canvas id="KondisiPieChart" width="350px" height="150px"></canvas>'));
        var ctx = document.getElementById('KondisiPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    } 
    

    function PieKelas() {
        var Data = JSON.parse($('#<%=JsonChartPieKelasPasien.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['Kelas 1','Kelas 2', 'Kelas 3'],
                datasets: [{
                    data: Data,
                    backgroundColor: ['rgba(13, 110, 253, 0.7)','rgba(61, 13, 253, 0.7)', 'rgba(252, 186, 3, 0.7)'],
                    borderColor: ['rgba(0, 0, 0)'],
                    hoverOffset: 4,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        position: 'bottom',
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

        $('#KelasPieChart').replaceWith($('<canvas id="KelasPieChart" width="350px" height="150px"></canvas>'));
        var ctx = document.getElementById('KelasPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    } 
</script>