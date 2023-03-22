var origin = document.location.origin;

$(document).ready(function () {
    $("select[name='PaisId']").change(function () {
        var selected = this.value;
        console.log(selected);
        if (selected) {
            $.get(origin + '/api/Container/Marcas/' + selected).then(function (data) {
                $("select[name='MarcaId']").find('option').remove();
                $.each(data, function (index, value) {
                    $("select[name='MarcaId']").append($("<option/>", { value: value.MarcaId, text: value.Nombre }));
                });
            });
        }
    });
});