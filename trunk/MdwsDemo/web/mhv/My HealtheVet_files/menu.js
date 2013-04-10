
// @todo Re-evaluate need for IDs

var tryClosePause = 1000;

var bookTag = "DIV";
var menuTag = "UL";
var menuItemTag = "LI";
var menuItemLinkTag = "A";
var menuHookTag = "DIV"

var bookClassRegex = new RegExp("bea-portal-book(?:-(primary))?$");
var menuClassRegex = new RegExp("bea-portal-book(?:-(primary))?-menu$");
var hookClassRegex = new RegExp("bea-portal-book(?:-(primary))?-menu-hook$");
var itemClassRegex = new RegExp("bea-portal-book(?:-(primary))?-menu-item(?:-(active))?$");
var linkClassRegex = new RegExp("bea-portal-book(?:-(primary))?-menu-item-link$");

var rootClassRegex = new RegExp("bea-portal-book(?:-(primary))?-menu-root$");
var nestedClassRegex = new RegExp("bea-portal-book(?:-(primary))?-menu-nested$");

var books = new Array();
var currentMenuItem;

function initDynamicMenus()
{
    initBooks();
    createMenus();
}

function initBooks()
{
    var bookCandidates = document.getElementsByTagName(bookTag);

    for (var i = 0; i < bookCandidates.length; i++)
    {
        if (bookCandidates[i].tagName && bookCandidates[i].tagName == bookTag
            && bookCandidates[i].className && bookCandidates[i].className.match(bookClassRegex))
        {
            var book = initBook(bookCandidates[i]);

            if (book)
            {
                books[books.length] = book;
            }
        }
    }
}

function initBook(tag)
{
    var book;
    var menuInstance = getFirstChildByClassRegex(tag, menuTag, menuClassRegex, bookClassRegex);
    var hookInstance = getFirstChildByClassRegex(tag, menuHookTag, hookClassRegex, bookClassRegex);

    if (tag && menuInstance && hookInstance)
    {
        var menu = initMenu(menuInstance);
        var hook = initMenuHook(hookInstance);

        if (menu && hook)
        {
            book = new Book(tag, menu, hook);
        }
    }

    return book;
}

function initMenu(tag)
{
    if (!tag)
    {
        alert("No tag defined!");
    }

    tag.style.display = "none";
    var menuItems = new Array();
    var child = tag.firstChild;

    while (child != null)
    {
        if (child.tagName && child.tagName == menuItemTag && child.className.match(itemClassRegex))
        {
            var itemGroups = child.className ? child.className.match(itemClassRegex) : null;

            if (itemGroups && itemGroups.length >= 1)
            {
                var isActive = false;

                if ((itemGroups.length == 2 && itemGroups[1] == "active")
                    || (itemGroups.length == 3 && itemGroups[2] == "active"))
                {
                    isActive = true;
                }

                menuItems[menuItems.length] = initMenuItem(child, isActive);
            }
        }

        child = child.nextSibling;
    }

    var groups = tag.className.match(menuClassRegex);
    var bookClassQualifier = (groups && groups.length >= 2 && groups[1] == "primary" ? "-primary" : "");
    return new Menu(tag, menuItems, bookClassQualifier);
}

function initMenuHook(tag)
{
    return new MenuHook(tag);
}

function initMenuItem(tag, isActive)
{
    var link;
    var menu;
    var child = tag.firstChild;

    while (child != null)
    {
        if (child.tagName && child.tagName == menuItemLinkTag && child.className.match(linkClassRegex))
        {
            var label = (child.innerText ? child.innerText : child.text);
            link = new Link(child, label);
        }
        else if (child.tagName && child.tagName == menuTag)
        {
            menu = initMenu(child);
        }

        child = child.nextSibling;
    }

    return new MenuItem(tag, link, isActive, menu);
}

function createMenus()
{
    for (var i = 0; i < books.length; i++)
    {
        createRootMenu(books[i].menu, books[i].hook, i);
    }
}

function createRootMenu(menu, hook, menuId)
{
    var menuContext = document.createElement("UL");
    menuContext.className = "bea-portal-book" + menu.bookClassQualifier + "-menu-root";
    menuContext.id = "menu-" + menuId;
    hook.tag.appendChild(menuContext);
    var itemClassName = "bea-portal-book" + menu.bookClassQualifier + "-menu-root-item";

    for (var i = 0; i < menu.menuItems.length; i++)
    {
        createMenuItem(menu.menuItems[i], menuContext, itemClassName, i, "LI", true);
    }
}

function createSubMenu(menu, hook, contextId)
{
    var menuContext = document.createElement("div");
    menuContext.style.display = "none";
    menuContext.className = "bea-portal-book" + menu.bookClassQualifier + "-menu-nested";
    menuContext.id = contextId + "-menu";
    hook.tag.appendChild(menuContext);
    var itemClassName = "bea-portal-book" + menu.bookClassQualifier + "-menu-nested-item";

    for (var i = 0; i < menu.menuItems.length; i++)
    {
        createMenuItem(menu.menuItems[i], menuContext, itemClassName, i, "DIV", false);
    }

    return menuContext;
}

