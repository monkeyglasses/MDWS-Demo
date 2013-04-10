var activePosition = 0;

/* determine if this menu item is the active menu item */
function getMenuClass(menuPosition) {
  // menuPosition should be an int
  if (menuPosition == activePosition) {
    return true;
  }
  return false;
}

/* set the menu position to be active */
function setMenuPosition(menuPosition) {
  // menuposition should be an int
  this.activePosition = menuPosition;
  setMenuClass(menuPosition);
}

/* set the actual active style class for this link id */
function setMenuClass(menuPosition) {
  this.idString = "leftNavMenuItem"+menuPosition;
  identity=document.getElementById(idString);
  if (getMenuClass(menuPosition)) {
    identity.className="mhv-leftnav-menu-item-active";
  }
}


