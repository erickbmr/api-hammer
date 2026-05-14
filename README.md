# api-hammer

A .NET 6 console application for load testing APIs by sending batches of HTTP POST requests.

## Requirements

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Running

```bash
dotnet build
dotnet run
```

## Usage

The app guides you through three steps via interactive prompts.

### Step 1 — Choose the payload source

```
1 - input a simple JSON content
2 - generate a random JSON content
0 - exit
```

**Option 1 — Manual JSON**

Paste a flat JSON object in a single line. Each field drives how the payload varies across requests:

| Field type | Behavior per request |
|---|---|
| `string` | value + sequential index (`"name0"`, `"name1"`, …) |
| `int` | random integer |
| `bool` | alternates `true` / `false` |

Example input:
```
{"name":"user","age":0,"active":false}
```

**Option 2 — Random JSON**

Generates payloads using the built-in `PayloadExample` schema (id, name, lastName, city, country, code). No input needed.

After choosing the source, enter the number of requests to send.

---

### Step 2 — Choose the load mode

```
0 - requests made at the same time        (Concurrent)
1 - requests made one after another       (Sequential / fire-and-forget)
2 - requests that wait for a response     (WaitForResponse)
```

| Mode | Behavior |
|---|---|
| **Concurrent** | All requests are fired simultaneously via `Task.WhenAll`. Responses are printed after all complete. |
| **Sequential** | Requests are fired in a loop without awaiting (fire-and-forget). Useful for max throughput with no response reading. |
| **WaitForResponse** | Each request waits for a response before the next one is sent. Response body is printed after each call. |

---

### Step 3 — Set the target and authentication

```
target: https://your-api.com/endpoint
```

Then choose the authentication type:

```
0 - none
1 - token    → prompts for the Bearer token value
2 - basic    → prompts for userName and password
```

---

## Example session

```
1 - input a simple JSON content
2 - generate a random JSON content
0 - exit
(1/2/0): 2

number of requests: 5

0 - requests made at the same time
1 - requests made one after another (fire and forget)
2 - requests that wait for a response before the next
(0/1/2): 2

now, add the POST requests info...
target: https://httpbin.org/post
authentication...
0 - none
1 - token
2 - basic
(0/1/2): 0

{ ... response body 1 ... }
{ ... response body 2 ... }
{ ... response body 3 ... }
{ ... response body 4 ... }
{ ... response body 5 ... }
```
