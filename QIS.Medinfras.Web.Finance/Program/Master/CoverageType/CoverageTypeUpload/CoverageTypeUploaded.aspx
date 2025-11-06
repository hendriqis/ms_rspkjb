<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="CoverageTypeUploaded.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.CoverageTypeUploaded" %>

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
    <li id="btnSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
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
    <script type="text/javascript">
        function onLoad() {
        }

        //#region Button Refresh, Save, Approved
        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        $('#<%=btnSave.ClientID %>').live('click', function () {
            showToastConfirmation('Apakah yakin akan proses SAVE ?', function (result) {
                if (result) {
                    onCustomButtonClick('save');
                } else {
                    cbpView.PerformCallback('refresh');
                }
            })
        });
        //#endregion

        //#region Download

        $('#<%=btnDownload.ClientID %>').live('click', function () {
            onCustomButtonClick('download');
        });

        function downloadDocument(stringparam) {
            if (cboCoverageType.GetValue() == "1") {
                var fileName = "COVERAGE_TYPE.csv";
            } else if (cboCoverageType.GetValue() == "2") {
                var fileName = "COVERAGE_TYPE_DEPARTMENT.csv";
            } else if (cboCoverageType.GetValue() == "3") {
                var fileName = "COVERAGE_TYPE_DEPARTMENT_CLASS.csv";
            } else if (cboCoverageType.GetValue() == "4") {
                var fileName = "COVERAGE_TYPE_ITEM.csv";
            } else if (cboCoverageType.GetValue() == "5") {
                var fileName = "COVERAGE_TYPE_ITEM_CLASS.csv";
            } else if (cboCoverageType.GetValue() == "6") {
                var fileName = "COVERAGE_TYPE_ITEM_GROUP.csv";
            } else if (cboCoverageType.GetValue() == "7") {
                var fileName = "COVERAGE_TYPE_ITEM_GROUP_CLASS.csv";
            } else {
                var fileName = "COVERAGE_TYPE_SERVICE_UNIT.csv";
            }

            var link = document.createElement("a");
            link.href = 'data:text/csv,' + encodeURIComponent(stringparam);
            link.download = fileName;
            link.click();
        }
        //#endregion

        //#region Upload
        $('#<%=btnUpload.ClientID %>').die('change');
        $('#<%=btnUpload.ClientID %>').live('click', function () {
            document.getElementById('<%=CoverageDocumentUpload.ClientID %>').click();
        });

        $('#<%=CoverageDocumentUpload.ClientID %>').die('change');
        $('#<%=CoverageDocumentUpload.ClientID %>').live('change', function () {
            readURL(this);

            if ($('#<%=hdnUploadedFile.ClientID %>').val() != "" && $('#<%=hdnUploadedFile.ClientID %>').val() != null) {
                onCustomButtonClick('upload');
            } else {
                displayErrorMessageBox('Upload Failed', 'Silahkan coba lagi.');
            }
        });

        function readURL(input) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#<%=hdnUploadedFile.ClientID %>').val(e.target.result);
            };
            reader.readAsDataURL(input.files[0]);
        }
        //#endregion

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "download") {
                downloadDocument(retval);
            }
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

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region detail
        $('#<%=grdView.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            $('#containerDetail2').hide();
            if ($trDetail.attr('class') != 'trDetail') {
                $('#ulTabGrdDetail li:eq(0)').click();

                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='6'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('.grdDetail1 tr:gt(0)').remove();
                $('.grdDetail2 tr:gt(0)').remove();
                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewDetail2.PerformCallback('refresh');
                cbpViewDetail1.PerformCallback('refresh');
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $('.grdDetail1 tr:gt(0)').remove();
                $('.grdDetail2 tr:gt(0)').remove();

                $trDetail.remove();
            }
        });

        $('.lnkDepartment a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeUpload/CoverageTypeDepartmentUploadCtl.ascx");
            openUserControlPopup(url, id, 'Facility', 1000, 500);
        });

        $('.lnkItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeUpload/CoverageTypeItemUploadCtl.ascx");
            openUserControlPopup(url, id, 'Item', 1000, 520);
        });

        $('.lnkItemGroup a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeUpload/CoverageTypeItemGroupUploadCtl.ascx");
            openUserControlPopup(url, id, 'Item Group', 1000, 520);
        });

        $('.lnkDepartmentClass a').live('click', function () {
            var coverageTypeID = $('#<%=hdnExpandID.ClientID %>').val();
            var departmentID = $(this).closest('tr').find('.keyField').html();
            var param = coverageTypeID + '|' + departmentID;
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeUpload/CoverageTypeDepartmentClassUploadCtl.ascx");
            openUserControlPopup(url, param, 'Class', 1000, 500);
        });

        $('.lnkServiceUnit a').live('click', function () {
            var coverageTypeID = $('#<%=hdnExpandID.ClientID %>').val();
            var departmentID = $(this).closest('tr').find('.keyField').html();
            var param = coverageTypeID + '|' + departmentID;
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeUpload/CoverageTypeServiceUnitUploadCtl.ascx");
            openUserControlPopup(url, param, 'Service Unit', 1000, 500);
        });

        $('.lnkItemClass a').live('click', function () {
            var coverageTypeID = $('#<%=hdnExpandID.ClientID %>').val();
            var itemID = $(this).closest('tr').find('.keyField').html();
            var param = coverageTypeID + '|' + itemID;
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeUpload/CoverageTypeItemClassUploadCtl.ascx");
            openUserControlPopup(url, param, 'Class', 1000, 500);
        });
        //#endregion

        //#region Paging Detail 2
        function onCbpViewDetail2EndCallback(s) {
            $('#containerImgLoadingViewDetail2').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('.grdDetail2 tr:eq(1)').click();

                pageCountGrdDetail2 = pageCount;
            }
            else {
                $('.grdDetail2 tr:eq(1)').click();
            }
        }
        var pageCountGrdDetail2 = -1;
        //#endregion
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <input type="hidden" id="hdnUploadedFile" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 30%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Filter Skema Jaminanan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboCoverageType" ClientInstanceName="cboCoverageType" Width="250px"
                                        runat="server">
                                    </dxe:ASPxComboBox>
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
                                    <asp:FileUpload ID="CoverageDocumentUpload" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail'));showLoadingPanel(); }"
                                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="CoverageTypeUploadID" HeaderStyle-CssClass="keyField"
                                                    ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                                            alt='' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CoverageTypeUploadCode" HeaderText="Kode Tipe Jaminan"
                                                    HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CoverageTypeUploadName" HeaderText="Nama Tipe Jaminan"
                                                    HeaderStyle-HorizontalAlign="Left" />
                                                <asp:HyperLinkField HeaderText="Instalasi" Text="Instalasi" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="150px" ItemStyle-CssClass="lnkDepartment" />
                                                <asp:HyperLinkField HeaderText="Kelompok Item" Text="Kelompok Item" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="150px" ItemStyle-CssClass="lnkItemGroup" />
                                                <asp:HyperLinkField HeaderText="Item" Text="Item" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="150px" ItemStyle-CssClass="lnkItem" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Data Tidak Tersedia")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
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
                    </div>
