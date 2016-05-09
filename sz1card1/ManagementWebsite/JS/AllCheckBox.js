var Erit;
window["undefined"] = window["undefined"];

if (!Erit) {
    Erit = {};
} else if (typeof Erit != "object") {
    throw new Error("This namespace has been registered.");
} else if (Erit.newClass) {
    throw new Error("The newClass has been created.");
}

Erit.SetCheckAll = function() { };

Erit.addLoadEvent = function(obj, evt, fn) {
    if (obj.addEventListener) {
        obj.addEventListener(evt, fn, false);
    }
    else if (obj.attachEvent) {
        obj.attachEvent('on' + evt, fn);
    }
};

Erit.SetCheckAll.prototype = {
    checkObj: null,
    flag: 0,
    init: function(name, eid) {
        this.checkObj = this.getInputList(name);
        var C = this.checkObj;
        var cl = C.length;
        var oThis = this;
        this.flag = 0;
        var Eid = this.get(eid);

        this.get(eid).onclick = function() {
            oThis.setEvent(this, C);
        };

        for (var i = 0; i < cl; i++) {
            C[i].onclick = function() {
                oThis.setChAllStatus(this, Eid, cl);
            }
        }
    },
    get: function(id) {
        return document.getElementsByName(id);
    },
    getInputList: function(name) {
        return document.getElementsByName(name);
    },
    setEvent: function(oThis, C) {
        var cl = C.length;
        if (oThis.checked) {
            for (var i = 0; i < cl; i++) {
                C[i].checked = true;
                if (this.flag < cl) {
                    this.flag++;
                }
            }
        } else if (!oThis.checked) {
            for (var j = 0; j < cl; j++) {
                C[j].checked = false;
                if (this.flag > 0) {
                    this.flag--;
                }
            }
        }
    },
    getSelectValue: function() {
        var chObj = this.checkObj;
        var cl = chObj.length;
        var chva = [];
        for (var i = 0; i < cl; i++) {
            if (chObj[i].checked == true) {
                chva.push(chObj[i].value);
            }
        }
        return chva;
    },
    setChAllStatus: function(oThis, e, l) {
        if (oThis.checked == true) {
            if (this.flag <= l) {
                this.flag++;
            }
        } else if (oThis.checked == false) {
            if (this.flag >= 0) {
                this.flag--;
            }
        }

        for (var j = 0; j < l; j++) {
            if (this.flag == l) {
                e.checked = true;
            } else {
                e.checked = false;
            }
        }
    }
};

Erit.addLoadEvent(window, "load", function() {
    var er = new Erit.SetCheckAll();
    er.init("chkId", "chkAll");
  /*  er.get("btn1").onclick = function() {
        alert(er.getSelectValue());
    }  */ 
});   