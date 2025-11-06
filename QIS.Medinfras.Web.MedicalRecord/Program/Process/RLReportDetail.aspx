<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
CodeBehind="RLReportDetail.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.RLReportDetail" %>


<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>


<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGenerate" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Generate")%>
        </div>
    </li>
    <li id="btnReportProcess" runat="server" style="display:none"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Export")%></div></li>
    <li runat="server" id="btnEditRecord" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" />
        <div>
            <%=GetLabel("Edit")%>
        </div>
    </li>
    <li runat="server" id="btnSaveRecord" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" />
        <div>
            <%=GetLabel("Save")%>
        </div>
    </li>
    <li runat="server" id="btnCancelRecord" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" />
        <div>
            <%=GetLabel("Cancel")%>
        </div>
    </li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        $(function () {
            function getRLReportExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }
            $('#lblRLReportType.lblLink').click(function () {
                openSearchDialog('rlreport', getRLReportExpression(), function (value) {
                    $('#<%=txtRLReportType.ClientID %>').val(value);
                    onTxtRLRreportCodeChanged(value);
                });
            });

            $('#<%=txtRLReportType.ClientID %>').change(function () {
                onTxtRLRreportCodeChanged($(this).val());
            });

            $('#<%=btnGenerate.ClientID %>').click(function () {
                showLoadingPanel();

                $('#<%=btnEditRecord.ClientID %>').show();
                $('#<%=btnReportProcess.ClientID %>').show();
                $('#<%=btnSaveRecord.ClientID %>').hide();
                $('#<%=btnCancelRecord.ClientID %>').hide();

                $('#<%=hdnSelectedMonth.ClientID %>').val(cboMonth.GetValue());
                $('#<%=hdnSelectedYear.ClientID %>').val(cboYear.GetValue());

                cbpView.PerformCallback('refresh');
            });

            $('#<%=btnEditRecord.ClientID %>').click(function () {
                $('#<%=btnReportProcess.ClientID %>').hide();
                $('#<%=btnEditRecord.ClientID %>').hide();
                $('#<%=btnSaveRecord.ClientID %>').show();
                $('#<%=btnCancelRecord.ClientID %>').show();

                $('.txtRLReportData').each(function () {
                    $(this).removeAttr('readonly');
                });
            });

            $('#<%=btnCancelRecord.ClientID %>').click(function () {
                $('#<%=btnEditRecord.ClientID %>').show();
                $('#<%=btnReportProcess.ClientID %>').show();
                $('#<%=btnSaveRecord.ClientID %>').hide();
                $('#<%=btnCancelRecord.ClientID %>').hide();

                cbpView.PerformCallback('refresh');
            });

            $('#<%=btnReportProcess.ClientID %>').click(function () {
                $('#<%=btnExport.ClientID%>').click();
            });

            $('#<%=btnSaveRecord.ClientID %>').click(function () {
                var param;
                $('#<%=hdnParam.ClientID %>').val(param);
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onCustomButtonClick('save');
            });

            function onTxtRLRreportCodeChanged(value) {
                var filterExpression = "RLReportID = '" + value + "'";
                Methods.getObject('GetRLReportList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnRLReportTypeID.ClientID %>').val(result.RLReportID);
                        $('#<%=txtRLReportType.ClientID %>').val(result.RLReportName);
                    }
                    else {
                        $('#<%=hdnRLReportTypeID.ClientID %>').val('');
                        $('#<%=txtRLReportType.ClientID %>').val('');
                    }
                });
            }
        });

        function onAfterCustomClickSuccess(type, retval) {
            $('#<%=btnEditRecord.ClientID %>').show();
            $('#<%=btnReportProcess.ClientID %>').show();
            $('#<%=btnSaveRecord.ClientID %>').hide();
            $('#<%=btnCancelRecord.ClientID %>').hide();

            $('.txtRLReportData').each(function () {
                $(this).attr('readonly', 'readonly');
            });
        }
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnMainID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMonth" runat="server" />
    <input type="hidden" value="" id="hdnSelectedYear" runat="server" />
    <input type="hidden" value="" id="hdnRLReportCode" runat="server" />

    <div class="pageTitle">
        <%=GetLabel("Detail Laporan RL")%>
    </div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 145px"/>
                    </colgroup>
                    <tr id="trRLReportType" runat="server">
                        <td class="tdLabel"><label class="lblLink" id="lblRLReportType"><%=GetLabel("Tipe Laporan RL")%></label></td>
                        <td colspan="2">
                            <input type="hidden" value="" runat="server" id="hdnRLReportTypeID" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtRLReportType" Width="400px" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trPeriode" runat="server">
                        <td class="tdLabel">
                            <label class="tdLabel">
                            <%=GetLabel("Periode")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboMonth" Width="120px" ClientInstanceName="cboMonth" runat="server" />
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboYear" Width="120px" ClientInstanceName="cboYear" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="position: relative;">    
                    <fieldset id="fsPatientList">
                        <dxcp:aspxcallbackpanel id="cbpView" runat="server" width="100%" clientinstancename="cbpView"
                            showloadingpanel="false" oncallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                EndCallback="function(s,e){ hideLoadingPanel(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 450px;">
                                        <div style="text-align:right; width:100%;">
                                            <label id="lblPeriod" style="font-size: 16px; font-weight: bold;" runat="server"></label>
                                        </div>
                                        <table class="grdSelected" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <th id="thColumnCaption" runat="server"></th>
                                                <asp:Repeater ID="rptHeader" runat="server">   
                                                    <ItemTemplate>
                                                        <th><%#: Eval("Value")%></th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                            <asp:Repeater ID="rptView" runat="server" OnItemDataBound="rptView_ItemDataBound">                                            
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <input type="hidden" id="hdnRowID" runat="server" />
                                                            <%#: Eval("RowTitle")%>
                                                        </td>
                                                        <asp:Repeater ID="rptViewDetail" runat="server">   
                                                            <ItemTemplate>
                                                                <td>
                                                                    <input type="hidden" value='<%#: Eval("Code") %>' runat="server" id="hdnRLReportCode" />
                                                                    <asp:TextBox ID="txtRLReportData" ReadOnly="true" CssClass="txtRLReportData number" runat="server" Width="100%" />
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:aspxcallbackpanel>
                    </fieldset>             
                </div>
            </td>
        </tr>



    </table>
    <div style="display:none;">
        <asp:Button ID="btnExport" Visible="true" runat="server" OnClick="btnExport_Click" Text="Export" />
    </div>
</asp:Content>
