# OrderSolution

Thread-safe implementation of IOrder with unit tests.

## Features

- Thread-safe buy execution
- Prevents multiple executions
- Raises Placed event on success
- Raises Errored event on failure
- Fully unit tested
- Lock-free implementation using Interlocked

## How To Run

```bash
dotnet build
dotnet test
