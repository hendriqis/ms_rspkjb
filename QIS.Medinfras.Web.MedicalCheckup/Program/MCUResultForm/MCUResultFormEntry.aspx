<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="MCUResultFormEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.MCUResultFormEntry" %>

<%@ Register Src="~/Program/MCUPatientToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%--<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomLeftButtonToolbar" runat="server">
    <li id="btnBackToList" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>--%>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
  <%--  <li id="btnPrint" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
            <%=GetLabel("Print PDF")%></div>
    </li>--%>
  <%--  <li runat="server" crudmode="R" id="btnPrintExcel">
        <asp:Button ID="btnPrintToExcel" Text="Submit" OnClick="PrintToExcelButton_Click"
            runat="server" CssClass="hidden" />
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" />
        <div>
            <%=GetLabel("Print XLS")%></div>
    </li>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
   <%--      $('#<%=btnBackToList.ClientID %>').live('click', function () {
            showLoadingPanel();
            document.location = ResolveUrl("~/Program/PatientList/VisitList.aspx?id=resultform");
        });
  
//        $('#<%=btnPrint.ClientID %>').live('click', function () {
//            if ($('#<%=hdnID.ClientID %>').val() != "" || $('#<%=hdnID.ClientID %>').val() != null) {
//                cbpView.PerformCallback('printout');
//            } else {
//                displayMessageBox('MEDINFRAS', "Belum ada hasil MCU Form yang dapat dicetak.");
//            }
//        });

