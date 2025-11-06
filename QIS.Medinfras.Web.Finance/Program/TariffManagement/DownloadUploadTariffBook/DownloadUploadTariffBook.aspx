<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="DownloadUploadTariffBook.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.DownloadUploadTariffBook" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnDownload" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdownload.png")%>' alt="" /><div>
            <%=GetLabel("Download")%></div>
    </li>
    <li id="btnUpload" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbupload.png")%>' alt="" /><div>
            <%=GetLabel("Upload")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">

        function onGetCurrID() {
            return $('#<%=hdnBookID.ClientID %>').val();
        }

        $(function () {
        
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnBookID.ClientID %>', '<%=pnlView.ClientID %>', cbpProcessDetail, '');
        });

        $('#btnRefresh').live('click', function () {
            cbpProcessDetail.PerformCallback('refresh');
        });

        //#region Download & Upload

        $('#<%=btnDownload.ClientID %>').live('click', function () {
            ///onCustomButtonClick('download');

            var oBookID = $('#<%=hdnBookID.ClientID %>').val();
            var url = ResolveUrl("~/Program/TariffManagement/DownloadUploadTariffBook/DownloadTarifBookEntryCtl.ascx");
            openUserControlPopup(url, oBookID, 'Proses Download Detail', 500, 200); 
           
        });

        function downloadBPJSDocument(stringparam) {
            var fileName = $('#<%=hdnFileName.ClientID %>').val();
         
            var link = document.createElement("a");
            link.href = 'data:text/csv,' + encodeURIComponent(stringparam);
            link.download = fileName;
            link.click();
        }

        $('#<%=btnUpload.ClientID %>').die('change');
        $('#<%=btnUpload.ClientID %>').live('click', function () {
            /////document.getElementById('<%=TariffDocumentUpload.ClientID %>').click();

            var oBookID = $('#<%=hdnBookID.ClientID %>').val();
            if (oBookID == "") {
                displayErrorMessageBox('Error', 'Silahkan dipilih dahulu nomor buku tarif yang akan di proses.');
            } else {
                var url = ResolveUrl("~/Program/TariffManagement/DownloadUploadTariffBook/UploadTarifBookEntryCtl.ascx");
                openUserControlPopup(url, oBookID, 'Proses Upload Detail', 1024, 500); 
            }
  

        });

       $('#<%=TariffDocumentUpload.ClientID %>').die('change');
     
       $('#<%=TariffDocumentUpload.ClientID %>').live('change', function () {
            readURL(this);

            if ($('#<%=hdnTariffUploadedFile.ClientID %>').val() != "" && $('#<%=hdnTariffUploadedFile.ClientID %>').val() != null) {
                onCustomButtonClick('upload');
                //onCustomButtonClick('uploadnew');
            } else {
                displayErrorMessageBox('Upload Failed', 'Silahkan coba lagi.');
            }
        });
        function readURL(input) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#<%=hdnTariffUploadedFile.ClientID %>').val(e.target.result);
            };
            reader.readAsDataURL(input.files[0]);
        }
        //#endregion

        $('.imgUploadDetail.imgLink').live('click', function () {
            var rowID = $(this).parent().parent().attr('id');
            var oBookID = rowID;
            $('#<%=hdnBookID.ClientID %>').val(rowID);
            var url = ResolveUrl("~/Program/TariffManagement/DownloadUploadTariffBook/UploadProcessDetailCtl.ascx");
            openUserControlPopup(url, oBookID, 'Proses Upload Detail', 1250, 500);
            console.log(rowID);
        });

        function onCbpProcessDetailEndCallback() {
            hideLoadingPanel();

            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "download") {
                downloadBPJSDocument(retval);
            }

            cbpProcessDetail.PerformCallback('refresh');

        }
        function onRefreshGrid() {
            cbpProcessDetail.PerformCallback('refresh');
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnBookID" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <input type="hidden" id="hdnTariffUploadedFile" runat="server" value="" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td>
                    <h4>
                        <%=GetLabel("Data Pencarian")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 30%" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Skema Tariff")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <colgroup>
                                            <col style="width: 300px" />
                                            <col style="width: 100px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td align="left">
                                                <dxe:ASPxComboBox ID="cboGCTariffScheme" ClientInstanceName="cboGCTariffScheme" runat="server"
                                                    Width="300px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td>
                                    <asp:FileUpload ID="TariffDocumentUpload" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h4>
                        <%=GetLabel("Data Buku Tariff")%></h4>
                    <div style="position: relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                            ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="BookID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="DocumentNo" HeaderText="No Buku Tarif" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="TariffScheme" HeaderText="Skema Tarif" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="StartingDateInString" HeaderText="Tanggal Berlaku" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="Notes" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="TransactionStatus" HeaderText="Status Buku Tariff" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="150px" />
                                                <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <%=GetLabel("Informasi Dibuat")%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="padding: 3px; text-align: center">
                                                            <label class="lblNormal">
                                                                <%#: Eval("CreatedByName") %></label><br />
                                                            <label class="lblNormal">
                                                                <%#: Eval("CreatedDateInStringFull") %></label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <%=GetLabel("Proses")%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div id="<%#: Eval("BookID") %>">
                                                         <div style="padding: 3px; text-align: center">
                                                            <img class="imgUploadDetail <%# Eval("cfIsHasOutstandingItemUpload").ToString() == "True" ? "imgLink" : "imgDisabled"%>"
                                                                title='<%=GetLabel("Proses")%>' src='<%# Eval("cfIsHasOutstandingItemUpload").ToString() == "True" ? ResolveUrl("~/Libs/Images/Toolbar/upload_tariff.png") : ResolveUrl("~/Libs/Images/Toolbar/upload_tariff_disabled.png")%>'
                                                                alt="" style="margin-right: 2px" />
                                                        </div>
                                                        </div>
                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="Div1">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
