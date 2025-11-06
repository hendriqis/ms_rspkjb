<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.Master" 
    AutoEventWireup="true" CodeBehind="PrescriptionStatusEdit.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionStatusEdit" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
 <%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>


<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Proses")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtPrescriptionDate.ClientID %>');
            $('#<%=txtPrescriptionDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#ulTabTransaction li').click(function () {
                $('#ulTabTransaction li.selected').removeAttr('class');
                $contentID = $(this).attr('contentid');
                $('#<%=hdnContentID.ClientID %>').val($contentID);
                onRefreshGrd();
                $(this).addClass('selected');
            });

            $('#<%=txtPrescriptionDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGrd();
            });

//            $('#<%=txtBarcodeEntry.ClientID %>').focus();
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalReg = window.setInterval(function () {
            onRefreshGrd();
        }, interval);

        function onRefreshGrd() {

            window.clearInterval(intervalReg);
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
            intervalReg = window.setInterval(function () {
                onRefreshGrd();
            }, interval);
        }
        
        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrd();
            }, 0);
        }

        $('#<%=btnProcess.ClientID %>').die('click');
        $('#<%=btnProcess.ClientID %>').live('click', function () {
            var param = "";
            $('#<%=grdView.ClientID %> .chkIsSelected input:checked').each(function () {
                var prescriptionOrderDetailID = $(this).closest('tr').find('.keyField').html();
                if (param != "")
                    param += ',';
                param += prescriptionOrderDetailID;
            });
            $('#<%=hdnLstSelected.ClientID %>').val(param);
            cbpProcess.PerformCallback('save');
        });

        function onCboDepartmentValueChanged() {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                else
                    $('#<%=hdnID.ClientID %>').val('');

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            cbpView.PerformCallback('refresh');
        }

        $('#<%=chkIsIgnoreDate.ClientID %>').die();
        $('#<%=chkIsIgnoreDate.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtPrescriptionDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtPrescriptionDate.ClientID %>').removeAttr('readonly');
            onRefreshGrd();
        });
    </script> 
    <input type="hidden" id="hdnLstSelected" value="" runat="server" />
    <input type="hidden" value="containerProses" id="hdnContentID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />

    <div style="height:435px;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width:100%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table>
                        <colgroup>
                            <col style="width:120px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Farmasi")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onRefreshGrd(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal") %></label></td>
                            <td><asp:TextBox runat="server" ID="txtPrescriptionDate" Width="120px" CssClass="datepicker" />&nbsp;<asp:CheckBox ID="chkIsIgnoreDate" runat="server" Checked="false" /><%:GetLabel("Abaikan Tanggal")%>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="300px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No.Order" FieldName="PrescriptionOrderNo" />
                                        <qis:QISIntellisenseHint Text="No.Registrasi" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien") %></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="300px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Scan No. Resep")%></label></td>
                            <td><asp:TextBox ID="txtBarcodeEntry" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="padding-top:5px;padding-bottom:1px; font-size:0.95em">
                                    <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("menit")%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="containerUlTabPage">
                        <ul class="ulTabPage" id="ulTabTransaction">
                            <li class="selected" contentid="containerProses1"><%=GetLabel(GetLabel("BELUM DIKERJAKAN")) %></li>
                            <li contentid="containerProses2"><%=GetLabel(GetLabel("SEDANG DIKERJAKAN")) %></li>
                            <li contentid="containerProses3"><%=GetLabel(GetLabel("PENGECEKAN")) %></li>
                            <li contentid="containerProses4"><%=GetLabel(GetLabel("SUDAH SELESAI")) %></li>
                            <li contentid="containerDone"><%=GetLabel("SUDAH DISERAHKAN") %></li>
                        </ul>
                    </div> 
                    <div id="containerApprove" class="containerTransDt">
                        <div id="containerEntryApprove" style="margin-top:4px;">
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel()}" EndCallback="function(s,e){onCbpViewEndCallback(s)}"/>
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkIsSelected" CssClass="chkIsSelected" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText = "Waktu Masuk" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <div><%#: Eval("PrescriptionDateInString") %></div>
                                                                <div><%#: Eval("ChargesTransactionTime") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>   
                                                        <asp:TemplateField HeaderText = "Waktu Mulai" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <div><%#: Eval("cfStartDateInString") %></div>
                                                                <div><%#: Eval("StartTime") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                        
                                                        <asp:TemplateField HeaderText = "Waktu Selesai" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <div><%#: Eval("cfCompleteDateInString") %></div>
                                                                <div><%#: Eval("CompleteTime") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField> 
                                                        <asp:BoundField DataField="ChargesTransactionNo" HeaderText="No. Resep" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                        <asp:TemplateField HeaderText = "No. Registrasi" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <div><%#: Eval("RegistrationNo") %></div>
                                                                <div><%#: Eval("ServiceUnitName") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="MedicalNo" HeaderText="No. RM" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                                        <asp:TemplateField HeaderText = "Informasi Pasien" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div><%#: Eval("PatientName") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DepartmentName" HeaderText="Asal Pasien" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                        <asp:BoundField DataField="ChargesTransactionStatusWatermark" HeaderText="Transaction Status" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="RegistrationStatus" HeaderText="Registration Status" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada data resep")%>
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
                                <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
                                    ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
                                        EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
                                </dxcp:ASPxCallbackPanel>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
