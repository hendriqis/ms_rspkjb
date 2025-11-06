<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BodyDiagramEditCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.BodyDiagramEditCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

    
<script type="text/javascript" id="dxis_bodydiagrameditctl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagrameditctl3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagrameditctl4" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.mouse.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagrameditctl5" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.draggable.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagrameditctl6" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.droppable.js")%>'></script>

<script type="text/javascript" id="dxss_bodydiagrameditctl">
    //#region Deklarasi Variabel
    var ID = '<%=patientBodyDiagramID%>';
    var currentSymbol;   //menyimpan symbol yang baru saja didrop. Misal W1, B1, dst
    var currentSymbolEdit;   //menyimpan symbol yang sedang diedit.
    var $currentCloneSymbol; // Menyimpan symbol yang baru saja diclone
    var memNotesText;
    var ctrW = { val: '<%=ctrW%>' }; // Counter Symbol W
    var ctrB = { val: '<%=ctrB%>' }; // Counter Symbol B
    var ctrF = { val: '<%=ctrF%>' }; // Counter Symbol F
    var ctrC = { val: '<%=ctrC%>' }; // Counter Symbol C
    var ctrD = { val: '<%=ctrD%>' }; // Counter Symbol D
    var ctrS = { val: '<%=ctrS%>' }; // Counter Symbol S
    var imgEditUrl = '<%= ResolveUrl("~/Libs/Images/Button/edit.png")%>';
    var imgDeleteUrl = '<%= ResolveUrl("~/Libs/Images/Button/delete.png")%>';
    //#endregion

    //#region Init
    $('.btnEditRemarks').click(OnEditNotes);
    $('.btnDeleteRemarks').click(OnDeleteNotes);

    $('.insideDropZone').each(function () {
        $imgWrapper = $('.containerImageEdit');
        var top = parseFloat($(this).find('.topValue').val()) * $imgWrapper.height() / 100;
        var left = parseFloat($(this).find('.leftValue').val()) * $imgWrapper.width() / 100;

        $(this).css({
            top: top + "px",
            left: left + "px"
        });
    });

    $('.insideDropZone').draggable({
        containment: '.editContainer',
        drag: function (event, ui) {
            insideDropZone = false;
        },
        stop: function () {
            if (!insideDropZone) {
                GetTrNotesBySymbolCode($(this).find('.symbolCode').text()).remove();
                $(this).remove();
            }
        }
    });
    $('.insideDropZone').dblclick(function () {
        $tr = GetTrNotesBySymbolCode($(this).find('.symbolCode').text());
        openPcNotes($tr.find("td").eq(2).html(), $tr.find("td").eq(6).html());
    });
    //#endregion

    //#region Drag n Drop Symbol
    $('.containerImageEdit').droppable({
        drop: function (event, ui) {
            insideDropZone = true;
            var $clone = ui.helper.clone();

            var $newPosX = ui.offset.left - $(this).offset().left;
            var $newPosY = ui.offset.top - $(this).offset().top;

            $clone.css({
                top: $newPosY + "px",
                left: $newPosX + "px"
            });

            $currentCloneSymbol = $clone;
            if (!$clone.is('.insideDropZone')) {
                openPcNotes();
                $clone.addClass('insideDropZone');
                var symbolGroup = $clone.find('.symbolGroupValue').val();
                var ctr;
                switch (symbolGroup) {
                    case 'B': ctr = ctrB; break;
                    case 'W': ctr = ctrW; break;
                    case 'F': ctr = ctrF; break;
                    case 'C': ctr = ctrC; break;
                    case 'D': ctr = ctrD; break;
                    case 'S': ctr = ctrS; break;
                }
                ctr.val++;
                currentSymbol = symbolGroup + ctr.val;
                $symbolCode = $("<div class='symbolCode'>" + currentSymbol + "</div>");
                $clone.append($symbolCode);
                $(this).append($clone);
                $clone.draggable({
                    containment: '.editContainer',
                    drag: function (event, ui) {
                        insideDropZone = false;
                    },
                    stop: function () {
                        if (!insideDropZone) {
                            GetTrNotesBySymbolCode($(this).find('.symbolCode').text()).remove();
                            $(this).remove();
                        }
                    }
                });
                $clone.dblclick(function () {
                    $tr = GetTrNotesBySymbolCode($(this).find('.symbolCode').text());
                    openPcNotes($tr.find("td").eq(2).html(), $tr.find("td").eq(6).html());
                });
            }
        }
    });

    $('.drag').draggable({
        helper: 'clone',
        drag: function (event, ui) {
            insideDropZone = false;
        }
    });
    //#endregion

    //#region Tombol Save, Apply
    $("#btnSave").click(OnSaveClick);
    $("#btnCancel").click(OnCancelClick);
    function OnCancelClick() {
        closePcBodyDiagramEdit();
    }

    function dateToYMD(date) {
        var d = date.getDate();
        var m = date.getMonth() + 1;
        var y = date.getFullYear();
        return '' + y + '-' + (m <= 9 ? '0' + m : m) + '-' + (d <= 9 ? '0' + d : d);
    }

    function OnSaveClick() {
        showLoadingPanel();

        var $imgPreview = $('#<%=imgPreview.ClientID %>');

        var canvas = document.createElement('canvas');
        canvas.setAttribute('width', $imgPreview.width());
        canvas.setAttribute('height', $imgPreview.height());
        var ctx = canvas.getContext("2d");

        var bgImg = new Image;
        bgImg.src = $imgPreview.attr('src');
        ctx.drawImage(bgImg, 0, 0, $imgPreview.width(), $imgPreview.height());

        $imgWrapper = $('.containerImageEdit');
        var containerPosition = $imgWrapper.position();
        var listImageSymbol = '';

        //format listImageSymbol : "idxImgSrc|symbol1|symbol2|....;
        //format symbol-n : id;left;top;symbolCode;notes

        //loop untuk mengambil symbol
        $('.insideDropZone').each(function () {
            var left = parseFloat($(this).position().left);
            var top = parseFloat($(this).position().top);

            var width = $(this).width();
            var height = $(this).height();

            var img = new Image;
            var $elem = $(this).find('img');
            var $symbolCode = $(this).find('.symbolCode');
            img.src = $elem.attr("src");

            //find notes from symbol code
            var notes;
            var found = false;
            $('#tblEditNotes tr').each(function () {
                if (!found && $symbolCode.text() == $(this).find("td").eq(2).html()) {
                    notes = $(this).find("td").eq(6).html();
                    $(this).remove();
                    found = true;
                }
            });
            var $symbolSrc = $('#<%=tblContainerSymbol.ClientID %> .drag img[src = "' + $elem.attr("src") + '"]');
            var idSymbol = $symbolSrc.parent().find('.symbolGroupValue').val();

            var leftPercentage = left * 100 / $imgWrapper.width();
            var topPercentage = top * 100 / $imgWrapper.height();
            listImageSymbol += '|' + idSymbol + ';' + leftPercentage + ';' + topPercentage + ';' + $symbolCode.text() + ';' + notes;

            var leftImage = left - (($imgWrapper.width() - $imgPreview.width()) / 2);
            var topImage = top - (($imgWrapper.height() - $imgPreview.height()) / 2);

            ctx.drawImage(img, leftImage, topImage, width, height);
            //Draw Symbol
            ctx.font = "bold 10px Arial";
            ctx.textAlign = "center";
            ctx.fillText($symbolCode.text(), leftImage + (width / 2), topImage - 1);
            $(this).remove();
        });
        var image = canvas.toDataURL("image/png");
        image = image.replace('data:image/png;base64,', '');
        var url = '<%= ResolveUrl("~/Libs/Service/UploadService.asmx/UpdateBodyDiagram")%>';
        $.ajax({
            async: false,
            type: 'POST',
            url: url,
            data: '{ "ID" : "' + ID + '", "imageData" : "' + image + '", "listSymbol" : "' + listImageSymbol + '" }',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            error: function (msg) {
                showToast('Failed', msg.responseText);
                hideLoadingPanel();
            },
            success: function (msg) {
                if (typeof onRefreshControl != 'undefined')
                    onRefreshControl('edit');
                hideLoadingPanel();
                closePcBodyDiagramEdit();
            }
        });
    }
    //#endregion

    //#region Popup Notes
    $("#btnNotesOk").click(submitPcNotes);
    $("#btnNotesCancel").click(cancelPcNotes);
    function openPcNotes(symbolCode, text) {
        symbolCode = typeof symbolCode !== 'undefined' ? symbolCode : '';
        text = typeof text !== 'undefined' ? text : '';
        currentSymbolEdit = symbolCode;
        $("#<%=txtFillNotes.ClientID %>").val(text);
        pcNotes.SetHeaderText('Notes');
        pcNotes.Show();
        $("#<%=txtFillNotes.ClientID %>").focus();
    }
    function submitPcNotes() {
        memNotesText = $("#<%=txtFillNotes.ClientID %>").val();
        pcNotes.Hide();
        if (currentSymbolEdit == '')
            addTableNotesRow(memNotesText);
        else {
            GetTrNotesBySymbolCode(currentSymbolEdit).find("td").eq(6).text(memNotesText);
        }
    }
    function cancelPcNotes() {
        pcNotes.Hide();
        if (currentSymbolEdit == '') {
            var ctr;
            switch ($currentCloneSymbol.find('.symbolCode').html().substr(0, 1)) {
                case 'B': ctr = ctrB; break;
                case 'W': ctr = ctrW; break;
                case 'F': ctr = ctrF; break;
                case 'C': ctr = ctrC; break;
                case 'D': ctr = ctrD; break;
                case 'S': ctr = ctrS; break;
            }
            ctr.val--;
            $currentCloneSymbol.remove();
        }
    }
    //#endregion

    //#region Table Notes
    function restoreTableNotesRow(symbolCode, notes) {
        createTableNotesRow(symbolCode, notes);
    }

    function createTableNotesRow(symbolCode, notes) {
        var groupVal = symbolCode.substring(0, 1);

        var $item = $('#<%=tblContainerSymbol.ClientID %> .drag .symbolGroupValue[value = "' + groupVal + '"]').parent();
        $imgSymbol = $item.find('img');
        var title = $imgSymbol.attr('title');
        var imgSrc = $imgSymbol.attr('src');

        var tbl = document.getElementById('tblEditNotes');
        var row = document.createElement('tr');
        row.appendChild(createImgSymbol(imgSrc, title));
        row.appendChild(createTd(':'));
        row.appendChild(createTd(symbolCode));
        row.appendChild(createTd(':'));
        row.appendChild(createTd(title));
        row.appendChild(createTd(':'));
        row.appendChild(createTd(notes));
        row.appendChild(createBtnEdit());
        row.appendChild(createBtnDelete());
        tbl.appendChild(row);
    }

    function addTableNotesRow(notes) {
        createTableNotesRow(currentSymbol, notes);
    }
    function createTd(val) {
        var td = document.createElement('td');
        td.innerHTML = val;
        return td;
    }
    function OnEditNotes() {
        $tr = $(this).parent().parent();
        openPcNotes($tr.find("td").eq(2).html(), $tr.find("td").eq(6).html());
    }
    function OnDeleteNotes() {
        $tr = $(this).parent().parent();
        var symbolCode = $tr.find("td").eq(2).html();

        var found = false;
        $('.insideDropZone').each(function () {
            if (!found && symbolCode == $(this).find('.symbolCode').text()) {
                $(this).remove();
                found = true;
            }
        });

        $(this).parent().parent().remove();
    }
    function createImgSymbol(imgSrc, toolTip) {
        var td = document.createElement('td');
        var img = document.createElement('img');
        img.src = imgSrc;
        img.setAttribute('title', toolTip);
        img.setAttribute('style', 'height:16px;width:16px');
        td.appendChild(img);
        return td;
    }
    function createBtnEdit() {
        var td = document.createElement('td');
        var img = document.createElement('img');
        img.src = imgEditUrl;
        img.setAttribute('style', 'height:16px;width:16px');
        img.setAttribute('title', 'Edit');
        $(img).click(OnEditNotes);
        td.appendChild(img);
        return td;
    }
    function createBtnDelete() {
        var td = document.createElement('td');
        var img = document.createElement('img');
        img.src = imgDeleteUrl;
        img.setAttribute('title', 'Delete');
        img.setAttribute('style', 'height:16px;width:16px');
        $(img).click(OnDeleteNotes);
        td.appendChild(img);
        return td;
    }
    //#endregion

    //#region Utility Method
    function GetTrNotesBySymbolCode(symbolCode) {
        var $tr = null;
        var found = false;
        $('#tblEditNotes tr').each(function () {
            if (!found && symbolCode == $(this).find("td").eq(2).html()) {
                $tr = $(this);
                found = true;
            }
        });
        return $tr;
    }
    //#endregion


    $(function () {
        setTimeout(function () {
            $imgPreview = $('#<%=imgPreview.ClientID %>');
            var height = 0;
            var width = 0;
            width = $imgPreview.width();
            height = $imgPreview.height();
            if (width > height) {
                height = 300 * height / width;
                width = 300;
            }
            else {
                width = 300 * width / height;
                height = 300;
            }

            $imgPreview.width(width + 'px');
            $imgPreview.height(height + 'px');
        }, 100);
    });

    function closePcBodyDiagramEdit() {
        pcRightPanelContent.Hide();
    }
