<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="ControlOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ControlOrderList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        $('.lnkItemName a').live('click', function () {
            $tr = $(this).closest('tr');
            var visitID = $tr.find('.hdnVisitID').val();
            var regNo = $tr.find('.hdnRegistrationNo').val();
            var patientName = $tr.find('.hdnPatientName').val();
            var itemID = $tr.find('.hdnItemID').val();
            var itemCode = $tr.find('.hdnItemCode').val();
            var itemName = $tr.find('.hdnItemName1').val();
            var id = visitID + '|' + regNo + '|' + patientName + '|' + itemID + '|' + itemCode + '|' + itemName;
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/ControlOrder/ControlOrderDtCustomCtl.ascx");
            openUserControlPopup(url, id, 'Item Detail', 1200, 500);
        });

        $('.imgOutstanding.imgLink1').die('click');
        $('.imgOutstanding.imgLink1').live('click', function () {
            $tr = $(this).closest('tr');
            var visitID = $tr.find('.hdnVisitID').val();
            var regNo = $tr.find('.hdnRegistrationNo').val();
            var patientName = $tr.find('.hdnPatientName').val();
            var itemID = $tr.find('.hdnItemID').val();
            var itemCode = $tr.find('.hdnItemCode').val();
            var itemName = $tr.find('.hdnItemName1').val();
            var id = visitID + '|' + regNo + '|' + patientName + '|' + itemID + '|' + itemCode + '|' + itemName;
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/ControlOrder/ControlOutstandingOrderCtl.ascx");
            openUserControlPopup(url, id, 'Item Detail', 1000, 550);
        });
               
    </script>
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnImagingHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" id="hdnSelectedRegistration" runat="server" value="" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetMenuCaption()%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsBedStatus">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Registrasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <%=GetLabel("Halaman Ini Akan")%>
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                        <%=GetLabel("setiap")%>
                        <%=GetRefreshGridInterval() %>
                        <%=GetLabel("menit")%>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MedicalNo" HeaderText="No. Rekam Medis" HeaderStyle-Width="110px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="PatientName" HeaderStyle-Width="250px" HeaderText="Nama Pasien"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Paket MCU" ItemStyle-CssClass="lnkItemName" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input type="hidden" class="hdnItemID" value='<%#:Eval("ItemID") %>' />
                                                    <input type="hidden" class="hdnVisitID" value='<%#:Eval("VisitID") %>' />
                                                    <input type="hidden" class="hdnRegistrationID" value='<%#:Eval("RegistrationID") %>' />
                                                    <input type="hidden" class="hdnRegistrationNo" value='<%#:Eval("RegistrationNo") %>' />
                                                    <input type="hidden" class="hdnPatientName" value='<%#:Eval("PatientName") %>' />
                                                    <input type="hidden" class="hdnItemCode" value='<%#:Eval("ItemCode") %>' />
                                                    <input type="hidden" class="hdnItemName1" value='<%#:Eval("ItemName1") %>' />
                                                    <a>
                                                        <%#:Eval("cfItemComparison") %></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BusinessPartner" HeaderText="Pembayar" HeaderStyle-Width="230px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div style="text-align: center">
                                                        <img class="imgOutstanding <%#"imgLink1"%>" title='<%=GetLabel("Outstanding Order")%>'
                                                            src='<%# ResolveUrl("~/Libs/Images/Toolbar/outstanding_order.png")%>' style='<%# Eval("countOutstandingTestOrder").ToString() == "True" ? "width:25px": "width:24px;heght:24px;display:none" %>'
                                                            alt="" />
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
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
