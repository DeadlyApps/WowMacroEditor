/// <reference path="../jquery-1.5.1.js" />
/// <reference path="../Jeditable.js" />

$(function () {

    var siteUrl = $('#hidSiteUrl').val();

    var isOwner = $('#hidIsOwner').val() === 'true';

    if (!isOwner)
        return;
    $('#profilePage .displayName').editable(siteUrl + 'Profile/UpdateDisplayName', {
        tooltip: 'Click to edit...',
        callback: function (value, settings) {
            $('#profilePage .displayName').html(value);
        }
    });

    $('#profilePage #guildWebsite').editable(siteUrl + 'Profile/UpdateGuildWebsite', {
        tooltip: 'Click to edit...'
    });
    $('#profilePage #age').editable(siteUrl + 'Profile/UpdateBirthDate', {
        tooltip: 'Click to edit (Birthday format as MM/DD/YYYY)...'
    });
    $('#profilePage .description-edit').editable(siteUrl + 'Profile/UpdateDescription', {
        type: 'textarea',
        tooltip: 'Click to edit...',
        width: '290px',
        height: '200px',
        submit: 'OK',
        cancel: 'Cancel'
    });
    $('#profilePage #location').editable(siteUrl + 'Profile/UpdateLocation', {
        tooltip: 'Click to edit...'
    });

    $('#profilePage .email').editable(siteUrl + 'Profile/UpdateEmail', {
        tooltip: 'Click to edit...'
    });


});
