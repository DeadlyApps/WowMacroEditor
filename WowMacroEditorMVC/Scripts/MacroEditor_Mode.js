CodeMirror.defineMode("clike", function (config, parserConfig) {
    var indentUnit = config.indentUnit, keywords = parserConfig.keywords,
      cpp = parserConfig.useCPP, multiLineStrings = parserConfig.multiLineStrings, $vars = parserConfig.$vars;
    var isOperatorChar = /[+\-*&%=<>!?|]/;

    function chain(stream, state, f) {
        state.tokenize = f;
        return f(stream, state);
    }

    var type;
    function ret(tp, style) {
        type = tp;
        return style;
    }

    function tokenBase(stream, state) {
        var ch = stream.next();
        if (ch == "/") {
            stream.eatWhile(/[\w\$_]/);
            if (keywords && keywords.propertyIsEnumerable(stream.current())) return ret("keyword", "slash-command");
            return ret("word", "slash-command");
        }
        else if (ch == ";") {
            if (keywords && keywords.propertyIsEnumerable(stream.current())) return ret("keyword", "meta-command");
            return ret("word", "meta-command");
        }
        else if (ch == "#") {
            stream.eatWhile(/[\w\$_]/);
            if (keywords && keywords.propertyIsEnumerable(stream.current())) return ret("keyword", "meta-command");
            return ret("word", "meta-command");
        }
        else if (ch == "[") {
            stream.eatWhile(/[^\]]/);
            stream.eatWhile(/[\]]/);
            if (keywords && keywords.propertyIsEnumerable(stream.current())) return ret("keyword", "meta-command");
            return ret("word", "bracket-command");
        }
        else if (ch == " ") {
            stream.eatWhile(/[\w\$_]/);
            if (followupcommands && followupcommands.propertyIsEnumerable(stream.current())) return ret("keyword", "option");
            return ret("word", "option");
        }
        else {
            stream.eatWhile(/[\w\$_]/);
            if (keywords && keywords.propertyIsEnumerable(stream.current())) return ret("keyword", "c-like-word");
            return ret("word", "c-like-word");
        }
    }

    function tokenString(quote) {
        return function (stream, state) {
            var escaped = false, next, end = false;
            while ((next = stream.next()) != null) {
                if (next == quote && !escaped) { end = true; break; }
                escaped = !escaped && next == "\\";
            }
            if (end || !(escaped || multiLineStrings))
                state.tokenize = tokenBase;
            return ret("string", "c-like-string");
        };
    }

    function tokenComment(stream, state) {
        var maybeEnd = false, ch;
        while (ch = stream.next()) {
            if (ch == "/" && maybeEnd) {
                state.tokenize = tokenBase;
                break;
            }
            maybeEnd = (ch == "*");
        }
        return ret("comment", "c-like-comment");
    }

    function Context(indented, column, type, align, prev) {
        this.indented = indented;
        this.column = column;
        this.type = type;
        this.align = align;
        this.prev = prev;
    }

    function pushContext(state, col, type) {
        return state.context = new Context(state.indented, col, type, null, state.context);
    }
    function popContext(state) {
        return state.context = state.context.prev;
    }

    // Interface

    return {
        startState: function (basecolumn) {
            return {
                tokenize: tokenBase,
                context: new Context((basecolumn || 0) - indentUnit, 0, "top", false),
                indented: 0,
                startOfLine: true
            };
        },

        token: function (stream, state) {
            var ctx = state.context;
            if (stream.sol()) {
                if (ctx.align == null) ctx.align = false;
                state.indented = stream.indentation();
                state.startOfLine = true;
            }
            if (stream.eatSpace()) return null;
            var style = state.tokenize(stream, state);
            if (style == "c-like-word" && stream.string.indexOf("/cast") == 0) return "cast-ability";
            if (type == "comment") return style;
            if (ctx.align == null) ctx.align = true;

            if ((type == ";" || type == ":") && ctx.type == "statement") popContext(state);
            else if (type == "{") pushContext(state, stream.column(), "}");
            else if (type == "[") pushContext(state, stream.column(), "]");
            else if (type == "(") pushContext(state, stream.column(), ")");
            else if (type == "}") {
                if (ctx.type == "statement") ctx = popContext(state);
                if (ctx.type == "}") ctx = popContext(state);
                if (ctx.type == "statement") ctx = popContext(state);
            }
            else if (type == ctx.type) popContext(state);
            else if (ctx.type == "}") pushContext(state, stream.column(), "statement");
            state.startOfLine = false;
            return style;
        },

        indent: function (state, textAfter) {
            if (state.tokenize != tokenBase) return 0;
            var firstChar = textAfter && textAfter.charAt(0), ctx = state.context, closing = firstChar == ctx.type;
            if (ctx.type == "statement") return ctx.indented + (firstChar == "{" ? 0 : indentUnit);
            else if (ctx.align) return ctx.column + (closing ? 0 : 1);
            else return ctx.indented + (closing ? 0 : indentUnit);
        },

        electricChars: "{}"
    };
});

(function () {
    function keywords(str) {
        var obj = {}, words = str.split(" ");
        for (var i = 0; i < words.length; ++i) obj[words[i]] = true;
        return obj;
    }
    var cKeywords = "show showtooltip assist cancelaura cancelform cast castrandom castsequence changeactionbar clearfocus cleartarget click dismount equip equipslot equipset focus petagressive petattack petautocastoff petautocaston petdefensive petfollow petpassive petstay startattack stopattack stopcasting stopmacro swapactionbar target targetenemy targetfriend targetlasttarget targetparty targetraid use usetalents userandom";

    CodeMirror.defineMIME("text/x-csrc", {
        name: "clike",
        useCPP: true,
        keywords: keywords(cKeywords)
    });

} ());

(function () {
    function followupcommands(str) {
        var obj = {}, words = str.split(" ");
        for (var i = 0; i < words.length; ++i) obj[words[i]] = true;
        return obj;
    }
    var cKeywords = "actionbar button channeling combat dead equipped exists flyable flying group harm help indoors modifier mounted outdoors party pet raid stance spec stealth swimming target unithasvehicleui vehicleui ";

    CodeMirror.defineMIME("text/x-csrc", {
        name: "clike",
        useCPP: true,
        keywords: cKeywords
    });

} ());