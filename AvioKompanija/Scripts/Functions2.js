function loadRez(korisnickoIme) {

    $.get('/api/letovi/rezervacije', { korisnickoIme: korisnickoIme })
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
            '<td class="p-3 flex flex-wrap justify-center items-center">' +
            `<img width="80" height="80" src="` + imgSrc + '"/>' + rezervacija.Let.AvioKompanija

        if (getStatusString(rezervacija.Let.Status) == "Zavrsen") {
            row += `<a href="aviokompanija.html?naziv=${rezervacija.Let.AvioKompanija}" class="text-xs w-full m-1 hover:text-blue-400">
           <i class="bi bi-star-fill text-yellow-300"></i><i class="bi bi-star-fill text-yellow-300"></i><i class="bi bi-star-fill text-yellow-300"></i> Ostavite recenziju?</a>`
        }
        row+='</td>' +
            '<td class="p-3"><i class="fas fa-plane-departure"></i>  ' + rezervacija.Let.PolaznaDest + '</td>' +
            '<td class="p-3"><i class="fas fa-plane-arrival"></i> ' + rezervacija.Let.OdredisnaDest + '</td>' +
            '<td class="p-3">' + rezervacija.Let.DatVrPolaska + '</td>' +
            '<td class="p-3">' + rezervacija.Let.DatVrDolaska + '</td>' +
            '<td class="p-3">' + rezervacija.Let.BrZauzetih + '/' + rezervacija.Let.BrSlobodnih + '</td>' +
            '<td class="p-3">' + rezervacija.BrojPutnika + '</td>' +
            '<td class="p-3">' + rezervacija.UkupnaCena + 'RSD</td>' +
            '<td class="p-3">' + getStatusRezString(rezervacija.Status) + '</td>'
        if (getStatusRezString(rezervacija.Status) == "Kreirana" || getStatusRezString(rezervacija.Status) == "Odobrena") {
            if (isMoreThan24Hours(rezervacija.Let.DatVrPolaska)==true) {
                row += `<td class="p-3"><button onclick='otkaziRezModal(${JSON.stringify(rezervacija)})' id="cancle" class="text-white p-1 bg-red-400 rounded-md cursor-pointer m-2">Otkazi</button></td>`

            } else {
                row += '<td class="flex flex-wrap p-3">Preostalo je manje od 24h do pocetka leta!</td>'
            }

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
function postReservation(reservationData) {
    $.ajax({
        url: '/api/letovi/rezervisi',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(reservationData),
        success: function (response) {
            console.log(response);
            $("#pocetnaDest").text("")
            $("#krajnjaDest").text("")
            $("#cena").text("")
            $("#reserveModal").hide()
            $("#modal-overlay").hide()
            currentFlight = {}
            $("#brPutnika").val("")
            getLetovi();
            loadRez(currentUsername)
        },
        error: function (xhr, status, error) {
            console.log("Reservation failed:", xhr.status + " " + error);
            console.log("Response text:", xhr.responseText);
            $("#errorReserve").text(xhr.responseJSON.Message)
        }
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
            loadRez(currentUsername);
        })
        .fail(function (xhr, status, error) {
            console.error(xhr.status + " Error: " + error);
        });
}
function isMoreThan24Hours(dateTimeString) {
    // Parse the date and time string into a JavaScript Date object
    var dateParts = dateTimeString.split(' ');
    console.log(dateParts);

    var date = dateParts[0].split('/');
    var time = dateParts[1].split(':');

    var day = parseInt(date[0], 10);
    var month = parseInt(date[1], 10) - 1; // JavaScript months are 0-based
    var year = parseInt(date[2], 10);
    var hours = parseInt(time[0], 10);
    var minutes = parseInt(time[1], 10);

    var givenDate = new Date(year, month, day, hours, minutes);
    console.log(givenDate);

    // Get the current date and time
    var currentDate = new Date();

    // Calculate the difference in milliseconds
    var diffMilliseconds = currentDate - givenDate;

    // Convert the difference to hours
    var diffHours = diffMilliseconds / (1000 * 60 * 60);

    // Check if the difference is more than 24 hours
    console.log(diffHours);

    return Math.abs(diffHours) > 24;
}
function postRecenzija(recenzija) {
    console.log("clicked!");

    // Convert the recenzija object to JSON
    const jsonData = JSON.stringify(recenzija);
    console.log("JSON data: ", jsonData); // Debugging line

    $.ajax({
        url: "/api/letovi/postRecenzija",
        type: "POST",
        contentType: "application/json",
        data: jsonData,
        success: function (data) {
            console.log("Server returned: " + data);
            $("#naslov").val("")
            $("#sadrzaj").val("")
            $("#rating-value").text("");
            $("#errorResponse").hide()
            $("#successResponse").text(data)
            ucitajRecenzijeOdobrene(recenzija.AvioKompanija.Naziv)

        },
        error: function (error) {
            console.error("Error: ", error);
            $("#successResponse").hide()
            $("#errorResponse").text(error.responseJSON.Message)

        }
    });
        
}
function ucitajRecenzije(naziv) {
    var recenzijeBody = $("#recenzije")
    recenzijeBody.empty()
    
    // Uzimanje ID-a aviokompanije iz URL parametara
    $.get('/api/letovi/recenzije', { nazivKompanije: naziv })
        .done(function (response) {

            console.log("Recenzije:" + JSON.stringify(response));
            recenzijeBody.append(' <h2 class="font-thin text-xl m-3 w-full">Recenzije:</h2>')
            $.each(response, function (index, recenzija) {
                var ocenaIcons = '';
                for (var i = 1; i <= 5; i++) {
                    if (i <= recenzija.Ocena) {
                        ocenaIcons += '<i class="bi bi-star-fill text-yellow-400"></i>'; // Koristi se Bootstrap icon za zvezdicu
                    } else {
                        ocenaIcons += '<i class="bi bi-star text-gray-400"></i>'; // Koristi se Bootstrap icon za praznu zvezdicu
                    }
                }
                var rec = `<div class="w-full p-3 bg-gray-700 rounded-md min-h-52 flex m-4 flex-wrap">
                    <label class="text-white">Status:<span id="status class="w-full">${getRecStatus(recenzija.Status)}</span></label>
                    <h2 class="w-full mb-1 ml-2 text-xl text-white font-thin"><i class="bi bi-person text-green-500"></i> ${recenzija.Recezent.KorisnickoIme}</h2>
                    <p class="text-md flex w-full mt-0 mb-1 ml-2">${ocenaIcons}</p>
                    <p class="text-md text-white w-full mb-1 ml-2">Ocena:<span class="text-green-400">${recenzija.Ocena}</span> / 5</p>
                    <h3 class="m-1 text-white font-semibold w-full text-lg">${recenzija.Naslov}</h3>
                    <p class="p-2 text-white text-md text-black font-thin">${recenzija.Sadrzaj}</p>
                    <div id="odobravanje" class="w-full flex justify-center p-2">
                    <button onclick='changeRecenzija(${JSON.stringify(recenzija)},"odobri")' class="p-1 rounded-md bg-green-400 text-white cursor-pointer m-2">Odobri</button>
                    <button onclick='changeRecenzija(${JSON.stringify(recenzija)},"odbij")' class="p-1 rounded-md bg-red-400 text-white cursor-pointer m-2">Odbij</button>
                    </div>
                </div>`

               
                   
                    recenzijeBody.append(rec)
                
            })
        })
        .fail(function (xhr, status, error) {
            console.log("Failed to get current recenzije:", error);

        });
}
function ucitajRecenzijeOdobrene(naziv) {
    var recenzijeBody = $("#recenzije")
    recenzijeBody.empty()

    // Uzimanje ID-a aviokompanije iz URL parametara
    $.get('/api/letovi/recenzije', { nazivKompanije: naziv })
        .done(function (response) {

            console.log("Recenzije:" + "Nestoooooooooo");
            recenzijeBody.append(' <h2 class="font-thin text-xl m-3 w-full">Recenzije:</h2>')
            $.each(response, function (index, recenzija) {
                var ocenaIcons = '';
                for (var i = 1; i <= 5; i++) {
                    if (i <= recenzija.Ocena) {
                        ocenaIcons += '<i class="bi bi-star-fill text-yellow-400"></i>'; // Koristi se Bootstrap icon za zvezdicu
                    } else {
                        ocenaIcons += '<i class="bi bi-star text-gray-400"></i>'; // Koristi se Bootstrap icon za praznu zvezdicu
                    }
                }
                var rec= `<div class="w-full p-3 bg-gray-700 rounded-md min-h-52 flex m-4 flex-wrap">
                    <label><span id="status class="w-full text-green-400">${getRecStatus(recenzija.Status)}</span></label>
                    <h2 class="w-full mb-1 ml-2 text-xl text-white font-thin"><i class="bi bi-person text-green-500"></i> ${recenzija.Recezent.KorisnickoIme}</h2>
                    <p class="text-md flex w-full mt-0 mb-1 ml-2">${ocenaIcons}</p>
                    <p class="text-md text-white w-full mb-1 ml-2">Ocena:<span class="text-green-400">${recenzija.Ocena}</span> / 5</p>
                    <h3 class="m-1 text-white font-semibold w-full text-lg">${recenzija.Naslov}</h3>
                    <p class="p-2 text-white text-md text-black font-thin">${recenzija.Sadrzaj}</p>
                    <div id="odobravanje" class="w-full flex justify-center p-2 hidden">
                    <button onclick='changeRecenzija(${JSON.stringify(recenzija)},"odobri")' class="p-2 rounded-md bg-green-400 text-white cursor-pointer m-2">Odobri</button>
                    <button onclick='changeRecenzija(${JSON.stringify(recenzija)},"odbij")' class="p-2 rounded-md bg-red-400 text-white cursor-pointer m-2">Odbij</button>
                    </div>
                </div>`

                console.log("Status:"+recenzija)
                if (getRecStatus(recenzija.Status) == "Odobrena"){
                    recenzijeBody.append(rec)

                }

                

            })
        })
        .fail(function (xhr, status, error) {
            console.log("Failed to get current recenzije:", error);

        });
}
function loadUser() {
    $.get('/api/login/currentuser')
        .done(function (userResponse) {
            currentUser = userResponse
            console.log("Logged in user:", JSON.stringify(userResponse));
            currentUsername = userResponse.KorisnickoIme
            $("#username").html('<i class="bi bi-person text-green-500"></i> '+userResponse.KorisnickoIme)
            $("#email").html(`<i class="bi bi-envelope-at text-green-500"></i> `+userResponse.Email)
            $("#ime").text('Ime:' + userResponse.Ime + ' ' + userResponse.Prezime)
            $("#datRodj").text(`Datum rodjenja:` + userResponse.DatumRodjenja)
            $("#pol").text('Pol:'+userResponse.Pol)
            $("#tip").text('Tip:'+getRole(userResponse.Tip))
            // Dodajte ostala polja po potrebi
            $("#logoutButton").show();
            loadRez(currentUsername)
        })
        .fail(function (xhr, status, error) {
            console.log("Failed to get current user:", error);
            $("#message").text("Failed to get current user: " + error);
        });
}
function changeUser(user) {
   

    $.ajax({
        url: '/api/login/updateUser',
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(user),
        success: function (response) {
            console.log('User updated successfully:', response);
            $('#updateUser').hide();
            loadUser()
            // Optionally, update the user list on the page
        },
        error: function (error) {
            console.error('Error updating user:', error);
            $("#errorUpdate").text(error.responseJSON.Message)
        }
    });

}
function getRole(role) {
    switch (role) {
        case 0:
            return "Putnik"
          
        case 1:
            return "Admin"
           
        default:
            break;

    }
}
function getRecStatus(status) {
    switch (status) {
        case 0:
            return "Kreirana"

        case 1:
            return "Odobrena"
        case 2:
            return "Odbijena"
        default:
            break;

    }
}
function changeRecenzija(recenzija, stanje) {
    if (stanje == "odobri") {
        recenzija.Status = 1
    } else {
        recenzija.Status=2
    }
    $.ajax({
        url: '/api/letovi/updateRecenzija',
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(recenzija),
        success: function (response) {
            console.log('Recenzija updated successfully:', response);
            var urlParams = new URLSearchParams(window.location.search);
            var naziv = urlParams.get('naziv');
           
                ucitajRecenzije(naziv)

           
        },
        error: function (error) {
            console.error('Error updating user:', error);
            
        }
    });

}
