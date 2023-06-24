function getBetInfoTemplate() {
    return `    <div class="container" id="BetInfo">
        <div class="row text-center">
            <div class="col">
                <div class="mb-3">
                    <header id="dateHeader" class="h3">2023-6-21</header>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">組頭</label>
                    <input class="form-control readonly" type="text" readonly id="bookieText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">二星總數</label>
                    <input class="form-control readonly" type="text" readonly id="totalTwoStarText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">三星總數</label>
                    <input class="form-control readonly" type="text" readonly id="totalThreeStarText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">四星總數</label>
                    <input class="form-control readonly" type="text" readonly id="totalFourStarText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">車組總數</label>
                    <input class="form-control readonly" type="text" readonly id="totalCarSetText" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">總額</label>
                    <input class="form-control readonly" type="text" readonly id="totalBetDollarsText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">二星中獎</label>
                    <input class="form-control readonly" type="text" readonly id="twoStarBonusText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">三星中獎</label>
                    <input class="form-control readonly" type="text" readonly id="threeStarBonusText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">四星中獎</label>
                    <input class="form-control readonly" type="text" readonly id="fourStarBonusText" />
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label class="form-label">總額</label>
                    <input class="form-control readonly" type="text" readonly id="totalBonusDollarsText" />
                </div>
            </div>
        </div>
        <table class="table">
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
            <tbody id="customerBetInfo">
            </tbody>
        </table>
    </div>`;
}

function createTr(id, deleteCallback) {
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
            deleteBet(id, deleteCallback);
        });

    var tdActions = document.createElement('td');
    tdActions.append(deleteBtn);

    $(tr).append(tdContent);
    $(tr).append(tdTwoStarOdds);
    $(tr).append(tdThreeStarOdds);
    $(tr).append(tdFourStarOdds);
    $(tr).append(tdCarSetOdds);
    $(tr).append(tdActions);
    $('#customerBetInfo').append(tr);

    return tr;

}

function insertDetails(data) {
    $('#customerBetInfo tbody').empty();

    //統計
    $('#dateHeader').innerText = data.date;
    $('#bookieText').val(data.bookieType);
    $('#totalTwoStarText').val(data.totalTwoStar);
    $('#totalThreeStarText').val(data.totalThreeStar);
    $('#totalFourStarText').val(data.totalFourStar);
    $('#totalCarSetText').val(data.totalCarSetStar);
    $('#totalBetDollarsText').val(data.totalBetDollarsStar);
    $('#twoStarBonusText').val(data.twoStarBonus);
    $('#threeStarBonusText').val(data.threeStarBonus);
    $('#fourStarBonusText').val(data.fourStarBonus);
    $('#totalBonusDollarsText').val(data.totalBonusDollars);

    //排碰
    $.each(data.betInfoList, function (index, info) {
        var tr = createTr(info.id);
        tr.children[0].innerHTML = JSON.stringify(info.columnList); //todo
        tr.children[1].innerHTML = info.twoStarOdds;
        tr.children[2].innerHTML = info.threeStarOdds;
        tr.children[3].innerHTML = info.fourStarOdds;
    });

    //車組
    $.each(data.carSetInfoList, function (index, carSetInfo) {
        var tr = createTr(carSetInfo.id);
        tr.children[0].innerHTML = carSetInfo.ballNumber;
        tr.children[4].innerHTML = carSetInfo.odds;
    });
}

function deleteBet(id, callback) {
    $.ajax({
        url: "api/Bet",
        method: "DELETE",
        data: id
    }).done(function (data) {
        if (data === true) {
            alert('刪除成功');

            callback();
        }
    });

}
