<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="VitalSign.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VitalSign" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" style="width:100%">
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 145px" />
                <col style="width: 10px" />
                <col style="width: 145px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal" style="font-weight:bold">
                        <%=GetLabel("Tanggal Pemeriksaan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
                </td>
                <td style="text-align:center">
                    <%=GetLabel("s/d") %>
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" Width="120px" CssClass="datepicker" runat="server" />
                </td>
                <td>
                    <div class="menuTitle">
                        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_vitalsignlist">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).find('.keyUser').html());
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtFromDate.ClientID %>');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtToDate.ClientID %>');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtFromDate.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=txtToDate.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSavePatientPhoto() {
            var MRN = $('#<%:hdnMRN.ClientID %>').val();
            var filterExpression = 'MRN = ' + MRN;
            hideLoadingPanel();
            pcRightPanelContent.Hide();
        }
        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
        function onBeforeBasePatientPageListEdit() {
            if ($('#<%=hdnCurrentUserID.ClientID %>').val() == $('#<%=hdnCurrentSessionID.ClientID %>').val()) {
                return true;
            }
            else {
                showToast('Warning', 'Maaf, tidak diijinkan mengedit catatan user lain.');
                return false;
               
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'patientPhoto') {
                var param = $('#<%:hdnMRN.ClientID %>').val();
                return param;
            }
        }
    </script>
    <style type="text/css">
        .keyUser
        {
            display: none;
        }
    </style>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnIsFallRisk" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                 <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div>
                                            Pemeriksaan Tanda Vital dan Indikator Lainnya</div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <b>
                                                <%#: Eval("ObservationDateInString")%>,
                                                <%#: Eval("ObservationTime") %>,
                                                <%#: Eval("ParamedicName") %>
                                            </b>
                                        </div>
                                        <div>
                                            <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                <ItemTemplate>
                                                    <div style="padding-left: 20px; float: left; width: 300px;">
                                                        <strong>
                                                            <div style="width: 110px; float: left; color: blue" class="labelColumn">
                                                                <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                            <div style="width: 20px; float: left;">
                                                                :</div>
                                                        </strong>
                                                        <div style="float: left;">
                                                            <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                    </div>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <br style="clear: both" />
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div>
                                            <span style="font-weight:bold; text-decoration: underline; color:Black"><%=GetLabel("Catatan Tambahan :")%></span>
                                            <br />
                                            <span style="font-style:italic">
                                                <%#: Eval("Remarks")%>
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada data pemeriksaaan Tanda Vital untuk pasien ini") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
