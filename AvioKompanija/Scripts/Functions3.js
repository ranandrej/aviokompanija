function loadUsers() {

    $.get('/api/admin/korisnici')
        .done(function (data) {


            updateTableKorisnici(data)







        })
}
function serachUsers(ime, prezime) {
   
    $.get('/api/admin/searchKorisnici', { ime: ime, prezime: prezime})
        .done(function (data) {

            
            updateTableKorisnici(data)







        })
}
function updateTableKorisnici(data) {
    $('#korisnici').empty();
    var tableBody = $('#korisnici');
    tableBody.append(` <tr class="bg-gray-700 items-start text-gray-200 p-4">
                <th class="p-3">Username</th>
                <th class="p-3">Ime</th>
                <th class="p-3">Prezime</th>
                <th class="p-3">E-mail</th>
                <th class="p-3">Datum Rodjenja</th>
                <th class="p-3">Pol</th>
                <th class="p-3">Uloga</th>
               
            </tr>`)
    $.each(data, function (index, user) {
       

        var row = '<tr class="items-start text-gray-500 p-2 divide-y divide-slate-400">' +
            '<td class="p-3">' + user.KorisnickoIme + '</td>' +
            '<td class="p-3">' + user.Ime + '</td>' +
            '<td class="p-3">' + user.Prezime + '</td>' +
            '<td class="p-3">' + user.Email + '</td>' +
            '<td class="p-3">' + user.DatumRodjenja+ '</td>' +
            '<td class="p-3">' + user.Pol + '</td>' +
            '<td class="p-3">' + getRole(user.Tip) +'</td>' 
            
        row += '</tr>'
        tableBody.append(row);
    });

}
function sortUsers(smer) {
    $.get('/api/admin/sortKorisnici', { smer: smer })
        .done(function (data) {

            updateTableKorisnici(data);


        })
}
function sortUsersByDatRodj(smer) {
    $.get('/api/admin/sortKorisnici', { smer: smer })
        .done(function (data) {

            updateTableKorisnici(data);


        })
}
function loadDeleteModal(kompanija) {
    $("#modal-overlay").show()
    $("#deleteModal").show()
    $("#obrisiKompaniju").click(function () {
        obrisiKompaniju(kompanija)
    })
}
function deleteFlight(let) {
    var jsonData = JSON.stringify(let);

    // Send the POST request
    $.ajax({
        url: "/api/letovi/removeLet",
        type: "POST",
        contentType: "application/json",
        data: jsonData
    })
        .done(function (data) {
            console.log(data);

            loadFlights()
        })
        .fail(function (xhr, status, error) {
            console.error(xhr.status + " Error: " + error);
            $("#errorDeleteLet").text(xhr.responseJSON.Message)
        });
}
function obrisiKompaniju(kompanija) {
    var jsonData = JSON.stringify(kompanija);

    // Send the POST request
    $.ajax({
        url: "/api/letovi/removeKompanija",
        type: "POST",
        contentType: "application/json",
        data: jsonData
    })
        .done(function (data) {
            console.log(data);
            $("#deleteModal").hide();
            $("#modal-overlay").hide();
            loadKompanijeTable()
        })
        .fail(function (xhr, status, error) {
            console.error(xhr.status + " Error: " + error);
        });

}
function updateTableAvioKompanije(data) {
    $('#kompanije').empty();
    var tableBody = $('#kompanije');
    tableBody.append(` <tr class="bg-gray-700 items-start text-gray-200 p-4">
                <th class="p-3">Naziv</th>
                <th class="p-3">Adresa</th>
                <th class="p-3">Br.Letova</th>
                <th class="p-3">Br.Rezenzija</th>
                <th class="p-3">Brisanje</th>
                <th class="p-3">Izmena</th>
               
               
            </tr>`)
    $.each(data, function (index, kompanija) {
        var hasActiveFlight = kompanija.Letovi.some(function (let) {
            return let.Status == 0;
        });

        var row = '<tr class="items-start text-gray-500 p-2 divide-y divide-slate-400">' +
            '<td class="p-3">' + kompanija.Naziv + '</td>' +
            '<td class="p-3">' + kompanija.Adresa + '</td>' +
            '<td class="p-3">' + kompanija.Letovi.length + '</td>' +
            '<td class="p-3">' + kompanija.Recenzije.length + '</td>' 

        if (hasActiveFlight == false) {
            row += `<td class="p-3"><button onclick='loadDeleteModal(${JSON.stringify(kompanija)})' id="deleteKompanijaModal" class="text-white p-1 bg-red-400 rounded-md cursor-pointer m-2">Obrsis</button></td>`
        } else {
            row += '<td class="p-3">Brisanje nije moguce</td>'
        }
        row += `<td class="p-3"><button onclick='loadChangeModal(${JSON.stringify(kompanija)})' id="changeKompanija" class="text-white p-1 bg-yellow-400 rounded-md cursor-pointer m-2">Izmeni</button></td>`
        row += '</tr>'
        tableBody.append(row);
    });

}
function loadKompanijeTable() {
    $.get('/api/letovi/aviokompanije')
        .done(function (data) {

            updateTableAvioKompanije(data);


        })
}
function dodajAvioKompaniju(kompanija) {
    var jsonData = JSON.stringify(kompanija);

    // Send the POST request
    $.ajax({
        url: "/api/letovi/addKompanija",
        type: "POST",
        contentType: "application/json",
        data: jsonData
    })
        .done(function (data) {
            console.log(data);

            loadKompanijeTable();
            $("#addKompanijaModal").hide()
        })
        .fail(function (xhr, status, error) {
            console.error(xhr.status + " Error: " + error);
            console.log("Response text:", xhr.responseText);
            $("#errorAddKompanija").text(xhr.responseJSON.Message)
        });
}
function pretraziKompanije(companyName, companyAddress) {
    if (companyName != "" || companyAddress != "") {
    $.get('/api/admin/searchKompanije', { companyName: companyName, companyAddress: companyAddress })
        .done(function (data) {
           
                updateTableAvioKompanije(data);

            


        })
    } else {
        loadKompanijeTable()
    }
}
function changeKompanija(kompanija) {

    console.log(kompanija);
    // Send the POST request
    $.ajax({
        url: "/api/letovi/changeKompanija",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(kompanija)
    })
        .done(function (data) {
            console.log(data);

            loadKompanijeTable();
            $("#nazivKompanije").val("")
            $('#nazivKompanije').prop('readonly', false);
            $("#adresaKompanije").val("")
            $("#infoKompanije").val("")
            $("#addKompanijaModal").hide()
        })
        .fail(function (xhr, status, error) {
            console.error(xhr.status + " Error: " + error);
            console.log("Response text:", xhr.responseText);
            $("#errorAddKompanija").text(xhr.responseJSON.Message)
        });
}
function loadChangeModal(kompanija) {
    $("#addKompanijaModal").show()
    $("#izmeniKompaniju").show()
    $("#dodajKompaniju").hide()
    $("#nazivKompanije").val(kompanija.Naziv)
    $('#nazivKompanije').prop('readonly', true);
    $("#adresaKompanije").val(kompanija.Adresa)
    $("#infoKompanije").val(kompanija.Informacije)
    
    $("#izmeniKompaniju").click(function () {
        var kompanija = {
            Naziv: $("#nazivKompanije").val(),
            Adresa: $("#adresaKompanije").val(),
            Informacije: $("#infoKompanije").val(),
            Letovi: [],
            Recenzije: []
        }
        changeKompanija(kompanija)
    })
}
function loadFlights() {
    $('#letoviTable').empty()
    var tableBody = $('#letoviTable');

    $.get("/api/letovi", function (data, status) {
        tableBody.append(`<tr class="bg-gray-700 items-start text-gray-200 p-4">
            <th class="p-3">AvioKompanija</th>
            <th class="p-3">Polazak</th>
            <th class="p-3">Destinacija</th>
            <th class="p-3">Vreme Polaska</th>
            <th class="p-3">Vreme Dolaska</th>
            <th class="p-3">Br. Mesta</th>
            <th class="p-3">Cena</th>
            <th class="p-3">Status</th>
            <th class="p-3">Brisanje</th>
        </tr>`)
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
                `<td class="p-3"><button onclick='deleteFlight(${JSON.stringify(flight)})' class="text-white p-1 bg-red-400 rounded-md cursor-pointer m-2">Obrsis</button></td>`
                '</tr>';
            
                tableBody.append(row);

            
        });
    });

}
function dodajLet(let) {
    console.log(let);
    // Send the POST request
    $.ajax({
        url: "/api/letovi/addLet",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(let)
    })
        .done(function (data) {
            console.log(data);

            loadFlights()
            $("#addLetModal").hide()
        })
        .fail(function (xhr, status, error) {
            console.error(xhr.status + " Error: " + error);
            console.log("Response text:", xhr.responseText);
            $("#errorAddLet").text(xhr.responseJSON.Message)
        });
}