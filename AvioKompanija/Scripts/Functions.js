function searchFlights(startDest, endDest, startDateStr, endDateStr,avioKompanija) {

    $.get('/api/letovi', { startDest: startDest, endDest: endDest, startDateStr: startDateStr, endDateStr: endDateStr, avioKompanija: avioKompanija })
        .done(function (data) {
            
            // Očisti trenutni sadržaj tabele
            $('#flightTable').empty();
            var tableBody = $('#flightTable');
            tableBody.append(` <tr class="bg-gray-300 items-start text-gray-500 p-4">
                <th class="p-3">AvioKompanija</th>
                <th class="p-3">Polazak</th>
                <th class="p-3">Destinacija</th>
                <th class="p-3">Vreme Polaska</th>
                <th class="p-3">Vreme Dolaska</th>
                <th class="p-3">Br. Mesta</th>
                <th class="p-3">Cena</th>
                <th class="p-3">Status</th>
            </tr>`)
            $.each(data, function (index,flight) {
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
                    '<td class="p-3">' + getStatusString(flight.Status) + '</td>' +
                    '</tr>';
                tableBody.append(row);
            });
        });
}
function sortFlights(smer) {
    $.get('/api/letovi/sort', { smer: smer })
        .done(function (data) {

            updateTable(data);


        })
}
function updateTable(data) {
    $('#flightTable').empty();
    var tableBody = $('#flightTable');
    tableBody.append(` <tr class="bg-gray-300 items-start text-gray-500 p-4">
                <th class="p-3">AvioKompanija</th>
                <th class="p-3">Polazak</th>
                <th class="p-3">Destinacija</th>
                <th class="p-3">Vreme Polaska</th>
                <th class="p-3">Vreme Dolaska</th>
                <th class="p-3">Br. Mesta</th>
                <th class="p-3">Cena</th>
                <th class="p-3">Status</th>
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
            '<td class="p-3">' + getStatusString(flight.Status) + '</td>' +
            '</tr>';
        tableBody.append(row);
    });
   
}


