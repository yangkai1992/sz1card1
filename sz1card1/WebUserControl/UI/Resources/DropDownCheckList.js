/************************************************************************
* DropDownCheckList.js
* 
*  Client-side javascript to support the ASP.NET DropDownCheckList 
*  server control.
*
*  written by Mike Ellison, 20-September-2005
*
************************************************************************
*  Copyright (c) 2005 Mike Ellison.  All rights reserved.
*  
*  Redistribution and use in source and binary forms, with or without 
*  modification, are permitted provided that the following conditions are met:
*  
*    * Redistributions of source code must retain the above copyright notice, 
*      this list of conditions, the following disclaimer, and the following
*      acknowledgements. 
*  	
*    * Redistributions in binary form must reproduce the above copyright notice, 
*      this list of conditions and the following disclaimer in the documentation 
*  	and/or other materials provided with the distribution.
*  	
*    * The name of the author may not be used to endorse or promote products 
*      derived from this software without specific prior written permission. 
*  
*  THIS SOFTWARE IS PROVIDED BY THE AUTHOR "AS IS" AND ANY EXPRESS OR IMPLIED 
*  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
*  MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
*  IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, 
*  INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
*  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
*  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY 
*  OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
*  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
*  EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*
*  Acknowledgements:
*      - The "shim" technique used to support Internet Explorer 6.x
*        is courtesy Joe King of Coalesys, Inc., 
*        http://dotnetjunkies.com/WebLog/jking/archive/2003/07/21/488.aspx
*
*      - Javascript functions findPosX and findPosY for locating objects absolutely
*        are courtesy Peter-Paul Koch, http://www.quirksmode.org
*  
************************************************************************/

/************************************************************************
* Utility Functions (global to the page)
************************************************************************/
var DDCL_DROPDOWNMODE_INLINE = 0;
var DDCL_DROPDOWNMODE_ONTOP = 1;
var DDCL_DROPDOWNMODE_ONTOPWITHSHIM = 2;
var DDCL_DISPLAYTEXTLIST_LABELS = 0;
var DDCL_DISPLAYTEXTLIST_VALUES = 1;

ddcl_Objects = new Array();
function DDCL_GetObject(id) {
    for (var i = 0; i < ddcl_Objects.length; i++) {
        // alert(id.substring(ddcl_Objects[i].length) + ", " + ddcl_Objects[i].id);
        if (id.substring(0, ddcl_Objects[i].id.length) == ddcl_Objects[i].id) {
            // alert('gotit');
            return ddcl_Objects[i];
        }
    }
    return null;
}

// -----------------------------------------------------
function DDCL_findPosX(obj) {
    /***
    Original script by Peter-Paul Koch, http://www.quirksmode.org
    ***/
    var curleft = 0;
    if (obj.offsetParent) {
        while (obj.offsetParent) {
            curleft += obj.offsetLeft
            obj = obj.offsetParent;
        }
    }
    else if (obj.x)
        curleft += obj.x;
    return curleft;
}

function DDCL_findPosY(obj) {
    /***
    Original script by Peter-Paul Koch, http://www.quirksmode.org
    ***/
    var curtop = 0;
    if (obj.offsetParent) {
        while (obj.offsetParent) {
            curtop += obj.offsetTop
            obj = obj.offsetParent;
        }
    }
    else if (obj.y)
        curtop += obj.y;
    return curtop;
}
// -----------------------------------------------------


function DDCL_HandleCheckboxClickById(id) {
    var obj = DDCL_GetObject(id);
    obj.HandleCheckboxClick();
}

// called as the onclick event handler for a check box
function DDCL_HandleCheckboxClick(e) {
    if (!e) var e = window.event;
    if (e) {
        var elem = this;
        if (elem) {
            var obj = DDCL_GetObject(elem.id);
            if (obj) {
                obj.HandleCheckboxClick();
                obj.inCheckboxDiv = true;
            }
        }
    }
}


// called as the onclick event handler for a display box
function DDCL_HandleDisplayBoxClick(e) {
    if (!e) var e = window.event;
    if (e) {
        var elem = this;
        if (elem) {
            var obj = DDCL_GetObject(elem.id);
            if (obj) {
                obj.HandleDisplayBoxClick();
                obj.inCheckboxDiv = true;
            }
        }
    }
}

// called as the onclick event handler for a checkbox div
function DDCL_HandleCheckboxDivClick(e) {
    if (!e) var e = window.event;
    if (e) {
        var obj = DDCL_GetObject(this.id);
        if (obj)
            obj.inCheckboxDiv = true;
    }
}

