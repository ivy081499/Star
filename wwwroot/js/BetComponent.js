//  產生表格
function genTable() {
    var table = document.createElement('table');
    table.addClass('table align-middle');
    return table;
}

// 產生「排」碰資料列
//todo
function genColumnBetRow(data) {
    var tr = document.createElement('tr');

    var tdContent = document.createElement('td');
    tdContent.addClass('p-1 text-right');

    var tdOdds = document.createElement('td');

    $(tr).append(tdContent).append(tdOdds);
    return tr;
}

// 產生「連」碰資料列
//todo
function genColumnBetRow(data) {
    var tr = document.createElement('tr');

    var tdContent = document.createElement('td');
    tdContent.addClass('p-1');

    var tdOdds = document.createElement('td');

    $(tr).append(tdContent).append(tdOdds);
 return tr;
}

// 產生「連」碰資料列
//todo
function genCarBetRow(data) {
    var tr = document.createElement('tr');

    var tdContent = document.createElement('td');
    tdContent.addClass('p-1 text-end');

    var tdOdds = document.createElement('td');

    $(tr).append(tdContent).append(tdOdds);
    return tr;
}