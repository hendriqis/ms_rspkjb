<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="TestOrderResultList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TestOrderResultList" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnIsBridgingWithLIS" runat="server" />
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnListVisitID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromDate.ClientID %>');
            setDatePicker('<%=txtToDate.ClientID %>');

            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        $('#<%=lvwView.ClientID %> #chkSelectAll').live('change', function () {
            var isChecked = $(this).is(':checked');
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('#<%=lvwView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }

        $('.lblDetail').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val();
            var testTo = $(this).closest('tr').find('.HealthcareServiceUnitID').val();
            var laboratoryServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            var isBridgingWithLIS = $('#<%=hdnIsBridgingWithLIS.ClientID %>').val();
            var url = '';
            if (testTo == laboratoryServiceUnitID) {
                if (isBridgingWithLIS == '0') {
                    url = ResolveUrl("~/Libs/Controls/LaboratoryResultCtl.ascx");
                }
                else {
                    url = ResolveUrl("~/Libs/Controls/LaboratoryResultBridgingCtl.ascx");
                }
            } else {
                url = ResolveUrl("~/Libs/Controls/ImagingResultCtl.ascx");
            }
            openUserControlPopup(url, id, 'Test Result', 1100, 500);
        });

        function onCboVoidReasonValueChanged() {
            if (cboVoidReason.GetValue() == Constant.DeleteReason.OTHER)
                $('#trVoidOtherReason').show();
            else
                $('#trVoidOtherReason').hide();
        }

        function onAfterCustomClickSuccess() {
            cbpView.PerformCallback('refresh');
        }

    </script>
    <input type="hidden" value="" id="hdnTestOrderID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label>
                                    <%=GetLabel("Tanggal Transaksi")%></label>
                            </td>
                            <td>
                                <table>
                                    <colgroup>
                                        <col style="width: 160px" />
                                        <col style="width: 30px" />
                                        <col style="width: 160px" />
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFromDate" Width="120px" runat="server" CssClass="datepicker" />
                                        </td>
                                        <td>
                                            <label>
                                                <%=GetLabel("s/d")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" Width="120px" runat="server" CssClass="datepicker" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsIgnoreDate" runat="server" Checked="false" /><%:GetLabel(" Abaikan Tanggal")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblPenunjangMedis">
                                    <%=GetLabel("Penunjang Medis")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboServiceUnitPerHealthcare" ClientInstanceName="cboServiceUnitPerHealthcare"
                                    Width="200px">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                  <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th rowspan="2" style="width:180px"><%=GetLabel("Unit Pelayanan") %></th>
                                                    <th colspan="2"><%=GetLabel("Informasi Transaksi")%></th>
                                                    <th colspan="2"><%=GetLabel("Informasi Order")%></th>
                                                    <th rowspan="2"><%=GetLabel("Catatan") %></th>
                                                    <th rowspan="2"><%=GetLabel("Ada Hasil") %></th>
                                                    <th rowspan="2"><%=GetLabel("Detail") %></th>
                                                </tr>
                                                <tr>
                                                    <th style="width:150px"><%=GetLabel("No Transaksi") %></th>
                                                    <th style="width:80px"><%=GetLabel("Tanggal") %><br /><%=GetLabel("Jam Transaksi") %></th>
                                                    <th style="width:150px"><%=GetLabel("No Order") %></th>
                                                    <th style="width:80px"><%=GetLabel("Tanggal") %><br /><%=GetLabel("Jam Order") %></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect lvwView" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th rowspan="2" style="width:180px"><%=GetLabel("Unit Pelayanan") %></th>
                                                    <th colspan="2"><%=GetLabel("Informasi Transaksi")%></th>
                                                    <th colspan="2"><%=GetLabel("Informasi Order")%></th>
                                                    <th rowspan="2"><%=GetLabel("Catatan") %></th>
                                                    <th rowspan="2"><%=GetLabel("Ada Hasil") %></th>
                                                    <th rowspan="2"><%=GetLabel("Detail") %></th>
                                                </tr>
                                                <tr>
                                                    <th style="width:150px"><%=GetLabel("No Transaksi") %></th>
                                                    <th style="width:80px"><%=GetLabel("Tanggal") %><br /><%=GetLabel("Jam Transaksi") %></th>
                                                    <th style="width:150px"><%=GetLabel("No Order") %></th>
                                                    <th style="width:80px"><%=GetLabel("Tanggal") %><br /><%=GetLabel("Jam Order") %></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="left">
                                                    <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                    <input type="hidden" class="HealthcareServiceUnitID" value="<%#: Eval("HealthcareServiceUnitID")%>" />
                                                    <input type="hidden" class="ServiceUnitName" value="<%#: Eval("ServiceUnitName")%>" />
                                                    <div><%#: Eval("ServiceUnitName")%></div>
                                                </td>
                                                <td align="left"><%#: Eval("TransactionNo")%></td>
                                                <td align="center"><%#: Eval("cfTransactionDateInString")%>
                                                                <div><%#: Eval("TransactionTime")%></div>   
                                                </td>
                                                <td align="left"><%#: Eval("TestOrderNo")%>
                                                                <div><%#: Eval("ParamedicOrderName")%></div>
                                                                <div><%# Eval("CreatedByTestOrder").ToString() == "" ? "":"Diorder oleh : " + Eval("CreatedByTestOrder").ToString() %></div>
                                                </td>
                                                <td align="center"><%#: Eval("cfTestOrderDateInString")%>
                                                                <div><%#: Eval("TestOrderTime")%></div>   
                                                </td>
                                                <td align="left"><%#: Eval("RemarksOrder")%></td>
                                                <td align="center"><%#: Eval("ResultOut")%></td>
                                                <td align="center"><a class="lblLink lblDetail"><%=GetLabel("Detail") %></a></td>
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
