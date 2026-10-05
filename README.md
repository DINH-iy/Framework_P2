# Dungeon Crawler

Een kleine turn-based dungeon crawler om C#, .NET, ASP.NET Core, HTTP/JSON,
Entity Framework Core en backend-logica te leren.

## Leerdoel

De speler gebruikt een console-applicatie. De console praat via HTTP en JSON
met de API. De console gebruikt nooit rechtstreeks Entity Framework of de
database.

```text
Console-applicatie
    |
    | HTTP/JSON met HttpClient
    v
ASP.NET Core Web API
    |
    v
Controllers
    |
    v
Services en business logic
    |
    v
Entity Framework Core
    |
    v
MySQL: ConsoleGame
```

De console is verantwoordelijk voor invoer, menu's, HTTP-aanroepen en het
tekenen van het spel. De API is verantwoordelijk voor regels, encounters,
combat, levels, items, winconditie en het opslaan van de game state.

## Leerroute: negen weken

De planning gaat uit van ongeveer vier uur per week. Elke week introduceert
maximaal twee of drie nieuwe concepten. Een week begint altijd met iets dat al
werkt en eindigt met een klein, zichtbaar resultaat.

### Week 1 - Projecten starten

**Startpunt:** twee lege .NET-projecten.

**Nieuwe concepten:** C# console-applicatie en ASP.NET Core Web API.

**Bouw:**

- Maak de API en console aan.
- Laat beide projecten starten.
- Voeg een eenvoudige testactie toe die bijvoorbeeld `API werkt` teruggeeft.

**Eindresultaat:** de student begrijpt dat de console en API twee aparte
programma's zijn.

### Week 2 - HTTP-communicatie

**Startpunt:** beide programma's starten lokaal.

**Nieuwe concepten:** HTTP requests en JSON.

**Bouw:**

- Maak een `GET` endpoint in de API.
- Gebruik `HttpClient` vanuit de console.
- Toon de API-response in de console.

**Eindresultaat:** de console kan informatie ophalen zonder database.

### Week 3 - API-structuur

**Startpunt:** een werkende GET-aanroep.

**Nieuwe concepten:** controllers en dependency injection.

**Bouw:**

- Verplaats endpoints naar een controller.
- Maak één eenvoudige service.
- Geef die service via constructor injection aan de controller.

**Eindresultaat:** de student ziet het verschil tussen controller, service en
console.

### Week 4 - Database en game starten

**Startpunt:** de API kan requests ontvangen en services gebruiken.

**Nieuwe concepten:** EF Core en entities.

**Bouw:**

- Maak `Player` en `Game`.
- Maak `GameDbContext`.
- Maak de eerste migration.
- Bouw `POST /api/game`.
- Laat de console een spelernaam versturen.

**Eindresultaat:** een speler en game worden via de API in MySQL opgeslagen.

### Week 5 - Game state ophalen en bewegen

**Startpunt:** een opgeslagen game.

**Nieuwe concepten:** relaties en game state.

**Bouw:**

- Voeg `GET /api/game/{id}` toe.
- Voeg tien opeenvolgende levels toe.
- Voeg rustplaatsen toe waar de speler volledig geneest.
- Laat de API bepalen wat er op het volgende level gebeurt.

**Eindresultaat:** de speler kan doorgaan naar het volgende level en ziet de
huidige level- en roomstatus.

### Week 6 - Enemies en combat

**Startpunt:** beweging en game state werken.

**Nieuwe concepten:** business logic en meerdere services.

**Bouw:**

- Voeg `Enemy` toe en seed Skeleton, Goblin en Orc.
- Laat de API random encounters bepalen.
- Voeg light en heavy attacks toe.
- Laat de API damage, speed, tegenaanval en rewards berekenen.

**Eindresultaat:** een volledige turn-based combatronde werkt via de API.

### Week 7 - Items en inventory

**Startpunt:** de speler kan vechten en rewards ontvangen.

**Nieuwe concepten:** many-to-many relaties en DTO's.

**Bouw:**

