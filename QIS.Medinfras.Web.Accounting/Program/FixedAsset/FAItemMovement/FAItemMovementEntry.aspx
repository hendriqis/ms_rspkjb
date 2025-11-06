<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/FixedAssetPage/MPBaseFixedAssetPageTrx.master"
    AutoEventWireup="true" CodeBehind="FAItemMovementEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemMovementEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Program/FixedAsset/FixedAssetToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtMovementDate.ClientID %>');

            //#region Location
            function onGetLocationFilterExpression() {
                return "IsDeleted = 0";
            }
            $('#lblLocation.lblLink').click(function () {
                openSearchDialog('falocation', onGetLocationFilterExpression(), function (value) {
                    $('#<%=txtToLocationCode.ClientID %>').val(value);
                    ontxtToLocationCodeChanged(value);
                });
            });

            $('#<%=txtToLocationCode.ClientID %>').change(function () {
                ontxtToLocationCodeChanged($(this).val());
            });

            function ontxtToLocationCodeChanged(value) {
                var filterExpression = onGetLocationFilterExpression() + " AND FALocationCode = '" + value + "'";
                Methods.getObject('GetFALocationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtToLocationName.ClientID %>').val(result.FALocationName);
                        $('#<%=hdnToLocationID.ClientID %>').val(result.FALocationID);
                    }
                    else {
                        $('#<%=txtToLocationCode.ClientID %>').val('');
                        $('#<%=txtToLocationName.ClientID %>').val('');
                        $('#<%=hdnToLocationID.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

        $('#lblAddData').die('click');
        $('#lblAddData').live('click', function () {
            $('#<%=hdnIsAdd.ClientID %>').val("1");
            $('#<%=hdnMovementID.ClientID %>').val("");
            $('#<%=txtMovementNo.ClientID %>').val("");
            $('#<%=hdnFromLocationID.ClientID %>').val("");
            $('#<%=hdnToLocationID.ClientID %>').val("");
            $('#<%=txtToLocationCode.ClientID %>').val("");
            $('#<%=txtToLocationName.ClientID %>').val("");
            $('#<%=txtRemarks.ClientID %>').val("");
            $('#containerEntry').show();
        });

        $('#btnCancel').die('click');
        $('#btnCancel').live('click', function () {
            $('#containerEntry').hide();
        });

        $('#btnSave').die('click');
        $('#btnSave').live('click', function () {
            var isAdd = $('#<%=hdnIsAdd.ClientID %>').val();
            cbpViewProcess.PerformCallback('process');
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnMovementID.ClientID %>').val(entity.MovementID);
            $('#<%=txtMovementNo.ClientID %>').val(entity.MovementNo);
            $('#<%=txtMovementDate.ClientID %>').val(entity.MovementDateInDatePickerFormat);
            $('#<%=hdnFromLocationID.ClientID %>').val(entity.FromFALocationID);
            $('#<%=hdnToLocationID.ClientID %>').val(entity.ToFALocationID);
            $('#<%=txtToLocationCode.ClientID %>').val(entity.ToLocationCode);
            $('#<%=txtToLocationName.ClientID %>').val(entity.ToLocationName);
            $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            $('#<%=hdnIsAdd.ClientID %>').val("0");
            $('#containerEntry').show();
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnMovementID.ClientID %>').val(entity.MovementID);
            cbpViewProcess.PerformCallback('delete');
        });

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

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewProcessEndCallback(s) {
            hideLoadingPanel();
            $('#containerEntry').hide();
            cbpView.PerformCallback('refresh');
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var movementID = $('#<%=hdnMovementID.ClientID %>').val();
            var fixedAssetID = $('#<%=hdnFixedAssetID.ClientID %>').val();

            if (code == 'AC-00005') {
                filterExpression.text = "FixedAssetID = " + fixedAssetID;
                return true;
            }
            else {
                filterExpression.text = "MovementID = " + movementID;
                return true;
            }
        }

        $('.imgPrint.imgLink').die('click');
        $('.imgPrint.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);
            var reportCode = "AC-00005";
            var fixedAssetID = $('#<%=hdnFixedAssetID.ClientID %>').val();
            var movementNo = obj.MovementNo;

            var filterExpression = { text: "" };
            filterExpression.text = "FixedAssetID = '" + fixedAssetID + "' AND MovementNo = '" + movementNo + "'";
            openReportViewer(reportCode, filterExpression.text);
        });
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnIsAdd" runat="server" value="1" />
    <input type="hidden" id="hdnMovementID" runat="server" value="" />
    <input type="hidden" id="hdnFixedAssetID" runat="server" value="" />
    <input type="hidden" id="hdnFromLocationID" runat="server" value="" />
    <table class="tblContentArea">
        <tr>
            <td>
                <div id="containerEntry" style="margin-top: 4px; width: 100%; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsTrx" style="margin: 0">
                        <table style="width: 100%" class="tblEntryDetail">
                            <tr>
                                <td style="width: 50%">
                                    <table style="width: 100%">
                                        <colgroup>
                                            <col style="width: 150px" />
                                        </colgroup>
                                        <tr>
                                            <td id="td3">
                                                <label class="lblNormal">
                                                    <%=GetLabel("No. Pemindahan") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMovementNo" ReadOnly="true" Width="150px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Tanggal") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtMovementDate" Width="120px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="td1">
                                                <label class="lblMandatory lblLink" id="lblLocation">
                                                    <%=GetLabel("Lokasi") %></label>
                                            </td>
                                            <td>
                                                <input type="hidden" runat="server" id="hdnToLocationID" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 30%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtToLocationCode" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtToLocationName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="td2">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Keterangan") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Rows="2" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="MovementID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <img class="imgEdit <%#: Eval("IsEditable").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                        src='<%# Eval("IsEditable").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    &nbsp;
                                                    <img class="imgDelete <%#: Eval("IsEditable").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                        src='<%# Eval("IsEditable").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" value="<%#:Eval("MovementID") %>" bindingfield="MovementID" />
                                                    <input type="hidden" value="<%#:Eval("MovementNo") %>" bindingfield="MovementNo" />
                                                    <input type="hidden" value="<%#:Eval("MovementDateInDatePickerFormat") %>" bindingfield="MovementDateInDatePickerFormat" />
                                                    <input type="hidden" value="<%#:Eval("FromFALocationID") %>" bindingfield="FromFALocationID" />
                                                    <input type="hidden" value="<%#:Eval("ToFALocationID") %>" bindingfield="ToFALocationID" />
                                                    <input type="hidden" value="<%#:Eval("ToLocationCode") %>" bindingfield="ToLocationCode" />
                                                    <input type="hidden" value="<%#:Eval("ToLocationName") %>" bindingfield="ToLocationName" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="MovementNo" HeaderText="No. Pemindahan" HeaderStyle-Width="140px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MovementDateInString" HeaderText="Tgl. Mutasi" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="FromLocationName" HeaderText="Dari Lokasi" HeaderStyle-Width="250px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ToLocationName" HeaderText="Kepada Lokasi" HeaderStyle-Width="250px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Informasi Dibuat" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("CreatedByName") %></div>
                                                    <div>
                                                        <%#:Eval("cfCreatedDateInFullString") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <img class="imgPrint imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                        alt="" style="float: center; margin-left: 7px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div style="width: 100%; text-align: center">
                                        <span class="lblLink" id="lblAddData" style="text-align: center">
                                            <%= GetLabel("Add Data")%></span>
                                    </div>
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
                    <dxcp:ASPxCallbackPanel ID="cbpViewProcess" runat="server" Width="100%" ClientInstanceName="cbpViewProcess"
                        ShowLoadingPanel="false" OnCallback="cbpViewProcess_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewProcessEndCallback(s); }" />
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
