var menufx_menuClassRegex = new RegExp("bea-portal-book-primary-menu-single-item(?:-active)?$");

function initRolloverMenus()
{
    var listItems = document.getElementsByTagName('li');
    for (var i = 0; i < listItems.length; i++)
    {
        if (listItems[i].className && listItems[i].className.match(menufx_menuClassRegex))
        {
            initRolloverMenu(listItems[i]);
        }
    }
}

function initRolloverMenu(listItem)
{
    var image = getImage(listItem);

    if (image && image.src && image.longDesc)
    {
        image.rollOffSrc = image.src;
        image.rollOnSrc = image.longDesc;

        image.fxEnabled = true;
        addEventToElement(image, "mouseover", fxRollIn, true);
        addEventToElement(image, "mouseout", fxRollOut, true);
    }
}

function getImage(listItem)
{
    var image;
    var images = listItem.getElementsByTagName("img");
    if (images && images.length == 1)
    {
        image = images[0];
    }

    return image;
}

function fxRollIn(event)
{
    var image = getEventSource(getEvent(event));
    if (image && image.rollOnSrc && image.fxEnabled)
    {
        image.src = image.rollOnSrc;
    }
}

function fxRollOut(event)
{
    var image = getEventSource(getEvent(event));
    if (image && image.rollOffSrc && image.fxEnabled)
    {
        image.src = image.rollOffSrc;
    }
}

