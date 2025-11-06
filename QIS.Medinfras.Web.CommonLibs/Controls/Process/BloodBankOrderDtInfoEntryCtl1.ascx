<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BloodBankOrderDtInfoEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.BloodBankOrderDtInfoEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_BloodBagEntryCtl">
    setDatePicker('<%=txtExpiredDate.ClientID %>');
    $('#<%=txtExpiredDate.ClientID %>').datepicker('option', 'minDate', '0');

    $('#lblPopupAddData').die('click');
    $('#lblPopupAddData').live('click', function () {
        $('#<%=hdnIDCtl.ClientID %>').val('');
        $('#<%=txtExpiredDate.ClientID %>').val('');
        cboItemID.SetSelectedIndex(0);
        cboItemID.SetEnabled(false);
        $('#containerEntryDataCtl').show();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        var message = "Hapus detail informasi kantong darah ?";
        $('#<%=hdnIDCtl.ClientID %>').val($(this).attr('recordID'));
        displayConfirmationMessageBox('DATA KANTONG DARAH :', message, function (result) {
            if (result) {
                if ($('#<%=hdnIDCtl.ClientID %>').val() != '') {
                    cbpView.PerformCallback('delete');
                }
                else {
                    displayErrorMessageBox('DATA KANTONG DARAH', 'Invalid record ID');
                }
            }
        });
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        var recordID = $(this).attr('recordID');
        var labelNo = $(this).attr('labelNo');
        var gcPackingQuality = $(this).attr('gcPackingQuality');
        var gcBloodQuality = $(this).attr('gcBloodQuality');
        var expiredDate = $(this).attr('expiredDate');
        $('#<%=txtBloodBagNo.ClientID %>').val(labelNo);
        $('#<%=hdnIDCtl.ClientID %>').val(recordID);
        cboPackingQuality.SetValue(gcPackingQuality);
        cboBloodQuality.SetValue(gcBloodQuality);
        $('#<%=txtExpiredDate.ClientID %>').val(expiredDate);
        cboItemID.SetEnabled(false);
        $('#containerEntryDataCtl').show();
    });

    $('#btnPopupCancel').die('click');
    $('#btnPopupCancel').live('click', function () {
        $('#containerEntryDataCtl').hide();
        cboItemID.SetEnabled(true);
    });

    function onCbpViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                displayErrorMessageBox('DATA KANTONG DARAH', 'Error Message : ' + param[2]);
                cbpView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerEntryDataCtl').hide();
            }
        }
        else if (param[0] == 'delete') {
        if (param[1] == 'fail') {
            displayErrorMessageBox('DATA KANTONG DARAH', 'Error Message : ' + param[2]);
            cbpView.PerformCallback('refresh');
        }
        else {
            var pageCount = parseInt(param[2]);
            setPagingDetailItem(pageCount);
        }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    $('#btnPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntry', 'mpEntry'))
            cbpView.PerformCallback('save');
        return false;
    });

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function oncboItemIDValueChanged() {
        onRefreshGridView();
    }

    function onRefreshGridView() {
        cbpView.PerformCallback('refresh');
    }

</script>    

<input type="hidden" runat="server" id="hdnVisitID" value="" />
<input type="hidden" runat="server" id="hdnTestOrderID" value="" />

<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div id="containerEntryDataCtl" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnIDCtl" runat="server" value="" />
                    <fieldset id="fsEntry" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col width="180px" />
                                <col width="150px" />
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("No. Kantong Darah")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBloodBagNo" Width="150px" runat="server" />
                                </td>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Expired Date")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExpiredDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Kualitas Kantong")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboPackingQuality" ClientInstanceName="cboPackingQuality"
                                        Width="150px">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Kualitas Darah")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboBloodQuality" ClientInstanceName="cboBloodQuality"
                                        Width="150px">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align:center">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnPopupSave" value='<%= GetLabel("Simpan")%>' class="btnPopup w3-btn w3-hover-blue" style="width:80px" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPopupCancel" value='<%= GetLabel("Batal")%>' class="btnPopup w3-btn w3-hover-red" style="width:80px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            <table>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Jenis Darah")%>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboItemID" ClientInstanceName="cboItemID"
                                Width="220%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { oncboItemIDValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <img class="imgLink imgEdit" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-right: 2px;" recordID = "<%#:Eval("ID") %>" testOrderDetailID = "<%#:Eval("TestOrderDtID") %>" labelNo = "<%#:Eval("LabelNo") %>"
                                                        expiredDate =  "<%#:Eval("cfExpiredDate") %>" gcPackingQuality =  "<%#:Eval("GCPackingQuality") %>" gcBloodQuality = "<%#:Eval("GCBloodQuality") %>"/>
                                                    <img class="imgLink imgDelete" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" recordID = "<%#:Eval("ID") %>" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="50px"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("No. Kantong Darah")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("LabelNo")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Kualitas Kantong")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("PackingQuality")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Kualitas Darah")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("BloodQuality")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Expired Date")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("cfExpiredDate")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblPopupAddData">
                        <%= GetLabel("Tambah Data Kantong Darah")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
