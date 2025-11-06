<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="PatientEducationList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientEducationList" %>

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
    <script type="text/javascript" id="dxss_erphysicalexamlist">
        $(function () {
            $('#<%=grdFormList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdFormList.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnEducationFormGroup.ClientID %>').val($(this).find('.keyField').html());
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

            //#region Signature
            $('.btnSignature').live('click', function () {
                var recordID = $(this).attr('recordID');
                var recordDate = $(this).attr('recordDate');
                var recordTime = $(this).attr('recordTime');
                var signatureName = $(this).attr('signatureName');
                var data = (recordID + "|" + signatureName + "|" + recordDate + "|" + recordTime + "|" + "1" + "|" + "02");
                var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl1.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 500);
            });

            $('.btnSignature2').live('click', function () {
                var recordID = $(this).attr('recordID');
                var recordDate = $(this).attr('recordDate');
                var recordTime = $(this).attr('recordTime');
                var signatureName = $(this).attr('signatureName');
                var data = (recordID + "|" + signatureName + "|" + recordDate + "|" + recordTime + "|" + "2" + "|" + "02");
                var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl1.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 500);
            });

            $('.btnSignature3').live('click', function () {
                var recordID = $(this).attr('recordID');
                var recordDate = $(this).attr('recordDate');
                var recordTime = $(this).attr('recordTime');
                var signatureName = $(this).attr('signatureName');
                var data = (recordID + "|" + signatureName + "|" + recordDate + "|" + recordTime + "|" + "3" + "|" + "02");
                var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl1.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 500);
            });

            $('.btnSignature4').live('click', function () {
                var recordID = $(this).attr('recordID');
                var recordDate = $(this).attr('recordDate');
                var recordTime = $(this).attr('recordTime');
                var signatureName = $(this).attr('signatureName');
                var data = (recordID + "|" + signatureName + "|" + recordDate + "|" + recordTime + "|" + "4" + "|" + "02");
                var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl1.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 500);
            });

            $('.lblParamedicName').live('click', function () {
                var recordID = $(this).attr('recordID');
                var recordDate = $(this).attr('recordDate');
                var recordTime = $(this).attr('recordTime');
                var signatureName = $(this).attr('signatureName');
                var signature1 = $(this).attr('signature1');
                var signature2 = $(this).attr('signature2');
                var signature3 = $(this).attr('signature3');
                var signature4 = $(this).attr('signature4');
                var signatureData = signature1 + "|" + signature2 + "|" + signature3 + "|" + signature4;
                var data = (recordID + "|" + signatureName + "|" + recordDate + "|" + recordTime + "|" + "1" + "|" + signatureData);
                var url = ResolveUrl("~/Libs/Controls/ViewDigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 450);
            });

            $('.lblSignatureName2').live('click', function () {
                var recordID = $(this).attr('recordID');
                var recordDate = $(this).attr('recordDate');
                var recordTime = $(this).attr('recordTime');
                var signatureName = $(this).attr('signatureName');
                var signature1 = $(this).attr('signature1');
                var signature2 = $(this).attr('signature2');
                var signature3 = $(this).attr('signature3');
                var signature4 = $(this).attr('signature4');
                var signatureData = signature1 + "|" + signature2 + "|" + signature3 + "|" + signature4;
                var data = (recordID + "|" + signatureName + "|" + recordDate + "|" + recordTime + "|" + "2" + "|" + signatureData);
                var url = ResolveUrl("~/Libs/Controls/ViewDigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 450);
            });

            $('.lblSignatureName3').live('click', function () {
                var recordID = $(this).attr('recordID');
                var recordDate = $(this).attr('recordDate');
                var recordTime = $(this).attr('recordTime');
                var signatureName = $(this).attr('signatureName');
                var signature1 = $(this).attr('signature1');
                var signature2 = $(this).attr('signature2');
                var signature3 = $(this).attr('signature3');
                var signature4 = $(this).attr('signature4');
                var signatureData = signature1 + "|" + signature2 + "|" + signature3 + "|" + signature4;
                var data = (recordID + "|" + signatureName + "|" + recordDate + "|" + recordTime + "|" + "3" + "|" + signatureData);
                var url = ResolveUrl("~/Libs/Controls/ViewDigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 450);
            });

            $('.lblSignatureName4').live('click', function () {
                var recordID = $(this).attr('recordID');
                var recordDate = $(this).attr('recordDate');
                var recordTime = $(this).attr('recordTime');
                var signatureName = $(this).attr('signatureName');
                var signature1 = $(this).attr('signature1');
                var signature2 = $(this).attr('signature2');
                var signature3 = $(this).attr('signature3');
                var signature4 = $(this).attr('signature4');
                var signatureData = signature1 + "|" + signature2 + "|" + signature3 + "|" + signature4;
                var data = (recordID + "|" + signatureName + "|" + recordDate + "|" + recordTime + "|" + "4" + "|" + signatureData);
                var url = ResolveUrl("~/Libs/Controls/ViewDigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 450);
            });
            //#endregion
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
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/PatientEducation/PatientEducationListCtl1.ascx");
                var param = $('#<%=hdnEducationFormGroup.ClientID %>').val() + "|" + recordID + "|" + hasSignature2;
                openUserControlPopup(url, param, "Edukasi Pasien", 750, 500);
            }
            else {
                displayErrorMessageBox('Edukasi Pasien', 'Maaf, tidak diijinkan mengedit record user lain.');
                return false;
            }
        });

        $('.imgDeleteRecord.imgLink').die('click');
        $('.imgDeleteRecord.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var name = $(this).attr('formType');

            var paramedicID = $(this).attr('paramedicID');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var message = "Hapus record edukasi pasien untuk " + name + " ?";
                displayConfirmationMessageBox("Edukasi Pasien", message, function (result) {
                    if (result) {
                        cbpDelete.PerformCallback(recordID);
                    }
                });
            }
            else {
                displayErrorMessageBox('Edukasi Pasien', 'Maaf, tidak diijinkan menghapus record user lain.');
                return false;
            }
        });

        $('.imgViewRecord.imgLink').die('click');
        $('.imgViewRecord.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/PatientEducation/ViewPatientEducationCtl1.ascx");
            var param = $('#<%=hdnEducationFormGroup.ClientID %>').val() + "|" + recordID;
            openUserControlPopup(url, param, "Edukasi Pasien", 750, 500);
        });

        $('.imgCopyRecord.imgLink').die('click');
        $('.imgCopyRecord.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/PatientEducation/CopyPatientEducationFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var visitID = $(this).attr('visitID');
            var param = recordID + "|" + visitID;
            openUserControlPopup(url, param, "Copy Edukasi Pasien", 700, 500);
        });

        function addRecord() {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {              
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/PatientEducation/PatientEducationListCtl1.ascx");
                var param = $('#<%=hdnEducationFormGroup.ClientID %>').val() +"|" + "0";
                openUserControlPopup(url, param, "Edukasi Pasien", 700, 500);
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
                displayErrorMessageBox('Edukasi Pasien', param[1]);
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnEducationFormGroup" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
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
                                                <asp:BoundField DataField="StandardCodeName" HeaderText="Topik Edukasi" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada jenis topik edukasi yang bisa digunakan")%>
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
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                <HeaderTemplate>
                                                    <img class="imgAdd imgLink" title='<%=GetLabel("+ Edukasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                        alt="" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEditRecord imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" recordID = "<%#:Eval("EducationFormID") %>" paramedicID = '<%#:Eval("ParamedicID") %>' hasSignature2 = '<%#:Eval("cfIsHasSignature2") %>'  />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDeleteRecord imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" recordID = "<%#:Eval("EducationFormID") %>" formType = "<%#:Eval("EducationFormType") %>" paramedicID = '<%#:Eval("ParamedicID") %>'   />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgViewRecord imgLink" title='<%=GetLabel("Lihat Form Edukasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                    alt="" recordID = "<%#:Eval("EducationFormID") %>" paramedicID = '<%#:Eval("ParamedicID") %>'  />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgCopyRecord imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                    alt="" recordID = "<%#:Eval("EducationFormID") %>" formGroup="<%#:Eval("GCEducationFormGroup") %>" formType="<%#:Eval("GCEducationFormType") %>" visitID = "<%#:Eval("VisitID") %>" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="EducationFormID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="EducationFormID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Tanggal") %>
                                                        <br />
                                                        <%=GetLabel("Jam") %>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("cfEducationFormDate") %>
                                                        <br />
                                                        <%#:Eval("EducationFormTime") %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penerima Informasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <div>
                                                        <b><label id="lblSignatureName2" class='<%# Eval("cfIsHasSignature2").ToString() == "True" ? "lblLink lblSignatureName2": "lblNormal" %>'
                                                        recordID = '<%#:Eval("EducationFormID") %>' recordDate = '<%#:Eval("cfEducationFormDate") %>' recordTime = '<%#:Eval("EducationFormTime") %>' signatureName = '<%#:Eval("SignatureName2") %>' signature2 = '<%#:Eval("SignatureID2Type1") %>'>
                                                            <%#:Eval("SignatureName2") %></label></b>                                            
                                                    </div>
                                                    <div id="divSignature2" runat="server" style='margin-top: 5px; text-align: left'>
                                                        <input type="button" id="btnSignature2" runat="server" class="btnSignature2" value="Ttd" title="Tanda Tangan" recordID = '<%#:Eval("EducationFormID") %>' recordDate = '<%#:Eval("cfEducationFormDate") %>' recordTime = '<%#:Eval("EducationFormTime") %>' signatureName = '<%#:Eval("SignatureName2") %>'
                                                        style='<%# Eval("cfIsHasSignature2").ToString() == "True" ? "display:none;": "height: 25px; width: 60px; background-color: Red; color: White;" %>'  />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saksi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <div>
                                                        <b><label id="lblSignatureName3" class='<%# Eval("cfIsHasSignature3").ToString() == "True" ? "lblLink lblSignatureName3": "lblNormal" %>'
                                                        recordID = '<%#:Eval("EducationFormID") %>' recordDate = '<%#:Eval("cfEducationFormDate") %>' recordTime = '<%#:Eval("EducationFormTime") %>' signatureName = '<%#:Eval("SignatureName3") %>' signature3 = '<%#:Eval("SignatureID3Type1") %>'>
                                                            <%#:Eval("SignatureName3") %></label></b>                                            
                                                    </div>
                                                    <div id="divSignature3" runat="server" style='margin-top: 5px; text-align: left'>
                                                        <input type="button" id="btnSignature3" runat="server" class="btnSignature3" value="Ttd" title="Tanda Tangan" recordID = '<%#:Eval("EducationFormID") %>' recordDate = '<%#:Eval("cfEducationFormDate") %>' recordTime = '<%#:Eval("EducationFormTime") %>' signatureName = '<%#:Eval("SignatureName3") %>'
                                                        style='<%# Eval("cfIsHasSignature3").ToString() == "True" ? "display:none;" : "height: 25px; width: 60px; background-color: Red; color: White;" %>'  />
                                                    </div>                                                    
                                                    <div>
                                                        <b><label id="lblSignatureName4" class='<%# Eval("cfIsHasSignature4").ToString() == "True" ? "lblLink lblSignatureName4": "lblNormal" %>'
                                                        recordID = '<%#:Eval("EducationFormID") %>' recordDate = '<%#:Eval("cfEducationFormDate") %>' recordTime = '<%#:Eval("EducationFormTime") %>' signatureName = '<%#:Eval("SignatureName4") %>' signature4 = '<%#:Eval("SignatureID4Type1") %>'>
                                                            <%#:Eval("SignatureName4") %></label></b>                                            
                                                    </div>
                                                    <div id="divSignature4" runat="server" style='margin-top: 5px; text-align: left'>
                                                        <input type="button" id="btnSignature4" runat="server" class="btnSignature4" value="Ttd" title="Tanda Tangan" recordID = '<%#:Eval("EducationFormID") %>' recordDate = '<%#:Eval("cfEducationFormDate") %>' recordTime = '<%#:Eval("EducationFormTime") %>' signatureName = '<%#:Eval("SignatureName4") %>'
                                                        style='<%# Eval("cfIsHasSignature4").ToString() == "True" ? "display:none;": "height: 25px; width: 60px; background-color: Red; color: White;" %>' />
                                                    </div>  
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="EducationFormType" HeaderText="Form Edukasi" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="EducationMethod" HeaderText="Metode Edukasi" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="EducationMaterial" HeaderText="Material Edukasi" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Edukator" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <div>
                                                        <b><label id="lblParamedicName" class='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "lblLink lblParamedicName": "lblNormal" %>'
                                                        recordID = '<%#:Eval("EducationFormID") %>' recordDate = '<%#:Eval("cfEducationFormDate") %>' recordTime = '<%#:Eval("EducationFormTime") %>' signatureName = '<%#:Eval("ParamedicName") %>' signature1 = '<%#:Eval("SignatureID1Type1") %>'>
                                                            <%#:Eval("ParamedicName") %></label></b>                                            
                                                    </div>
                                                    <div id="divParamedicSignature" runat="server" style='margin-top: 5px; text-align: left'>
                                                        <input type="button" id="btnSignature" runat="server" class="btnSignature" value="Ttd" title="Tanda Tangan" recordID = '<%#:Eval("EducationFormID") %>' recordDate = '<%#:Eval("cfEducationFormDate") %>' recordTime = '<%#:Eval("EducationFormTime") %>' signatureName = '<%#:Eval("ParamedicName") %>'
                                                        style='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "display:none;": "height: 25px; width: 60px; background-color: Red; color: White;" %>'  />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="EducationEvaluation" HeaderText="Evaluasi" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada dokumentasi pemberian edukasi pasien") %>
                                            <br />
                                            <span class="lblLink" id="lblAddRecord">
                                                <%= GetLabel("+ Edukasi Pasien")%></span>
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
