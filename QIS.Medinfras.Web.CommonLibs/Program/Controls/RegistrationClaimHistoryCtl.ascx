<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationClaimHistoryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationClaimHistoryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_RegistrationClaimHistoryCtl">

    $('.txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    //#region INACBGMaster
    $('.lblINACBGMaster.lblLink').live('click', function () {
        $td = $(this).parent();
        $tr = $(this).closest('tr');
        var classID = $('#<%=hdnClassChargesID.ClientID %>').val();
        var filterExpression = "IsDeleted = 0 AND ClassID='" + classID + "'";
        openSearchDialog('inacbgmaster', filterExpression, function (value) {
            onINACBGMasterSelected(value);
        });
    });

    function onINACBGMasterSelected(value) {
        var filterExpression = "IsDeleted = 0 AND ID = '" + value + "'";
        Methods.getObject('GetINACBGMasterList', filterExpression, function (result) {
            if (result != null) {
                $td.find('.txtGrouperCodeClaim').val(result.GrouperCode);
                $td.find('.txtGrouperTypeClaim').val(result.GrouperDescription);
                $tr.find('.txtCoverageAmount').val(result.GrouperTariff).trigger('changeValue');
            }
            else {
                $td.find('.txtGrouperCodeClaim').val("");
                $td.find('.txtGrouperTypeClaim').val("");
                $td.find('.txtCoverageAmount').val("0").trigger('changeValue');
            }
        });
    }
    //#endregion

    $('.txtCoverageAmount').live('change', function () {
        $tr = $(this).closest('tr');
        var realCostAmount = parseFloat($tr.find('.hdnRealCostAmount').val());
        var coverageAmount = parseFloat($tr.find('.txtCoverageAmount').val());
        var patientAmount = parseFloat($tr.find('.txtPatientAmount').val());
        var differenceAmount = realCostAmount - coverageAmount - patientAmount;

        $tr.find('.txtDifferenceAmount').val(differenceAmount).trigger('changeValue');
    });

    $('.txtPatientAmount').live('change', function () {
        $tr = $(this).closest('tr');
        var realCostAmount = parseFloat($tr.find('.hdnRealCostAmount').val());
        var coverageAmount = parseFloat($tr.find('.txtCoverageAmount').val());
        var patientAmount = parseFloat($tr.find('.txtPatientAmount').val());
        var differenceAmount = realCostAmount - coverageAmount - patientAmount;

        $tr.find('.txtDifferenceAmount').val(differenceAmount).trigger('changeValue');
    });

    $('.btnSave').live('click', function () {
        $tr = $(this).closest('tr');
        var historyID = $tr.find('.keyField').html();
        if (historyID == 0) {
            var grouperCodeClaim = ($tr.find('.txtGrouperCodeClaim').val());
            var grouperTypeClaim = ($tr.find('.txtGrouperTypeClaim').val());
            var realCostAmount = parseFloat($tr.find('.hdnRealCostAmount').val().replace(/,/g, ''));
            var coverageAmount = parseFloat($tr.find('.txtCoverageAmount').val().replace(/,/g, ''));
            var occupiedAmount = parseFloat($tr.find('.txtOccupiedAmount').val().replace(/,/g, ''));
            var patientAmount = parseFloat("0");
            var differenceAmount = parseFloat(realCostAmount - coverageAmount - patientAmount);

            var param = 'save|' + historyID + '|' + grouperCodeClaim + '|' + grouperTypeClaim + '|' + realCostAmount + '|' + coverageAmount + '|' + occupiedAmount + '|' + patientAmount + '|' + differenceAmount;

            cbpViewDt.PerformCallback(param);
        }
    });

    //#region Paging Dt
    function onCbpViewDtEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDt"), pageCount, function (page) {
                cbpViewDt.PerformCallback('changepage|' + page);
            });
        }
        else if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }

            cbpViewDt.PerformCallback('refresh');
        }
        else {
            $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
    }
    //#endregion

</script>
<div>
    <input type="hidden" value="" runat="server" id="hdnVisitIDRCHCtl" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationIDRCHCtl" />
    <input type="hidden" value="0" id="hdnID" runat="server" />
    <input type="hidden" value="0" id="hdnClassChargesID" runat="server" />
     <table class="tblContentArea">
        <colgroup>
            <col style="width: 100px" />
            <col style="width: 300px" />
            <col />
            <col style="width: 40%" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNo" Width="180px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. SEP")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtNoSEP" Width="180px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Kelas Tagihan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtChargesClassName" Width="180px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatientInfo" Width="100%" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td colspan="3" style="vertical-align: top; width: 100%">
                <div style="position: relative; height:400px; overflow-y:auto">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        OnCallback="cbpViewDt_Callback" ShowLoadingPanel="false">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridProcessList">
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                        OnRowDataBound="grdViewDt_RowDataBound" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false"
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="HistoryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Info Log" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div style="font-size: small; font-style: italic">
                                                        <%#:Eval("CodingByName") %></div>
                                                    <div style="font-size: x-small">
                                                        <%#:Eval("cfCodingDateInString") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="250px" HeaderText="Info Grouper" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblINACBGMaster" class="lblLink lblINACBGMaster">
                                                        <%=GetLabel("Pilih Master INACBG")%></label>
                                                    <input type="text" runat="server" value='<%#:Eval("GrouperCodeClaim") %>' id="txtGrouperCodeClaim"
                                                        class="txtGrouperCodeClaim" style="width: 100%" placeholder="GrouperCodeClaim" />
                                                    <input type="text" runat="server" value='<%#:Eval("GrouperTypeClaim") %>' id="txtGrouperTypeClaim"
                                                        class="txtGrouperTypeClaim" style="width: 100%" placeholder="GrouperDescriptionClaim" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Nilai") %>
                                                    <br />
                                                    <%=GetLabel("Transaksi") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="hidden" class="hdnItemIndex" value='<%#: Container.DataItemIndex %>' />
                                                    <input type="hidden" class="hdnRealCostAmount" value='<%#:Eval("RealCostAmount") %>' />
                                                    <input type="text" runat="server" value='<%#:Eval("cfRealCostAmountInString") %>'
                                                        id="txtRealCostAmount" class="txtRealCostAmount txtCurrency" style="width: 100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                <HeaderTemplate>
                                                    <%=GetLabel("INACBG") %>
                                                    <br />
                                                    <%=GetLabel("Hak Pasien") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="text" runat="server" value='<%#:Eval("cfCoverageAmountInString") %>'
                                                        id="txtCoverageAmount" class="txtCoverageAmount txtCurrency" style="width: 100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                <HeaderTemplate>
                                                    <%=GetLabel("INACBG") %>
                                                    <br />
                                                    <%=GetLabel("Ditempati") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="text" runat="server" value='<%#:Eval("cfOccupiedAmountInString") %>'
                                                        id="txtOccupiedAmount" class="txtOccupiedAmount txtCurrency" style="width: 100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                Visible="false">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Nilai") %>
                                                    <br />
                                                    <%=GetLabel("Dibayar Pasien") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="text" runat="server" value='<%#:Eval("cfPatientAmountInString") %>'
                                                        id="txtPatientAmount" class="txtPatientAmount txtCurrency" style="width: 100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                Visible="false">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Nilai") %>
                                                    <br />
                                                    <%=GetLabel("Selisih") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="text" runat="server" value='<%#:Eval("cfCountDifferenceInString") %>'
                                                        id="txtDifferenceAmount" class="txtDifferenceAmount txtCurrency" style="width: 100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="button" id="btnSave" class="btnSave w3-button w3-blue" value="Simpan"
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No data to display.")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
