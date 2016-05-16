/* Version: 1.2.0.0
 * [summary] Babino.ComboBox javascript class.
 * 
 * [reference] Common.js
 * 
 * [help] functions to control the combobox from client-side:
 *   - get_length()
 *     [summary] get the number of items.
 *     [return type] Number
 *   - get_items()
 *     [summary] get a list of all items, each item returned as an "item" object. *
 *     [return type] Array
 *     [see also] "item" object
 *   - get_selectedItem()
 *     [summary] get the current selected item, item returned as an "item" object. *
 *     [return type] "item" object
 *     [see also] "item" object
 *   - set_selectedItem(index)
 *     [summary] set the current selected item.
 *     [param: index] Number, the index of item to be selected.
 *     [return type] Boolean, false if index is out of range, otherwise true.
 *   - get_value()
 *     [summary] get the value of the textbox.
 *     [return type] String
 *   - set_value(value)
 *     [summary] set the value of the textbox, only valid when the RenderMode is ComboBox.
 *     [param: value] String, the value to be set to the textbox.
 *     [return type] Boolean, true if ComboBox, otherwise false.
 *   - get_enabled()
 *     [summary] get the enabled status.
 *     [return type] Boolean
 *   - set_enabled(enabled)
 *     [summary] set the enabled status.
 *     [param: value] Boolean
 *   - get_readOnly()
 *     [summary] get the readOnly status.
 *     [return type] Boolean
 *   - set_readOnly(readOnly)
 *     [summary] set the readOnly status.
 *     [param: value] Boolean
 *     [return type] Boolean, true if ComboBox, otherwise false.
 *     
 *   * "item" object has following properties:
 *       - index: (Int) 0 based index
 *       - text: (String) text field
 *       - value: (String) value field
 *       - selected: (Boolean) true if selected, otherwise false.
 */

Babino.ComboBox = function(element) { 
    Babino.ComboBox.initializeBase(this, [element]);

    this._comboBoxId = null;
    this._textBoxStagingId = null;
    this._textBoxTextId = null;
    this._buttonDropDownId = null;
    this._divPopupId = null;
    this._divItemsContainerId = null;
    this._tableItemsId = null;
    
    this._divScrollId = null;
    this._divArrowUpId = null;
    this._divArrowDownId = null;
    this._divScrollBarId = null;
    
    this._buttonPostBackId = null;
    
    this._offsetX = null;
    this._offsetY = null;
    this._offsetWidth = null;
    this._maxHeight = null;
    this._selectedIndex = null;
    
    this._popupVisible = null;
    
    this._scrollMinTop = null;
    this._scrollMaxTop = null;
    this._scrollPreTop = null;
    this._scrollPreY = null;
    this._scrollLen = null;
    this._scrollMoving = false;
    this._scrollEnabled = false;
    this._scrollStepper = null;
    this._scrollStepTo = null;
    this._scrollStepLen = null;
    this._scrollStepInterval = null;
    this._scrollBarMinHeight = null;
    
    this._renderMode = null;
    
    this._enabled = null;
    this._readOnly = null;
    
    this._keypressStaging = new Array();
}