//        $('#<%=btnPrintExcel.ClientID %>').live('click', function () {
//            if ($('#<%=hdnID.ClientID %>').val() != "" || $('#<%=hdnID.ClientID %>').val() != null) {
//                var button = document.getElementById('<%=btnPrintToExcel.ClientID%>');
//                button.click();
//            } else {
//                displayMessageBox('MEDINFRAS', "Belum ada hasil MCU Form yang dapat dicetak.");
//            }
//        });
--%>
        $(function () {
            $('#<%=grdFormList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdFormList.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnGCResultType.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnResultType.ClientID %>').val($(this).find('.StandardCodeName').html());
                cbpView.PerformCallback('refresh');
            });
            $('#<%=grdFormList.ClientID %> tr:eq(1)').click();

            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnFormLayout.ClientID %>').val($(this).find('.FormLayout').html());
                $('#<%=hdnFormValue.ClientID %>').val($(this).find('.FormValue').html());
                $('#<%=hdnRemarks.ClientID %>').val($(this).find('.Remarks').html());
            });
        });

        $('.lnkView a').live('click', function () {
            var gcResultType = $('#<%=hdnGCResultType.ClientID %>').val();
            var resultType = $('#<%=hdnResultType.ClientID %>').val();
            var id = $(this).closest('tr').find('.keyField').html();
            var remarks = $('#<%=hdnRemarks.ClientID %>').val();
            var param = gcResultType + '|' + resultType + '|' + id + '|' + remarks;
            var url = ResolveUrl("~/Program/MCUResultForm/MCUResultFormViewDetailCtl.ascx");
            openUserControlPopup(url, param, 'Hasil MCU Form Detail', 1600, 800);
        });

        function onBeforeBasePatientPageListEdit() {
            return true;
        }

        function onBeforeBasePatientPageListDelete() {
            return true;
        }

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
            else if (param[0] == 'printout') {
                var FileString = $('#<%:hdnFileString.ClientID %>').val();
                if (FileString != "") {
                    //window.open("data:application/pdf;base64, " + FileString);
                    var random = Math.random().toString(36).substring(7);
                    var css = '<%=ResolveUrl("~/Libs/Styles/PrintLayout/paper.css")%>' + "?" + random;
                    var newWin = open('url', 'windowName', 'scrollbars=1,resizable=1,width=1000,height=580,left=0,top=0');
                    newWin.document.write('<html><head><title>MCU Rekap</title><link rel="stylesheet" type="text/css" href="' + css + '"> </head><style type="text/css" media="print"> .noPrint{display:none;} </style>');
                    var html = '<style>@page { size: A4 landscape }</style>';
                    html = '<body class="A4 landscape"> <div style="margin-left:20px;" class="noPrint"><input type="button" value="Print Halaman" onclick="javascript:window.print()" /></div>' + FileString + ' </body>';
                    newWin.document.write(html);
                    newWin.document.close();
                    newWin.focus();
                    newWin.print();
                }
                $('#<%:hdnFileString.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var hdnID = $('#<%=hdnID.ClientID %>').val();
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            var RegistrationID =  $('#<%=hdnRegistrationID.ClientID %>').val();
            if (hdnID == '') {
                errMessage.text = 'Harap isi form terlebih dahulu !';
                return false;
            }
            else {
                if(code == 'MC-00018'){
                     filterExpression.text = "RegistrationID =" + RegistrationID;
                        return true;
                }
               if (code == 'PM-00191' || code == 'MC-00014' || code == 'MC-00015' || code=='MC-00020' || code=='MC-00021') {
                    filterExpression.text = 'VisitID = ' + visitID;
                    return true;
                }
                 if (code == 'PM-90021' || code == 'PM-90034' || code == 'PM-90036' || code == 'MC-00009' || code == 'MC-00019' || code == 'PM-90070' || code == 'PM-90129' || code == 'PM-90038') {
                    filterExpression.text = 'VisitID = ' + visitID;
                    return true;
                }
                else {
                    filterExpression.text = visitID;
                    return true;
                }
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'paramedicTeam' || code == 'keteranganSehatperkiraanLahir') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else if (code == 'registrationEdit') {
                var regID = $('#<%:hdnRegistrationID.ClientID %>').val();
                var regDate = "";
                var regTime = "";
                Methods.getObject("GetvRegistrationList", "RegistrationID = " + regID, function (resultReg) {
                    if (resultReg != null) {
                        regDate = resultReg.RegistrationDateInDatePicker;
                        regTime = resultReg.RegistrationTime;
                    }
                });
                var param = regID + '|' + regDate + '|' + regTime;
                return param;
            }
            else if (code == 'keteranganIstirahat1' || code == 'keteranganIstirahat3' || code == 'kesehatanMata1' || code == 'keteranganSehat1'
                || code == 'keteranganDokter' || code == 'lembarKonsultasi' || code == 'rujukanrslain' || code == 'keteranganVaksin' || code == 'keteranganPermintaanMasukICU'
                || code == 'keteranganKematianRSBL' || code == 'keteranganButaWarna' || code == 'keteranganPemeriksaanCovid' || code == 'keteranganSehat2'
                || code == 'kontrolKesehatan' || code == 'keteranganSehat3' || code == 'kontrolKesehatan1' || code == 'downloadForm' || code == 'generateControlLetter'
                || code == 'keteranganKematian' || code == 'keteranganSehat') {
                return $('#<%:hdnVisitID.ClientID %>').val();
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }
    </script>
    <style>
    .hidden{display:none;}
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnGCGender" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnGCResultType" value="" />
    <input type="hidden" runat="server" id="hdnResultType" value="" />
    <input type="hidden" runat="server" id="hdnFormLayout" value="" />
    <input type="hidden" runat="server" id="hdnFormValue" value="" />
    <input type="hidden" runat="server" id="hdnRemarks" value="" />
    <div style="position: relative;">
        <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
            <colgroup>
                <col width="30%" />
                <col />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpFormList" runat="server" Width="100%" ClientInstanceName="cbpFormList"
                            ShowLoadingPanel="false" OnCallback="cbpFormList_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerHdImgLoadingView').show(); }"
                                EndCallback="function(s,e){ onCbpFormListEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="panFormList" CssClass="pnlContainerGridPatientPage">
                                        <asp:GridView ID="grdFormList" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="StandardCodeName" HeaderStyle-CssClass="hiddenColumn"
                                                    ItemStyle-CssClass="hiddenColumn StandardCodeName" />
                                                <asp:BoundField DataField="StandardCodeName" HeaderText="Jenis Hasil" HeaderStyle-CssClass="gridColumnText"
                                                    ItemStyle-CssClass="gridColumnText" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada jenis hasil MCU")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerHdImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="pagingHd">
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
                <td style="vertical-align: top">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <input type="hidden" value="" id="hdnFileString" runat="server" />
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn VisitID" />
                                            <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn RegistrationID" />
                                            <asp:BoundField DataField="FormLayout" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn FormLayout" />
                                            <asp:BoundField DataField="FormValue" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn FormValue" />
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Created By Name" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfCreatedDateInStringFullType" HeaderText="Created Date"
                                                HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:HyperLinkField HeaderText=" " Text="View" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-CssClass="lnkView" HeaderStyle-Width="60px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data") %>
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