- Voeg `Item` en `PlayerItem` toe.
- Beperk de inventory tot vier slots.
- Voeg een health potion toe.
- Voeg een equipable sword toe.
- Laat de API bepalen welke items gebruikt of uitgerust mogen worden.

**Eindresultaat:** items werken zowel binnen als buiten combat.

### Week 8 - Fouten en logging

**Startpunt:** de volledige game loop werkt.

**Nieuwe concepten:** middleware en migrations als ontwikkelproces.

**Bouw:**

- Voeg request-logging middleware toe.
- Sla methode, route, status en duur op in `ApiRequestLogs`.
- Zorg dat API-fouten begrijpelijke berichten teruggeven.
- Test foutgevallen zoals een ongeldige game of een actie tijdens combat.

**Eindresultaat:** de API is beter te volgen en fouten crashen de console niet
onnodig.

### Week 9 - Winnen en afronden

**Startpunt:** alle minimale gameplay werkt.

**Nieuwe concepten:** afronden en presenteren.

**Bouw:**

- Voeg een eenvoudige exit en winconditie toe.
- Ruim namen en formatting op.
- Controleer migrations en README-documentatie.
- Loop de volledige demo door van nieuwe game tot winst of game over.

**Eindresultaat:** een kleine maar complete backendgedreven dungeon crawler.

## Belangrijk: niet alles tegelijk

Werk steeds in deze volgorde:

1. Laat één klein onderdeel werken.
2. Test het handmatig via de console.
3. Leg uit welke verantwoordelijkheid bij de console, API, service of database ligt.
4. Voeg pas daarna het volgende concept toe.

Voorbeeld van een aanval:

```text
Speler kiest Attack
    |
    v
Console stuurt POST /api/game/12/attack
    |
    v
Controller ontvangt de request
    |
    v
Combat service berekent damage en turn order
    |
    v
EF Core slaat de game state op
    |
    v
API stuurt een DTO terug
    |
    v
Console toont het resultaat
```

## Starten

Start MySQL in WAMP en maak de database `ConsoleGame` aan.

Vanaf de repository-root:

```powershell
dotnet restore .\RPG_API\RPG_API.csproj
dotnet restore .\RPG_Console\RPG_Console.csproj
dotnet run --project .\RPG_API
```

In een tweede terminal:

```powershell
dotnet run --project .\RPG_Console
```

De API gebruikt `http://localhost:5265`.

## Database-commando's

Eenmalig EF tooling installeren:

```powershell
dotnet tool update --global dotnet-ef --version 9.0.0
```

Bestaande migrations toepassen:

```powershell
dotnet ef database update --project .\RPG_API --startup-project .\RPG_API
```

Nieuwe migration maken na een wijziging aan entities:

```powershell
dotnet ef migrations add MigrationName --project .\RPG_API --startup-project .\RPG_API --output-dir Database\Migrations
```

## Belangrijkste endpoints

| Methode | Route | Doel |
| --- | --- | --- |
| `POST` | `/api/game` | Nieuwe game starten |
| `GET` | `/api/game` | Opgeslagen games bekijken |
| `GET` | `/api/game/{id}` | Game state ophalen |
| `POST` | `/api/game/{id}/move` | Doorgaan naar het volgende level |
| `POST` | `/api/game/{id}/attack` | Light of heavy attack uitvoeren |
| `POST` | `/api/game/{id}/run` | Combat verlaten |
| `POST` | `/api/game/{id}/items/{slot}/use` | Item gebruiken |
| `GET` | `/api/game/{id}/equipable-items` | Equipable items ophalen |
| `POST` | `/api/game/{id}/equip` | Item equippen |

## Bewuste scope-limieten

Dit project bevat geen multiplayer, accounts, JWT, webfrontend, complexe AI,
quests, crafting, meerdere verdiepingen of procedural dungeon generation.
De levels, combat, inventory en inheritance zijn al voldoende extra leerstof voor
de beschikbare tijd. Voeg pas nieuwe features toe als de volledige minimale
game loop werkt.
