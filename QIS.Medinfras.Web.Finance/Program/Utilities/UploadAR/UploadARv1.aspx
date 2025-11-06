<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="UploadARv1.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.UploadARv1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $('#btnUploadFile').die('click');
        $('#btnUploadFile').live('click', function () {
            document.getElementById('<%=FinanceFileUpload.ClientID %>').click();
        });

        $('#<%=FinanceFileUpload.ClientID %>').die('change');
        $('#<%=FinanceFileUpload.ClientID %>').live('change', function () {
            readURL(this);
            if ($('#<%=hdnUploadedFile.ClientID %>').val() != "" && $('#<%=hdnUploadedFile.ClientID %>').val() != null) {
                cbpView.PerformCallback("upload");
            } else {
                displayErrorMessageBox('Upload Failed', 'Silahkan coba lagi.');
            }
        });

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=hdnUploadedFile.ClientID %>').val(e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        function onCbpViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail') {
                displayErrorMessageBox('Process Failed', 'Error Message : ' + param[1]);
            }
            else {
                showToast('Process Success', 'Proses Sukses');
            }

            hideLoadingPanel();
        }
    </script>
    <div>
        <input type="hidden" id="hdnUploadedFile" runat="server" value="" />
        <table width="100%">
            <colgroup>
                <col width="50%" />
                <col width="50%" />
            </colgroup>
            <tr valign="top">
                <td>
                    <table width="100%">
                        <colgroup>
                            <col width="150px" />
                        </colgroup>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30px" />
                                    </colgroup>
                                    <tr style="display: none">
                                        <td>
                                            <asp:FileUpload ID="FinanceFileUpload" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 3px; padding-bottom: 5px">
                                            <input type="button" id="btnUploadFile" value="Browse" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 300px">
                                                        <%=GetLabel("No. Tagihan") %>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 300px">
                                                        <%=GetLabel("No. Tagihan") %>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%#: Eval("ARInvoiceNo")%></b>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
