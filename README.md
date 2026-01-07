# Event Management System - Dokumentacja Projektu

System zarządzania wydarzeniami stworzony w technologii ASP.NET Core MVC. Aplikacja umożliwia przeglądanie wydarzeń, zapisywanie się na nie oraz zarządzanie nimi przez administratora.

## 1. Wymagania systemowe
Aby uruchomić projekt, na komputerze muszą być zainstalowane:
* **Visual Studio 2022** (z obsługą ASP.NET and web development).
* **.NET SDK** (wersja 8.0 lub nowsza).
* **SQL Server Express LocalDB** (instaluje się zazwyczaj razem z Visual Studio).

## 2. Instalacja i Konfiguracja

### Krok 1: Pobranie projektu
Sklonuj repozytorium lub pobierz pliki projektu na dysk lokalny.

### Krok 2: Konfiguracja Bazy Danych
Projekt wykorzystuje Entity Framework Core oraz bazę danych SQL Server LocalDB.
Łańcuch połączenia (Connection String) znajduje się w pliku `appsettings.json` i wygląda następująco:
`"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-Event_Management;Trusted_Connection=True;MultipleActiveResultSets=true"`

**Ważne:** Przed pierwszym uruchomieniem należy utworzyć bazę danych na podstawie migracji.

1.  Otwórz projekt w Visual Studio.
2.  Otwórz konsolę: **Narzędzia (Tools)** -> **Menedżer pakietów NuGet** -> **Konsola menedżera pakietów**.
3.  Wpisz polecenie:
    ```powershell
    Update-Database
    ```
4.  Poczekaj na komunikat `Done.`. Baza danych została utworzona, a konto administratora dodane.

## 3. Użytkownicy Testowi (Hasła)

System posiada wbudowany mechanizm automatycznego tworzenia konta Administratora przy pierwszym uruchomieniu.

### Administrator (Pełny dostęp)
* **Login (Email):** `admin@admin.com`
* **Hasło:** `Password123!`
* **Uprawnienia:**
    * Tworzenie, edycja i usuwanie Wydarzeń (Events).
    * Zarządzanie Kategoriami (Categories) i Miejscami (Places).
    * Podgląd wszystkich danych.

### Użytkownik Zwykły (Przykładowy)
Możesz zarejestrować nowego użytkownika ręcznie przez formularz "Register" lub użyć poniższego (jeśli został utworzony):
* **Login (Email):** `user@user.pl`
* **Hasło:** `Password123!`
* **Uprawnienia:**
    * Przeglądanie listy wydarzeń.
    * Możliwość zapisania się na wydarzenie (przycisk "Zapisz się").
    * Brak dostępu do edycji i usuwania danych.

## 4. Opis działania aplikacji (Instrukcja)

### Dla Gościa (Niezalogowany)
* Użytkownik widzi stronę startową.
* Może przeglądać listę wydarzeń (Menu -> Events).
* Może wejść w szczegóły wydarzenia, ale przy próbie zapisu zostanie poproszony o logowanie.
* Ma dostęp do Panelu Logowania (Login) i Rejestracji (Register).

### Dla Zwykłego Użytkownika (Rola: User)
1.  Po zalogowaniu użytkownik wchodzi w zakładkę **Events**.
2.  Na liście widzi podstawowe informacje.
3.  Klika przycisk **"Szczegóły / Zapisz się"**.
4.  W widoku szczegółów widzi zielony przycisk **"Zapisz się na wydarzenie"**.
5.  Po kliknięciu system rejestruje udział. Przycisk zmienia się na nieaktywny z komunikatem **"Już bierzesz udział"**.
6.  Użytkownik nie widzi przycisków edycji ani usuwania, nie ma dostępu do zarządzania kategoriami.

### Dla Administratora (Rola: Admin)
1.  Administrator po zalogowaniu ma dostęp do ukrytych dla innych zakładek **Categories** oraz **Places**. Może tam dodawać nowe opcje do słowników.
2.  W zakładce **Events** widzi dodatkowe przyciski: **Create New**, **Edit**, **Delete**.
3.  Podczas tworzenia wydarzenia (Create) wybiera Kategorię i Miejsce z list rozwijanych (relacje w bazie danych).
4.  Administrator może edytować każde wydarzenie i usuwać je z systemu.

---
*Autorzy: Kacper Śmietana, Marek Rychwa*
