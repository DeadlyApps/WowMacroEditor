/// <reference path="../WowMacroEditor.js" />

var WME = WME || {};
WME.Macros = WME.Macros || {};



WME.Macros.MacroDisplayController = function (elemMacroDisplayTable) {
    var macroDisplayTable = $(elemMacroDisplayTable);
    var updateScoreTimeout;
    var macroID = macroDisplayTable.data('macroid');

    $('.upvote', macroDisplayTable).click(Upvote);
    $('.downvote', macroDisplayTable).click(Downvote);

    function Upvote() {
        if (!WME.isLoggedIn) {
            WME.redirectToLogin();
            return;
        }

        var canUpVote = $('.upvote', macroDisplayTable).hasClass('clickable');
        var canDownVote = $('.downvote', macroDisplayTable).hasClass('clickable');
        if (!canUpVote) {
            return;
        }

        var score = GetScore();
        score++;

        SaveScoreChange(true);
        UpdateScoreField(score);

        if (canUpVote && canDownVote) {
            $('.upvote', macroDisplayTable).removeClass('clickable');
            $('.downvote', macroDisplayTable).addClass('clickable');
        }
        else {
            $('.downvote', macroDisplayTable).addClass('clickable');
        }
    }

    function Downvote() {
        if (!WME.isLoggedIn) {
            WME.redirectToLogin();
            return;
        }
        var canUpVote = $('.upvote', macroDisplayTable).hasClass('clickable');
        var canDownVote = $('.downvote', macroDisplayTable).hasClass('clickable');
        if (!canDownVote) {
            return;
        }

        var score = GetScore();
        score--;
        SaveScoreChange(false);
        UpdateScoreField(score);

        if (canUpVote && canDownVote) {
            $('.upvote', macroDisplayTable).addClass('clickable');
            $('.downvote', macroDisplayTable).removeClass('clickable');
        }
        else {
            $('.upvote', macroDisplayTable).addClass('clickable');
        }
    }

    function GetScore() {
        return Number($('.score span', macroDisplayTable).html());
    }

    function UpdateScoreField(scoreValue) {
        $('.score span', macroDisplayTable).html(scoreValue.toString());
    }

    function SaveScoreChange(directionUp) {
        var url = WME.siteBasePath + 'Macros/RankMacro?id=' + macroID + '&RankUp=' + directionUp;
        console.log(url);
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            success: function () {
                console.log("Vote " + directionUp + " registered for " + macroID);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Error when submitting vote save.");
                console.log(jqXHR);
                console.log(textStatus);
                console.log(errorThrown);
            }
        });
    }


};

WME.Macros.bindMacroDisplayControllers = function () {
    $('.macroDisplay').each(function () {
        var controller = new WME.Macros.MacroDisplayController(this);
    });
};

$(function () {
    WME.Macros.bindMacroDisplayControllers();

});

