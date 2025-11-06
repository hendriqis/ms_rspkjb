<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostIHSVaccinationHistoryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PostIHSVaccinationHistoryCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_Referralctl">
    $(function () {
        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $('#<%=hdnSelectedID.ClientID %>').val($(this).closest('tr').find('.keyField').html());
                $(this).addClass('selected');
            }
        });
        $('#<%=grdView.ClientID %> tr:eq(1)').click();

        $('#<%=rblDisplayType.ClientID %> input').change(function () {
            if ($(this).val() == "0") {
                $('#<%=trRegistrationInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trRegistrationInfo.ClientID %>').attr("style", "display:none");
            }
            cbpView.PerformCallback();
        });
    });

    function onBeforeProcess(param) {
        if (!getSelectedItem()) {
            return false;
        }
        else
        {
            return true;
        }
    }

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnSelectedID.ClientID %>').val();
        alert(result);
        return result;
    }

    function onAfterProcessPopupEntry(param) {
    }

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
        }
        else {
            $cell.removeClass('highlight');
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
            displayErrorMessageBox("Integrasi SATUSEHAT", "Belum ada item yang dipilih untuk diproses!");
            return false;
        }
        else {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            return true;
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    //#region Registration No
    function getRegistrationNoFilterExpression() {
        var filterExpression = "<%:OnGetRegistrationNoFilterExpression() %>";
        return filterExpression;
    }

    $('#lblRegistrationNo.lblLink').live('click', function () {
        openSearchDialog('registration', getRegistrationNoFilterExpression(), function (value) {
            $('#<%:txtRegistrationNo.ClientID %>').val(value);
            onTxtRegistrationNoChanged(value);
        });
    });
    $('#<%:txtRegistrationNo.ClientID %>').live('change', function () {
        onTxtRegistrationNoChanged($(this).val());
    });
    function onTxtRegistrationNoChanged(value) {
        var filterExpression = getRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
        Methods.getObject('GetvRegistration3List', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                $('#<%=hdnVisitID.ClientID %>').val(result.VisitID);
                $('#<%=txtIHSEncounterID.ClientID %>').val(result.ExternalRegistrationNo);
            }
            else {
                $('#<%=hdnRegistrationID.ClientID %>').val('');
                $('#<%=hdnVisitID.ClientID %>').val('');
                $('#<%=txtRegistrationNo.ClientID %>').val('');
                $('#<%=txtIHSEncounterID.ClientID %>').val('');
            }
            cbpView.PerformCallback('refresh');
        });
    }
    //#endregion
</script>
<input type="hidden" id="hdnRegistrationID" runat="server" />
<input type="hidden" id="hdnVisitID" runat="server" />
<input type="hidden" id="hdnMRN" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />
<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table style="width:100%">
                    <colgroup>
                        <col width="115px" />
                        <col width="200px" />
                        <col width="125px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">Riwayat Vaksinasi</label>
                        </td>
                        <td colspan="3">
                            <asp:RadioButtonList ID="rblDisplayType" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Table">
                                <asp:ListItem Text=" Vaksinasi dilakukan pada saat kunjungan" Value="0" Selected="True" />
                                <asp:ListItem Text=" Vaksinasi dilakukan di luar Faskes (Dilaporkan)" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="trRegistrationInfo" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" id="lblRegistrationNo">
                                <%=GetLabel("No. Registrasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <td style="padding:10px">
                            <label>IHS Encounter ID</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIHSEncounterID" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlRujukan" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="VaccinationTypeName" HeaderText="Vaksinasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="SequenceNo" HeaderText="Vaksin Ke-" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="60px" />
                                        <asp:BoundField DataField="cfVaccinationDate" HeaderText="Tanggal Vaksin" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="cfVaccinationItemName" HeaderText="Jenis Vaksin" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfCVXGroup" HeaderText="CVX Group" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfCVXName" HeaderText="CVX Name" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="KFACode" HeaderText="Kode KFA" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="IHSReferenceID" HeaderText="IHS ID" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
