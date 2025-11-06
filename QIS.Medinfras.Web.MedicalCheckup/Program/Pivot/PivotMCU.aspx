<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true"
    CodeBehind="PivotMCU.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.PivotMCU" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
<%--<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.7.min.js")%>' type='text/javascript'></script>--%>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.9.0.ui.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/libs/d3.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/libs/jquery.ui.touch-punch.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/libs/c3.min.js")%>"></script>

<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/dist/pivot.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/dist/export_renderers.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/dist/d3_renderers.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/dist/c3_renderers.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/jquery/select2.min.js")%>"></script>

 <script type="text/javascript">
     $(function () {
         $("#divPivot").hide();
         $('.txtDate').datepicker({
             defaultDate: "w",
             changeMonth: true,
             changeYear: true,
             dateFormat: "dd-mm-yy",
             showOn: "button",
             //showOn: "both",
             buttonImage: ResolveUrl("~/Libs/Images/calendar.gif"),
             buttonImageOnly: true
         });

         $('.txtDate').datepicker('option', 'maxDate', '0');

         var Data = $('#<%=Pivot1.ClientID %>').val();

         if (Data != '') {
             loadOutputFromJson(Data);
         }

         var cbo = document.getElementById("cboBusinessPartner");
         var lst = $('#<%=hdnListCustomer.ClientID %>').val();
         if (lst != '') {
             var arr = lst.split('|');
             if (arr.length > 0) {
                 for (var i = 0; i < arr.length; i++) {
                     var option = document.createElement("option");
                     option.text = arr[i];
                     cbo.add(option);
                 }
             }
         }
         else {
             var option = document.createElement("option");
             option.text = "No Data Found";
             cbo.add(option);
         }
     });

    function loadOutputFromJson(Data) {
        var derivers = $.pivotUtilities.derivers;

        var renderers = $.extend(
            $.pivotUtilities.renderers,
            $.pivotUtilities.c3_renderers,
            $.pivotUtilities.d3_renderers,
            $.pivotUtilities.export_renderers
        );

        var utils = $.pivotUtilities;
        var heatmap = utils.renderers["Table"];
        var sumOverSum = utils.aggregators["Sum over Sum"];

        var tpl = $.pivotUtilities.aggregatorTemplates;

        $("#output").pivotUI(JSON.parse(Data), {
            renderers: renderers,
            rendererName: "Table"
        });
    }

    $(document).ready(function () {
        $('.cboBusinessPartner').select2({
            placeholder: "Pilih Penjamin.."
        });

        $('.cboBatchNo').select2({
            placeholder: "Pilih No Batch.."
        });

        $('.cboResultType').select2({
            placeholder: "Pilih Halaman.."
        });

        $('#cboResultType').on('select2:select', function (e) {
            var data = e.params.data;
            var json = onProcessCbo(data);
        });

        $('#cboBatchNo').on('select2:select', function (e) {
            alert('aaaa');
            var data = e.params.data;
            var json = onProcessCbo(data);
            Methods.getResultTypeByRequestBatchNo(json.id, function (result) {
                if (result != '') {
                    var resultInfo = result.split('|');
                    if (resultInfo[0] == "1") {
                        if (resultInfo[2] != '') {
                            $('#cboResultType').empty();
                            var cbo = document.getElementById("cboResultType");
                            var arr = resultInfo[2].split(';');
                            if (arr.length > 0) {
                                for (var i = 0; i < arr.length; i++) {
                                    var option = document.createElement("option");
                                    option.text = arr[i];
                                    cbo.add(option);
                                }
                            }
                        }
                        else {
                            $("#divPivot").hide();
                            var option = document.createElement("option");
                            option.text = "No Data Found";
                            cbo.add(option);
                        }
                    }
                    else {
                        $("#divPivot").hide();
                        ShowSnackbarWarning(resultInfo[1]);
                        $('#cboResultType').empty();
                    }
                }
                else {
                    $("#divPivot").hide();
                    ShowSnackbarError("Process Error");
                }
            });
        });

        $('#cboBusinessPartner').on('select2:select', function (e) {
            var data = e.params.data;
            var json = onProcessCbo(data);
            var day = $('#<%=txtDate.ClientID %>').val().substring(0, 2);
            var month = $('#<%=txtDate.ClientID %>').val().substring(3, 5);
            var year = $('#<%=txtDate.ClientID %>').val().substring(6);
            var date = year + month + day;
            Methods.getRequestBatchNo(json.id, date, function (result) {
                if (result != '') {
                    var resultInfo = result.split('|');
                    if (resultInfo[0] == "1") {
                        if (resultInfo[2] != '') {
                            $('#cboBatchNo').empty();
                            $('#cboResultType').empty();
                            var cbo = document.getElementById("cboBatchNo");
                            var arr = resultInfo[2].split(',');
                            if (arr.length > 0) {
                                for (var i = 0; i < arr.length; i++) {
                                    var option = document.createElement("option");
                                    option.text = arr[i];
                                    cbo.add(option);
                                }
                                var batchNo = $('#cboBatchNo').select2('data');
                                Methods.getResultTypeByRequestBatchNo(batchNo[0].text, function (result) {
                                    if (result != '') {
                                        var resultInfo = result.split('|');
                                        if (resultInfo[0] == "1") {
                                            if (resultInfo[2] != '') {
                                                $('#cboResultType').empty();
                                                var cbo = document.getElementById("cboResultType");
                                                var arr = resultInfo[2].split(';');
                                                if (arr.length > 0) {
                                                    for (var i = 0; i < arr.length; i++) {
                                                        var option = document.createElement("option");
                                                        option.text = arr[i];
                                                        cbo.add(option);
                                                    }
                                                }
                                            }
                                            else {
                                                $("#divPivot").hide();
                                                var option = document.createElement("option");
                                                option.text = "No Data Found";
                                                cbo.add(option);
                                            }
                                        }
                                        else {
                                            $("#divPivot").hide();
                                            ShowSnackbarWarning(resultInfo[1]);
                                            $('#cboResultType').empty();
                                        }
                                    }
                                    else {
                                        $("#divPivot").hide();
                                        ShowSnackbarError("Process Error");
                                    }
                                });
                            }
                        }
                        else {
                            $("#divPivot").hide();
                            var option = document.createElement("option");
                            option.text = "No Data Found";
                            cbo.add(option);
                        }
                    }
                    else {
                        $("#divPivot").hide();
                        ShowSnackbarWarning(resultInfo[1]);
                        $('#cboBatchNo').empty();
                        $('#cboResultType').empty();
                    }
                }
                else {
                    $("#divPivot").hide();
                    ShowSnackbarError("Process Error");
                }
            });
        });
    });

    function onProcessCbo(data) {
        var jsonString = JSON.stringify(data);
        var json = JSON.parse(jsonString);
        return json;
    }
    $('#btnDownload').live('click', function () {
        var htmls = "";
        var uri = 'data:application/vnd.ms-excel;base64,';
        var template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>';
        var base64 = function (s) {
            return window.btoa(unescape(encodeURIComponent(s)))
        };

        var format = function (s, c) {
            return s.replace(/{(\w+)}/g, function (m, p) {
                return c[p];
            })
        };

        htmls = $(".pvtTable").html();
        if (htmls != "") {
            var ctx = {
                worksheet: 'Worksheet',
                table: htmls
            }


            var link = document.createElement("a");
            link.download = "export.xls";
            link.href = uri + base64(format(template, ctx));
            link.click();
        } else {
            ShowSnackbarError("Tidak ada data");
        }
    });

    $('#<%=btnRefresh.ClientID %>').live('click', function () {
        if ($('#cboResultType').val() != null && $('#cboBatchNo').val() != null) {
            var batchNo = $('#cboBatchNo').select2('data');
            var resultType = $('#cboResultType').select2('data');
            var day = $('#<%=txtDate.ClientID %>').val().substring(0, 2);
            var month = $('#<%=txtDate.ClientID %>').val().substring(3, 5);
            var year = $('#<%=txtDate.ClientID %>').val().substring(6);
            var date = year + month + day;
            Methods.onLoadPivot(batchNo[0].text, resultType[0].text, date, function (resultPiv) {
                if (resultPiv != '') {
                    var arr = resultPiv.split('|');
                    if (arr[0] == "1") {
                        $("#divPivot").show();
                        $("#btnDownload").show();
                        loadOutputFromJson(arr[2]);
                    }
                    else {
                        $("#divPivot").hide();
                        ShowSnackbarWarning(arr[1]);
                    }
                }
            });
        }
        else {
            $("#divPivot").hide();
            ShowSnackbarError("Nomor Batch dan Halaman harus diisi");
        }
    });

    </script>
    <style type="text/css">
    .pvtTotal, .pvtTotalLabel, .pvtGrandTotal {display: none}
    </style>
    <input type="hidden" value="" id="Pivot1" runat="server" />
    <input type="hidden" value="" id="hdnListCustomer" runat="server" />
    <div class="container-fluid">
    <div class="row">
        <div class="card shadow mb-4">
        <%--Header--%>
            <div >
                <h4>
                    PIVOT MCU FORM</h4>
            </div>
            <%--EndHeader--%>
            <%--Body--%>
                <div class="card">
                    <div class="card-body">
                        <table style="width:100%">
                            <colgroup>
                                <col style="width: 10%" />
                                <col style="width: 90%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <div>
                                        Tanggal Registrasi
                                    </div>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDate"  runat="server" CssClass="txtDate datepicker" MaxLength=10 />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div> Penjamin Bayar
                                    </div>
                                </td>
                                <td>
                                    <select class="cboBusinessPartner" id="cboBusinessPartner" style="width: 25%">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div> Nomor Batch
                                    </div>
                                </td>
                                <td>
                                    <select class="cboBatchNo" id="cboBatchNo" style="width: 25%">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div> Halaman
                                    </div>
                                </td>
                                <td>
                                    <select class="cboResultType" id="cboResultType" style="width: 25%">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="button" id="btnRefresh" value="Refresh" style="width: 150px;" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="card-body" >
                <input type="button" id="btnDownload" value="download" style="display:none;"  />
                <div id="divPivot">
                    <div class="chart-area">
                        <div style="width: 100%; height: 100%; overflow-x: auto" class="box" id="output" ></div>
                    </div>
                </div>
                </div>
                <%--EndBody--%>
            </div>
        </div>
    </div>
</asp:Content>
