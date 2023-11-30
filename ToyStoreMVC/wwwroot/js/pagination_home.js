$(document).ready(function () {
    //khởi tạo page = 1
    GetPageData(1);
});
function GetPageData(pageNum, pageSize) {
    $("#tblData").empty();
    $("#paged").empty();
    $.getJSON("Customer/Home/GetPage", { pageNumber: pageNum, pageSize: pageSize }, function (response) {
        var rowData = "";
        response.length
        for (var item of response.data) {

            rowData = rowData + `<div class="col-sm-6 col-md-3 mb-5 h-100">
            <div class="badge-img position-absolute text-uppercase text-white">
													<span class="badge-sale">`+ item.discount +`%</span>
												</div>

                <div class="card card-span h-100 rounded-3">
												<a asp-action="Details" asp-route-id="`+item.id+`"><img class="img-fluid rounded-3 h-100" src="`+item.imageUrl+`" /></a>
												<div class="card-body">
													<a asp-action="Details" asp-route-id="`+ item.id + `" class="text-1000 mb-1">` + item.name +`</a>
													<div class="row">
														<h5 class="text-info col-7 pe-2">`+ new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(item.price - (item.price * item.discount / 100)) + `</h5>
															<h6 class="text-decoration-line-through col-5 p-0">`+ new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(item.price) + `</h6>
													</div>
												</div>
											</div>
										</div>`

                    

        };

        $("#tblData").append(rowData);
        PaggingTemplate(response.totalPages, response.currentPage);
    });
}

function PaggingTemplate(totalPage, currentPage) {
    var template = "";
    var TotalPages = totalPage;
    var CurrentPage = currentPage;
    var PageNumberArray = Array();


    var countIncr = 0;

    var j = 1;
    if (currentPage > 3) {
        j = currentPage - 2;
    }

    for (j; j <= totalPage; j++) {
        PageNumberArray[countIncr] = j;
        countIncr++;
    };
    PageNumberArray = PageNumberArray.slice(0, 5);
    var FirstPage = 1;
    var LastPage = totalPage;
    if (totalPage != currentPage) {
        var ForwardOne = currentPage + 1;
    }
    var BackwardOne = 1;
    if (currentPage > 1) {
        BackwardOne = currentPage - 1;
    }

    template = template + '<ul class="pagination justify-content-center">';

    //
    if (currentPage == 1) {
        template = template + '<li class="page-item disabled"><a class="page-link" onclick="GetPageData(' + FirstPage + ')">Trang đầu</a></li>';
    } else {
        template = template + '<li class="page-item"><a class="page-link" onclick="GetPageData(' + FirstPage + ')">Trang đầu</a></li>';
    }
    template = template + '<a onclick="GetPageData(' + BackwardOne + ')"></a>';

    var numberingLoop = "";
    for (var i = 0; i < PageNumberArray.length; i++) {
        //
        if (currentPage == PageNumberArray[i]) {
            numberingLoop = numberingLoop + '<li class="page-item active"><a class="page-link" onclick="GetPageData(' + PageNumberArray[i] + ')" >' + PageNumberArray[i] + '</a></li>';
        } else {
            numberingLoop = numberingLoop + '<li class="page-item"><a class="page-link" onclick="GetPageData(' + PageNumberArray[i] + ')" >' + PageNumberArray[i] + '</a></li>';
        }
        
    }
    template = template + numberingLoop + '<a onclick="GetPageData(' + ForwardOne + ')" ></a>';

    if (currentPage == totalPage) {
        template = template + '<li class="page-item disabled"><a class="page-link" onclick="GetPageData(' + LastPage + ')">Trang cuối</a></li></ul>';
    } else {
        template = template + '<li class="page-item"><a class="page-link" onclick="GetPageData(' + LastPage + ')">Trang cuối</a></li></ul>';
    }
        
    $("#paged").append(template);
    $('#selectedId').change(function () {
        GetPageData(1, $(this).val());
    });
}