<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="GeneratePatientFromGuest.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.GeneratePatientFromGuest" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
        });

        function onAfterSaveGenerateMR() {
            refreshGrdRegisteredPatient();
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function refreshGrdRegisteredPatient() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            refreshGrdRegisteredPatient();
        });

        function getCheckedMember() {
            var lstSelectedMember = [];
            $('#<%=pnlGridView.ClientID %> tr .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    lstSelectedMember.push(key);
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join('|'));
        }

        function onAfterCustomClickSuccess(type, retval) {
            refreshGrdRegisteredPatient();
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {

        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                refreshGrdRegisteredPatient();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }


        $('.btnSave').live('click', function () {
            $tr = $(this).closest('tr');
            var ID = $tr.find('.hdnKey').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/PatientEntryCtl.ascx");
            var param = "generate|" + ID;
            openUserControlPopup(url, param, 'Data Pasien', 980, 600);
        });
    </script>
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnQuickText" runat="server" value="" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table>
                            <colgroup>
                                <col style="width: 20%" />
                                <col style="width: 5%" />
                                <col style="width: 75%" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td colspan="2">
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="560px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pengunjung" FieldName="FullName" />
                                            <qis:QISIntellisenseHint Text="No. Pengunjung" FieldName="GuestNo" />
                                            <qis:QISIntellisenseHint Text="Jenis Kelamin" FieldName="Gender" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s);}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 400px; overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("No. Pengunjung")%>
                                                    </th>
                                                    <th style="width: 150px;text-align: left">
                                                        <%=GetLabel("Nama Pengunjung")%>
                                                    </th>
                                                    <th style="width: 450px; text-align: left">
                                                        <%=GetLabel("Informasi Pengunjung")%>
                                                    </th>
                                                    <th style="width: 20px">
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
                                            <table id="tblView" runat="server" class="grdView lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("No. Pengunjung")%>
                                                    </th>
                                                    <th style="width: 150px;text-align: left">
                                                        <%=GetLabel("Nama Pengunjung")%>
                                                    </th>
                                                    <th style="width: 450px; text-align: left">
                                                        <%=GetLabel("Informasi Pengunjung")%>
                                                    </th>
                                                    <th style="width: 20px">
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trItem">
                                                <td class="keyField" >
                                                    <input type="hidden" value="" class="hdnKey" id="hdnKey" runat="server" />
                                                    <%#: Eval("GuestID")%>
                                                </td>
                                                <td align="center">
                                                    <div style="font-weight: bold; font-size: medium">
                                                        <%#: Eval("GuestNo") %>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <%#: Eval("FullName") %>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <i>
                                                            <label style="font-size: smaller">
                                                                <%=GetLabel("Tgl. Lahir : ")%></label></i><%#: Eval("DateOfBirthInString") %><br />
                                                        <i>
                                                            <label style="font-size: smaller">
                                                                <%=GetLabel("Jenis Kelamin : ")%></label></i><%#: Eval("Gender") %><br />
                                                        <i>
                                                            <label style="font-size: smaller">
                                                                <%=GetLabel("No.Telp : ")%></label></i><%#: Eval("PhoneNo") %><br />
                                                        <i>
                                                            <label style="font-size: smaller">
                                                                <%=GetLabel("Alamat : ")%></label></i><%#: Eval("StreetName") %><%#: Eval("City") %></div>
                                                </td>
                                                <td align="center">
                                                    <input type="button" id="btnSave" class="btnSave w3-btn w3-hover-green" value="Generate RM" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
