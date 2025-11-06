<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KunjunganCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Dashboard.Program.KunjunganCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <h1 color:"#000">
        Jumlah Kunjungan Per Unit Pelayanan</h1>
                                    <label id="lblDateTime" runat="server">
                            </label>
    <div class="row">
        <div class="col-xl">
            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col">
                            <asp:TextBox ID="txtFrom" runat="server" CssClass="datepicker" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtTo" runat="server" CssClass="datepicker" />
                        </div>
                        <div class="col">
                            <dxe:ASPxComboBox CssClass="form-control" runat="server" ID="cboDepartment" ClientInstanceName="cboDepartment"
                                Width="300px" OnCallback="cboDepartment_Callback">
                                <ClientSideEvents EndCallback="function(s,e){ onCboDepartmentEndCallBack(); }" ValueChanged="function(s,e){ onCboDepartmentChanged(); }" />
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <br />
                            <input type="button" class="btn btn-primary" id="btnSave" value="Load Chart" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xl">
            <div class="card border-primary">
                <div class="card-body text-primary">
                    <canvas id="ChartDiagramLayout" width="400px" height="300px"></canvas>
                </div>
            </div>
        </div>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
        <ClientSideEvents BeginCallback="function(s,e){}" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                    position: relative; font-size: 0.95em;">
                    <input type="hidden" value="" id="JsonChartData" runat="server" />
                    <input type="hidden" value="" id="hdnTimeNow" runat="server" />
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%:txtFrom.ClientID %>');
            setDatePicker('<%:txtTo.ClientID %>');

            $('#<%=txtFrom.ClientID %>').datepicker({
                dateFormat: 'dd-mm-yy',
                todayHighlight: true
            });

            $('#<%=txtTo.ClientID %>').datepicker({
                dateFormat: 'dd-mm-yy',
                todayHighlight: true
            });
        }

        var interval = 1000;

        var intervalID = window.setInterval(function () {
            onRefreshDateTime();
        }, interval);

        function onRefreshDateTime() {
                window.clearInterval(intervalID);
                cbpView.PerformCallback('refreshHour');
                intervalID = window.setInterval(function () {
                    onRefreshDateTime();
                }, interval);
        }

        function onCboDepartmentEndCallBack() { }
        function onCboDepartmentChanged() { }

        $('#btnSave').click(function (evt) {
            cbpView.PerformCallback('refresh');
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            if(s.cpResult == 'refreshHour') {
                var textNow = $('#<%=hdnTimeNow.ClientID %>').val();
                $('#<%=lblDateTime.ClientID %>').html(textNow);
            }
            else {
                ChartBar();
            }
        }

        function ChartBar() {
            var Data = JSON.parse($('#<%=JsonChartData.ClientID %>').val());
            var DataOption = {
                type: 'line',
                data: {
                    datasets: [{
                        label: 'Jumlah Kunjungan',
                        data: Data,
                        backgroundColor: [
                            //'blue',
                            'rgb(0, 0, 128)',
                        ],
                    }]
                },
                options: {
                //animasi grafik goyang (only type line)
                    animations: {
                      tension: {
                        duration: 1000,
                        easing: 'linear',
                        from: 1,
                        to: 0,
                        loop: true
                      }
                    },
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
    </script>