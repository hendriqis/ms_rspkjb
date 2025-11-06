<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true"
    CodeBehind="Example3.aspx.cs" Inherits="QIS.Medinfras.Web.Dashboard.Program.Example3" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <div class="container-fluid">
        <div class="d-sm-flex align-items-center justify-content-between mb-2">
            <h5 class="text-dark">
                <label class="lblHeader" runat="server" id="lblHeader">
                </label>
            </h5>
            <a class="text-dark">
                <label id="lblDateTime" runat="server">
                </label>
            </a>
        </div>
        <div class="row">
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-primary shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 text-center">
                                <img id="imgDoctor" class="img-fluid" runat="server" title="Doctor" />
                            </div>
                            <div class="col-md-8 text-center">
                                <div class="text-xs text-center fw-bolder text-primary text-uppercase mb-1">
                                    Total Dokter Aktif</div>
                                <div class="h1 text-center mb-0 fw-bolder text-primary">
                                    <label class="lblParamedicCount" runat="server" id="lblParamedicCount">
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-success shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 text-center">
                                <img id="imgNurse" class="card-img" runat="server" title="Nurse" />
                            </div>
                            <div class="col-md-8 text-center">
                                <div class="text-xs text-center fw-bolder text-primary text-uppercase mb-1">
                                    Total Perawat Aktif</div>
                                <div class="h1 text-center mb-0 fw-bolder text-primary">
                                    <label class="lblNurseCount" runat="server" id="lblNurseCount">
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-info shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 text-center">
                                <img id="imgClinic" class="card-img" runat="server" title="Clinic" />
                            </div>
                            <div class="col-md-8 text-center">
                                <div class="text-xs text-center fw-bolder text-primary text-uppercase mb-1">
                                    Total Klinik Aktif</div>
                                <div class="h1 text-center mb-0 fw-bolder text-primary">
                                    <label class="lblClinicCount" runat="server" id="lblClinicCount">
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-warning shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                    Pending Requests</div>
                                <div class="h5 mb-0 font-weight-bold text-gray-800">
                                    18</div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-comments fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div id="Graph" class="col-xl-8 col-lg-7">
                <div class="card shadow mb-4">
                    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                        <h6 class="m-0 font-weight-bold text-primary">
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
                    <div class="card-body">
                        <div class="chart-area">
                            <canvas id="ChartDiagramLayout"></canvas>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-6 mb-4">
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">
                            Today's Appointment</h6>
                    </div>
                    <div class="card-body">
                        <table class="tblContentArea" style="width: 100%">
                            <tr>
                                <td>
                                    <div style="padding: 5px; min-height: 150px;">
                                        <dxcp:ASPxCallbackPanel ID="cbpViewAppointment" runat="server" Width="100%" ClientInstanceName="cbpViewAppointment"
                                            ShowLoadingPanel="false" OnCallback="cbpViewAppointment_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { oncbpViewAppointmentEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent2" runat="server">
                                                    <asp:Panel runat="server" ID="pnlService" CssClass="pnlContainerGrid overflow-scroll"
                                                        Style="width: 100%; height: 400px; margin-left: auto; margin-right: auto; position: relative;
                                                        font-size: 0.95em;">
                                                        <asp:ListView ID="lvwView" runat="server">
                                                            <EmptyDataTemplate>
                                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                                    cellspacing="0" rules="all">
                                                                    <tr>
                                                                        <th style="width: 150px">
                                                                            <%= GetLabel("Nama Pasien")%>
                                                                        </th>
                                                                        <th style="width: 250px">
                                                                            <%= GetLabel("Klinik")%>
                                                                        </th>
                                                                        <th style="width: 100px">
                                                                            <%=GetLabel("Jam")%>
                                                                        </th>
                                                                        <th style="width: 100px">
                                                                            <%=GetLabel("Confirm")%>
                                                                        </th>
                                                                    </tr>
                                                                    <tr class="trEmpty">
                                                                        <td colspan="5">
                                                                            <%=GetLabel("Tidak ada informasi perjanjian pasien pada saat ini") %>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EmptyDataTemplate>
                                                            <LayoutTemplate>
                                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                                    cellspacing="0" rules="all">
                                                                    <tr>
                                                                        <th style="width: 150px">
                                                                            <%= GetLabel("Nama Pasien")%>
                                                                        </th>
                                                                        <th style="width: 250px">
                                                                            <%= GetLabel("Klinik")%>
                                                                        </th>
                                                                        <th style="width: 100px">
                                                                            <%=GetLabel("Jam")%>
                                                                        </th>
                                                                        <th style="width: 100px">
                                                                            <%=GetLabel("Confirm")%>
                                                                        </th>
                                                                    </tr>
                                                                    <tr runat="server" id="itemPlaceholder">
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr runat="server" id="trMutationDetail">
                                                                    <td>
                                                                        <div style="float: left">
                                                                            <%#: Eval("PatientName")%>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div>
                                                                            <%#: Eval("ServiceUnitName")%>
                                                                        </div>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div>
                                                                            <%#: Eval("StartTime")%>
                                                                            -
                                                                            <%#: Eval("EndTime")%>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div>
                                                                            V / X
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 mb-4">
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">
                            Illustrations</h6>
                    </div>
                    <div class="card-body">
                        <div class="text-center">
                            <img class="img-fluid px-3 px-sm-4 mt-3 mb-4" style="width: 25rem;" src="img/undraw_posting_photo.svg"
                                alt="...">
                        </div>
                        <p>
                            Add some quality, svg illustrations to your project courtesy of <a target="_blank"
                                rel="nofollow" href="https://undraw.co/">unDraw</a>, a constantly updated collection
                            of beautiful svg images that you can use completely free and without attribution!</p>
                        <a target="_blank" rel="nofollow" href="https://undraw.co/">Browse Illustrations on
                            unDraw &rarr;</a>
                    </div>
                </div>
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">
                            Development Approach</h6>
                    </div>
                    <div class="card-body">
                        <p>
                            SB Admin 2 makes extensive use of Bootstrap 4 utility classes in order to reduce
                            CSS bloat and poor page performance. Custom CSS classes are used to create custom
                            components and custom utility classes.</p>
                        <p class="mb-0">
                            Before working with this theme, you should become familiar with the Bootstrap framework,
                            especially the utility classes.</p>
                    </div>
                </div>
            </div>
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
                    animations: {
                        borderWidth: {
                            duration: 1000,
                            easing: 'linear',
                            to: 1,
                            from: 8
                        }
                    },
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
    </script>
</asp:Content>