<div id="tempContainerGrdDetail" style="display:none">
        <div id="containerGrdDetail" class="borderBox" style="width: 100%;padding: 10px 5px;">
            <div class="containerUlTabPage">
                <ul class="ulTabPage" id="ulTabGrdDetail">
                    <li class="selected" contentid="containerDetail1"><%=GetLabel("Instalasi") %></li>
                    <li contentid="containerDetail2"><%=GetLabel("Item") %></li>
                </ul>
            </div>
            <div style="position: relative;">
                <div id="containerDetail1" class="containerGrdDt">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail1" runat="server" Width="100%" ClientInstanceName="cbpViewDetail1"
                        ShowLoadingPanel="false" OnCallback="cbpViewDetail1_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail1').show(); }"
                            EndCallback="function(s,e){ $('#containerImgLoadingViewDetail1').hide(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                    <asp:ListView runat="server" ID="lvwDetail1">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect grdDetail1" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th style="width:250px" rowspan="2"  align="left"><%=GetLabel("Instalasi")%></th>
                                                    <th colspan="4"><%=GetLabel("Pelayanan")%></th>
                                                    <th colspan="4"><%=GetLabel("Peralatan dan Bahan Medis")%></th>
                                                    <th colspan="4"><%=GetLabel("Barang Umum")%></th>  
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Kelas")%></th>  
                                                </tr>
                                                <tr>  
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>  
                                                 
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                    
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="15">
                                                        <%=GetLabel("Data Tidak Tersedia")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th style="width:250px" rowspan="2" align="left"><%=GetLabel("Instalasi")%></th>
                                                    <th colspan="4"><%=GetLabel("Pelayanan")%></th>
                                                    <th colspan="4"><%=GetLabel("Peralatan dan Bahan Medis")%></th>
                                                    <th colspan="4"><%=GetLabel("Barang Umum")%></th>    
                                                    <th style="width:100px" rowspan="2"><%=GetLabel("Unit Pelayanan")%></th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Kelas")%></th>
                                                </tr>
                                                <tr>  
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                 
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                    
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("DepartmentID")%></td>
                                                <td><%#: Eval("DepartmentName")%></td>                                                

                                                <td align="right"><%#: Eval("DisplayMarkupAmount1")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount1")%></td>
                                                <td align="right"><%#: Eval("DisplayCoverageAmount1")%></td>
                                                <td align="right"><%#: Eval("DisplayCashBackAmount1")%></td>

                                                <td align="right"><%#: Eval("DisplayMarkupAmount2")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount2")%></td>
                                                <td align="right"><%#: Eval("DisplayCoverageAmount2")%></td>
                                                <td align="right"><%#: Eval("DisplayCashBackAmount2")%></td>

                                                <td align="right"><%#: Eval("DisplayMarkupAmount3")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount3")%></td>
                                                <td align="right"><%#: Eval("DisplayCoverageAmount3")%></td>
                                                <td align="right"><%#: Eval("DisplayCashBackAmount3")%></td>

                                                <td align="center" class="lnkServiceUnit"><a><%=GetLabel("Unit Pelayanan")%></a></td> 
                                                <td align="center" class="lnkDepartmentClass"><a><%=GetLabel("Kelas")%></a></td>                                            
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel> 
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail1">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>   
                </div> 
                <div id="containerDetail2" class="containerGrdDt">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail2" runat="server" Width="100%" ClientInstanceName="cbpViewDetail2"
                        ShowLoadingPanel="false" OnCallback="cbpViewDetail2_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail2').show(); }"
                            EndCallback="function(s,e){ onCbpViewDetail2EndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto">
                                    <asp:ListView runat="server" ID="lvwDetail2">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect grdDetail2" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th style="width:350px" align="left"><%=GetLabel("Item")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                    <th style="width:70px"><%=GetLabel("Kelas")%></th>                                           
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="7">
                                                        <%=GetLabel("Data Tidak Tersedia")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th style="width:350px" align="left"><%=GetLabel("Item")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                    <th style="width:70px"><%=GetLabel("Kelas")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("ItemID")%></td>
                                                <td><%#: Eval("ItemName1")%></td>

                                                <td align="right"><%#: Eval("DisplayMarkupAmount")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount")%></td>
                                                <td align="right"><%#: Eval("DisplayCoverageAmount")%></td>
                                                <td align="right"><%#: Eval("DisplayCashBackAmount")%></td>
                                                <td align="center" class="lnkItemClass"><a><%=GetLabel("Kelas")%></a></td>   
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>  
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail2">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div> 
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDetail2"></div>
                        </div>
                    </div> 
                </div>
            </div>
        </div>
    </div>
</asp:Content>