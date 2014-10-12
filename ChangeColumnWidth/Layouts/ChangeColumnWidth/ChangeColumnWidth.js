function init() {
    var listId = SP.ListOperation.Selection.getSelectedList();
    var pageUrl = SP.Utilities.Utility.getLayoutsPageUrl(
       'ChangeColumnWidth/ChangeColumnWidth.aspx?ListId=' + listId + '&PagePath=' + _spPageContextInfo.serverRequestPath);
    var options = {
        title: 'Change Column Width',
        url: pageUrl,
        width:400,
        dialogReturnValueCallback: PopUpClosed 
    }
    SP.UI.ModalDialog.showModalDialog(options);

}
function PopUpClosed(dialogResult, returnValue) {
    if (dialogResult == SP.UI.DialogResult.OK) {
        statusID = SP.UI.Status.addStatus('Status:', returnValue);
        SP.UI.Status.setStatusPriColor(statusID, 'green');
        setTimeout(function () { SP.UI.Status.removeStatus(statusID); }, 4000);
    }
    else if (dialogResult == SP.UI.DialogResult.Cancel) {
        statusID = SP.UI.Status.addStatus('Status:', returnValue);
        SP.UI.Status.setStatusPriColor(statusID, 'red');
        setTimeout(function () { SP.UI.Status.removeStatus(statusID); }, 4000);
    }
}