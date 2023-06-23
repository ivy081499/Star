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
                    <th scope="col" class="w-75">內容</th>
                    <th scope="col">二星</th>
                    <th scope="col">三星</th>
                    <th scope="col">四星</th>
                    <th scope="col">車組</th>
                </tr>
            </thead>
            <tbody id="customerBetInfo">
            </tbody>
        </table>
    </div>`;
}
//<script type="text/template" data-template="betInfo">
    //<div class="container" id="BetInfo">
    //    <div class="row text-center">
    //        <div class="col">
    //            <div class="mb-3">
    //                <header id="dateHeader" class="h3">2023-6-21</header>
    //            </div>
    //        </div>
    //    </div>
    //    <div class="row">
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">組頭</label>
    //                <input class="form-control readonly" type="text" readonly id="bookieText" />
    //            </div>
    //        </div>
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">二星總數</label>
    //                <input class="form-control readonly" type="text" readonly id="totalTwoStarText" />
    //            </div>
    //        </div>
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">三星總數</label>
    //                <input class="form-control readonly" type="text" readonly id="totalThreeStarText" />
    //            </div>
    //        </div>
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">四星總數</label>
    //                <input class="form-control readonly" type="text" readonly id="totalFourStarText" />
    //            </div>
    //        </div>
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">車組總數</label>
    //                <input class="form-control readonly" type="text" readonly id="totalCarSetText" />
    //            </div>
    //        </div>
    //    </div>
    //    <div class="row">
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">總額</label>
    //                <input class="form-control readonly" type="text" readonly id="totalBetDollarsText" />
    //            </div>
    //        </div>
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">二星中獎</label>
    //                <input class="form-control readonly" type="text" readonly id="twoStarBonusText" />
    //            </div>
    //        </div>
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">三星中獎</label>
    //                <input class="form-control readonly" type="text" readonly id="threeStarBonusText" />
    //            </div>
    //        </div>
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">四星中獎</label>
    //                <input class="form-control readonly" type="text" readonly id="fourStarBonusText" />
    //            </div>
    //        </div>
    //        <div class="col">
    //            <div class="mb-3">
    //                <label class="form-label">總額</label>
    //                <input class="form-control readonly" type="text" readonly id="totalBonusDollarsText" />
    //            </div>
    //        </div>
    //    </div>
    //    <table class="table">
    //        <thead>
    //            <tr>
    //                <th scope="col" class="w-75">內容</th>
    //                <th scope="col">二星</th>
    //                <th scope="col">三星</th>
    //                <th scope="col">四星</th>
    //                <th scope="col">車組</th>
    //            </tr>
    //        </thead>
    //        <tbody id="customerBetInfo">
    //        </tbody>
    //    </table>
    //</div>
//</script>