function createMenuItem(menuItem, menuContext, itemClassName, itemId, tagName, isRoot)
{
    var itemContext = document.createElement(tagName);
    itemContext.className = itemClassName + (menuItem.isActive ? "-active" : "");
    menuContext.appendChild(itemContext);
    itemContext.id = menuContext.id + "-item-" + itemId;

    if (menuItem.link)
    {
        var link = document.createElement("a");
        link.href = menuItem.link.tag.href;
        var label = document.createTextNode(menuItem.link.label);
        addEventToElement(link, "mouseover", menuItemHoverIn, false);
        addEventToElement(link, "mouseout", menuItemHoverOut, false);
        itemContext.appendChild(link);
        link.appendChild(label);
    }

    if (menuItem.menu)
    {
        var subMenu = createSubMenu(menuItem.menu, new MenuHook(itemContext), itemContext.id);
        itemContext.appendChild(subMenu);
    }
}

function menuItemHoverIn(evt)
{
    var event = getEvent(evt);
    var source = getEventSource(event);
    var parent = source.parentNode;
    currentMenuItem = parent;
    parent.renderedHeight = parent.offsetHeight;
    parent.renderedWidth = parent.offsetWidth;
    closeAll(parent);
    openPath(parent);
}

function menuItemHoverOut(evt)
{
    var event = getEvent(evt);
    var source = getEventSource(event);
    var parent = source.parentNode;
    currentMenuItem = null;
    tryClose(parent);
}

var tmpMenuItem;
function tryClose(menuItem)
{
    if (!currentMenuItem)
    {
        tmpMenuItem = menuItem;
        setTimeout("wlp_default_close(tmpMenuItem)", tryClosePause);
    }
}

function wlp_default_close(menuItem)
{
    if (menuItem && !currentMenuItem)
    {
        closeAll(menuItem);
    }
}

function getMenuChild(parent)
{
    var child = parent.firstChild;
    var done = false;

    while (!done && child)
    {
        if (child && child.className && child.className.match(nestedClassRegex))
        {
            done = true;
        }
        else
        {
            child = child.nextSibling;
        }
    }

    return child;
}

function openPath(menuItem)
{
    var path = new Array();
    var childMenu = getMenuChild(menuItem);

    if (childMenu)
    {
        path[path.length] = new Array();
        path[path.length - 1][0] = menuItem;
        path[path.length - 1][1] = childMenu;
    }

    var menu = menuItem.parentNode;

    while (menu && !menu.className.match(rootClassRegex))
    {
        menuItem = menu.parentNode;
        path[path.length] = new Array();
        path[path.length - 1][0] = menuItem;
        path[path.length - 1][1] = menu;
        menu = menuItem.parentNode;
    }

    for (var i = 0; i < path.length; i++)
    {
        openMenu(path[i][0], path[i][1], path.length - 1 - i);
    }
}

function openMenu(menuItem, menu, depth)
{
    var width = (menuItem.offsetWidth == 0 ? menuItem.renderedWidth : menuItem.offsetWidth);
    var pos = (depth == 0 ? 1 : getMenuItemPosition(menuItem));
    var height = (menuItem.offsetHeight == 0 ? menuItem.renderedHeight : menuItem.offsetHeight);
    var coords = (depth == 0 ? getDocumentOffset(menuItem) : [menuItem.offsetLeft, menuItem.offsetTop]);
    menu.style.position = "absolute";
    menu.style.display = "block";
    menu.style.left = coords[0] + (depth == 0 ? 0 : width) + "px";
    menu.style.top = coords[1] + (pos * height) + "px";
}

function getDocumentOffset(object)
{
    var coords = new Array();
    coords[0] = object.offsetLeft;
    coords[1] = object.offsetTop;

    while((object = object.offsetParent) != null)
    {
        coords[0] += object.offsetLeft;
        coords[1] += object.offsetTop;
    }

    return coords;
}

function getMenuItemPosition(menuItem)
{
    var pos = 0;
    var parentMenu = menuItem.parentNode;
    var childMenuItem = parentMenu.firstChild;
    var hit = false;

    while (childMenuItem)
    {
        if (childMenuItem == menuItem)
        {
            hit = true;
            break;
        }

        pos++;
        childMenuItem = childMenuItem.nextSibling;
    }

    if (!hit)
    {
        alert("Parent menu does not contain child menu item!");
    }

    return pos;
}

// @review Consider a closeAllBut(menuItem) that closes everything in the path above the menuItem for Mozilla

function closeAll(menuItem)
{
    var rootMenu = menuItem.parentNode;

    while (rootMenu && !rootMenu.className.match(rootClassRegex))
    {
        rootMenu = rootMenu.parentNode.parentNode;
    }

    closeAllChildren(rootMenu);
}

function closeAllChildren(menu)
{
    var child = menu.firstChild;

    while (child)
    {
        var subMenu = getMenuChild(child);

        if (subMenu)
        {
            closeAllChildren(subMenu);
            subMenu.style.display = "none";
        }

        child = child.nextSibling;
    }
}

function Book(tag, menu, hook)
{
    this.tag = tag;
    this.menu = menu;
    this.hook = hook;
}

function Menu(tag, menuItems, bookClassQualifier)
{
    this.tag = tag;
    this.menuItems = menuItems;
    this.bookClassQualifier = bookClassQualifier;
}

function MenuHook(tag)
{
    this.tag = tag;
}

function MenuItem(tag, link, isActive, menu)
{
    this.tag = tag;
    this.link = link;
    this.isActive = isActive;
    this.menu = menu;
}

function Link(tag, label)
{
    this.tag = tag;
    this.label = label;
}