Babino.ComboBox.prototype = {

    initialize : function() {
        Babino.ComboBox.callBaseMethod(this, 'initialize');
        
        this.hidePopup();
        
        if($get(this._comboBoxId) != null) this.initializeControlTree($get(this._comboBoxId));
        
        this._bodyClickHandler = Function.createDelegate(this, this._onBodyClick);
        this._scrollbarBeginMovingHandler = Function.createDelegate(this, this._onScrollbarBeginMoving);
        this._scrollbarMovingHandler = Function.createDelegate(this, this._onScrollbarMoving);
        this._scrollbarEndMovingHandler = Function.createDelegate(this, this._onScrollbarEndMoving);
        this._scrollJumpingHandler = Function.createDelegate(this, this._onScrollJumping);
        this._scrollStepUpHandler = Function.createDelegate(this, this._onScrollStepUp);
        this._scrollStepDownHandler = Function.createDelegate(this, this._onScrollStepDown);
        this._scrollStepEndHandler = Function.createDelegate(this, this._onScrollStepEnd);
        this._scrollStepHandler = Function.createDelegate(this, this._onScrollStep);
        this._keypressHandler = Function.createDelegate(this, this._onKeypress);

        $addHandler(document.body, 'click', this._bodyClickHandler);
        if(this._renderMode == Babino.ComboBox.RenderMode.DropDownList)
            $addHandler($get(this._textBoxTextId), 'keypress', this._keypressHandler);
        $addHandler($get(this._buttonDropDownId), 'keypress', this._keypressHandler);
    },
    
    dispose : function() {
        Babino.ComboBox.callBaseMethod(this, 'dispose');
    },
    
    // Methods
    
    initializeControlTree : function(element) {
        element._rootNodeId = this._comboBoxId;
        for(var i=0; i<element.childNodes.length; i++)
            if(element.childNodes[i].tagName != null)
                this.initializeControlTree(element.childNodes[i]);
    },
    
    show : function() {
        if(this._popupVisible)
        {
            this.hidePopup();
        }
        else
        {
            if(Babino.ComboBox.__VisiblePopup != null) Babino.ComboBox.__VisiblePopup.hidePopup();
            
            $get(this._divPopupId).style.visibility = "hidden";
            $get(this._divPopupId).style.display = "inline";
            $get(this._divPopupId).style.height = "auto";
            
            var offsetWidth = parseInt(this._offsetWidth);
            var offsetX = parseInt(this._offsetX);
            var offsetY = parseInt(this._offsetY);
            
            // calculate the width of the popup dropdown list.
            var width = $get(this._textBoxTextId).offsetWidth + $get(this._buttonDropDownId).offsetWidth + offsetWidth
            
            var location = Sys.UI.DomElement.getLocation($get(this._textBoxTextId));
            
            var top = location.y + $get(this._textBoxTextId).offsetHeight;
            var left = location.x;
            
            // Resolve discrepancy when one or more parent elements have a "position" style which is set to "relative" or "absolute".
            var pos = Babino.Common.GetRelativePos($get(this._textBoxTextId));
            top = top - (location.y - pos.y);
            left = left - (location.x - pos.x);
            
            top += offsetY;
            left += offsetX;
            
            $get(this._divPopupId).style.width = width + "px";
            // remove the discrepancy caused by border.
            if($get(this._divPopupId).offsetWidth != width)
                $get(this._divPopupId).style.width = (width - ($get(this._divPopupId).offsetWidth - width)) + "px";
            $get(this._divPopupId).style.top = top + "px";
            $get(this._divPopupId).style.left = left + "px";

                
            if(this._maxHeight != "" && $get(this._divPopupId).offsetHeight > this._maxHeight)
            {
                $get(this._divScrollId).style.visibility = "hidden";
                $get(this._divScrollId).style.display = "inline";
                
                // calculate the scroll length of the popuped dropdown list.
                this._scrollLen = $get(this._divPopupId).offsetHeight - this._maxHeight;
                
                this._scrollEnabled = true;

                $addHandler($get(this._divScrollBarId), 'mousedown', this._scrollbarBeginMovingHandler);
                $addHandler(document.body, 'mousemove', this._scrollbarMovingHandler);
                $addHandler(document.body, 'mouseup', this._scrollbarEndMovingHandler);
                $addHandler($get(this._divScrollId), 'mousedown', this._scrollJumpingHandler);
                $addHandler($get(this._divArrowUpId), 'mousedown', this._scrollStepUpHandler);
                $addHandler($get(this._divArrowDownId), 'mousedown', this._scrollStepDownHandler);
                $addHandler(document.body, 'mouseup', this._scrollStepEndHandler);
                
                $get(this._divPopupId).style.height = this._maxHeight + "px";
                // remove the discrepancy caused by border.
                if($get(this._divPopupId).offsetHeight != this._maxHeight)
                    $get(this._divPopupId).style.height = (this._maxHeight - ($get(this._divPopupId).offsetHeight - this._maxHeight)) + "px";
                
                // initialize the minimum height of the scroll bar if it's null.
                if(this._scrollBarMinHeight == null) this._scrollBarMinHeight = $get(this._divScrollBarId).offsetHeight;
                
                // calculate the height of the scroll bar.
                var scrollBarMaxHeight = $get(this._divScrollId).offsetHeight - $get(this._divArrowUpId).offsetHeight - $get(this._divArrowDownId).offsetHeight;
                var scrollBarHeight = scrollBarMaxHeight - this._scrollLen;
                if(scrollBarHeight < this._scrollBarMinHeight) scrollBarHeight = this._scrollBarMinHeight;
                
                $get(this._divScrollBarId).style.height = scrollBarHeight + "px";
                // remove the discrepancy caused by border.
                if($get(this._divScrollBarId).offsetHeight != scrollBarHeight)
                    $get(this._divScrollBarId).style.height = (scrollBarHeight - ($get(this._divScrollBarId).offsetHeight - scrollBarHeight)) + "px";
                
                // calculate the minimum and maximum top of the scroll bar.
                this._scrollMinTop = $get(this._divArrowUpId).offsetHeight;
                this._scrollMaxTop = $get(this._divScrollId).offsetHeight - $get(this._divArrowDownId).offsetHeight - $get(this._divScrollBarId).offsetHeight;
                
                $get(this._divPopupId).style.visibility = "visible";
                $get(this._divScrollId).style.visibility = "visible";
                
                this.onScroll();
            }
            else
            {
                this._scrollEnabled = false;
                
                $get(this._divPopupId).style.visibility = "visible";
            }

            this._popupVisible = true;
            Babino.ComboBox.__VisiblePopup = this;
            
            this.tryAttachAjaxControlToolkit();
        }
    },
    
    hidePopup : function() {
        if(this._popupVisible)
        {
            $get(this._divPopupId).style.display = "none";
            $get(this._divScrollId).style.display = "none";
            
            if(this._scrollEnabled)
            {
                this._scrollEnabled = false;
                
                $removeHandler($get(this._divScrollBarId), 'mousedown', this._scrollbarBeginMovingHandler);
                $removeHandler(document.body, 'mousemove', this._scrollbarMovingHandler);
                $removeHandler(document.body, 'mouseup', this._scrollbarEndMovingHandler);
                $removeHandler($get(this._divScrollId), 'mousedown', this._scrollJumpingHandler);
                $removeHandler($get(this._divArrowUpId), 'mousedown', this._scrollStepUpHandler);
                $removeHandler($get(this._divArrowDownId), 'mousedown', this._scrollStepDownHandler);
                $removeHandler(document.body, 'mouseup', this._scrollStepEndHandler);
            }
            
            this._popupVisible = false;
            Babino.ComboBox.__VisiblePopup = null;
            
            this.tryDetachAjaxControlToolkit();
            
            // when _selectedIndex has been changed by _onKeypress event,
            //     change the staging textbox value and execute "onComboboxChanged" event.
            var selectedIndexChanged = ($get(this._textBoxStagingId).value != this._selectedIndex);
            $get(this._textBoxStagingId).value = this._selectedIndex;
            if(selectedIndexChanged) this.onComboboxChanged();
        }
    },
    
    onChanged : function() {
        $get(this._textBoxStagingId).value = -1;
        
        if(this._selectedIndex > -1) $get(this._tableItemsId).rows[this._selectedIndex].className = "";
        
        this._selectedIndex = -1;
        
        this.hidePopup();
        
        this.onComboboxChanged();
    },
    
    onSelected : function(element) {
        var selectedIndexChanged = ($get(this._textBoxStagingId).value != element.rowIndex);
        
        this.select(element.rowIndex);
        
        $get(this._textBoxStagingId).value = element.rowIndex;
        
        this.hidePopup();
        
        if(selectedIndexChanged) this.onComboboxChanged();
    },
    
    select : function(index) {
        if(index == -1)
        {
            $get(this._textBoxTextId).value = "";
            
            if(this._selectedIndex > -1) $get(this._tableItemsId).rows[this._selectedIndex].className = "";
            
            this._selectedIndex = -1;
        }
        else
        {
            $get(this._textBoxTextId).value = Babino.Common.GetInnerText($get(this._tableItemsId).rows[index].cells[0]);
            
            if(this._selectedIndex > -1) $get(this._tableItemsId).rows[this._selectedIndex].className = "";
            $get(this._tableItemsId).rows[index].className = "selected_BCB86A76";
            
            this._selectedIndex = index;
        }
    },
    
    onComboboxChanged : function() {
        Babino.Common.TryFireEvent($get(this._buttonPostBackId), 'click');
    },
    
    onScroll : function() {
        $get(this._divItemsContainerId).scrollTop = this._scrollLen * ($get(this._divScrollBarId).offsetTop - this._scrollMinTop) / (this._scrollMaxTop - this._scrollMinTop)
    },
    
    ensureControlTreeInitialized : function() {
        if($get(this._comboBoxId) != null && $get(this._comboBoxId)._rootNodeId == null)
        {
            this.initializeControlTree($get(this._comboBoxId));
        }
    },
    
    get_length : function() {
        return $get(this._tableItemsId).rows.length;
    },
    
    get_items : function() {
        var items = new Array();
        
        var item, text, value, selected;
        for(var i=0; i<$get(this._tableItemsId).rows.length; i++)
        {
            text = Babino.Common.GetInnerText($get(this._tableItemsId).rows[i].cells[0]);
            value = Babino.Common.GetInnerText($get(this._tableItemsId).rows[i].cells[1]);
            selected = (i == this._selectedIndex);
            
            item = "{'index':" + i + ",'text':'" + text + "','value':'" + value + "','selected':" + selected + "}";
            item = eval('(' + item + ')');
            
            items.push(item);
        }
        
        return items;
    },
    
    get_selectedItem : function() {
        var text = "", value = "", selected = false;
        
        if(this._selectedIndex > -1)
        {
            text = Babino.Common.GetInnerText($get(this._tableItemsId).rows[this._selectedIndex].cells[0]);
            value = Babino.Common.GetInnerText($get(this._tableItemsId).rows[this._selectedIndex].cells[1]);
            selected = true;
        }
        
        var item = "{'index':" + this._selectedIndex + ",'text':'" + text + "','value':'" + value + "','selected':" + selected + "}";
        
        return eval('(' + item + ')');
    },
    
    set_selectedItem : function(index) {
        if(index >= -1 && index < $get(this._tableItemsId).rows.length)
        {
            var selectedIndexChanged = ($get(this._textBoxStagingId).value != index);
            
            this.select(index);
            
            $get(this._textBoxStagingId).value = index;

            this.hidePopup();
        
            if(selectedIndexChanged) this.onComboboxChanged();
            
            return true;
        }
        else
        {
            return false;
        }
    },
    
    get_value : function() {
        return $get(this._textBoxTextId).value;
    },
    
    set_value : function(value) {
        if(this._renderMode == Babino.ComboBox.RenderMode.ComboBox)
        {
            var textChanged = ($get(this._textBoxStagingId).value != -1 || $get(this._textBoxTextId).value != value);
            
            $get(this._textBoxStagingId).value = -1;
            
            $get(this._textBoxTextId).value = value;
            
            if(this._selectedIndex > -1) $get(this._tableItemsId).rows[this._selectedIndex].className = "";
            
            this._selectedIndex = -1;
            
            this.hidePopup();
            
            if(textChanged) this.onComboboxChanged();
            
            return true;
        }
        else
        {
            return false;
        }
    },
    
    _onBodyClick : function(e) {
        this.ensureControlTreeInitialized();
        
        if($get(this._comboBoxId) != null && e.target._rootNodeId != this._comboBoxId)
            this.hidePopup();
    },
    
    _onScrollbarBeginMoving : function(e) {
        this._scrollPreTop = $get(this._divScrollBarId).offsetTop;
        this._scrollPreY = e.clientY;
        this._scrollMoving = true;
    },
    
    _onScrollbarMoving : function(e) {
        if(this._scrollMoving)
        {
            var newTop = this._scrollPreTop + e.clientY - this._scrollPreY;
            if(newTop < this._scrollMinTop) newTop = this._scrollMinTop;
            if(newTop > this._scrollMaxTop) newTop = this._scrollMaxTop;
            $get(this._divScrollBarId).style.top = newTop + "px";
            
            this.onScroll();
        }
    },
    
    _onScrollbarEndMoving : function(e) {
        this._scrollMoving = false;
        
        if(e.target.id != this._textBoxStagingId)
            $get(this._buttonDropDownId).focus();
    },
    
    _onScrollJumping : function(e) {
        if(e.target.id == this._divScrollId)
        {
            var location = Sys.UI.DomElement.getLocation($get(this._divArrowUpId));
            var newTop = e.clientY - location.y - $get(this._divScrollBarId).offsetHeight / 2;
            if(newTop < this._scrollMinTop) newTop = this._scrollMinTop;
            if(newTop > this._scrollMaxTop) newTop = this._scrollMaxTop;
            $get(this._divScrollBarId).style.top = newTop + "px";
            
            this._onScrollbarBeginMoving(e);
            
            this.onScroll();
        }
    },
    
    _onScrollStepUp : function() {
        this._scrollStepTo = "up";
        this._onScrollStep();
        
        this._scrollStepper = setInterval(this._scrollStepHandler, this._scrollStepInterval);
    },
    
    _onScrollStepDown : function() {
        this._scrollStepTo = "down";
        this._onScrollStep();

        this._scrollStepper = setInterval(this._scrollStepHandler, this._scrollStepInterval);
    },
    
    _onScrollStepEnd : function(e) {
        if(this._scrollStepper != null) clearInterval(this._scrollStepper);
        
        if(e.target.id != this._textBoxStagingId)
            $get(this._buttonDropDownId).focus();
    },
    
    _onScrollStep : function() {
        var newTop = $get(this._divScrollBarId).offsetTop;
        if(this._scrollStepTo == "up") newTop -= this._scrollStepLen;
        else newTop += this._scrollStepLen;
        if(newTop < this._scrollMinTop) newTop = this._scrollMinTop;
        if(newTop > this._scrollMaxTop) newTop = this._scrollMaxTop;
        $get(this._divScrollBarId).style.top = newTop + "px";
        
        this.onScroll();
    },
    
    // Interfaces for AjaxControlToolkit: hidePopup(), get_id(), _popupBehavior
    //   - AjaxControlToolkit will recognize the dropdownlist as an "AjaxControlToolkit.PopupControlBehavior.__VisiblePopup".
    //   - The dropdownlist will be closed when any AjaxControlToolkit popup is popuped.
    
    // hidePopup() : this method is already implemented.
    // get_id() : return the id of this ajax control 
    get_id : function() {
        return this._comboBoxId;
    },
    
    // _popupBehavior : always return true instead of return an AjaxControlToolkit.PopupBehavior object
    _popupBehavior : true,
    
    tryAttachAjaxControlToolkit : function() {
        try
        {
            if(typeof(AjaxControlToolkit) !== 'undefined')
                if(typeof(AjaxControlToolkit.PopupControlBehavior) !== 'undefined')
                {
                    if(AjaxControlToolkit.PopupControlBehavior.__VisiblePopup)
                    {
                        if(AjaxControlToolkit.PopupControlBehavior.__VisiblePopup._popupBehavior)
                            AjaxControlToolkit.PopupControlBehavior.__VisiblePopup.hidePopup();
                    }
                    AjaxControlToolkit.PopupControlBehavior.__VisiblePopup = this;
                }
        }
        catch(e) {}
    },
    
    tryDetachAjaxControlToolkit : function() {
        try
        {
            if(typeof(AjaxControlToolkit) !== 'undefined')
                if(typeof(AjaxControlToolkit.PopupControlBehavior) !== 'undefined')
                    AjaxControlToolkit.PopupControlBehavior.__VisiblePopup = null;
        }
        catch(e) {}
    },
    
    // search when typing
    _onKeypress : function(e) {
        if(!this._popupVisible) return;
        
        var searchStr = "";
        
        var timestamp = new Date();
        var utc_timestamp = Date.UTC(timestamp.getFullYear(), timestamp.getMonth() + 1, timestamp.getDate(), timestamp.getHours(), timestamp.getMinutes(), timestamp.getSeconds(), timestamp.getMilliseconds());

        var new_keypressStaging = new Array();
        
        if(this._keypressStaging.length > 0 && utc_timestamp - parseInt(this._keypressStaging[this._keypressStaging.length - 1][1]) <= 1000)
        {
            for(var i=0; i<this._keypressStaging.length; i++)
            {
                searchStr += this._keypressStaging[i][0];
                new_keypressStaging.push(this._keypressStaging[i]);
            }
        }
        
        var keyChar = String.fromCharCode(e.charCode).toLowerCase();
        searchStr += keyChar;
        
        var obj = eval("(['" + keyChar + "','" + utc_timestamp + "'])");
        new_keypressStaging.push(obj);
        
        this._keypressStaging = new_keypressStaging;
        
        var rowIndex = this.search(searchStr);
        
        if(rowIndex >= 0)
        {
            this.move2row(rowIndex);
            
            this.select(rowIndex);
        }
    },
    
    search : function(str) {
        var rowIndex = -1;
        
        if(str != "")
        {
            for(var i=0; i<$get(this._tableItemsId).rows.length; i++)
            {
                if(Babino.Common.GetInnerText($get(this._tableItemsId).rows[i].cells[0]).toLowerCase().indexOf(str) == 0)
                {
                    rowIndex = i;
                    break;
                }
            }
        }
        
        return rowIndex;
    },
    
    move2row : function(rowIndex) {
        if(!this._scrollEnabled) return;
    
        var row = $get(this._tableItemsId).rows[rowIndex];
        
        if(row.offsetTop < $get(this._divItemsContainerId).scrollTop)
        {
            $get(this._divItemsContainerId).scrollTop = row.offsetTop;
        }
        else if(row.offsetTop + row.offsetHeight > $get(this._divItemsContainerId).scrollTop + $get(this._divPopupId).offsetHeight)
        {
            $get(this._divItemsContainerId).scrollTop = row.offsetTop + row.offsetHeight - $get(this._divPopupId).offsetHeight;
        }

        var newTop = $get(this._divItemsContainerId).scrollTop * (this._scrollMaxTop - this._scrollMinTop) / this._scrollLen + this._scrollMinTop;
        if(newTop < this._scrollMinTop) newTop = this._scrollMinTop;
        if(newTop > this._scrollMaxTop) newTop = this._scrollMaxTop;
        $get(this._divScrollBarId).style.top = newTop + "px";
    },
    
    // Properties
    
    get_comboBoxId : function() {
        return this._comboBoxId;
    },

    set_comboBoxId : function(value) {
        if (this._comboBoxId !== value) {
            this._comboBoxId = value;
            this.raisePropertyChanged('comboBoxId');
        }
    },
    
    get_textBoxStagingId : function() {
        return this._textBoxStagingId;
    },

    set_textBoxStagingId : function(value) {
        if (this._textBoxStagingId !== value) {
            this._textBoxStagingId = value;
            this.raisePropertyChanged('textBoxStagingId');
        }
    },
    
    get_textBoxTextId : function() {
        return this._textBoxTextId;
    },

    set_textBoxTextId : function(value) {
        if (this._textBoxTextId !== value) {
            this._textBoxTextId = value;
            this.raisePropertyChanged('textBoxTextId');
        }
    },
    
    get_buttonDropDownId : function() {
        return this._buttonDropDownId;
    },

    set_buttonDropDownId : function(value) {
        if (this._buttonDropDownId !== value) {
            this._buttonDropDownId = value;
            this.raisePropertyChanged('buttonDropDownId');
        }
    },
    
    get_divPopupId : function() {
        return this._divPopupId;
    },

    set_divPopupId : function(value) {
        if (this._divPopupId !== value) {
            this._divPopupId = value;
            this.raisePropertyChanged('divPopupId');
        }
    },
    
    get_divItemsContainerId : function() {
        return this._divItemsContainerId;
    },

    set_divItemsContainerId : function(value) {
        if (this._divItemsContainerId !== value) {
            this._divItemsContainerId = value;
            this.raisePropertyChanged('divItemsContainerId');
        }
    },
    
    get_tableItemsId : function() {
        return this._tableItemsId;
    },

    set_tableItemsId : function(value) {
        if (this._tableItemsId !== value) {
            this._tableItemsId = value;
            this.raisePropertyChanged('tableItemsId');
        }
    },
    
    get_divScrollId : function() {
        return this._divScrollId;
    },

    set_divScrollId : function(value) {
        if (this._divScrollId !== value) {
            this._divScrollId = value;
            this.raisePropertyChanged('divScrollId');
        }
    },
    
    get_divArrowUpId : function() {
        return this._divArrowUpId;
    },

    set_divArrowUpId : function(value) {
        if (this._divArrowUpId !== value) {
            this._divArrowUpId = value;
            this.raisePropertyChanged('divArrowUpId');
        }
    },
    
    get_divArrowDownId : function() {
        return this._divArrowDownId;
    },

    set_divArrowDownId : function(value) {
        if (this._divArrowDownId !== value) {
            this._divArrowDownId = value;
            this.raisePropertyChanged('divArrowDownId');
        }
    },
    
    get_divScrollBarId : function() {
        return this._divScrollBarId;
    },

    set_divScrollBarId : function(value) {
        if (this._divScrollBarId !== value) {
            this._divScrollBarId = value;
            this.raisePropertyChanged('divScrollBarId');
        }
    },
    
    get_buttonPostBackId : function() {
        return this._buttonPostBackId;
    },

    set_buttonPostBackId : function(value) {
        if (this._buttonPostBackId !== value) {
            this._buttonPostBackId = value;
            this.raisePropertyChanged('buttonPostBackId');
        }
    },
    
    get_offsetX : function() {
        return this._offsetX;
    },

    set_offsetX : function(value) {
        if (this._offsetX !== value) {
            this._offsetX = value;
            this.raisePropertyChanged('offsetX');
        }
    },
    
    get_offsetY : function() {
        return this._offsetY;
    },

    set_offsetY : function(value) {
        if (this._offsetY !== value) {
            this._offsetY = value;
            this.raisePropertyChanged('offsetY');
        }
    },
    
    get_offsetWidth : function() {
        return this._offsetWidth;
    },

    set_offsetWidth : function(value) {
        if (this._offsetWidth !== value) {
            this._offsetWidth = value;
            this.raisePropertyChanged('offsetWidth');
        }
    },
    
    get_maxHeight : function() {
        return this._maxHeight;
    },

    set_maxHeight : function(value) {
        if (this._maxHeight !== value) {
            this._maxHeight = value;
            this.raisePropertyChanged('maxHeight');
        }
    },
    
    get_selectedIndex : function() {
        return this._selectedIndex;
    },

    set_selectedIndex : function(value) {
        if (this._selectedIndex !== value) {
            this._selectedIndex = value;
            this.raisePropertyChanged('selectedIndex');
        }
    },
    
    get_popupVisible : function() {
        return this._popupVisible;
    },

    set_popupVisible : function(value) {
        if (this._popupVisible !== value) {
            this._popupVisible = value;
            this.raisePropertyChanged('popupVisible');
        }
    },

    get_scrollStepLen : function() {
        return this._scrollStepLen;
    },

    set_scrollStepLen : function(value) {
        if (this._scrollStepLen !== value) {
            this._scrollStepLen = value;
            this.raisePropertyChanged('scrollStepLen');
        }
    },
    
    get_scrollStepInterval : function() {
        return this._scrollStepInterval;
    },

    set_scrollStepInterval : function(value) {
        if (this._scrollStepInterval !== value) {
            this._scrollStepInterval = value;
            this.raisePropertyChanged('scrollStepInterval');
        }
    },
    
    get_renderMode : function() {
        return this._renderMode;
    },

    set_renderMode : function(value) {
        if (this._renderMode !== value) {
            this._renderMode = value;
            this.raisePropertyChanged('renderMode');
        }
    },
    
    get_enabled : function() {
        return this._enabled;
    },
    
    set_enabled : function(enabled) {
        var isPageLoad = (this._enabled == null);
        
        if (this._enabled !== enabled) {
            this._enabled = enabled;
            this.raisePropertyChanged('enabled');
        }
            
        if(!isPageLoad)
        {
            if(this._enabled)
            {
                $get(this._textBoxTextId).disabled = false;
                $get(this._textBoxTextId).className = "text_BCB86A76";
                $get(this._buttonDropDownId).disabled = false;
                $get(this._buttonDropDownId).className = "arrow_BCB86A76";
            }
            else
            {
                $get(this._textBoxTextId).disabled = true;
                $get(this._textBoxTextId).className = "text_disabled_BCB86A76";
                $get(this._buttonDropDownId).disabled = true;
                $get(this._buttonDropDownId).className = "arrow_disabled_BCB86A76";
                
                this.hidePopup();
            }
        }
    },
    
    get_readOnly : function() {
        return this._readOnly;
    },
    
    set_readOnly : function(readOnly) {
        var isPageLoad = (this._readOnly == null);
        
        if (this._readOnly !== readOnly) {
            this._readOnly = readOnly;
            this.raisePropertyChanged('readOnly');
        }

        if(this._renderMode == Babino.ComboBox.RenderMode.ComboBox)
        {
            if(!isPageLoad) $get(this._textBoxTextId).readOnly = this._readOnly;
            return true;
        }
        else
        {
            return false;
        }
    }
}

