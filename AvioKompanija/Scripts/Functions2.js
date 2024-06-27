function loadRez(korisnickoIme) {

    $.get('/api/letovi/rezervacije', { korisnickoIme: korisnickoIme })
        .done(function (data) {


            updateTableRezervacije(data)







        })
}
function getRez() {

    $.get('/api/letovi/getAllRez')
        .done(function (data) {


            updateTableRezervacije(data)







        })
}
function getLetovi() {

    $.get('/api/letovi/getAll')
        .done(function (data) {


            updateTable(data)







        })
}
function updateTableRezervacije(data) {
    $('#rezTable').empty()
    var tableBody = $('#rezTable');
    
    tableBody.append(` <tr class="bg-gray-700 items-start text-gray-200 p-4">
                <th class="p-3">AvioKompanija</th>
                <th class="p-3">Polazak</th>
                <th class="p-3">Destinacija</th>
                <th class="p-3">Vreme Polaska</th>
                <th class="p-3">Vreme Dolaska</th>
                <th class="p-3">Br. Mesta</th>
                <th class="p-3">Br. Putnika</th>
                <th class="p-3">Ukupna cena</th>
                <th class="p-3">Status</th>
                <th class="p-3">Otkazivanje</th>
            </tr>`)
    $.each(data, function (index, rezervacija) {
        var imgSrc = '/Content/' + rezervacija.Let.AvioKompanija + '.png'; // Construct image source path

        var row = '<tr class="items-start text-gray-500 p-2 divide-y divide-slate-400">' +
            '<td class="p-3">' +
            `<img width="80" height="80" src="` + imgSrc + '"/>' + rezervacija.Let.AvioKompanija + '</td>' +
            '<td class="p-3"><i class="fas fa-plane-departure"></i>  ' + rezervacija.Let.PolaznaDest + '</td>' +
            '<td class="p-3"><i class="fas fa-plane-arrival"></i> ' + rezervacija.Let.OdredisnaDest + '</td>' +
            '<td class="p-3">' + rezervacija.Let.DatVrPolaska + '</td>' +
            '<td class="p-3">' + rezervacija.Let.DatVrDolaska + '</td>' +
            '<td class="p-3">' + rezervacija.Let.BrZauzetih + '/' + rezervacija.Let.BrSlobodnih + '</td>' +
            '<td class="p-3">' + rezervacija.BrojPutnika + '</td>' +
            '<td class="p-3">' + rezervacija.UkupnaCena + 'RSD</td>' +
            '<td class="p-3">' + getStatusRezString(rezervacija.Status) + '</td>'
        if (getStatusRezString(rezervacija.Status) == "Kreirana" || getStatusRezString(rezervacija.Status) == "Odobrena") {
            row += `<td class="p-3"><button onclick='otkaziRezModal(${JSON.stringify(rezervacija)})' id="cancle" class="text-white p-1 bg-red-400 rounded-md cursor-pointer m-2">Otkazi</button></td>`

        }

            row+='</tr>';

        tableBody.append(row);


    });

}

function getStatusRezString(status) {
    switch (status) {
        case 0:
            return "Kreirana";
        case 1:
            return "Odobrena";
        case 2:
            return "Odbijena";

        default:
            return "Nepoznat"; // Možete dodati default vrijednost ili neki drugi pristup
    }
}
function updateTable(data) {
    $('#flightTable').empty();
    var tableBody = $('#flightTable');
    tableBody.append(` <tr class="bg-gray-700 items-start text-gray-200 p-4">
                <th class="p-3">AvioKompanija</th>
                <th class="p-3">Polazak</th>
                <th class="p-3">Destinacija</th>
                <th class="p-3">Vreme Polaska</th>
                <th class="p-3">Vreme Dolaska</th>
                <th class="p-3">Br. Mesta</th>
                <th class="p-3">Cena</th>
                <th class="p-3">Status</th>
                <th class="p-3">Rezervacija</th>
            </tr>`)
    $.each(data, function (index, flight) {
        var imgSrc = '/Content/' + flight.AvioKompanija + '.png'; // Construct image source path

        var row = '<tr class="items-start text-gray-500 p-2 divide-y divide-slate-400">' +
            '<td class="p-3">' +
            `<img width="80" height="80" src="` + imgSrc + '"/>' + flight.AvioKompanija + '</td>' +
            '<td class="p-3"><i class="fas fa-plane-departure"></i>  ' + flight.PolaznaDest + '</td>' +
            '<td class="p-3"><i class="fas fa-plane-arrival"></i> ' + flight.OdredisnaDest + '</td>' +
            '<td class="p-3">' + flight.DatVrPolaska + '</td>' +
            '<td class="p-3">' + flight.DatVrDolaska + '</td>' +
            '<td class="p-3">' + flight.BrZauzetih + '/' + flight.BrSlobodnih + '</td>' +
            '<td class="p-3">' + flight.Cena + 'RSD</td>' +
            '<td class="p-3">' + getStatusString(flight.Status) + '</td>'
        if (flight.Status == 0) {
            row += `<td class="p-3"><button onclick='reserveFlight(${JSON.stringify(flight)})' id="reserve" class="text-white p-1 bg-green-400 rounded-md cursor-pointer m-2">Rezervisi</button></td>`


        }
        row += '</tr>'
        tableBody.append(row);
    });

}
function otkaziRezModal(rezervacija) {
    $("#cancleModal").show()
    $("#cancleRez").click(function () {
        otkaziRez(rezervacija)
    })
}
function otkaziRez(rezervacija) {
    // Serialize the rezervacija object to JSON
    var jsonData = JSON.stringify(rezervacija);

    // Send the POST request
    $.ajax({
        url: "/api/letovi/otkazi",
        type: "POST",
        contentType: "application/json",
        data: jsonData
    })
        .done(function (data) {
            console.log(data);
            $("#cancleModal").hide();
            getLetovi();
            getRez();
        })
        .fail(function (xhr, status, error) {
            console.error(xhr.status + " Error: " + error);
        });
}