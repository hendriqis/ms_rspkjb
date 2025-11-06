<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="ReferralLetterFormList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ReferralLetterFormList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomLeftButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_ReferralLetterFormList">
        $(function () {
            $('#<%=grdFormList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdFormList.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnConsentFormGroup.ClientID %>').val($(this).find('.keyField').html());
                cbpView.PerformCallback('refresh');
                $('#<%=hdnID.ClientID %>').val('')
            });
            $('#<%=grdFormList.ClientID %> tr:eq(1)').click();

            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=btnRefresh.ClientID %>').click(function () {
                onRefreshControl();
            });
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging Header
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#pagingHd"), pageCount, function (page) {
                cbpFormList.PerformCallback('changepage|' + page);
            });
        });
        function onCbpFormListEndCallback(s) {
            $('#containerHdImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdFormList.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingHd"), pageCount, function (page) {
                    cbpFormList.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdFormList.ClientID %> tr:eq(1)').click();
        }
        //#endregion

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
            else if (param[0] == 'view') {
                var base64 = param[1];
                let pdfWindow = window.open("");
                pdfWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64, " + base64 + "'></iframe>");
                cbpView.PerformCallback('refresh');
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Detail Grid Button
        $('.imgAdd.imgLink').die('click');
        $('.imgAdd.imgLink').live('click', function (evt) {
            addRecord();
        });

        $('#lblAddRecord').die('click');
        $('#lblAddRecord').live('click', function () {
            addRecord();
        });

        $('.imgEditRecord.imgLink').die('click');
        $('.imgEditRecord.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var hasSignature2 = $(this).attr('hasSignature2');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/ReferralLetter/ReferralLetterFormCtl.ascx");
                var param = $('#<%=hdnConsentFormGroup.ClientID %>').val() + "|" + recordID + "|" + hasSignature2;
                openUserControlPopup(url, param, "Surat Keterangan", 750, 500);
            }
            else {
                displayErrorMessageBox('Surat Keterangan', 'Maaf, tidak diijinkan mengedit record user lain.');
                return false;
            }
        });

        $('.imgDeleteRecord.imgLink').die('click');
        $('.imgDeleteRecord.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var name = $(this).attr('formType');

            var paramedicID = $(this).attr('paramedicID');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var message = "Hapus record surat keterangan untuk " + name + " ?";
                displayConfirmationMessageBox("Surat Keerangan", message, function (result) {
                    if (result) {
                        cbpDelete.PerformCallback(recordID);
                    }
                });
            }
            else {
                displayErrorMessageBox('Surat Keterangan', 'Maaf, tidak diijinkan menghapus record user lain.');
                return false;
            }
        });

        $('.imgViewRecord.imgLink').die('click');
        $('.imgViewRecord.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/ReferralLetter/ViewReferralLetterFormCtl.ascx");
            var param = $('#<%=hdnConsentFormGroup.ClientID %>').val() + "|" + recordID;
            openUserControlPopup(url, param, "Surat Keterangan", 750, 500);
        });

        function addRecord() {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/ReferralLetter/ReferralLetterFormCtl.ascx");
                var param = $('#<%=hdnConsentFormGroup.ClientID %>').val() +"|" + "0";
                openUserControlPopup(url, param, "Surat Keterangan", 700, 500);
            }
        }
        //#endregion

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onCbpDeleteEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Surat Keterangan', param[1]);
            }
        }

        $('.lnkView').live('click', function () {
            var id = $(this).attr('recordID');
            $('#<%=hdnIDForView.ClientID %>').val(id);
            cbpView.PerformCallback('view');
        });
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnConsentFormGroup" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnIDForView" value="" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <div style="position: relative;">
         <table style="width:100%">
            <colgroup>
                <col style="width:20%"/>
                <col style="width:80%"/>
            </colgroup>
            <tr>
                <td style="vertical-align:top">
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpFormList" runat="server" Width="100%" ClientInstanceName="cbpFormList"
                            ShowLoadingPanel="false" OnCallback="cbpFormList_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerHdImgLoadingView').show(); }"
                                EndCallback="function(s,e){ onCbpFormListEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="panFormList" CssClass="pnlContainerGridPatientPage">
                                        <asp:GridView ID="grdFormList" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                                            EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="StandardCodeName" HeaderText="Jenis Surat Keterangan" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada jenis surat keterangan yang bisa digunakan")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>    
                        <div class="imgLoadingGrdView" id="containerHdImgLoadingView" >
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="pagingHd"></div>
                            </div>
                        </div> 
                    </div>
                </td>
                <td style="vertical-align:top">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound" >
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                                <HeaderTemplate>
                                                    <img class="imgAdd imgLink" title='<%=GetLabel("+ Surat Keterangan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                        alt="" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEditRecord imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" paramedicID = '<%#:Eval("ParamedicID") %>'  />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDeleteRecord imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" formType = "<%#:Eval("StandardCodeName") %>" paramedicID = '<%#:Eval("ParamedicID") %>'   />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Tanggal") %>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("cfLetterDate") %> <%#:Eval("LetterTime") %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PrintNumber" HeaderText="Jumlah Cetak" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Terakhir dicetak") %>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("LastPrintedByName") %> <br>
                                                        <%#:Eval("cfLastPrintedDate") %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <div>
                                                        <a class="lnkView" recordid="<%#:Eval("ID") %>"> View</a>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada dokumentasi pemberian surat rujukan") %>
                                            <br />
                                            <span class="lblLink" id="lblAddRecord">
                                                <%= GetLabel("+ Surat Rujukan")%></span>
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
                </td>
            </tr>
        </table>

    </div>
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpDelete" runat="server" Width="100%" ClientInstanceName="cbpDelete"
            ShowLoadingPanel="false" OnCallback="cbpDelete_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
