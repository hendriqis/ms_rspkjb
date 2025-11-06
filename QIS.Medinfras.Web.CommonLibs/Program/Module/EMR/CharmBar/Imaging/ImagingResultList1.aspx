<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="ImagingResultList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ImagingResultList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnIDCBCtl.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDt.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewDt.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnItemIDCBCtl.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnReferenceNoCBCtl.ClientID %>').val($(this).find('.accessionNo').html());
            }
        });

        $(function () {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('.btnViewReport').live('click', function () {
                var hdnIS0030 = $('#<%=hdnIS0030.ClientID %>').val();
                if (hdnIS0030 == "1") {
                    $('#<%=hdnResultReady.ClientID %>').val("0");

                    var ItemID = $(this).closest('tr').find('.ItemID').html();
                    var keyImagingID = $(this).closest('tr').find('.ImagingID').html();
                    var filterExpression = " ItemID='" + ItemID + "' and ImagingID = '" + keyImagingID + "' AND IsDeleted=0";


                    Methods.getObject("GetvPatientChargesDtImagingResultList", filterExpression, function (result) {
                        if (result != null) {
                            if (result.ResultGCTransactionStatus == "X121^001" || result.ResultGCTransactionStatus == "X121^999" || result.ResultGCTransactionStatus == "" || keyImagingID == 0) {
                                $('#<%=hdnResultReady.ClientID %>').val("0");
                            } else {
                                $('#<%=hdnResultReady.ClientID %>').val("1"); //ready
                            }
                        } else {
                            $('#<%=hdnResultReady.ClientID %>').val("0");
                        }
                    });

                    if ($('#<%=hdnResultReady.ClientID %>').val() == "0") {
                        showToast("ERROR", 'Error Message : ' + "untuk hasil dari pemeriksaan tersebut belum tersedia !");
                        return false;
                    }
                }
                var testOrderID = $('#<%=hdnIDCBCtl.ClientID %>').val();
                var itemID = $(this).closest('tr').find('.keyField').html();
                if (itemID != '' && testOrderID != '') {
                    var id = itemID + '|' + testOrderID;
                    var url = ResolveUrl("~/libs/Program/Module/EMR/CharmBar/Imaging/ViewImagingResultCtl1.ascx");
                    openUserControlPopup(url, id, 'Hasil Pemeriksaan Radiologi', 1000, 500);
                }

            });

            $('.btnViewPDF').live('click', function () {
                var fileName = $(this).closest('tr').find('.fileName').html();
                if (fileName != '') {
                    var url = $('#<%=hdnDocumentPathCBCtl.ClientID %>').val() + fileName;
                    window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
                }
                else {
                    showToast("ERROR", 'Error Message : ' + "Tidak ada file hasil untuk pemeriksaan ini !");
                }
            });

            function submitToPopup(f) {
                var w = window.open('', 'form-target', 'width=1366, height=768, any-other-option, ...');
                f.target = 'form-target';
                f.submit();
            };			

            $('.btnViewer').live('click', function () {
                $('#<%=hdnReferenceNoCBCtl.ClientID %>').val($(this).closest('tr').find('.accessionNo').html());
                //                var postData = "code=d57ada2e648c49d19b58f20fd21c5b93&number="+$('#<%=hdnReferenceNoCBCtl.ClientID %>').val();
                var postData = $('#<%=hdnReferenceNoCBCtl.ClientID %>').val();
                if (postData != '' && postData != '&nbsp;') {
                    if ($('#<%=hdnRISVendorCBCtl.ClientID %>').val() == "X081^04") {
                        var viewerUrlLink = $(this).closest('tr').find('.imageViewerLinkUrl').html();
                        if (viewerUrlLink == '' || viewerUrlLink == '&nbsp;') {
                            if ($('#<%=hdnViewerUrlCBCtl.ClientID %>').val() != '') {
                                var viewerUrl = $('#<%=hdnViewerUrlCBCtl.ClientID %>').val() + postData + "&id=1&redirect=y";
                                window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                            }
                            else {
                                displayErrorMessageBox("WARNING", "Belum ada hasil foto");
                            }
                        }
                        else {
                            window.open(viewerUrlLink, "popupWindow", "width=600, height=600,scrollbars=yes");
                        }
                    }
                    else if ($('#<%=hdnRISVendorCBCtl.ClientID %>').val() == "X081^05") {
                        var viewerUrl = $(this).closest('tr').find('.imageViewerLinkUrl').html();
                        window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    }
                    else if ($('#<%=hdnRISVendorCBCtl.ClientID %>').val() == "X081^02") {
                        var viewerUrl = $(this).closest('tr').find('.imageViewerLinkUrl').html();
                        if (viewerUrl == '&nbsp;') {
                            var Accession = $(this).closest('tr').find('.accessionNo').html();
                            var viewerUrl1 = 'http://192.100.10.10/NovaWeb/WebViewer/Account/Login?UserName=temp&Password=temp123&ReturnUrl=%2fNovaWeb%2fWebViewer%2fStudyBrowser%3fAccession%3d' + Accession + '&Accession=' + Accession;
                            
                            var mapForm = document.createElement("form");
                            mapForm.target = "_blank";
                            mapForm.method = "POST"; // or "post" if appropriate
                            mapForm.id = "form3";
                            mapForm.class = "form3";
                            mapForm.style.display = "none";
                            mapForm.action = viewerUrl1;

                            var mapInput2 = document.createElement("input");
                            mapInput2.type = "hidden";
                            mapInput2.name = "UserName";
                            mapInput2.value = "temp";
                            mapForm.appendChild(mapInput2);

                            var mapInput3 = document.createElement("input");
                            mapInput3.type = "hidden";
                            mapInput3.name = "Password";
                            mapInput3.value = "temp123";
                            mapForm.appendChild(mapInput3);

                            document.body.appendChild(mapForm);
                            submitToPopup(mapForm);
                        }
                        else {
                            window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                        }
                    }
                    else if ($('#<%=hdnRISVendorCBCtl.ClientID %>').val() == "X081^06") {
                        var viewerUrl = $('#<%=hdnViewerUrlCBCtl.ClientID %>').val(); // + postData + "&Username=hisuser&Password=hisuser";

                        var mapForm = document.createElement("form");
                        mapForm.target = "_blank";
                        mapForm.method = "POST"; // or "post" if appropriate
                        mapForm.id = "form2";
                        mapForm.style.display = "none";
                        mapForm.action = viewerUrl;

                        var mapInput2 = document.createElement("input");
                        mapInput2.type = "text";
                        mapInput2.name = "Username";
                        mapInput2.value = "hisuser";
                        mapForm.appendChild(mapInput2);

                        var mapInput3 = document.createElement("input");
                        mapInput3.type = "text";
                        mapInput3.name = "Password";
                        mapInput3.value = "hisuser";
                        mapForm.appendChild(mapInput3);

                        var mapInput1 = document.createElement("input");
                        mapInput1.type = "text";
                        mapInput1.name = "AccessionNumber";
                        mapInput1.value = postData;
                        mapForm.appendChild(mapInput1);

                        document.body.appendChild(mapForm);

                        map = window.open(viewerUrl, '', 'menubar=no,toolbar=no,height=' + (window.screen.availHeight - 30) + ',scrollbars=no,status=no,width=' + (window.screen.availWidth - 10) + ',left=0,top=0,dependent=yes');

                        //                        map = window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                        if (map) {
                            document.getElementById("form2").submit();
                        } else {
                            alert('You must allow popups for this map to work.');
                        }
                    }
                    else {
                        var viewerUrl = $('#<%=hdnViewerUrlCBCtl.ClientID %>').val() + postData;
                        window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    }
                }
                else if ($('#<%=hdnRISVendorCBCtl.ClientID %>').val() == "X081^07") {
                    var medicalNo = $('#<%=hdnMedicalNoCBCtl.ClientID %>').val().replaceAll('-', '');
                    var viewerUrl = $('#<%=hdnViewerUrlCBCtl.ClientID %>').val() + medicalNo;
                    window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                }
                else {
                    showToast("ERROR", 'Error Message : ' + "Accession Number untuk membuka file image tidak tersedia !");
                }
            });
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpressionCBCtl.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

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

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>

    <input type="hidden" value="" id="hdnIDCBCtl" runat="server" />   
    <input type="hidden" value="" id="hdnItemIDCBCtl" runat="server" />  
    <input type="hidden" value="" id="hdnReferenceNoCBCtl" runat="server" />   
    <input type="hidden" value="" id="hdnViewerUrlCBCtl" runat="server" />    
    <input type="hidden" value="" id="hdnDocumentPathCBCtl" runat="server" />    
    <input type="hidden" id="hdnHealthcareServiceUnitIDCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnRISVendorCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnIsRISUsingPDFResultCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnMedicalNoCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnIS0030" runat="server" value="0" />
    <input type="hidden" value="0" id="hdnResultReady" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Tanggal Transaksi") %> - <%=GetLabel("Time") %></div>
                                                    <div><%=GetLabel("No. Transaksi") %> | <%=GetLabel("Status") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("TransactionDateInString")%> | <%#: Eval("TransactionTime") %></div>
                                                    <div><%#: Eval("TransactionNo")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pemeriksaan untuk pasien ini")%>
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
                </div>
            </td>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewDt_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Nama Pemeriksaan") %></div>
                                                    <div><%=GetLabel("Dokter Pelaksana") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><span style="font-weight:bold"><%#: Eval("ItemName1") %></span></div>
                                                    <div><span style="color:blue"><%#: Eval("ParamedicName")%></span></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DetailReferenceNo" HeaderText = "Accession No." HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-CssClass="accessionNo"/>
                                            <asp:BoundField DataField="FileName" HeaderText = "File Name" HeaderStyle-CssClass="hiddenColumn" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-CssClass="hiddenColumn fileName"/>
                                            <asp:BoundField DataField="ImageViewerLinkUrl" HeaderText = "Viewer Link" HeaderStyle-CssClass="hiddenColumn" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-CssClass="hiddenColumn imageViewerLinkUrl"/>
                                           <asp:BoundField DataField="ResultGCTransactionStatus" HeaderText = "" HeaderStyle-CssClass="hiddenColumn" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-CssClass="hiddenColumn ResultGCTransactionStatus"/>
                                           <asp:BoundField DataField="ImagingID" HeaderText = "" HeaderStyle-CssClass="hiddenColumn" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-CssClass="hiddenColumn ImagingID"/>
                                           <asp:BoundField DataField="ItemID" HeaderText = "" HeaderStyle-CssClass="hiddenColumn" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-CssClass="hiddenColumn ItemID"/>
                                           
                                           <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("STATUS BRIDGING")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("cfRISBridgingStatus")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("STATUS HASIL")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("ResultTransactionStatus")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Report" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <div>
                                                        <input type="button" id="btnViewReport" runat="server" class="btnViewReport" value="View Report"
                                                            style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                                    </div>                                                                       
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File PDF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <div>
                                                        <input type="button" id="btnViewPDF" runat="server" class="btnViewPDF" value="View PDF"
                                                            style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                                    </div>                                                                       
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PACS Viewer" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <div>
                                                        <input type="button" id="btnViewer" runat="server" class="btnViewer" value="View Image"
                                                            style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                                    </div>                                                                       
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pemeriksaan untuk pasien ini") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt"></div>
                        </div>
                    </div> 
                </div>
            </td>
        </tr>
    </table>

    
</asp:Content>