function DDCL_HandleDocumentClick(id) {
    var obj = DDCL_GetObject(id);
    if (obj) {
        if (obj.inCheckboxDiv == true)
            obj.inCheckboxDiv = false;
        else
            obj.CloseCheckList();
    }
}


/************************************************************************
* Object definition
************************************************************************/
// constructor; takes a required id as a parameter; this will be the
// same as the ClientID of the ASP.NET control
function DDCL_DropDownCheckList(id, textWhenNone, separator, truncateString, dropDownMode, allowExpand, displayList) {
    // remember the id, displayMode, and text to display when no
    // boxes are checked
    this.id = id;
    this.textWhenNone = textWhenNone;
    this.separator = separator;
    this.truncateString = truncateString;
    this.dropDownMode = dropDownMode;
    this.allowExpand = allowExpand;
    this.displayList = displayList;

    // get the elements representing the display box div and
    // checklist div; these will be referenced by object methods
    this.divCheckboxes = document.getElementById(id + "_checkboxes");
    this.divBoundingBox = document.getElementById(id + "_boundingbox");
    this.divText = document.getElementById(id + "_text");
    this.divDisplayBox = document.getElementById(id + "_displaybox");
    this.img = document.getElementById(id + "_img");
    this.shim = document.getElementById(id + "_shim");

    this.boundingBoxWidth = this.divBoundingBox.offsetWidth;

    this.divCheckboxes.style.display = "none";

    // wire click events for the display box div and the checkboxes div
    this.divDisplayBox.onclick = DDCL_HandleDisplayBoxClick;
    this.divCheckboxes.onclick = DDCL_HandleCheckboxDivClick;

    // wire click events for the checkboxes
    var e = this.divCheckboxes.getElementsByTagName("input");
    for (var i = 0; i < e.length; i++) {
        if (e[i].type == "checkbox") {
            e[i].onclick = DDCL_HandleCheckboxClick;
        }
    }

    // if the browser supports bubbling events, install a default click
    // handler for the document too, that will close the checkboxes div
    // if there is a click outside it
    if (document.attachEvent) {
        document.attachEvent('onclick'
                              , function() { eval("DDCL_HandleDocumentClick('" + id + "');") }
                             );
    }
    else if (document.addEventListener) {
        document.addEventListener('click'
                              , function() { eval("DDCL_HandleDocumentClick('" + id + "');") }
                              , false);
    }


    // initial display of checked items, or the textWhenNone string
    this.DisplayCheckedItems();

    // add this object to our reference array on the page so we can get it 
    // back later
    ddcl_Objects.push(this);

}


// get a label given a checkbox element
function DDCL_DropDownCheckList_GetLabelForCheckbox(elem) {
    var e = this.divCheckboxes.getElementsByTagName("label");
    for (var i = 0; i < e.length; i++) {
        if (e[i].htmlFor == elem.id) {
            for (var j = 0; j < e[i].childNodes.length; j++) {
                if (e[i].childNodes[j].nodeType == 3) //text type
                {
                    return e[i].childNodes[j].nodeValue;
                }
            }
        }
    }

    // still here?  no <label> for this checkbox then
    return null;
}


