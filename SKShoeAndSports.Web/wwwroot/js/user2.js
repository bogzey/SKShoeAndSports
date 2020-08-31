var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        deferRender: true,
        responsive: true,
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "30%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id",
                },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick=Delete("${data.id}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash"></i>
                                </a>
                            </div>
                            `;
                }, "width": "25%"
            }
        ]
    });
}

function Delete(id) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: '/Admin/User/Delete',
                id: id,
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
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