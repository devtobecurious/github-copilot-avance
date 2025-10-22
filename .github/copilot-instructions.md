<!--
Short, actionable instructions for AI coding agents working in this repository.
Keep this file to ~20-50 lines. Update if project layout or build requirements change.
-->
# Global
Always communicate in French
No formal or informal address forms.
IMPORTANT: Do not use "I" or "you" pronouns. 
Explanations should be concise, targeting senior developers with 6+ years of experience.

# Copilot instructions (repo-specific)
This repo is a saas .NET web application (minimal API) targeting .NET 10 with features organized under `Features/`.
Follow these concise rules to be productive and avoid breaking the project.

0. Structure
- Features directory contains each feature for the web api
- Each feature container endpoint (in minimal api) has its own `Models`, `Repositories`, and `Services` folders.
- `Program.cs` is the entry point for the minimal api application.

1. Big picture
   - Entry point: `Program.cs` (minimal API). OpenAPI is enabled via `builder.Services.AddOpenApi()`.
   - Feature layout: `Features/<Feature>/Models`, `.../Repositories`, `.../Services`. Keep that separation.
   - Repositories are defined as interfaces (e.g. `IGameSessionRepository`) and services implement business rules (e.g. `GameSessionService`).

2. Key files to inspect before editing
   - `Program.cs` — API mappings and DI registrations.
   - `copilot avance.csproj` — target framework (`net10.0`) and package references (OpenAPI preview package).
   - `Features/GameSessions/Models/GameSession.cs` — domain model shape (Guid Id, timestamps, IsActive).
   - `Features/GameSessions/Repositories/IGameSessionRepository.cs` — repository contract (async CRUD signatures).
   - `Features/GameSessions/Services/GameSessionService.cs` — business logic layer (contains English comments).
  

3. Conventions & patterns
   - Namespace pattern: `Features.<Feature>.<Layer>` (use the same when adding files).
   - Async-first: repository and service methods use `Task`/`Task<T>` — follow async signatures.
   - Keep models as plain POCOs placed under `Models` unless introducing DTOs for API boundaries.
   - Use dependency injection; register implementations in `Program.cs`. Before adding a concrete repository, check if one already exists or if the project relies on external implementations.
   - All endpoint in same feature should be grouped in a extension class <Feature>Extensions.cs (e.g. `GameSessionsExtensions.cs`), and using MapGroup to group endpoints.

4. Build / run / debug (developer workflows)
   - Build: `dotnet build` (ensure .NET 10 SDK/preview is installed if build fails).
   - Run: `dotnet run --project "copilot avance.csproj"` or run via the solution in an IDE.
   - There are no tests in the repo; do not add tests without asking the repo owner.

5. External integrations and cautions
   - The project references a preview OpenAPI package for .NET 10. If you get package or SDK errors, verify the developer has the matching SDK.
   - Each time a repository is created, use mysql connection for ef core integration

6. Small examples (follow these patterns)
   - New repository implementation: create `Features/GameSessions/Repositories/Implementations/InMemoryGameSessionRepository.cs` and register in `Program.cs` with `builder.Services.AddSingleton<IGameSessionRepository, InMemoryGameSessionRepository>();`.
   - Add API route: keep minimal API style (e.g. `app.MapGet("/games/{id}", async (IGameSessionRepository repo, Guid id) => await repo.GetByIdAsync(id));`) and prefer injecting interfaces.

7. Language & comments
   - All comments must be in english.  Translate when necessary but preserve intent.

8. Feature creation workflow
   When creating a new feature, follow this structured workflow:
   1. Ask for feature title
   2. Ask for feature content/description
   3. Ask for expected outcomes
   4. Ask for acceptance criteria
   IMPORTANT: Always validate the object structure before proceeding
   5. Create dedicated branch for the feature (wait for approval)
   6. List all planned steps in <Feature>.steps.md file
   7. Make conventional commits at each step completion
   8. Complete code review after all steps
   IMPORTANT: Iterate until code builds and is CLEAN
   9. Ensure code is functional and clean
   10. Wait for approval before merging to main branch

