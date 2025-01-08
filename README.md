# currency-api
Proste API do pobierania kursów walut. Obecnie dostępne są kursy walut obcych z [Tabeli Kursów C Narodowego Banku Polskiego](https://nbp.pl/statystyka-i-sprawozdawczosc/kursy/tabela-c/).

API ma dostęp do bazy danych, w której zapisywane są  pobrane dane dotyczące kursu danej waluty na dany dzień, co minimalizuje zapytania do serwisu NBP.

## Uruchomienie

Pobranie repozytorium 

`git pull https://github.com/damoch/currency-api`
`cd currency-api/CurrencyAPI`

Przygotowanie bazy danych

`dotnet ef database update`

Od tej chwili można uruchamiać jedną komendą

`dotnet watch`


Przykładowe zapytanie:

`curl http://localhost:XXX/CurrencyData?currencyCode=eur`
(Nalezy zmienić XXX na odpowiedni numer portu)

## Roadmap

1. Dodanie lepszej obsługi świąt państwowych (obecnie obsługiwane są tylko weekendy, w każdy inny dzień zostaje zwrócona generyczna informacja o błędzie). Szkic takiej obsługi znajduje się na branchu feature/date-validation
2. Obsługa innych tabel walut obcych.
3. Dodanie aplikacji panelu administracyjnego