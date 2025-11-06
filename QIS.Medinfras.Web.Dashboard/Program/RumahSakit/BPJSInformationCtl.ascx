<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BPJSInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Dashboard.Program.BPJSInformationCtl" %>
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
                            <img src='<%=ResolveUrl("~/Libs/Images/Dashboard/bpjsHeader.png") %>' class="card-img-top" alt="...">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row row-cols-3">
                <div class="col">
                    <div class="card shadow mb-4">
                        <div class="card">
                            <div class="card-header bg-primary text-white">
                                <B>JUMLAH PASIEN PER POLIKLINIK</B>
                            </div>
                            <div class="chart">
                                <canvas id="ChartNamaPoli"></canvas>
                            </div>
                            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalNamaPoli">
                                <B>SEE DETAILS</B>
                            </button>
                        </div>
                    </div>
                    <div class="card shadow mb-4">
                        <div class="card">
                            <div class="card-header bg-primary text-white">
                                <B>JUMLAH PASIEN PER JENIS PESERTA</B>
                            </div>
                            <div class="chart">
                                <canvas id="ChartJenisPeserta"></canvas>
                            </div>
                                <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalJenisPeserta">
                                <B>SEE DETAILS</B>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="card shadow mb-4">
                        <div class="card">
                            <div class="card-header bg-primary text-white">
                                <B>JUMLAH PASIEN PER DOKTER</B>
                            </div>
                            <div class="chart">
                                <canvas id="ChartPerPM"></canvas>
                            </div>
                                <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalPerPM">
                                <B>SEE DETAILS</B>
                            </button>
                        </div>
                    </div>
                    <div class="card shadow mb-4">
                        <div class="card">
                            <div class="card-header bg-primary text-white">
                                <B>JUMLAH PASIEN PER DIAGNOSIS</B>
                            </div>
                            <div class="chart">
                                <canvas id="ChartPerNM"></canvas>
                            </div>
                                <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#ModalDiagnosa">
                                <B>SEE DETAILS</B>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="col-3">
                    <div class="card shadow mb-4">
                        <div class="card">
                            <div class="card-header bg-primary text-white">
                                <B>KELAS TANGGUNGAN PASIEN</B>
                            </div>
                            <div class="card-body">
                                <div class="chart-pie" id="pieKT">
                                    <canvas id="KelasTanggunganPieChart"></canvas>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card shadow mb-4">
                        <div class="card">
                            <div class="card-header bg-primary text-white">
                                <B>GENDER PASIEN</B>
                            </div>
                            <div class="card-body">
                                <div class="chart-pie" id="pieGen">
                                    <canvas id="GenderPieChart"></canvas>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card shadow mb-4">
                        <div class="card">
                            <div class="card-header bg-primary text-white">
                                <B>ESP</B>
                            </div>
                            <div class="card-body">
                                <div class="chart-pie" id="pieESP">
                                    <canvas id="PieMetodeESP"></canvas>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade " id="modalNamaPoli" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel"><b>JUMLAH PASIEN PER POLIKLINIK</b></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="chart">
                        <canvas id="MaxChartNamaPoli"></canvas>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
            </div>
        </div>
        <div class="modal fade " id="modalJenisPeserta" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="H1"><b>JUMLAH PASIEN PER JENIS PESERTA</b></h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="chart">
                            <canvas id="MaxChartJenisPeserta"></canvas>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade " id="modalPerPM" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="H2"><b>JUMLAH PASIEN PER DOKTER</b></h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="chart">
                            <canvas id="MaxChartPM"></canvas>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade " id="ModalDiagnosa" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="H3"><b>JUMLAH PASIEN PER DIAGNOSA</b></h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="chart">
                            <canvas id="MaxChartNM"></canvas>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
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
                <input type="hidden" value="" id="JsonChartBarPoli" runat="server" />
                <input type="hidden" value="" id="JsonChartPieKelasTanggungan" runat="server" />
                <input type="hidden" value="" id="JsonChartBarJenisPeserta" runat="server" />
                <input type="hidden" value="" id="JsonDataPerPM" runat="server" />
                <input type="hidden" value="" id="JsonChartBarNamaDiagnosa" runat="server" />
                <input type="hidden" value="" id="JsonChartPieGender" runat="server" />
                <input type="hidden" value="" id="JsonChartBarStacked1" runat="server" />
                <input type="hidden" value="" id="JsonChartBarStacked2" runat="server" />
                <input type="hidden" value="" id="JsonChartPieESP" runat="server" />
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
<script type="text/javascript" id="dxss_masterrsctl">
    $(function () {
        BarPoli();
        MaxBarPoli();
        BarJenisPeserta();
        MaxBarJenisPeserta();
        PieKelasTanggungan();
        BarPM();
        MaxBarPM();
        BarNamaDiagnosa();
        MaxBarNamaDiagnosa();
        PieGender();
        BarPM();
//        BarStack();
        PieESP();
    });
    
    $('.modal').appendTo("body") 
    var myModal = document.querySelectorAll('.modal');
    var myInput = document.getElementById('myInput')
    var btn = document.querySelectorAll("button.modal-button");

    for (var i = 0; i < btn.length; i++) {
        btn[i].onclick = function(e) {
        e.preventDefault();
        modal = document.querySelector(e.target.getAttribute("href"));
        }
    }

    window.onclick = function(event) {
    if (event.target.classList.contains('modal')) {
            for (var index in modals) {
            if (typeof modals[index].style !== 'undefined') modals[index].style.display = "none";    
            }
        }
    }

    myModal.addEventListener('shown.bs.modal', function () {
      myInput.focus()
    })

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

    function BarPoli() {
        var Data = JSON.parse($('#<%=JsonChartBarPoli.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderRadius: 5,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
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
                        title: {
                            display: true,
                            text: 'Jumlah Pasien'
                        },
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       },
                    x: {
                        title: {
                            display: true,
                            text: 'Nama Politeknik'
                        },
                        display: false
                       }
                },
                parsing: {
                    xAxisKey: 'NamaPoliklinik',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#ChartNamaPoli').replaceWith($('<canvas id="ChartNamaPoli" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('ChartNamaPoli').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function MaxBarPoli() {
        var Data = JSON.parse($('#<%=JsonChartBarPoli.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderRadius: 5
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
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
                        title: {
                            display: true,
                            text: 'Jumlah Pasien'
                        },
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       }
                },
                parsing: {
                    xAxisKey: 'NamaPoliklinik',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#MaxChartNamaPoli').replaceWith($('<canvas id="MaxChartNamaPoli" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('MaxChartNamaPoli').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function BarJenisPeserta() {
        var Data = JSON.parse($('#<%=JsonChartBarJenisPeserta.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderRadius: 5,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
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
                        title: {
                            display: true,
                            text: 'Jumlah Pasien'
                        },
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       },
                    x: {
                        title: {
                            display: true,
                            text: 'Jenis Peserta'
                        },
                        display: false
                       }
                },
                parsing: {
                    xAxisKey: 'JenisPeserta',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#ChartJenisPeserta').replaceWith($('<canvas id="ChartJenisPeserta" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('ChartJenisPeserta').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function MaxBarJenisPeserta() {
        var Data = JSON.parse($('#<%=JsonChartBarJenisPeserta.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderRadius: 5
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
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
                        title: {
                            display: true,
                            text: 'Jumlah Pasien'
                        },
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       },
                },
                parsing: {
                    xAxisKey: 'JenisPeserta',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#MaxChartJenisPeserta').replaceWith($('<canvas id="MaxChartJenisPeserta" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('MaxChartJenisPeserta').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function PieKelasTanggungan() {
        var Data = JSON.parse($('#<%=JsonChartPieKelasTanggungan.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['Kelas 1','Kelas 2','Kelas 3'],
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

        $('#KelasTanggunganPieChart').replaceWith($('<canvas id="KelasTanggunganPieChart" width="350px" height="150px"></canvas>'));
        var ctx = document.getElementById('KelasTanggunganPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function BarPM() {
        var Data = JSON.parse($('#<%=JsonDataPerPM.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderRadius: 5,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
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
                        title: {
                            display: true,
                            text: 'Jumlah Pasien'
                        },  
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       },
                    x: {
                        title: {
                            display: true,
                            text: 'Nama Paramedic'
                        },
                        display: false
                       }
                },
                parsing: {
                    xAxisKey: 'ParamedicName',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#ChartPerPM').replaceWith($('<canvas id="ChartPerPM" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('ChartPerPM').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function MaxBarPM() {
        var Data = JSON.parse($('#<%=JsonDataPerPM.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderRadius: 5
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
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
                        title: {
                            display: true,
                            text: 'Jumlah Pasien'
                        },
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       }
                },
                parsing: {
                    xAxisKey: 'ParamedicName',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#MaxChartPM').replaceWith($('<canvas id="MaxChartPM" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('MaxChartPM').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function BarNamaDiagnosa() {
        var Data = JSON.parse($('#<%=JsonChartBarNamaDiagnosa.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderRadius: 5,
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
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
                        title: {
                            display: true,
                            text: 'Jumlah Pasien'
                        },
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       },
                    x: {
                        title: {
                            display: true,
                            text: 'Nama Diagnosa'
                        },
                        display: false
                       }
                },
                parsing: {
                    xAxisKey: 'NamaDiagnosa',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#ChartPerNM').replaceWith($('<canvas id="ChartPerNM" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('ChartPerNM').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function MaxBarNamaDiagnosa() {
        var Data = JSON.parse($('#<%=JsonChartBarNamaDiagnosa.ClientID %>').val());
        var DataOption = {
            type: 'bar',
            data: {
                datasets: [{
                    label: ['Jumlah Pasien'],
                    data: Data,
                    backgroundColor: poolColors(Data.length), 
                    borderRadius: 5
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
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
                        title: {
                            display: true,
                            text: 'Jumlah Pasien'
                        },
                        suggestedMax: 4,  
                        ticks: {
                            stepSize: 1
                            }
                       }
                },
                parsing: {
                    xAxisKey: 'NamaDiagnosa',
                    yAxisKey: 'Jumlah'
                }
            }
        }

        $('#MaxChartNM').replaceWith($('<canvas id="MaxChartNM" width="400px" height="200px"></canvas>'));
        var ctx = document.getElementById('MaxChartNM').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

    function PieGender() {
        var Data = JSON.parse($('#<%=JsonChartPieGender.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['Male','Female'],
                datasets: [{
                    data: Data,
                    backgroundColor: ['rgba(52, 70, 235)',  'rgba(235, 52, 98)'],
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

        $('#GenderPieChart').replaceWith($('<canvas id="GenderPieChart" width="350px" height="150px"></canvas>'));
        var ctx = document.getElementById('GenderPieChart').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

//    function BarStack() {
//        var Data1 = JSON.parse($('#<%=JsonChartBarStacked1.ClientID %>').val());
//        var Data2 = JSON.parse($('#<%=JsonChartBarStacked2.ClientID %>').val());
//        var DataOption = {
//            type: 'bar',
//            data: {
//                datasets: [
//                {
//                    label: ['Male'],
//                    data: Data1,
//                    backgroundColor: 'rgba(52, 70, 235)', 
//                    borderRadius: 5
//                },
//                {
//                    label: ['Female'],
//                    data: Data2,
//                    backgroundColor: 'rgba(235, 52, 98)', 
//                    borderRadius: 5
//                },]
//            },
//            options: {
//                plugins: {
//                    legend: {
//                        display: false
//                    }
//                },
//                responsive: true,
//                animations: {
//                  tension: {
//                    duration: 1000,
//                    easing: 'linear',
//                    from: 1,
//                    to: 0,
//                    loop: true
//                  }
//                },
//                scales: {
//                    y: {
//                        stacked: true,
//                        title: {
//                            display: true,
//                        },  
//                        suggestedMax: 4,  
//                        ticks: {
//                            stepSize: 1
//                            }
//                       },
//                    x: {
//                        stacked: true,
//                        title: {
//                            display: true,
//                        },
//                        display: false
//                       }
//                },
//                parsing: {
//                    xAxisKey: 'JenisPeserta',
//                    yAxisKey: 'Jumlah'
//                }
//            }
//        }

//        $('#ChartStack').replaceWith($('<canvas id="ChartStack" width="400px" height="200px"></canvas>'));
//        var ctx = document.getElementById('ChartStack').getContext('2d');
//        var chart = new Chart(ctx, DataOption);
//    }

    function PieESP() {
        var Data = JSON.parse($('#<%=JsonChartPieESP.ClientID %>').val());
        var DataOption = {
            type: 'doughnut',
            data: {
                labels: ['Manual','Non-Manual'],
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

        $('#PieMetodeESP').replaceWith($('<canvas id="PieMetodeESP" width="350px" height="150px"></canvas>'));
        var ctx = document.getElementById('PieMetodeESP').getContext('2d');
        var chart = new Chart(ctx, DataOption);
    }

</script>