</script>    
<style type="text/css">    
    .drag img, .insideDropZone img{
        height:24px;
    }
    .drag, .insideDropZone
    {
        cursor:pointer;
        z-index:1000;
    }
    .insideDropZone
    {
        position:relative;
    }
    .insideDropZone .symbolCode
    {
        position:absolute;
        top:-12px;
        width:100%;
        text-align:center;
        font-weight:bold;
        left:0;
    }
    .clear{clear:both;}
    
    #tblEditNotes {
        border-collapse:collapse;
        border:1px solid;
        position:relative; 
        width:100%;
    }
    
    #tblEditNotes tr td {
        vertical-align: top;
        text-align:left;
        padding:2px;
        border-bottom:1px solid;
        border-top:1px solid;
        font-size:12px;
    }
    #tblEditNotes img {
        cursor:pointer;
    }
    #tblEditNotes tr:nth-child(odd) {
	    background-color: #DDD; color: black;
    }
    #tblEditNotes tr:nth-child(even) {
	    background-color: #FFF; color: black;
    }    
    #tblEditNotes tr td:nth-child(5)  
    {
        font-weight:bolder;    
    }
    
    .containerImageEdit
    {
        float:left;
        margin:10px 20px 20px 15px;
        border: 1px solid #9C9898;
        height:300px;
        width:300px;display: table-cell; vertical-align: middle;
        text-align:center;
        position:relative;
    }
    .editContentMiddle {
        margin-right : 50px;
        border:1px solid #9C9898;
        padding-top: 10px;
        height:350px;
        width:1000px;
    }

    .editContentRight {
        float: right;
        padding: 10px 45px 0px 10px;
        width: 30px;
        border:1px solid #9C9898;
        height:350px;
    }   
    .editContentFooter {
        padding-top: 10px;
        text-align: center;
        clear: both;
        width:100%;
        height:30px;
    }
    .editContainer
    {
        border:1px solid #9C9898;
    }

