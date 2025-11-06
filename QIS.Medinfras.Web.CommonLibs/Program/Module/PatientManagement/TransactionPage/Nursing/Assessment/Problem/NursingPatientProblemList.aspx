<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="NursingPatientProblemList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingPatientProblemList" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnQuickPicks" runat="server" CRUDMode="C"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div><%=GetLabel("Quick Picks")%></div></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).find('.keyUser').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtDate.ClientID %>');
            $('#<%=txtDate.ClientID %>').datepicker('option', 'minDate', '0');
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
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

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.selected'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.selected');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.selected').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=txtDate.ClientID %>').val(entity.cfProblemDateDatePickerFormat);
                $('#<%=txtTime.ClientID %>').val(entity.ProblemTime);
                cboParamedic.SetValue(entity.ParamedicName);
                $('#<%=hdnProblemID.ClientID %>').val(entity.ProblemID);
                $('#<%=txtProblemCode.ClientID %>').val(entity.ProblemCode);
                $('#<%=txtProblemName.ClientID %>').val(entity.ProblemName);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                cboParamedic.SetValue($('#<%=hdnDefaultParamedicID.ClientID %>').val());
            }
        }
        //#endregion

        //#region Problem
        $('#lblProblemCode.lblLink').live('click', function () {
            openSearchDialog('nursingProblem', ' IsDeleted = 0', function (value) {
                $('#<%=txtProblemCode.ClientID %>').val(value);
                onTxtProblemCodeChanged(value);
            });
        });

        $('#<%=txtProblemCode.ClientID %>').live('change', function () {
            onTxtProblemCodeChanged($(this).val());
        });

        function onTxtProblemCodeChanged(value) {
            var filterExpression = " ProblemCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetNursingProblemList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnProblemID.ClientID %>').val(result.ProblemID);
                    $('#<%=txtProblemName.ClientID %>').val(result.ProblemName);
                }
                else {
                    $('#<%=txtProblemCode.ClientID %>').val('');
                    $('#<%=hdnProblemID.ClientID %>').val('');
                    $('#<%=txtProblemName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onBeforeEditRecord(entity, errMessage) {
            if ($('#<%=hdnCurrentUserID.ClientID %>').val() == $('#<%=hdnCurrentSessionID.ClientID %>').val()) {
                return true;
            }
            else {
                errMessage.text = 'Maaf, tidak diijinkan mengedit Masalah Keperawatan user lain.';
                return false;
            }
        }

        function onBeforeDeleteRecord(entity, errMessage) {
            if ($('#<%=hdnCurrentUserID.ClientID %>').val() == $('#<%=hdnCurrentSessionID.ClientID %>').val()) {
                return true;
            }
            else {
                errMessage.text = 'Maaf, tidak diijinkan mengedit Masalah Keperawatan user lain.';
                return false;
            }
        }

        $(function () {
            $('#<%=btnQuickPicks.ClientID %>').click(function () {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/NursingProblem/NursingProblemQuickPicksCtl1.ascx");
                openUserControlPopup(url, '', 'Quick Picks : Masalah Keperawatan', 800, 400);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhQuickEntry" runat="server">
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">   
    <style type="text/css">
        .keyUser
        {
            display: none;
        }
    </style> 
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnIsCompleted" runat="server" />
    <input type="hidden" value="" id="hdnProblemID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedVisitID" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Perawat")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboParamedic" ClientInstanceName="cboParamedic" Width="400px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam ")%></label></td>
            <td><asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label id="lblProblemCode" class="lblMandatory lblLink"><%=GetLabel("Masalah")%></label></td>
            <td colspan="2">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td><asp:TextBox ID="txtProblemCode" Width="100px" runat="server" /></td>
                        <td><asp:TextBox ID="txtProblemName" Width="300px" Enabled="false" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("VisitID") %>" bindingfield="VisitID" />
                                        <input type="hidden" value="<%#:Eval("cfProblemDate") %>" bindingfield="cfProblemDate" />
                                        <input type="hidden" value="<%#:Eval("cfProblemDateDatePickerFormat") %>" bindingfield="cfProblemDateDatePickerFormat" />
                                        <input type="hidden" value="<%#:Eval("ProblemTime") %>" bindingfield="ProblemTime" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                        <input type="hidden" value="<%#:Eval("ProblemID") %>" bindingfield="ProblemID" />
                                        <input type="hidden" value="<%#:Eval("ProblemCode") %>" bindingfield="ProblemCode" />
                                        <input type="hidden" value="<%#:Eval("ProblemName") %>" bindingfield="ProblemName" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfProblemDate" HeaderText="Tanggal" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                <asp:BoundField DataField="ProblemTime" HeaderText="Jam" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ProblemName" HeaderText="Masalah Keperawatan"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ParamedicName" HeaderText="Perawat"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada masalah keperawatan untuk pasien ini")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>
