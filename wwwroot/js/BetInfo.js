﻿function generateDailyReportTemplate() {
    return `
    <div class="container" id="BetInfo">
        <div class="row">
             <div class="col">
                 <header id="dateHeader" class="h3" style="display:flex;justify-content:center;align-items:center;height:100%;"></header>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">二星總數</label>
                    <input class="form-control readonly" type="number" readonly id="totalTwoStarText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">三星總數</label>
                    <input class="form-control readonly" type="number" readonly id="totalThreeStarText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">四星總數</label>
                    <input class="form-control readonly" type="number" readonly id="totalFourStarText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">車組總數</label>
                    <input class="form-control readonly" type="number" readonly id="totalCarSetText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">支數總額</label>
                    <input class="form-control readonly" type="number" readonly id="totalBetMoneyText" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">二星中獎</label>
                    <input class="form-control readonly" type="number" readonly id="twoStarBonusText" data-bonus />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">三星中獎</label>
                    <input class="form-control readonly" type="number" readonly id="threeStarBonusText" data-bonus/>
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">四星中獎</label>
                    <input class="form-control readonly" type="number" readonly id="fourStarBonusText" data-bonus/>
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">總獎金</label>
                    <input class="form-control readonly" type="number" readonly id="totalBonusMoneyText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">總輸贏</label>
                    <input class="form-control readonly" type="number" readonly id="winLoseMoneyText" />
                </div>
            </div>
        </div>
    </div>`;
}

function generateBetInfoTableTemplate() {
    return `
    <div class="container">
       <header class="h4 text-center" data-bookie></header>
       <table class="table mb-5">
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
            <tbody>
            </tbody>
        </table>
    </div>
    `;
}

function createTr(table, bookie, id, date, customerName) {
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
    $(table).append(tr);

    return tr;

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

        setReportView(data);

        setTableView(data);
    });

    function setReportView(data) {
        var html = generateDailyReportTemplate();
        $('#betDetails').append(html);
        $('#dateHeader').text(data.date);


        //統計
        $('#totalTwoStarText').val(data.dailyReport.totalTwoStar);
        $('#totalThreeStarText').val(data.dailyReport.totalThreeStar);
        $('#totalFourStarText').val(data.dailyReport.totalFourStar);
        $('#totalCarSetText').val(data.dailyReport.totalCarSet);
        $('#totalBetMoneyText').val(data.dailyReport.totalBetMoney);
        $('#twoStarBonusText').val(data.dailyReport.twoStarBonus);
        $('#threeStarBonusText').val(data.dailyReport.threeStarBonus);
        $('#fourStarBonusText').val(data.dailyReport.fourStarBonus);
        $('#totalBonusMoneyText').val(data.dailyReport.totalBonusMoney);
        $('#winLoseMoneyText').val(data.dailyReport.winLoseMoney);
    }

    function setTableView(data) {
        var bookieList = getDistinctBookieList(data.betInfoList, data.carSetInfoList);

        $.each(bookieList, function (index, bookie) {
            var html = generateBetInfoTableTemplate();
            $('#betDetails').append(html);

            var header = $('#betDetails [data-bookie]')[index];
            $(header).text(`給 ${bookie}`);

            var table = $('#betDetails table')[index];

            //排碰
            $.each(data.betInfoList, function (index, info) {
                if (info.bookie != bookie) {
                    return;
                }

                var tr = createTr(table, data.bookie, info.id, data.date, data.customer.name);
                tr.children[0].innerHTML = info.betContent; //todo
                tr.children[1].innerHTML = info.twoStarOdds;
                tr.children[2].innerHTML = info.threeStarOdds;
                tr.children[3].innerHTML = info.fourStarOdds;
            });

            //車組
            $.each(data.carSetInfoList, function (index, carSetInfo) {
                if (info.BookieType != bookie.bookieType) {
                    return;
                }

                var tr = createTr(table, data.bookie, carSetInfo.id, data.date, data.customer.name);
                tr.children[0].innerHTML = carSetInfo.carSetNumber;
                tr.children[4].innerHTML = carSetInfo.odds;
            });
        });
    }

    function getDistinctBookieList(betInfoList, carSetInfoList) {
        var bookies = [];

        $.each(betInfoList, checkAndPush);
        $.each(carSetInfoList, checkAndPush);

        return bookies;

        function checkAndPush(index, data) {
            if ($.inArray(data.bookie, bookies) > -1) {
                return;
            }
            bookies.push(data.bookie);
        }
    }


}