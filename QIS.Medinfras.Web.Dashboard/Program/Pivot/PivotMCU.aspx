<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true"
    CodeBehind="PivotMCU.aspx.cs" Inherits="QIS.Medinfras.Web.Dashboard.Program.PivotMCU" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
<%--script id="dxss_pivotCtl1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-3.5.1.js")%>' type='text/javascript'></script>
 
 <script  id="dxss_pivotCtl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui-1.9.2.min.js")%>' type='text/javascript'></script>
--%>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.7.min.js")%>' type='text/javascript'></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.9.0.ui.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/libs/d3.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/libs/jquery.ui.touch-punch.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/libs/c3.min.js")%>"></script>

<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/dist/pivot.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/dist/export_renderers.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/dist/d3_renderers.js")%>"></script>
<script type="text/javascript" src="<%= ResolveUrl("~/Libs/Scripts/Pivot/dist/c3_renderers.js")%>"></script>

 <script type="text/javascript">
     $(function () {
         var Data = $('#<%=Pivot1.ClientID %>').val();

         loadOutputFromJson(Data);

         $("#txtDate").datepicker();

         $('.txtDate').each(function () {
             setDatePickerElement($(this));
             $(this).datepicker('option', 'minDate', '0');
         });

         $('.txtDate').change(function () {
             cboPivot.PerformCallback(cboPivot.GetValue().toString());
         });
     });

    function loadOutputFromJson(Data) {
         var derivers = $.pivotUtilities.derivers;

         var renderers = $.extend(
            $.pivotUtilities.renderers,
            $.pivotUtilities.c3_renderers,
            $.pivotUtilities.d3_renderers,
            $.pivotUtilities.export_renderers
            );

         $("#output").pivotUI(JSON.parse(Data), {
             renderers: renderers,
             rendererName: "Table"
         });
     }

     function onCboPivotChanged(cboPivot) {
         cboPivot.PerformCallback(cboPivot.GetValue().toString());
     }

     function onCboPivotEndCallback(s, e) {
         cboPivot.SetValue(cboPivot.cpSelected);
         loadOutputFromJson(cboPivot.cpResult);
     }

    </script>
    <style type="text/css">
    .pvtTotal, .pvtTotalLabel, .pvtGrandTotal {display: none}
    </style>
    <input type="hidden" value="" id="Pivot1" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />

    <div class="container-fluid">
        <div class="row">
                <div class="card shadow mb-4">

                <%--Header--%>
                    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                        <h6 style="color:White; font-weight:bold;">
                            PIVOT MCU FORM</h6>
                    </div>
                    <%--EndHeader--%>
                    <%--Body--%>
                        <div class="card">
                            <div class="card-body">
                                <div class="row"> 
                                    <div class="col" style="width:100%">
                                        Tanggal Registrasi  :   <asp:TextBox ID="txtDate"  runat="server" CssClass="txtDate datepicker" MaxLength=10 />
                                        </br>
                                        </br>
                                        Pilih Hasil         :   <dxe:ASPxComboBox CssClass="form-control" runat="server" ID="cboPivot" ClientInstanceName="cboPivot"
                                                Width="300px" OnCallback="cboPivot_Callback">
                                            <%--<ClientSideEvents EndCallback="function(s,e){ onCboPivotChanged(); }" ValueChanged="function(s,e){ onCboPivotChanged(); }" />--%>
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { onCboPivotChanged(s); }" EndCallback=" onCboPivotEndCallback"/>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                                <div class="chart-area">
                                    <div style="width: 100%; height: 100%; overflow-x: auto" class="box" id="output" ></div>
                            </div>
                        </div>
                        <%--EndBody--%>
                    </div>
                </div>
        </div>
</asp:Content>
