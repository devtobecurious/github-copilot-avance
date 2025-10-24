# Copilot Avancé - Application Web API .NET

## Description

Application web API .NET 9.0 avec architecture Minimal API, organisée par fonctionnalités (Features).

## Prérequis

- .NET 9.0 SDK
- IDE compatible (Visual Studio, VS Code, Rider)

## Structure du projet

```
Features/
├── GameSessions/      # Gestion des sessions de jeu
│   ├── Models/
│   ├── Repositories/
│   └── Services/
└── MagicSessions/     # Gestion des sessions de jeu Magic
    ├── Models/
    └── Repositories/
```

## Démarrage

### Restaurer les dépendances
```bash
dotnet restore "github-copilot-avance.sln"
```

### Compiler le projet
```bash
dotnet build "github-copilot-avance.sln"
```

### Lancer l'application
```bash
dotnet run --project "copilot avance.csproj"
```

L'API sera disponible sur `http://localhost:5194` (ou le port configuré).

## Endpoints disponibles

### Weather Forecast (exemple)
- **GET** `/weatherforecast` - Récupère des prévisions météo aléatoires

## OpenAPI

OpenAPI est activé en mode développement. Accès à la documentation :
- Endpoint OpenAPI : `/openapi`

## Conventions

- Namespace pattern : `Features.<Feature>.<Layer>`
- Async-first : méthodes asynchrones avec `Task`/`Task<T>`
- Modèles POCO sous `Models`
- Injection de dépendances configurée dans `Program.cs`
- Les endpoints d'une même feature sont groupés dans une classe d'extension

## Développement

Le projet suit les conventions .NET modernes :
- Top-level statements dans `Program.cs`
- Minimal API pour les endpoints
- Nullable reference types activés
- Implicit usings activés
