var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        deferRender: true,
        responsive: true,
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "brand.name", "width": "10%" },
            { "data": "name", "width": "10%" },
            { "data": "category.name", "width": "10%" },
            { "data": "subcategory.name", "width": "10%" },
            { "data": "productType.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Product/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-info"></i>
                                </a>
                                <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Admin/Product/DeleteProduct/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash"></i>
                                </a>
                            </div>
                            `;
                }, "width": "35%"
            }
        ]
    });
}

function Delete(url) {
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
                url: url,
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