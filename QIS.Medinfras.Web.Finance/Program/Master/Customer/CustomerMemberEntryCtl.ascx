<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerMemberEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.CustomerMemberEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_customermemberentryctl">
    $(function () {
        addItemFilterRow();
    });

    function addItemFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td></tr>");

        $input = $("<input type='text' id='txtQuickSearch' style='width:100%;height:20px' />").val($('#<%=hdnFilterParam.ClientID %>').val());
        $trFilter.find('td').eq(1).append($input);
        $trFilter.insertAfter($trHeader);
    }

    $('#txtQuickSearch').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            e.preventDefault();
            $('#<%=hdnFilterParam.ClientID %>').val($('#txtQuickSearch').val());
            cbpEntryPopupView.PerformCallback('refresh');
        }
    });

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnMemberID.ClientID %>').val('');
        $('#<%=txtMedicalNo.ClientID %>').val('');
        $('#<%=txtPatientName.ClientID %>').val('');
        $('#<%=txtMemberNo.ClientID %>').val('');

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        //if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
        cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnMemberID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#lblEntryPopupAddData').click();
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
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
            $('.grdPopup tr:eq(1)').click();
        }

        $('#containerPopupEntryData').hide();
        hideLoadingPanel();
        addItemFilterRow();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion

    //#region Member
    function onGetCustomerMemberFilterExpression() {
        var businessPartnerID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
        var filterExpression = "MRN NOT IN (SELECT MRN FROM CustomerMember WHERE BusinessPartnerID = " + businessPartnerID + ") AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblMember.lblLink').live('click', function () {
        openSearchDialog('patient', onGetCustomerMemberFilterExpression(), function (value) {
            $('#<%=txtMedicalNo.ClientID %>').val(value);
            onTxtCustomerMemberCodeChanged(value);
        });
    });

    $('#<%=txtMedicalNo.ClientID %>').live('change', function () {
        onTxtCustomerMemberCodeChanged($(this).val());
    });

    function onTxtCustomerMemberCodeChanged(value) {
        var filterExpression = onGetCustomerMemberFilterExpression() + " AND MedicalNo = '" + value + "'";
        Methods.getObject('GetvPatientList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnMemberID.ClientID %>').val(result.MRN);
                $('#<%=txtMedicalNo.ClientID %>').val(result.MedicalNo);
                $('#<%=txtPatientName.ClientID %>').val(result.PatientName);
            }
            else {
                $('#<%=hdnMemberID.ClientID %>').val('');
                $('#<%=txtMedicalNo.ClientID %>').val('');
                $('#<%=txtPatientName.ClientID %>').val('');
            }
        });
    }
    //#endregion 
</script>
<div style="height:440px; overflow-y:auto; overflow-x: hidden;">
    <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
    <input type="hidden" id="hdnFilterParam" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Anggota")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%" />
        </colgroup>
        <tr>
            <td style="padding:5px; vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Instansi")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtCustomerName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px; display:none;">
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0">
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:80px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblMember"><%=GetLabel("Anggota")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnMemberID" runat="server" value="" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:100px" />
                                            <col style="width:3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtMedicalNo" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtPatientName" ReadOnly="true" Width="80%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal" id="lblMemberNo"><%=GetLabel("No Peserta")%></label></td>
                                <td><asp:TextBox ID="txtMemberNo" Width="50%" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%=GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%=GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){$('#containerImgLoadingViewPopup').show();}"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s);}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                            <input type="hidden" class="hdnID" value="<%#: Eval("MRN")%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PatientName" ItemStyle-CssClass="tdCustomerPatientName" HeaderText="Nama Pasien" />
                                    <asp:BoundField DataField="MemberNo" ItemStyle-CssClass="tdCustomerMemberNo" HeaderText="No Member" HeaderStyle-Width="150px" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Data Tidak Tersedia")%>
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
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>