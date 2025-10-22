# Magic Session Creation Feature - Implementation Steps

## Feature Overview
**Title:** Ajout d'une nouvelle session de partie de jeu Magic entre amis
**Branch:** feature/magic-session-creation
**API Endpoint:** POST /sessions/create

## Implementation Steps

### Step 1: Create Domain Models
- [ ] Create `MagicSession.cs` model (Id, StartDate, StartTime, CreatedAt, Friends)
- [ ] Create `Friend.cs` model (Id, FirstName, LastName, Nickname)
- [ ] Create `Deck.cs` model (Id, Name)
- [ ] Create `SessionFriend.cs` model (relation between session, friend, deck)

### Step 2: Create DTOs for API
- [ ] Create `CreateMagicSessionRequest.cs` DTO
- [ ] Create `CreateMagicSessionResponse.cs` DTO
- [ ] Create `SessionFriendDto.cs` DTO

### Step 3: Create Repository Layer
- [ ] Create `IMagicSessionRepository.cs` interface
- [ ] Create `IFriendRepository.cs` interface  
- [ ] Create `IDeckRepository.cs` interface
- [ ] Create MySQL implementations for all repositories

### Step 4: Create Service Layer
- [ ] Create `MagicSessionService.cs` with business logic
- [ ] Implement validation for existing friends and decks
- [ ] Handle session creation with proper error handling

### Step 5: Create API Extensions
- [ ] Create `MagicSessionExtensions.cs` for endpoint mapping
- [ ] Implement POST /sessions/create endpoint
- [ ] Configure proper HTTP status codes and error responses

### Step 6: Register Dependencies
- [ ] Register repositories in `Program.cs`
- [ ] Register services in `Program.cs`
- [ ] Map MagicSession endpoints in `Program.cs`

### Step 7: Configure Database Context
- [ ] Update DbContext with new entities
- [ ] Create and run database migrations
- [ ] Ensure proper entity relationships

### Step 8: Final Testing & Validation
- [ ] Test API endpoint functionality
- [ ] Validate JSON request/response formats
- [ ] Test error scenarios (invalid deck, unknown friend)
- [ ] Ensure code builds without errors

## Acceptance Criteria
- ✅ POST /sessions/create endpoint available
- ✅ JSON input/output format as specified
- ✅ HTTP 201 for successful creation
- ✅ HTTP 400 for validation errors
- ✅ Deck and friend existence validation
- ✅ Clean, buildable code following project conventions