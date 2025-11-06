<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
CodeBehind="OperatingRoomScheduleInfo1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.OperatingRoomScheduleInfo1" %>

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

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <style type="text/css">        
        #divRoomInfo > a { text-decoration: none; padding:1px; color: Gray; }
        #divRoomInfo > a.selected { background-color: #f44336!important; color: White; }
        
        .tdSchedule {border:1px solid #AAA;}
        .tdScheduleTime {font-size:18px!important; color:#4d0000!important;text-align:center}    
    </style>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setCalendar("1");

            //#region Room Code
            $('#divRoomInfo a').click(function () {
                $('#divRoomInfo a.selected').removeClass('selected');
                $(this).addClass('selected');
                var roomCode = $(this).attr('contentID');
                var roomName = $(this).attr('contentName');

                $('#<%=hdnOperatingRoomCode.ClientID %>').val(roomCode);
                displayRoomDetail(roomCode, roomName);
            });
            //#endregion

            $('#<%=lblSelectedDate.ClientID %>').html($('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val());

            $('#divRoomInfo a').first().click();
        });

        function setCalendar(isAllowBackDate) {
            if (isAllowBackDate == '1') {
                $("#calAppointment").datepicker({
                    defaultDate: "w",
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: "dd-mm-yy",
                    //minDate: "0",
                    onSelect: function (dateText, inst) {
                        $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                        $('#<%=lblSelectedDate.ClientID %>').html(dateText);
                        $('#divRoomInfo a').first().click();
                    }
                });
            }
            else {
                $("#calAppointment").datepicker({
                    defaultDate: "w",
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: "dd-mm-yy",
                    minDate: "0",
                    onSelect: function (dateText, inst) {
                        $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                        $('#<%=lblSelectedDate.ClientID %>').html(dateText);
                        $('#divRoomInfo a').first().click();
                    }
                });
            }
        }

        //#region  Slide Show
        var slideIndex = 1;
        showDivs(slideIndex);

        function plusDivs(n) {
            showDivs(slideIndex += n);
        }

        function showDivs(n) {
            var i;
            var x = document.getElementsByClassName("mySlides");
            if (n > x.length) { slideIndex = 1 }
            if (n < 1) { slideIndex = x.length }
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            x[slideIndex - 1].style.display = "block";
        }
        //#endregion Slide Show

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshControl();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        function displayRoomDetail(roomCode, roomName) {
            var i, x, tablinks;
            x = document.getElementsByClassName("roomDetailInfo");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            $('#<%=divRoomName.ClientID %>').html(roomName);
            cbpViewDt.PerformCallback('refresh');
        }

        function isValidDate(value) {
            var dateWrapper = new Date(value);
            return !isNaN(dateWrapper.getDate());
        }
    </script>
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestDate" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitImagingID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitLaboratoryID" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" id="hdnOperatingRoomID" runat="server" value="" />
    <input type="hidden" id="hdnOperatingRoomCode" runat="server" value="" />
    <table style="width:100%">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <div class="w3-border w3-animate-left"> 
                    <table class="tblContentArea">
                        <tr>
                            <td style="padding: 5px; vertical-align: top">
                                <table style="width: 100%;">
                                    <colgroup>
                                        <col style="width: 200px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDate" />
                                                <div id="calAppointment">
                                                </div>
                                        </td>
                                        <td rowspan="3" valign="top" style="padding-bottom: 10px">
                                            <table border="0" cellpadding="0" cellspacing="1" width="100%" class="w3-table w3-border">
                                                <colgroup>
                                                    <col style="width:120px"/>
                                                    <col />
                                                    <col style="width:120px"/>
                                                    <col />
                                                </colgroup>
                                                <tr>                                                    
                                                    <td colspan="4">
                                                        <div id="lblSelectedDate" runat="server" class="lblSelectedDate w3-blue w3-xlarge" style="text-align:center; text-shadow:1px 1px 0 #444; width:100%; padding-bottom:2px">dd-MM-yyyy</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center">
                                                         <div id="divRoomInfo" class="w3-bar w3-xlarge">
                                                            <asp:Repeater ID="lstRoomCode" runat="server">
                                                                <ItemTemplate>
                                                                    <a href="#" class="class="lnkRoomCode w3-button" title='<%#: Eval("RoomName") %>' contentID = '<%#: Eval("RoomCode") %>' contentName='<%#: Eval("RoomName") %>'><%#: Eval("RoomCode") %></a>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div> 
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center">
                                                        <div id="divRoomName" runat="server" class="w3-win8-emerald w3-xlarge" style="text-align:center; text-shadow:1px 1px 0 #444; width:100%; padding-bottom:2px">Ruang Operasi</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center">
                                                        <div class="w3-animate-left" style="text-align:center">
                                                            <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                                                ShowLoadingPanel="false" OnCallback = "cbpViewDt_Callback">
                                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                                                    EndCallback="function(s,e){ $('#containerImgLoadingViewDt').hide(); }" />
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent2" runat="server">    
                                                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid1">
                                                                            <div id="containerAppointment" class="containerAppointment">
                                                                                <asp:GridView ID="grdAppointment" runat="server" CssClass="grdSelected grdAppointment"
                                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                                    OnRowDataBound="grdAppointment_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderStyle-Width="100px"  HeaderStyle-HorizontalAlign="Center"
                                                                                            ItemStyle-HorizontalAlign="Center">
                                                                                            <HeaderTemplate>
                                                                                                <div class="tdTime" style="text-align:center">
                                                                                                    <%=GetLabel("WAKTU OPERASI") %> </div>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <div class="tdTime w3-large">
                                                                                                    <%#: Eval("DisplayTime") %></div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField ItemStyle-CssClass="tdAppointment">
                                                                                            <HeaderTemplate>
                                                                                                <div class="tdTime" style="text-align:left">
                                                                                                    <%=GetLabel("DATA JADWAL") %> </div>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Repeater ID="rptOrderInformation" runat="server">
                                                                                                    <HeaderTemplate>
                                                                                                        <ul style="list-style-type:none;">
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <li style="padding-bottom:2px">
                                                                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color:#e6e6e6" >
                                                                                                                <colgroup>
                                                                                                                    <col style="width:100px;" />
                                                                                                                    <col style="width:77px" />
                                                                                                                    <col />
                                                                                                                    <col style="width:70px" />
                                                                                                                </colgroup>
                                                                                                                <tr>
                                                                                                                    <td style="background-color:#f2f2f2;">
                                                                                                                        <div style="text-align:left"><img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="77px" width="45px" style="float:left;margin-right: 10px;" /></div>
                                                                                                                    </td>
                                                                                                                    <td style="background-color:#f2f2f2; width:100%">
                                                                                                                        <div><span style="font-weight: bold; font-size:11pt"><%#: Eval("PatientName") %></span></div>
                                                                                                                        <div><%#: Eval("MedicalNo") %>, <%#: Eval("RegistrationNo")%>, <%#: Eval("cfDateOfBirth")%></div>
                                                                                                                        <div style="font-style:italic"><%#: Eval("cfPatientLocation")%></div>
                                                                                                                        <div><%#: Eval("ParamedicName")%></div>
                                                                                                                        <div><%#: Eval("BusinessPartnerName")%></div>
                                                                                                                    </td>
                                                                                                                    <td style="background-color:#f2f2f2">                                                                                                                
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </li>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        </ul>
                                                                                                    </FooterTemplate>
                                                                                                </asp:Repeater>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <EmptyDataTemplate>
                                                                                        <%=GetLabel("Belum ada penjadwalan kamar operasi untuk tanggal yang dipilih")%>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dxcp:ASPxCallbackPanel>
                                                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>

    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteRoomSchedule" runat="server" Width="100%" ClientInstanceName="cbpDeleteRoomSchedule"
            ShowLoadingPanel="false" OnCallback="cbpDeleteRoomSchedule_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpRoomScheduleStatusEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpStartOrder" runat="server" Width="100%" ClientInstanceName="cbpStartOrder"
            ShowLoadingPanel="false" OnCallback="cbpStartOrder_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpRoomScheduleStatusEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpStopOrder" runat="server" Width="100%" ClientInstanceName="cbpStopOrder"
            ShowLoadingPanel="false" OnCallback="cbpStopOrder_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpRoomScheduleStatusEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>

