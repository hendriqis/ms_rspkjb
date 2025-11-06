<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.Master" 
    AutoEventWireup="true" CodeBehind="ImportPatientDiagnosisFromTXT.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.ImportPatientDiagnosisFromTXT" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Proses")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        $(function () {

            $('#btnUploadFile').click(function () {
                document.getElementById('<%= FileUpload1.ClientID %>').click();
            });

            $('#<%=FileUpload1.ClientID %>').change(function () {
                var fileName = $('#<%=FileUpload1.ClientID %>').val();
                $('#<%=txtFileName.ClientID %>').val(fileName);
            });

        });

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

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            cbpView.PerformCallback('refresh');
        }
    </script> 
    <input type="hidden" id="hdnLstSelected" value="" runat="server" />
    <input type="hidden" value="" id="hdnContentID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
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
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("File TXT BPJS")%></label></td>
                            <td style="padding-left:1px"><asp:TextBox ID="txtFileName" Width="350px" runat="server" ReadOnly="true" /></td>
                            <td style="padding-left:5px; padding-bottom:3px">
                                <input type="button" id="btnUploadFile" value="Browse"/>
                                <input type="hidden" id="hdnUploadedFile1" runat="server" value="" />
                                <div style="display:none">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
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
                                                        <asp:BoundField DataField="PrescriptionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="ChargesTransactionTime" HeaderText="Jam Masuk" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="StartTime" HeaderText="Jam Mulai" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="CompleteTime" HeaderText="Jam Selesai" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
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
