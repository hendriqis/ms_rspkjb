<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ROSLookupCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ROSLookupCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_testorderdetail1">
    $(function () {
        setDatePicker('<%=txtObservationDate.ClientID %>');
        $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    function onBeforeProcess(param) {
        if (!getSelectedItem()) {
            return false;
        }
        else {
            return true;
        }
    }

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('gridCheckBoxSelectedItem');
        }
        else {
            $cell.removeClass('gridCheckBoxSelectedItem');
        }
    });

    function getSelectedItem() {
        var tempSelectedID = "";
        var count = 0;
        $('.grdView .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
            }
            tempSelectedID += id;
            count += 1;
        });
        if (count == 0) {
            showToast("Tanda Vital & Indikator Lainnya", "Belum ada record histori pemeriksaan yang dipilih !");
            return false;
        }
        else if (count > 1) {
            showToast("Tanda Vital & Indikator Lainnya", "Hanya 1 (satu) record histori pemeriksaan yang dapat dipilih !");
            return false;
        }
        else {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            return true;
        }
    }

    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
    });


    function onCbpViewEndCallback(s) {
        var param = s.cpResult.split('|');

        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            cbpView.PerformCallback('refresh');
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        $('#containerImgLoadingViewPopup').hide();
    }

    function onAfterProcessPopupEntry(param) {
        if (typeof onRefreshROSGrid == 'function')
            onRefreshROSGrid();
    }
</script>
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnSelectedID" runat="server" value="" />
<input type="hidden" id="hdnIsNewRecord" runat="server" value="1" />
<input type="hidden" id="hdnVisitNoteID" runat="server" value="0" />
<input type="hidden" id="hdnIsInitialAssessment" runat="server" value="0" />
<input type="hidden" runat="server" id="hdnLinkMedicalResumeID" value="0" />
<input type="hidden" runat="server" id="hdnLinkPreSurgeryAssessmentID" value="0" />
<input type="hidden" runat="server" id="hdnLinkChiefComplaintID" value="0" />

    <div>
        <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Date")%>
                        -
                        <%=GetLabel("Time")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                        Style="text-align: center" />
                </td>
            </tr>
        </table>
    </div>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server">
                    <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 400px">
                        <asp:GridView ID="grdView" runat="server"  CssClass="grdView"
                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                            OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false"  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="text-align: left">
                                            <%= GetLabel("Daftar Pemeriksaan")%>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="text-decoration:underline">
                                            <b>
                                                <%#: Eval("ObservationDateInString")%>,
                                                <%#: Eval("ObservationTime") %>,
                                                <%#: Eval("ParamedicName") %>
                                            </b>
                                        </div>
                                        <div>
                                            <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                                    <ItemTemplate>
                                                        <div style="padding-left: 20px; float: left; width: 300px;">
                                                            <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                <strong>
                                                                    <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                                                    : </strong></span>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                        <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%></span>
                                                        </div>
                                                    </ItemTemplate>
                                                <FooterTemplate>
                                                    <br style="clear: both" />
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>

