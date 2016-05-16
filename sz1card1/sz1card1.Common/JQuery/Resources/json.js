var sz1card1 = {};
(function() {
    function f(n) {
        return n < 10 ? '0' + n : n;
    }
    if (typeof Date.prototype.toJSON !== 'function') {
        Date.prototype.toJSON = function(key) {
            return this.getUTCFullYear() + '-' +
                 f(this.getUTCMonth() + 1) + '-' +
                 f(this.getUTCDate()) + 'T' +
                 f(this.getUTCHours()) + ':' +
                 f(this.getUTCMinutes()) + ':' +
                 f(this.getUTCSeconds()) + 'Z';
        };
        String.prototype.toJSON =
        Number.prototype.toJSON =
        Boolean.prototype.toJSON = function(key) {
            return this.valOf();
        };
    }
    var cx = /[\\?\?-\?\?\?\?\?-\?\?-\?\?-\?\?\?-\?]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\?\?-\?\?\?\?\?-\?\?-\?\?-\?\?\?-\?]/g,
        gap,
        indent,
        meta = {
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"': '\\"',
            '\\': '\\\\'
        },
        rep;
    function quote(string) {
        escapable.lastIndex = 0;
        return escapable.test(string) ?
            '"' + string.replace(escapable, function(a) {
                var c = meta[a];
                return typeof c === 'string' ? c :
                    '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
            }) + '"' :
            '"' + string + '"';
    }
    function str(key, holder) {
        var i,
            k,
            v,
            length,
            mind = gap,
            partial,
            val = holder[key];
        if (val && typeof val === 'object' &&
                typeof val.toJSON === 'function') {
            val = val.toJSON(key);
        }
        if (typeof rep === 'function') {
            val = rep.call(holder, key, val);
        }
        switch (typeof val) {
            case 'string':
                return quote(val);
            case 'number':
                return isFinite(val) ? String(val) : 'null';
            case 'boolean':
            case 'null':
                return String(val);
            case 'object':
                if (!val) {
                    return 'null';
                }
                gap += indent;
                partial = [];
                if (Object.prototype.toString.apply(val) === '[object Array]') {
                    length = val.length;
                    for (i = 0; i < length; i += 1) {
                        partial[i] = str(i, val) || 'null';
                    }
                    v = partial.length === 0 ? '[]' :
                    gap ? '[\n' + gap +
                            partial.join(',\n' + gap) + '\n' +
                                mind + ']' :
                          '[' + partial.join(',') + ']';
                    gap = mind;
                    return v;
                }
                if (rep && typeof rep === 'object') {
                    length = rep.length;
                    for (i = 0; i < length; i += 1) {
                        k = rep[i];
                        if (typeof k === 'string') {
                            v = str(k, val);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                } else {
                    for (k in val) {
                        if (Object.hasOwnProperty.call(val, k)) {
                            v = str(k, val);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                }
                v = partial.length === 0 ? '{}' :
                gap ? '{\n' + gap + partial.join(',\n' + gap) + '\n' +
                        mind + '}' : '{' + partial.join(',') + '}';
                gap = mind;
                return v;
        }
    }
    if (typeof sz1card1.stringify !== 'function') {
        sz1card1.stringify = function(val, replacer, space) {
            var i;
            gap = '';
            indent = '';
            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }
            } else if (typeof space === 'string') {
                indent = space;
            }
            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                     typeof replacer.length !== 'number')) {
                throw new Error('sz1card1.stringify');
            }
            return str('', { '': val });
        };
    }
    if (typeof sz1card1.parse !== 'function') {
        sz1card1.parse = function(text, reviver) {
        return eval('(' + text + ')');   
            /*var j;
            function walk(holder, key) {
            var k, v, val = holder[key];
            if (val && typeof val === 'object') {
            for (k in val) {
            if (Object.hasOwnProperty.call(val, k)) {
            v = walk(val, k);
            if (v !== undefined) {
            val[k] = v;
            } else {
            delete val[k];
            }
            }
            }
            }
            return reviver.call(holder, key, val);
            }
            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
            text = text.replace(cx, function(a) {
            return '\\u' +
            ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
            });
            }
            alert(text);
            if (/^[\],:{}\s]*$/.
            test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@').
            replace(/"[^"\\\n\r]*"|tr|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').
            replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {
            j = eval('(' + text + ')');
            return typeof reviver === 'function' ?
            walk({ '': j }, '') : j;
            }
            throw new SyntaxError('sz1card1.parse');*/
        };
    }
} ());