Babino.ComboBox.RenderMode = {'DropDownList' : 'DropDownList', 'ComboBox' : 'ComboBox'};

Babino.ComboBox.__VisiblePopup = null;

// Optional descriptor for JSON serialization.
Babino.ComboBox.descriptor = {
    properties: [   {name: 'comboBoxId', type: String},
                    {name: 'textBoxStagingId', type: String},
                    {name: 'textBoxTextId', type: String},
                    {name: 'buttonDropDownId', type: String},
                    {name: 'divPopupId', type: String},
                    {name: 'divItemsContainerId', type: String},
                    {name: 'tableItemsId', type: String},
                    {name: 'divScrollId', type: String},
                    {name: 'divArrowUpId', type: String},
                    {name: 'divArrowDownId', type: String},
                    {name: 'divScrollBarId', type: String},
                    {name: 'buttonPostBackId', type: String},
                    {name: 'offsetX', type: Number},
                    {name: 'offsetY', type: Number},
                    {name: 'offsetWidth', type: Number},
                    {name: 'maxHeight', type: String},
                    {name: 'selectedIndex', type: Number},
                    {name: 'popupVisible', type: Boolean},
                    {name: 'scrollStepLen', type: Number},
                    {name: 'scrollStepInterval', type: Number},
                    {name: 'renderMode', type: String},
                    {name: 'enabled', type: Boolean},
                    {name: 'readOnly', type: Boolean} ]
}

// Register the class as a type that inherits from Sys.UI.Control.
Babino.ComboBox.registerClass('Babino.ComboBox', Sys.UI.Control);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
