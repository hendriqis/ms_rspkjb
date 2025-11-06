<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadInvoiceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DownloadInvoiceCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_downloadctl">
    //#region Download
    $('#btnDownload').click(function (evt) {
        alert('klik donlot');
        __doPostBack('<%=btnDownloadProcess.UniqueID%>', '');
    });
    //#endregion
</script>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <table>
            <colgroup>
                <col style="width: 100px" />
                <col />
            </colgroup>
            <tr>
                <td>
                </td>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 70px" />
                            <col style="width: 5px" />
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <input type="button" id="btnDownload" value="D o w n l o a d" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div style="display: none;">
            <asp:Button ID="btnDownloadProcess" Visible="true" runat="server" OnClick="btnDownloadProcess_Click"
                Text="Download" UseSubmitBehavior="false" OnClientClick="return true;" />
        </div>
    </fieldset>
</div>