// refresh the checked items listing in the display box
function DDCL_DropDownCheckList_DisplayCheckedItems() {
    var sLabel = "";
    var sCurrent = "";
    var sFull = "";
    var sBefore = "";
    var sCompText = "";
    var bEllipsisAdded = false;

    // get all checkboxes in the checklist
    var e = this.divCheckboxes.getElementsByTagName("input");
    // clear the display text
    this.divText.innerHTML = "";
    // clear the title (tooltip) attribute
    this.divDisplayBox.title = "";

    // loop through all checkboxes in the checklist to see 
    // which ones are checked;
    for (var i = 0; i < e.length; i++) {
        if (e[i].type == "checkbox" && e[i].checked) {
            // if the checkbox is checked, get its associated label text
            if (this.displayList == DDCL_DISPLAYTEXTLIST_LABELS)
            // get the label for the checkbox
                sLabel = this.GetLabelForCheckbox(e[i]);
            else
            // get the value for the checkbox
                sLabel = e[i].value;

            // add the list separator if necessary
            if (sCurrent != "") {
                sCurrent += this.separator;
                sFull += this.separator;
            }

            sFull += sLabel;
            sCurrent += sLabel;


            if (bEllipsisAdded == false) {
                // add this one to the text box, then test for the
                // width against the display box
                this.divText.innerHTML = "<nobr>" + sCurrent + "</nobr>";
                if (this.divText.offsetWidth > this.boundingBoxWidth && this.allowExpand == false) {
                    // too big; shrink by what we can and add the ellipsis (or other trunacte string)
                    while (this.divText.offsetWidth > this.boundingBoxWidth && sCurrent.length > 0) {
                        sCurrent = sCurrent.substr(0, sCurrent.length - 1);
                        this.divText.innerHTML = "<nobr>" + sCurrent + this.truncateString + "</nobr>";
                    }

                    // and indicate the ellipsis (or other truncate text) has been added
                    bEllipsisAdded = true;
                }
            }
        }
    }

    // finally, if there are no contents, display the textWhenNone message
    if (this.divText.innerHTML == "") {
        if (this.textWhenNone == "")
            this.divText.innerHTML = "&nbsp;";
        else
            this.divText.innerHTML = this.textWhenNone;
    }

    // if we added the ellipsis, set the title attribute to the full string
    // (which will display as a tooltip in most browsers)
    if (bEllipsisAdded)
        this.divDisplayBox.title = sFull;
    else
        this.divDisplayBox.title = "";

}

// handle a click in a participating checkbox
function DDCL_DropDownCheckList_HandleCheckboxClick() {
    // refresh the checked items display box
    this.DisplayCheckedItems();
}

// handle a click in the display box
function DDCL_DropDownCheckList_HandleDisplayBoxClick() {
    // toggle the display of the checklist
    this.ToggleCheckList();
}

// toggle the display of the checklist boxes
function DDCL_DropDownCheckList_ToggleCheckList() {
    if (this.divCheckboxes.style.display != "none")
        this.CloseCheckList();
    else
        this.OpenCheckList();
}

// explicitly close the checklist
function DDCL_DropDownCheckList_CloseCheckList() {
    // hide the checkboxes div
    this.divCheckboxes.style.display = "none";

    // if we're using a shim, hide it as well
    if (this.dropDownMode == DDCL_DROPDOWNMODE_ONTOPWITHSHIM)
        this.shim.style.display = "none";
}

// explicitly open the checklist
function DDCL_DropDownCheckList_OpenCheckList() {
    // open the checkboxlist; first, position it below the displaybox

    // determine the position based on the dropDownMode
    if (this.dropDownMode == DDCL_DROPDOWNMODE_INLINE) {
        // inline mode; we're already setup as we need to be
        this.divCheckboxes.style.display = "block";
    }
    else {
        // on top modes; position the box
        this.divCheckboxes.style.left = DDCL_findPosX(this.divDisplayBox);
        this.divCheckboxes.style.top = DDCL_findPosY(this.divDisplayBox) + this.divDisplayBox.offsetHeight;

        this.divCheckboxes.style.display = "block";
        // if we want the shim, apply that now
        if (this.dropDownMode == DDCL_DROPDOWNMODE_ONTOPWITHSHIM) {
            this.shim.style.width = this.divCheckboxes.offsetWidth;
            this.shim.style.height = this.divCheckboxes.offsetHeight;
            this.shim.style.top = this.divCheckboxes.style.top;
            this.shim.style.left = this.divCheckboxes.style.left;
            this.shim.style.zIndex = this.divCheckboxes.style.zIndex - 1;
            this.shim.style.display = "block";
        }


    }
}

// wire methods as prototypes
DDCL_DropDownCheckList.prototype.DisplayCheckedItems = DDCL_DropDownCheckList_DisplayCheckedItems;
DDCL_DropDownCheckList.prototype.GetLabelForCheckbox = DDCL_DropDownCheckList_GetLabelForCheckbox;
DDCL_DropDownCheckList.prototype.HandleCheckboxClick = DDCL_DropDownCheckList_HandleCheckboxClick;
DDCL_DropDownCheckList.prototype.HandleDisplayBoxClick = DDCL_DropDownCheckList_HandleDisplayBoxClick;
DDCL_DropDownCheckList.prototype.ToggleCheckList = DDCL_DropDownCheckList_ToggleCheckList;
DDCL_DropDownCheckList.prototype.CloseCheckList = DDCL_DropDownCheckList_CloseCheckList;
DDCL_DropDownCheckList.prototype.OpenCheckList = DDCL_DropDownCheckList_OpenCheckList;
