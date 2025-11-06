<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="VisitHistory.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VisitHistory"
    EnableViewState="false" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.mouse.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.draggable.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.droppable.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.effects.core.js")%>'></script>
    <script id="dxis_episodesummaryctl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>'
        type='text/javascript'></script>
    <script id="dxis_episodesummaryctl3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>'
        type='text/javascript'></script>
    <script type="text/javascript">
        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                //$('.tdMedicalHistoryContent').each(function () {
                //$(this).attr('tempHtml', $(this).html());
                //$(this).html('');
                //$(this).html($(this).attr('tempHtml'));
                //$(this).removeAttr('tempHtml');
                //});
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

            //$('.tdMedicalHistoryContent').each(function () {
            // $(this).html($(this).attr('tempHtml'));
            // $(this).removeAttr('tempHtml');
            //});
            registerEventHandler();
        }
        //#endregion

        function registerEventHandler() {
            $('.trDraggable').each(function () {
                $(this).width($(this).parent().width());
            });
            $('.trDraggable').draggable({
                helper: 'clone',
                drag: function (event, ui) {
                    //insideDropZone = false;
                }
            });

            $('#ulAreaDrop > li').droppable({
                drop: function (event, ui) {
                    //insideDropZone = true;
                    var $tr = ui.helper.clone();
                    var visitID = $tr.find('.keyField').html();
                    var isFromMigration = $tr.find('.hiddenColumn').html();
                    if ($('#ulAreaDrop > li .hdnMedicalHistoryVisitID[value=' + visitID + ']').length < 1) {
                        $container = $(this);
                        $container.find('.hdnMedicalHistoryVisitID').val(visitID);
                        $container.find('.tdMedicalHistoryContent').html('');
                        $container.find('.tdMedicalHistoryContent').hide();
                        $container.find('.imgLoadingMedicalHistory').show();
                        var url = ResolveUrl('~/Libs/Program/Module/EMR/CharmBar/MedicalHistory/MedicalHistoryDetailCtl.ascx');
                        Methods.getHtmlControl(url, visitID + '|' + isFromMigration, function (result) {
                            $container.find('.imgLoadingMedicalHistory').hide();
                            $container.find('.tdMedicalHistoryContent').show();
                            $container.find('.tdMedicalHistoryContent').html(result.replace(/\VIEWSTATE/g, ''));
                        }, function () {
                            $container.find('.tdMedicalHistoryContent').show();
                            $container.find('.imgLoadingMedicalHistory').hide();
                        });
                    }
                }
            });
        }

        $(function () {
            registerEventHandler();
            $('.lnkMedicalRecordSummary a').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var isFromMigration = $(this).closest('tr').find('.hiddenColumn').html();

                if (isFromMigration != '1') {
                    var url = ResolveUrl("~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryCtl.ascx");
                    openUserControlPopup(url, id, 'Medical Record - Detail', 1300, 600);
                }
                else {
                    showToast("INFORMATION", "Sorry, this record is from external or previous system - cannot be displayed in this mode")
                }
            });

            $('.lnkMedicalResume a').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var isFromMigration = $(this).closest('tr').find('.hiddenColumn').html();
                var mrID = 0;
                var department = '';

                Methods.getObject("GetvConsultVisit7List", "VisitID = '" + id + "'", function (result) {
                    if (result != null) {
                        department = result.DepartmentID;

                        Methods.getObject("GetvMedicalResumeList", "VisitID = '" + id + "' AND IsDeleted = 0", function (result2) {
                            if (result2 != null) {
                                mrID = result2.ID;
                            }
                        });
                    }
                });

                if (mrID == 0) {
                    showToast("INFORMATION", "Sorry, this visit does not have a Medical Resume")
                }
                else {
                    if (isFromMigration != '1') {
                        if (department == 'INPATIENT') {
                            var url = ResolveUrl("~/Libs/Controls/EMR/MedicalResume/IPMedicalResumeCtl1.ascx");
                            var param = id + "|" + mrID;
                            openUserControlPopup(url, param, 'Medical Resume', 1300, 600);
                        }
                        else {
                            var url = ResolveUrl("~/Libs/Controls/EMR/MedicalResume/OPMedicalResumeCtl1.ascx");
                            var param = id + "|" + mrID;
                            openUserControlPopup(url, param, 'Medical Resume', 1300, 600);
                        }
                    }
                    else {
                        showToast("INFORMATION", "Sorry, this record is from external or previous system - cannot be displayed in this mode")
                    }
                }
            });
        });

    </script>
    <style type="text/css">
        #ulAreaDrop
        {
            margin: 0;
            padding: 0;
            height: 600px;
        }
        #ulAreaDrop > li
        {
            width: 410px;
            border: 1px solid #AAA;
            height: 100%;
            z-index: 10000;
            display: inline-block;
            list-style-type: none;
            margin-right: 10px;
            margin-left: 10px;
            position: relative;
        }
        .trDraggable
        {
            cursor: pointer;
            z-index: 19999;
        }
        .divDropHereInfo
        {
            color: Gray;
            width: 60%;
            text-align: center;
            margin-top: 100px;
        }
        #ulAreaDrop > li .imgLoadingMedicalHistory
        {
            display: none;
            position: absolute;
            left: 44%;
            top: 44%;
        }
    </style>
    <table style="width: 100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width: 30%" />
            <col style="width: 10px" />
        </colgroup>
        <tr style="height: 550px">
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 510px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Visit Information")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("VisitDateInString")%>,
                                                        <%#: Eval("RegistrationNo")%></div>
                                                    <div>
                                                        <%#: Eval("ServiceUnitName")%></div>
                                                    <div>
                                                        <%#: Eval("ParamedicName")%></div>
                                                    <div>
                                                        <%#: Eval("DisplayPatientDiagnosis")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:HyperLinkField HeaderText="View" Text="Details" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px" ItemStyle-CssClass="lnkMedicalRecordSummary" Visible="true" />
                                            <asp:HyperLinkField HeaderText="Medical Resume" Text="View" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px" ItemStyle-CssClass="lnkMedicalResume"/>
                                            <asp:BoundField DataField="IsDataMigration" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Patient Medical History To Display")%>
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
            </td>
            <td>
                &nbsp;
            </td>
            <td valign="top">
                <ul id="ulAreaDrop">
                    <li class="boxShadow">
                        <img class="imgLoadingMedicalHistory" style="display: none" src='<%= ResolveUrl("~/Libs/Images/Loading.gif")%>'
                            alt="" />
                        <input type="hidden" class="hdnMedicalHistoryVisitID" value="" />
                        <table style="width: 100%; height: 100%">
                            <tr>
                                <td class="tdMedicalHistoryContent" align="center" valign="top">
                                    <div class="divDropHereInfo">
                                        Drag and drop medical history here to compare</div>
                                </td>
                            </tr>
                        </table>
                    </li>
                    <li class="boxShadow">
                        <img class="imgLoadingMedicalHistory" src='<%= ResolveUrl("~/Libs/Images/Loading.gif")%>'
                            alt="" />
                        <input type="hidden" class="hdnMedicalHistoryVisitID" value="" />
                        <table style="width: 100%; height: 100%">
                            <tr>
                                <td class="tdMedicalHistoryContent" align="center" valign="top">
                                    <div class="divDropHereInfo">
                                        Drag and drop medical history here to compare</div>
                                </td>
                            </tr>
                        </table>
                    </li>
                </ul>
            </td>
        </tr>
    </table>
</asp:Content>
