function showDialogBoxNS(evt)
{
    var name = confirm("Are you sure you want to delete this portlet?")
    if(name == true)
    {
        var source = getEventSource(evt);
        var url = source.href;
        source.href = url+"&__deletePermanently=false";
        return true;
    }
    return false;
}

function showMultipleChoiceDialogBoxNS(evt)
{
    var name = confirm("Are you sure you want to delete this portlet?")
    var name1 = false;
    if(name == true)
    {
        var source = getEventSource(evt);
        var url = source.href;
        name1 = confirm("Press OK to delete permanently \nor Cancel to only delete from this session.\n");
        if(name1 == true)
        {
            source.href = url+"&__deletePermanently=true";
        }
        else
        {
            source.href = url+"&__deletePermanently=false";
        }
        return true;
    }
    return false;
}


function showDialogBoxIE()
{
    var name = confirm("Are you sure you want to delete this portlet?")
    if(name == true)
    {
        var source;
        var event = window.event;
        if(event != null)
        {
            source = event.srcElement;
        }
        var parent = source.parentNode;
        var url = parent.href;
        parent.href = url+"&__deletePermanently=false";
    }
    else
    {
        var event = window.event;
        if(event != null)
        {
            event.returnValue = false;
        }
    }
}

function showMultipleChoiceDialogBoxIE()
{
    var name = confirm("Are you sure you want to delete this portlet?")
    var name1 = false;
    if(name == true)
    {
        var source;
        var event = window.event;
        if(event != null)
        {
            source = event.srcElement;
        }
        var parent = source.parentNode;
        var url = parent.href;
        name1 = confirm("Press OK to delete permanently \nor Cancel to only delete from this session.\n");
        if(name1 == true)
        {
            parent.href = url+"&__deletePermanently=true";
        }
        else
        {
            parent.href = url+"&__deletePermanently=false";
        }
    }
    else
    {
        var event = window.event;
        if(event != null)
        {
            event.returnValue = false;
        }
    }
}

function detectNetscape()
{
    var n=navigator.appName
    if(n == "Netscape")
    {
        return true;
    }
    else
    {
        return false;
    }
}

function initPortletDeleteButtons()
{
    var portletDeleteButtonsSpanned = getSpannedDeleteButtons();
    var portletDeleteButtonsNonSpanned = getNonSpannedDeleteButtons();
    if(detectNetscape() == true)
    {
        for(var i = 0; i < portletDeleteButtonsSpanned.length; i++)
        {
            portletDeleteButtonsSpanned[i].onclick = showMultipleChoiceDialogBoxNS;
        }
        for(var i = 0; i < portletDeleteButtonsNonSpanned.length; i++)
        {
            portletDeleteButtonsNonSpanned[i].onclick = showDialogBoxNS;
        }
    }
    else
    {
        for(var i = 0; i < portletDeleteButtonsSpanned.length; i++)
        {
            portletDeleteButtonsSpanned[i].onclick = showMultipleChoiceDialogBoxIE;
        }
        for(var i = 0; i < portletDeleteButtonsNonSpanned.length; i++)
        {
            portletDeleteButtonsNonSpanned[i].onclick = showDialogBoxIE;
        }
    }
}

function getSpannedDeleteButtons()
{
    var anchors = document.getElementsByTagName("a");
    var spannedDeletes = new Array(0);
    var j = -1;
    for (var i = 0; i < anchors.length; i++)
    {
        var _className = anchors[i].className;
        if((_className == "bea-portal-button-delete") &&
           (anchors[i].parentNode.tagName == "span" || anchors[i].parentNode.tagName == "SPAN"))
        {
            j++;
            spannedDeletes[j] = anchors[i];
        }
    }
    return spannedDeletes;
}

function getNonSpannedDeleteButtons()
{
    var anchors = document.getElementsByTagName("a");
    var nonSpannedDeletes = new Array(0);
    var j = -1;
    for (var i = 0; i < anchors.length; i++)
    {
        var _className = anchors[i].className;
        if((_className == "bea-portal-button-delete") &&
           (anchors[i].parentNode.tagName != "span" && anchors[i].parentNode.tagName != "SPAN"))
        {
            j++;
            nonSpannedDeletes[j] = anchors[i];
        }
    }
    return nonSpannedDeletes;
}
