<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="PatientInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.PatientInformation" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        

        var isHoverTdExpand = false;
        $('.lvwView tr:gt(0) td.tdExpand').live({
            mouseenter: function () { isHoverTdExpand = true; },
            mouseleave: function () { isHoverTdExpand = false; }
        });

        $('.lvwView tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent().next();
            if (!$tr.is(":visible")) {
                //$trCollapse = $('.trDetail').filter(':visible');
                //$trCollapse.hide();
                //$trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $tr.show('slow');
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $tr.hide('fast');
            }
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onBeforeOpenTransactionDt() {
            return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
        }

      


    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnLstHealthcareServiceUnitID" runat="server" />
    <div style="padding:15px">
        <div style="display:none"><asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();" OnClick="btnOpenTransactionDt_Click" /></div>
        <input type="hidden" runat="server" id="hdnTransactionNo" value="" />
        <div class="pageTitle"><%=GetMenuCaption()%> : <%=GetLabel("Pilih Pasien")%></div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <fieldset id="fsPatientList">  
                        <table class="tblEntryContent" style="width:60%;">
                            <colgroup>
                                <col style="width:25%"/>
                                <col/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Ruang Perawatan")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Jadwal Makan")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMealTime" ClientInstanceName="cboMealTime" Width="100%" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
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
                            </tr>--%>
                            <%--<tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Barcode Entri (No RM)")%></label></td>
                                <td><asp:TextBox ID="txtBarcodeEntry" Width="120px" runat="server" /></td>
                            </tr>--%>
                        </table>
                    </fieldset>
                    <div style="padding:7px 0 0 3px;font-size:0.95em">
                        <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                    </div>

                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th style="width:20px"></th>
                                                    <th style="width:200px"><%=GetLabel("Informasi Kunjungan")%></th>
                                                    <th style="width:200px"><%=GetLabel("Informasi Rawat Inap")%></th>
                                                    <th style="width:380px"><%=GetLabel("Informasi Pasien")%></th>
                                                    <th style="width:250px"><%=GetLabel("Informasi Gizi")%></th>
                                                    <th><%=GetLabel("Informasi Jadwal Makan")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="6">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th style="width:20px"></th>
                                                    <th style="width:200px"><%=GetLabel("Informasi Kunjungan")%></th>
                                                    <th style="width:200px"><%=GetLabel("Informasi Rawat Inap")%></th>
                                                    <th style="width:380px"><%=GetLabel("Informasi Pasien")%></th>
                                                    <th style="width:250px"><%=GetLabel("Informasi Gizi")%></th>
                                                    <th><%=GetLabel("Informasi Jadwal Makan")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="tdExpand" style="text-align:center">
                                                    <img class="imgExpand" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                                </td>
                                                <td>
                                                    <div><%#: Eval("RegistrationNo") %></span>
                                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("RegistrationNo") %>' />
                                                    </div>                                                 
                                                </td>
                                                <td>
                                                    <div><%#: Eval("RoomName") %> (<%#: Eval("BedCode") %>)</div>                                           
                                                </td>
                                                <td>
                                                    <div><%#: Eval("PatientName") %> (<%#: Eval("DateOfBirthInString") %>, <%#: Eval("Gender") %>, <%#: Eval("MedicalNo") %>, <%#: Eval("Religion")%>)</div>                                           
                                                </td>
                                                <td><div><asp:Label runat="server" ID="lblNutritionInformation" /></div></td>
                                                <td>
                                                    <div><%#: Eval("MealTime")%></div>
                                                </td>
                                            </tr>
                                            <tr style="display:none" class="trDetail">
                                                <td><div> </div></td>
                                                <td>
                                                    <div>
                                                        <div><%#: Eval("RegistrationNo") %></span></div>
                                                        <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                                        <div><%#: Eval("ParamedicName")%></div>                                                    
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <div><%#: Eval("ClassName") %></span></div>
                                                        <div style="float:left"><%#: Eval("ServiceUnitName")%></div>
                                                        <div style="margin-left:100px"><%#: Eval("RoomName")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px">
                                                        <div><%#: Eval("PatientName") %></div>
                                                        <input type="hidden" value='<%#: Eval("GCGender")%>' class="hdnPatientGender" />
                                                        <table cellpadding="0" cellspacing="0">
                                                            <colgroup>
                                                                <col style="width:100px"/>
                                                                <col style="width:10px"/>
                                                                <col style="width:80px"/>
                                                                <col style="width:50px"/>
                                                                <col style="width:10px"/>
                                                                <col style="width:120px"/>
                                                            </colgroup>
                                                            <tr>
                                                                <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Nama Panggilan")%></td>
                                                                <td>&nbsp;</td>
                                                                <td><%#: Eval("PatientName")%></td>
                                                                <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("No RM")%></td>
                                                                <td>&nbsp;</td>
                                                                <td><%#: Eval("MedicalNo")%></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Tanggal Lahir")%></td>
                                                                <td>&nbsp;</td>
                                                                <td><%#: Eval("DateOfBirthInString")%></td>
                                                                <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Umur")%></td>
                                                                <td>&nbsp;</td>
                                                                <td><%#: Eval("PatientAge")%></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Agama")%></td>
                                                                <td>&nbsp;</td>
                                                                <td></td>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px">
                                                    </div>
                                                </td>
                                                <td><div>&nbsp;</div></td>
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
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging"></div>
                        </div>
                    </div> 
                </td>
            </tr>
        </table>
    </div>

    <div style="display: none">
       
    </div>
</asp:Content>
