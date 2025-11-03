$(document).ready(function () {

    $('#CountryId').change(function () {
        let countryId = $(this).val();
        console.log(countryId);
        $('#StateId').empty().append('<option value="">-- Select State --</option>');
        $('#DistrictId').empty().append('<option value="">-- Select District --</option>').prop('disabled', true);

        if (countryId) {
            $.getJSON(`/Location/GetStates?countryId=${countryId}`, function (data) {
                $('#StateId').prop('disabled', false);
                $.each(data, function (i, item) {
                    $('#StateId').append(`<option value="${item.stateId}">${item.stateName}</option>`);
                });
            });
        } else {
            $('#StateId').prop('disabled', true);
        }
    });

    $('#StateId').change(function () {
        let stateId = $(this).val();
        $('#DistrictId').empty().append('<option value="">-- Select District --</option>');

        if (stateId) {
            $.getJSON(`/Location/GetDistricts?stateId=${stateId}`, function (data) {
                $('#DistrictId').prop('disabled', false);
                $.each(data, function (i, item) {
                    $('#DistrictId').append(`<option value="${item.districtId}">${item.districtName}</option>`);
                });
            });
        } else {
            $('#DistrictId').prop('disabled', true);
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
