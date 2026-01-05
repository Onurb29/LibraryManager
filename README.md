# LibraryManager

Console-based Library Management System written in C#.

This project was created as a final project for **Introduction to Programming With C#** ("C# Coding With Microsoft Copilot").

## Features

- Add books to a fixed-size library (default capacity: 5)
- Remove books
- Search for books by full or partial title (case-insensitive)
- Borrow books with a **borrowing limit** (max 3 at a time)
- Check in / return books (clears the checked-out status)

## How it works 

- The library uses a fixed-size `string?[]` array to store book titles.
- Borrowed books are tracked separately to enforce the borrow limit.
- A checked-out flag is used to track whether a book is checked out.

## Requirements

- .NET SDK (project targets `net9.0`)

## Run the app

From the repository root:

```powershell
dotnet build

dotnet run --project .\LibraryManagementSystem\LibraryManagementSystem.csproj
```

## Commands

When prompted, type one of the commands below:

- `add` – add a book
- `remove` – remove a book
- `search` – search by title or partial title
- `borrow` – borrow a book (limited to 3 borrowed books)
- `checkin` – check in a checked-out book
- `return` – alias of `checkin`
- `exit` – quit the app

## Notes / future improvements

- Replace fixed-size arrays with `List<Book>`
- Track multiple users (each user with their own borrow limit)
- Persist books to a file or database so the library saves between runs

## License

MIT License. See `LICENSE`.

