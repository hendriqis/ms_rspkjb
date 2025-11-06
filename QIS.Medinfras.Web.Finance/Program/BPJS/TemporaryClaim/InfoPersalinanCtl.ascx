<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoPersalinanCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPersalinanCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_SuratPenagihan">

    $(function () {
        hideLoadingPanel();
    });


    function onCbpReportProcessEndCallback(s) {

        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == "fail") {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }

            else {

                showToast('Process Success');
            }
        } else if (param[0] == 'printout') {
            var FileString = $('#<%:hdnFileString.ClientID %>').val();
            if (FileString != "") {
                //window.open("data:application/pdf;base64, " + FileString);
                var random = Math.random().toString(36).substring(7);
                var css = '<%=ResolveUrl("~/Libs/Styles/PrintLayout/paper.css")%>' + "?" + random;
                var newWin = open('url', 'windowName', 'scrollbars=1,resizable=1,width=1000,height=580,left=0,top=0');
                newWin.document.write('<html><head><title>Info Laporan Persalinan</title><link rel="stylesheet" type="text/css" href="' + css + '"> </head><style type="text/css" media="print"> .noPrint{display:none;} </style>');
                var html = '<style>@page { size: A4 landscape }</style>';
                html = '<body class="A4 landscape"> <div style="margin-left:20px;" class="noPrint"><input type="button" value="Print Halaman" onclick="window.print()" /></div>' + FileString + ' </body>';
                newWin.document.write(html);
                newWin.document.close();
                newWin.focus();
                newWin.print();
            }
            $('#<%:hdnFileString.ClientID %>').val('');

        }
        pcRightPanelContent.Hide();

    }

    function getCheckedData() {
        var lstSelectedReportID = $('#<%=hdnSelectedReportID.ClientID %>').val().split(',');
        var lstSelectedReportCode = $('#<%=hdnSelectedReportCode.ClientID %>').val().split(',');

        var result = '';
        $('.chkIsSelectedDetail input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var DtID = $tr.find('.keyField').val();
                var hdnReportCode = $tr.find('.hdnReportCode').val();
                result += hdnReportCode + "|";

            }

        });

        if (result != "") {
            result = result.substring(0, result.length - 1);
            if (result != "") {
                $('#<%=hdnReportCode.ClientID %>').val(result);
            } else {
                $('#<%=hdnReportCode.ClientID %>').val("");
            }

        } else {
            $('#<%=hdnReportCode.ClientID %>').val("");
        }

    }

    $('#<%=btnProcess.ClientID %>').click(function () {

        cbpReportProcess.PerformCallback('printout');

    });
</script>
<input type="hidden" runat="server" id="hdnRegistrationID" value="0" />
<input type="hidden" runat="server" id="hdnMenuIDCtl" value="0" />
<input type="hidden" runat="server" id="hdnReportCode" value="" />
<input type="hidden" runat="server" id="hdnSepNo" value="" />
<input type="hidden" runat="server" id="hdnNoPeserta" value="" />
<input type="hidden" runat="server" id="hdnRegistrationNo" value="" />
<input type="hidden" runat="server" id="hdnVisitID" value="" />
<input type="hidden" value="" id="hdnMedicalNo" runat="server" />
<input type="hidden" value="" id="hdnPatientName" runat="server" />
<div style="padding: 10px;">
    <div>
        <table>
            <tbody>
                <tr>
                    <th>
                        Registration No
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtRegistrationNo" Width="150px" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <th>
                        Nomor Peserta
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtPeserta" Width="150px" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <th>
                        Nomor SEP
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtSepNo" Width="150px" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <input type="button" id="btnProcess" runat="server" value="Cetak Laporan Persalinan"
                            class="btn btn-w3" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div>
        <dxcp:ASPxCallbackPanel ID="cbpReportProcess" runat="server" Width="100%" ClientInstanceName="cbpReportProcess"
            ShowLoadingPanel="false" OnCallback="cbpReportProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpReportProcessEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlDocumentNotesGrdView" Style="width: 100%; margin-left: auto;
                        margin-right: auto; position: relative; font-size: 0.95em;">
                        <input type="hidden" id="hdnSelectedReportID" runat="server" value="0" />
                        <input type="hidden" id="hdnSelectedReportCode" runat="server" value="0" />
                        <input type="hidden" runat="server" id="hdnFileString" value="" />
                        <asp:GridView ID="grdReportMaster" runat="server" CssClass="grdView notAllowSelect"
                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("AssessmentID") %>" bindingfield="ID" class="hdnAssessmentID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AssessmentDate" HeaderText="Tanggal" HeaderStyle-Width="200px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="AssessmentTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="AssessmentTime" HeaderText="Dikaji Oleh" HeaderStyle-HorizontalAlign="Left" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div class="imgLoadingGrdView" id="containerImgLoadingDocumentNote">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
