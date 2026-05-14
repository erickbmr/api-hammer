# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run
dotnet run --project Lotest.csproj

# Restore dependencies
dotnet restore
```

There are no tests in this project yet.

## Architecture

This is a .NET 6 console app (`Lotest`) for load testing APIs by sending batches of HTTP POST requests. The entry point is `Program.cs`, which wires up DI and hands off to `InputWorker.Start()`.

**Request pipeline:**

1. `InputWorker` — interactive console loop. Prompts the user to choose between supplying a JSON payload (option 1) or generating random ones (option 2). Both options are currently TODO stubs that need to call `ContentWorker` then `RequestWorker`.

2. `ContentWorker` — produces `string[]` of JSON payloads, either by:
   - `GetPayloads(Dictionary<string, object> properties, int count)` — takes a parsed JSON schema and generates variations (strings get an index suffix, ints get a random value, bools alternate).
   - `GenerateRandomPayloads(int count)` — uses **Bogus** to generate fake `PayloadExample` objects.

3. `RequestWorker` — receives the payload array, prompts for target URL + auth, then fires requests via one of three modes driven by `LoadType`:
   - `Concurrent` — `Task.WhenAll`, all requests in parallel.
   - `Sequential` — fire-and-forget loop (no `await`).
   - `WaitForResponse` — sequential with `await` between each request.

4. `ClientHttp` / `IClient` — thin `HttpClient` wrapper registered via `IHttpClientFactory`. Supports `PostWithBasicAuthAsync`, `PostWithTokenAsync`, and `PostWithoutAuthAsync`.

**Key types:**
- `RequestHeaders` (DTO) — holds target URL, auth type, and credentials; constructed by `RequestWorker.GetRequestHeadersFromUser()`.
- `AuthenticationType` enum — `None`, `Token`, `Basic`.
- `LoadType` enum — `Concurrent`, `Sequential`, `WaitForResponse`.
- `Helper/JSON.cs` — Newtonsoft.Json wrappers for (de)serialization.
- `PayloadExample` — the concrete DTO used for random generation; swap or extend this when the random payload shape needs to change.

**Known TODOs in the code:**
- `InputWorker` options 1 and 2 are not yet wired to `ContentWorker`/`RequestWorker`.
- `RequestWorker` auth validation for `AuthenticationType.None` is not implemented (falls through to token path).
- `DoConcurrentRequests` prints `response.Content.ToString()` instead of reading the response body, so output will always show the type name rather than the actual response.
