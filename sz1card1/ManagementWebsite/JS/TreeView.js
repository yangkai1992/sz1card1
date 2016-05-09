function OnTreeNodeChecked() {
    var childcount = 0;
    var child = new Array
    var ele = event.srcElement;
    if (ele.type == 'checkbox') {
        var childrenDivID = ele.id.replace('CheckBox', 'Nodes');
        var div = document.getElementById(childrenDivID);
        if (div == null) return;
        var divchilfren = div.getElementsByTagName('div');
        for (var i = 0; i < divchilfren.length; i++) {
            if (divchilfren[i].id.length > 0) {
                child[childcount] = divchilfren[i].id
                childcount++;
            }
        }
        var checkBoxs = div.getElementsByTagName('INPUT');
        for (var i = 0; i < checkBoxs.length; i++) {

            if (checkBoxs[i].type == 'checkbox') {
                var checkbool = true;
                for (var j = 0; j < child.length; j++) {
                    var childreninput = document.getElementById(child[j]).getElementsByTagName('INPUT');
                    for (var n = 0; n < childreninput.length; n++) {
                        if (childreninput[n] == checkBoxs[i]) {
                            checkbool = false;
                            continue;
                        }
                    }
                    if (document.getElementById(child[j].replace('Nodes', 'CheckBox')) == checkBoxs[i]) {
                        checkbool = false;
                    }
                }
                if (checkbool) {

                    checkBoxs[i].checked = ele.checked;
                }
            }
        }
    }
}