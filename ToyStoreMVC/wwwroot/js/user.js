var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id",
                    lockoutend: "lockoutEnd"

                },

                "render": function (data) {

                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        // user is currently locked
                        return `
                            <div class="text-center">
                            <a href="/Admin/User/Upsert/${data.id}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=LockUnlock('${data.id}') class="btn btn-warning text-white" style="cursor:pointer">
                                    <i class="fas fa-lock-open"></i> Unlock
                                </a>
                            </div>
                            `;
                    }
                    else {
                        // user is currently unlocked
                        return `
                            <div class="text-center">
                            <a href="/Admin/User/Upsert/${data.id}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Lock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-lock"></i> Lock
                                </a>
                            </div>
                            `;
                    }
                
                }, "width": "30%"
            }
        ],
        dom: 'Plfrtip',
        language: {
            "emptyTable": "Không có dữ liệu",
            "search": "Tìm kiếm:",
            "paginate": {
                "first": "Trang đầu",
                "last": "Trang cuối",
                "next": "Trang sau",
                "previous": "Trang trước"
            },
            "info": "Hiển thị _START_ đến _END_ sản phẩm / Tổng _TOTAL_ sản phẩm",
            "infoEmpty": "",
            "lengthMenu": "Hiện _MENU_ sản phẩm",
        },
    });
}

function LockUnlock(id) {
    swal({ 
        title: "Bạn chắc chắn muốn mở khóa tài khoản này?",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "POST",
                url: '/Admin/User/LockUnlock',
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data) {
                    if (data.success) {
                        toastr.info(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function Lock(id) {
    swal({
        title: "Bạn chắc chắn muốn khóa tài khoản này?",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "POST",
                url: '/Admin/User/LockUnlock',
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data) {
                    if (data.success) {
                        toastr.info(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}