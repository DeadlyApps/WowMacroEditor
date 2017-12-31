$(document).ready(function () {
    var template = $("#BrowseTemplate");
    var results = $("#searchResults");
    var pager = $('[name="searchPager"]');
    var resultDataUrl = results.data("template-fill");
    var resultPagerUrl = pager.data("template-page-count");
    var page = 0;
    var term = "";
    var tag = "";

    results.html("Loading Macros");

    $(":input[data-autocomplete]").each(function () {
        $(this).autocomplete({ source: $(this).attr("data-autocomplete") });
        $(this).keyup(function () {
            term = $(this).val();
            getResuts();
        });
    });

    function getResuts() {
        var queryTerms = "?page=" + page + "&term=" + term + "&tag=" + tag;
        $.getJSON(resultDataUrl + queryTerms, function (data) {
            results.html(template.tmpl(data));
            var tagButtons = $('[name="tagButton"]');
            tagButtons.button();
            tagButtons.click(function () {
                tag = $(this).data('tag').trim();
                getResuts();
            });
            $(function () {
                WME.Macros.bindMacroDisplayControllers();
            });
        });

        $.getJSON(resultPagerUrl + queryTerms, function (data) {
            pager.html("");
            for (var i = 0; i < data; i++) {
                pager.append("<a name='pageButton' data-page='" + String(i) + "'>" + String(i + 1) + "</a>");
            }
            var pageButtons = $('[name="pageButton"]');
            pageButtons.button();
            pageButtons.click(function () {
                page = $(this).data("page");
                getResuts();
            });
        });
    }

    getResuts();

});