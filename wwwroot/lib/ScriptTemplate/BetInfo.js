function getBetInfoTemplate(bookie) {
    return `
    <div class="container" id="BetInfo${bookie}">
        <div class="row">
             <div class="col">
                 <header id="dateHeader${bookie}" class="h3" style="display:flex;justify-content:center;align-items:center;height:100%;"></header>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">組頭</label>
                    <input class="form-control readonly" type="text" readonly id="bookieText${bookie}" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">二星總數</label>
                    <input class="form-control readonly" type="number" readonly id="totalTwoStarText${bookie}" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">三星總數</label>
                    <input class="form-control readonly" type="number" readonly id="totalThreeStarText${bookie}" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">四星總數</label>
                    <input class="form-control readonly" type="number" readonly id="totalFourStarText${bookie}" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">車組總數</label>
                    <input class="form-control readonly" type="number" readonly id="totalCarSetText${bookie}" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">支數總額</label>
                    <input class="form-control readonly" type="number" readonly id="totalBetDollarsText${bookie}" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">二星中獎</label>
                    <input class="form-control readonly" type="number" readonly id="twoStarBonusText${bookie}" data-bonus />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">三星中獎</label>
                    <input class="form-control readonly" type="number" readonly id="threeStarBonusText${bookie}" data-bonus/>
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">四星中獎</label>
                    <input class="form-control readonly" type="number" readonly id="fourStarBonusText${bookie}" data-bonus/>
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">總獎金</label>
                    <input class="form-control readonly" type="number" readonly id="totalBonusDollarsText${bookie}" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">總輸贏</label>
                    <input class="form-control readonly" type="number" readonly id="winLoseDollarsText${bookie}" />
                </div>
            </div>
        </div>
        <table class="table mb-5 mt-5">
            <thead>
                <tr>
                    <th scope="col" class="w-70">內容</th>
                    <th scope="col">二星</th>
                    <th scope="col">三星</th>
                    <th scope="col">四星</th>
                    <th scope="col">車組</th>
                    <th scope="col">操作</th>
                </tr>
            </thead>
            <tbody id="customerBetInfo${bookie}">
            </tbody>
        </table>
    </div>`;
}

function createTr(bookie, id, date, customerName) {
    var tr = document.createElement('tr');
    var tdContent = document.createElement('td');
    var tdTwoStarOdds = document.createElement('td');
    var tdThreeStarOdds = document.createElement('td');
    var tdFourStarOdds = document.createElement('td');
    var tdCarSetOdds = document.createElement('td');

    var deleteBtn = document.createElement('button');
    $(deleteBtn)
        .text('刪除')
        .addClass('btn btn-danger')
        .attr('data-id', id)
        .on("click", function () {
            deleteBet(bookie, id, date, customerName);
        });

    var tdActions = document.createElement('td');
    tdActions.append(deleteBtn);

    $(tr).append(tdContent);
    $(tr).append(tdTwoStarOdds);
    $(tr).append(tdThreeStarOdds);
    $(tr).append(tdFourStarOdds);
    $(tr).append(tdCarSetOdds);
    $(tr).append(tdActions);
    $('#customerBetInfo' + bookie).append(tr);

    return tr;

}

function insertDetails(data) {
    $('#customerBetInfo' + data.bookie + ' tbody').empty();

    //統計
    $('#bookieText' + data.bookie).val(data.bookieText);
    $('#totalTwoStarText' + data.bookie).val(data.betStatistics.totalTwoStar);
    $('#totalThreeStarText' + data.bookie).val(data.betStatistics.totalThreeStar);
    $('#totalFourStarText' + data.bookie).val(data.betStatistics.totalFourStar);
    $('#totalCarSetText' + data.bookie).val(data.betStatistics.totalCarSet);
    $('#totalBetDollarsText' + data.bookie).val(data.betStatistics.totalBetDollars);
    $('#twoStarBonusText' + data.bookie).val(data.betStatistics.twoStarBonus);
    $('#threeStarBonusText' + data.bookie).val(data.betStatistics.threeStarBonus);
    $('#fourStarBonusText' + data.bookie).val(data.betStatistics.fourStarBonus);
    $('#totalBonusDollarsText' + data.bookie).val(data.betStatistics.totalBonusDollars);
    $('#winLoseDollarsText' + data.bookie).val(data.betStatistics.winLoseDollars);

    //排碰
    $.each(data.betInfoList, function (index, info) {
        var tr = createTr(data.bookie, info.id, data.date, data.customer.name);
        tr.children[0].innerHTML = info.betContent; //todo
        tr.children[1].innerHTML = info.twoStarOdds;
        tr.children[2].innerHTML = info.threeStarOdds;
        tr.children[3].innerHTML = info.fourStarOdds;
    });

    //車組
    $.each(data.carSetInfoList, function (index, carSetInfo) {
        var tr = createTr(data.bookie, carSetInfo.id, data.date, data.customer.name);
        tr.children[0].innerHTML = carSetInfo.carSetNumber;
        tr.children[4].innerHTML = carSetInfo.odds;
    });
}

function deleteBet(bookie, id, date, customerName) {
    $.ajax({
        url: "/api/Bet",
        method: "DELETE",
        data: {
            id: id,
            customerName: customerName,
            bookie: bookie,
            date: date,
        }
    }).done(function (data) {
        if (data === true) {
            alert('刪除成功');
            genertateDetailArea(date, customerName)
        } else {
            alert('刪除失敗');
        }
    });

}

function genertateDetailArea(date, customerName) {
    $('#betDetails').empty();

    $.ajax({
        url: "/api/bet/GetCustomerBet",
        method: "POST",
        data: {
            customerName: customerName,
            date: date
        },
    }).done(function (data) {
        //if (data.length == 0) {
        //    alert('無資料');
        //}


        $.each(data, function (index, value) {
            var html = getBetInfoTemplate(value.bookie);
            $('#betDetails').append(html);

            $('#dateHeader' + value.bookie).text(date);

            insertDetails(value);
        });
    });

}