</style>

<input type="hidden" id="hdnPatientBodyDiagramID" runat="server" value="" />
<div id="toolbarArea">
    <table id="Table2" runat="server" cellpadding="0">
        <tr>
            <td class="labelColumn">
                <td class="tdLabel"><%=GetLabel("Date") %> - <%=GetLabel("Time") %></td>
            </td>
            <td>
                <table cellpadding="0" cellspacing="5">
                    <tr>
                        <td style="padding-right: 1px;width:145px"><asp:TextBox Enabled="false" ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                        <td><asp:TextBox Enabled="false" ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<div class="editContainer" style="height:430px">
    <div class="editContentRight">
        <table id="tblContainerSymbol" runat="server"></table>
    </div>
    <div class="editContentMiddle">
        <div class="containerImageEdit boxShadow">
            <img id="imgPreview" runat="server" style="max-width:300px;max-height:300px;position:absolute;top:0;bottom:0;left:0;right:0;margin:auto;" src='' alt="" /> 
            <asp:Repeater ID="rptPatientBodyDiagramSymbol" runat="server">
                <ItemTemplate>
                    <div class="insideDropZone" style='position:absolute;'>
                        <input type="hidden" class="symbolGroupValue" value='<%#: DataBinder.Eval(Container.DataItem, "SymbolGroupValue")%>' />
                        <div class='symbolCode'><%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%></div>
                        <img alt="" src="<%#: Page.ResolveClientUrl((string)Eval("SymbolImageUrl"))%>"/>
                        <input type="hidden" class="topValue" value='<%#: DataBinder.Eval(Container.DataItem, "TopMargin")%>' />
                        <input type="hidden" class="leftValue" value='<%#: DataBinder.Eval(Container.DataItem, "LeftMargin")%>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater> 
        </div>
        <div>
            <div class="containerNotes">
                <div style="font-size:large;font-weight:bolder;margin:10px;text-align:left">Remarks :</div>
                <div style="position:relative;overflow:scroll;height:280px;margin:10px;">
                    <asp:Repeater ID="rptRemarks" runat="server">
                        <HeaderTemplate>
                            <table id="tblEditNotes" cellpadding="0" cellspacing="0">
                                <colgroup width="20px" />
                                <colgroup width="2px" />
                                <colgroup width="15px" />
                                <colgroup width="2px" />
                                <colgroup width="45px" />
                                <colgroup width="2px" />
                                <colgroup width="*" />
                                <colgroup width="16px" />
                                <colgroup width="16px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><img alt="" style="width:16px;height:16px" src="<%#: Page.ResolveClientUrl((string)Eval("SymbolImageUrl"))%>"/></td>
                                <td>:</td>
                                <td><%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%></td>
                                <td>:</td>
                                <td><%#: DataBinder.Eval(Container.DataItem, "SymbolName")%></td>
                                <td>:</td>
                                <td><%#: DataBinder.Eval(Container.DataItem, "Remarks")%></td>
                                <td><img class="btnEditRemarks" src='<%= ResolveUrl("~/Libs/Images/Button/edit.png")%>' style="height:16px;width:16px" Title="Edit"/></td>
                                <td><img class="btnDeleteRemarks" src='<%= ResolveUrl("~/Libs/Images/Button/delete.png")%>' style="height:16px;width:16px" Title="Delete"/></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>                
                </div>
            </div>  
        </div>
    </div>    
    <div class="editContentFooter">
        <table cellpadding="0" cellspacing="10" style="margin-left:auto;margin-right:auto;text-align:left">
            <tr>
                <td><input type="button" value='<%= GetLabel("Save")%>' id="btnSave" /></td>
                <td><input type="button" value='<%= GetLabel("Cancel")%>' id="btnCancel" /></td>
            </tr>
        </table>
    </div>    
</div> 



<!-- Popup Entry Notes -->
<dx:ASPxPopupControl id="pcNotes" runat="server" clientinstancename="pcNotes"
    height="150px" HeaderText="Notes" CloseAction="None" width="350px" Modal="True" PopupAction="None" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseButtonImage-Width="0">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server" ID="pccc1">
            <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <div style="text-align:center;width:100%;">
                            <asp:TextBox ID="txtFillNotes" runat="server" Width="320px" Height="50px" TextMode="MultiLine" />
                            <table style="margin-left:auto;margin-right:auto;margin-top:5px;">
                                <tr>
                                    <td><input type="button" id="btnNotesOk" style="width:100px" value='<%= GetLabel("Ok")%>' /></td>
                                    <td><input type="button" id="btnNotesCancel" style="width:100px" value='<%= GetLabel("Cancel")%>' /></td>
                                </tr>
                            </table> 
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>