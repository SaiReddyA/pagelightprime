$(document).ready(function () {
    $('#hdnStateDiv').hide();
    $('#hdnDistrictDiv').hide();
    $('#btnSave').hide();
    $('#CountryId').change(function () {
        let countryId = $(this).val();
        $('#StateId').empty();
        $('#DistrictId').empty();

        if (countryId) {
            $.getJSON(`/Location/GetStates?countryId=${countryId}`, function (data) {
                $('#hdnStateDiv').show();

                $.each(data, function (i, item) {
                    $('#StateId').append(`<option value="${item.stateId}">${item.stateName}</option>`);
                });

                if (data.length > 0) {
                    let firstStateId = data[0].stateId;
                    $('#StateId').val(firstStateId).trigger('change'); 
                }
            });
        } else {
            $('#hdnStateDiv').hide();
            $('#hdnDistrictDiv').hide();
            $('#btnSave').hide();
        }
    });

    $('#StateId').change(function () {
        let stateId = $(this).val();
        $('#DistrictId').empty();

        if (stateId) {
            $.getJSON(`/Location/GetDistricts?stateId=${stateId}`, function (data) {
                $('#hdnDistrictDiv').show();

                $.each(data, function (i, item) {
                    $('#DistrictId').append(`<option value="${item.districtId}">${item.districtName}</option>`);
                });

                if (data.length > 0) {
                    let firstDistrictId = data[0].districtId;
                    $('#DistrictId').val(firstDistrictId);
                    $('#btnSave').show();
                }
                
            });
        } else {
            $('#hdnDistrictDiv').hide();
        }
    });


    const message = $('#ToastMessageData').data('message');
    const type = $('#ToastMessageData').data('type');
    if (message) {
        showToast(message, type);
    }

    $(document).on('click', '.btn-edit', function () {
        const mappingId = $(this).data('id');
        const countryId = $(this).data('countryid');
        const stateId   = $(this).data('stateid');
        const districtId = $(this).data('districtid');
        const remarks = $(this).data('remarks');

        $('#MappingId').val(mappingId);
        $('#Remarks').val(remarks);
        $('#hdnDistrictDiv').show();
        $('#hdnStateDiv').show();
        $('#CountryId').val(countryId).trigger('change');
        setTimeout(function () {
            $('#StateId').val(stateId).trigger('change');
            setTimeout(function () {
                $('#DistrictId').val(districtId);
            }, 500);
        }, 500);

        $('#btnSave').text('Update Mapping');
    });

    $(document).on('click', '.btn-delete', function () {
        const id = $(this).data('id');
        if (confirm('Are you sure to delete this mapping?')) {
            $.post(`/Location/Delete?mappingId=${id}`, function () {
                reloadTable();
            });
        }
    });

    $(document).on('click', '#ResetId', function () {
        $('#StateId').prop('disabled', true);
        $('#DistrictId').prop('disabled', true);
        $('#btnSave').text('Save');
    });
    function reloadTable() {
        $('#mappingTableContainer').load('/Location/reload-table');
    }     
});
