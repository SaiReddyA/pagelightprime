// wwwroot/js/location-mapping.js

$(document).ready(function () {

    // Country change → load states
    $('#CountryId').change(function () {
        let countryId = $(this).val();
        console.log(countryId);
        $('#StateId').empty().append('<option value="">-- Select State --</option>');
        $('#DistrictId').empty().append('<option value="">-- Select District --</option>').prop('disabled', true);

        if (countryId) {
            $.getJSON(`/Location/GetStates?countryId=${countryId}`, function (data) {
                $('#StateId').prop('disabled', false);
                console.log(data);
                $.each(data, function (i, item) {
                    $('#StateId').append(`<option value="${item.stateId}">${item.stateName}</option>`);
                });
            });
        } else {
            $('#StateId').prop('disabled', true);
        }
    });

    // State change → load districts
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

    // Edit button click
    $(document).on('click', '.btn-edit', function () {
        const mappingId = $(this).data('id');
        const countryId = $(this).data('countryid');
        const stateId = $(this).data('stateid');
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

    // Delete button click
    $(document).on('click', '.btn-delete', function () {
        const id = $(this).data('id');
        if (confirm('Are you sure to delete this mapping?')) {
            $.post(`/Location/Delete?mappingId=${id}`, function () {
                reloadTable();
            });
        }
    });

    function reloadTable() {
        $('#mappingTableContainer').load('/Location/reload-table');
    }

    // Toast handling
    const message = $('#ToastMessageData').data('message');
    const type = $('#ToastMessageData').data('type');
    if (message) {
        showToast(message, type);
    }
});
