
function getParentByClassName(element, className)
{
    var parent = element;

    if (className)
    {
        while (parent && parent.className != className)
        {
            parent = parent.parentNode;
        }
    }

    return parent;
}

function getFirstChildByClassRegex(element, tagName, classRegex, stopClassRegex)
{
    var match;
    var child = element.firstChild;

    while (child != null && (!child.className || !child.className.match(stopClassRegex)))
    {
        if (child.tagName && child.tagName == tagName && child.className && child.className.match(classRegex))
        {
            match = child;
            break;
        }
        else
        {
            match = getFirstChildByClassRegex(child, tagName, classRegex, stopClassRegex);

            if (match)
            {
                break;
            }
        }

        child = child.nextSibling;
    }

    return match;
}

function getChildrenByClassRegex(root, classRegexes, tagName, matches)
{
    matches = (matches ? matches : new Array());

    if (root)
    {
        var child = root.firstChild;

        while (child)
        {
            if (!tagName || child.tagName == tagName)
            {
                for (index in classRegexes)
                {
                    if (child.className && child.className.match(classRegexes[index]))
                    {
                        matches[matches.length] = child;
                    }
                }
            }

            getChildrenByClassRegex(child, classRegexes, tagName, matches);
            child = child.nextSibling;
        }
    }

    return matches;
}

function addEventToElement(target, eventType, func, useCapture)
{
    var result = false;

    if (target.addEventListener)
    {
        target.addEventListener(eventType, func, useCapture);
        result = true;
    }
    else if (target.attachEvent)
    {
        result = target.attachEvent("on" + eventType, func);
    }
    else
    {
        alert("Handler could not be attached");
    }

    return result;
}

function addEventToElements(targets, eventType, func, useCapture)
{
    var result = true;

    for (var i = 0; i < targets.length; i++)
    {
        result &= addEventToElement(targets[i], eventType, func, useCapture);
    }

    return result;
}

function removeEventFromElement(target, eventType, func, useCapture)
{
    var result = false;

    if (target.removeEventListener)
    {
        target.removeEventListener(eventType, func, useCapture);
        result = true;
    }
    else if (target.detachEvent)
    {
        result = target.detachEvent("on" + eventType, func);
    }
    else
    {
        window.alert("Handler could not be removed");
    }

    return result;
}

function removeEventFromElements(targets, eventType, func, useCapture)
{
    var result = true;

    for (var i = 0; i < targets.length; i++)
    {
        result &= removeEventFromElement(targets[i], eventType, func, useCapture);
    }

    return result;
}

function getEvent(event)
{
    var evt = event;

    if (!evt)
    {
        evt = window.event;
    }

    return evt;
}

function getEventSource(event)
{
    var source;

    if (event && event.srcElement)
    {
        source = event.srcElement;
    }
    else if (event && event.currentTarget)
    {
        source = event.currentTarget;
    }
    else
    {
        window.alert("Event source not found!");
    }

    return source;
}

function getClickableImageEventSource(source)
{
    // If IE, the original source will be the image being clicked instead of the parent element that generated the event
    if (source && source.src)
    {
        source = source.parentNode;
    }

    return source;
}


/**
 * @deprecated
 */
//var console = (beaPortalConsole ? beaPortalConsole : new Console());

/**
 * @deprecated
 */
//function Console()
//{
  //  this.canvas = null;
  //  this.println = consolePrintln;
  //  this.show = consoleShow;
//}

/**
 * @deprecated
 */
//function consolePrintln(object)
//{
  //  if (!this.canvas)
   // {
    //    this.canvas = window.open("about:blank", "Console", "toolbar = no, width = 640, height = 480, directories = no, status = no, scrollbars = yes, resize = no, menubar = no");
    //}

    //this.canvas.document.write(object);
    //this.canvas.document.write("<br/>");
//}

/**
 * @deprecated
 */
//function consoleShow(object)
//{
  //  this.println(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

    //for (property in object)
    //{
      //  this.println(property + " = " + eval("object." + property));
    //}

    //this.println("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
//}
