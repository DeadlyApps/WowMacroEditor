/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-1.5.1.js" />
/// <reference path="jquery-ui-1.8.11.js" />

// Detect the presense of the console.log method and if it doesn't exist then don't throw an error when attempting to access it.
$(function () {

    (function () {
        if (typeof window.console !== "object")
            window.console = {
                log: function (message) {

                }
            };
    } ());
});

var WME = WME || {};
WME.siteBasePath = '';

WME.isLoggedIn = false;

WME.redirectToLogin = function () {
    var loginUrl = WME.siteBasePath + 'Account/Logon';
    window.location = loginUrl;
};


$(document).ready(function () {
    WME.siteBasePath = $('#hidSiteUrl').val();
    WME.isLoggedIn = $('#hidIsLoggedIn').val() == 'True' ? true : false;

    $(":input[name=TagString]").each(function () {
        $(this).watermark("Enter Tags e.g. Paladin,Pvp");
    });
    $(":input[name=Description]").each(function () {
        $(this).watermark("Describe your macro so other people can find it!");
    });
    $(":input[name=Title]").each(function () {
        $(this).watermark("Enter a descriptive title.");
    });
    $(":input[name=q]").each(function () {
        $(this).watermark("Enter your search here.");
    });
    $("a.bigButton").button();
    var s = $(".selected");
    $(".menu").buttonset();
    $(".Tag").buttonset();
    s.addClass("ui-state-active").mouseleave(function () {
        $(this).addClass("ui-state-active");
    });

    var searchButton = $(":submit");
    searchButton.button();
    var helpButton = $(".help-button");
    helpButton.button({
        icons: {
            primary: "ui-icon-help"
        },
        text: false
    })

    $("#dialog-help").dialog({
        autoOpen: false,
        modal: false,
        height: 500,
        width: 500,
        buttons: {
            "Ok!": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#dialog-note").dialog({
        resizable: false,
        height: 200,
        modal: true,
        buttons: {
            "Ok!": function () {
                $(this).dialog("close");
            }
        }
    });

    $(".embed-macro").accordion({
        collapsible: true
    });

    $('[name="search"]').click(function () {
        MoveToBrowse();
    });
    $('[name="q"]').click(function () {
        MoveToBrowse();
    });
    $('.Tag').click(function () {
        MoveToBrowse();
    });
})

function MoveToBrowse() {
    var loc = "/Macros/Browse";
    if (location.pathname != loc)
        window.location = location.protocol + "//" + location.host + loc;
}

function completeSearch() {
    $(".Tag").buttonset();

    if (typeof WME.Macros.bindMacroDisplayControllers !== "undefined") {
        WME.Macros.bindMacroDisplayControllers();
    }
}

function showHelp() {
    $("#dialog-help").html('<iframe id="help-iframe" width="100%" height="100%" frameborder="0" allowfullscreen></iframe>').dialog("open");
    $("#help-iframe").attr("src", "http://www.youtube.com/embed/adB0X8io3Hk?rel=0");
    return false;
}