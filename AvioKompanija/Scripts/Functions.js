function loadKompanije() {
    var sectionBody = $("#avioKompanije")
    $.get("/api/letovi/aviokompanije", function (data, status) {

        $.each(data, function (index, kompanija) {
            var imgSrc = '/Content/' + kompanija.Naziv + '.png'; // Construct image source path

            var card = `<div id="avioKompanija" class="bg-white rounded-md m-3 mb-6 w-1/4 flex flex-wrap">
                    <img class="w-full max-h-42" src="${imgSrc}" alt="Logo kompanije ${kompanija.Naziv}">
                    <div class="flex flex-wrap p-6">
                    <a href="Pages/aviokompanija.html?naziv=${kompanija.Naziv}" class="font-thin text-xl w-full hover:text-blue-400">${kompanija.Naziv}</a>
                    <p class="text-sm w-full">Adresa:${kompanija.Adresa}</p>
                    </div>
                </div>`;
            sectionBody.append(card);
        });
    });
}

function searchFlights(startDest, endDest, startDateStr, endDateStr, avioKompanija) {

    $.get('/api/letovi', { startDest: startDest, endDest: endDest, startDateStr: startDateStr, endDateStr: endDateStr, avioKompanija: avioKompanija })
        .done(function (data) {

            // Očisti trenutni sadržaj tabele
            updateTableActive(data)
        })
}

function sortFlights(smer) {
    $.get('/api/letovi/sort', { smer: smer })
        .done(function (data) {

            updateTableActive(data);


        })
}

function loadLetovi() {
    var tableBody = $('#flightTable');

    $.get("/api/letovi", function (data, status) {

        $.each(data, function (index, flight) {
            var imgSrc = '/Content/' + flight.AvioKompanija + '.png'; // Construct image source path

            var row = '<tr class="items-start text-gray-500 p-2 divide-y divide-slate-400">' +
                '<td class="p-3">' +
                `<img width="80" height="80" src="` + imgSrc + '"/>' + flight.AvioKompanija + '</td>' +
                '<td class="p-3"><i class="fas fa-plane-departure"></i> ' + flight.PolaznaDest + '</td>' +
                '<td class="p-3"><i class="fas fa-plane-arrival"></i> ' + flight.OdredisnaDest + '</td>' +
                '<td class="p-3">' + flight.DatVrPolaska + '</td>' +
                '<td class="p-3">' + flight.DatVrDolaska + '</td>' +
                '<td class="p-3">' + flight.BrZauzetih + '/' + flight.BrSlobodnih + '</td>' +
                '<td class="p-3">' + flight.Cena + ' RSD</td>' +
                '<td class="p-3">' + getStatusString(flight.Status) + '</td>' +

                '</tr>';
            if (flight.Status == 0) {
                tableBody.append(row);

            }
        });
    });
}
function loadSviLetovi() {
    var tableBody = $('#flightTable');
    
    $.get("/api/letovi", function (data, status) {

        $.each(data, function (index, flight) {
            var imgSrc = '/Content/' + flight.AvioKompanija + '.png'; // Construct image source path

            var row = '<tr class="items-start text-gray-500 p-2 divide-y divide-slate-400">' +
                '<td class="p-3">' +
                `<img width="80" height="80" src="` + imgSrc + '"/>' + flight.AvioKompanija + '</td>' +
                '<td class="p-3"><i class="fas fa-plane-departure"></i> ' + flight.PolaznaDest + '</td>' +
                '<td class="p-3"><i class="fas fa-plane-arrival"></i> ' + flight.OdredisnaDest + '</td>' +
                '<td class="p-3">' + flight.DatVrPolaska + '</td>' +
                '<td class="p-3">' + flight.DatVrDolaska + '</td>' +
                '<td class="p-3">' + flight.BrZauzetih + '/' + flight.BrSlobodnih + '</td>' +
                '<td class="p-3">' + flight.Cena + ' RSD</td>' +
                '<td class="p-3">' + getStatusString(flight.Status) + '</td>'
                if (flight.Status == 0) {
                row += `<td class="p-3"><button onclick='reserveFlight(${JSON.stringify(flight)})' id="reserve" class="text-white p-1 bg-green-400 rounded-md cursor-pointer m-2">Rezervisi</button></td>`


            }
            row += '</tr>'
           
                tableBody.append(row);

            
        });
    });
}
function getCurrentUser() {
    $.get('/api/login/currentuser')
        .done(function (userResponse) {
            console.log("Logged in user:", userResponse);
            $("#userInfo").show();
            $("#userUsername").append(userResponse.KorisnickoIme);
            switch (userResponse.Tip) {
                case 0:
                    $("#userRole").text("Role: Putnik");
                    break
                case 1:
                    $("#userRole").text("Role: Admin");
                    break
                default:
                    break;

            }

         
            $("#prijavaButton").hide()
            return userResponse
           
            // Dodajte ostala polja po potrebi
        })
        .fail(function (xhr, status, error) {
            console.log("Failed to get current user:", error);
            $("#message").text("Failed to get current user: " + error);
           return null
        });
}
function reserveFlight(flight) {


    
            $("#pocetnaDest").append("Od:"+flight.PolaznaDest)
            $("#krajnjaDest").append("Do:"+flight.OdredisnaDest)
            $("#cena").append("Cena:"+flight.Cena + " RSD")

            
            $("#reserveModal").show()
    $("#modal-overlay").show()
    currentFlight = flight

        



    
}
function ucitajDetaljeAviokompanije(naziv) {
    $.get('/api/letovi/aviokompanija', { naziv: naziv })
        .done(function (data) {
            var imgSrc = '/Content/' + data.Naziv + '.png';
            $('#slika').attr('src', imgSrc);
            $('#naziv').text(data.Naziv)
            $('#opis').text(data.Informacije)
            $('#adresa').text(data.Adresa)


        })
}

function getStatusString(status) {
    switch (status) {
        case 0:
            return "Aktivan";
        case 1:
            return "Otkazan";
        case 2:
            return "Zavrsen";

        default:
            return "Nepoznat"; // Možete dodati default vrijednost ili neki drugi pristup
    }
}


function updateTableActive(data) {
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
        if (flight.Status == 0) {
            tableBody.append(row);
        }
        
    });

}


   

