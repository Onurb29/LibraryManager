 
using System;

class LibraryManager
{
    private const int Capacity = 5;
    private const int BorrowLimit = 3;

    static void Main()
    {
        // Starter implementation: fixed-size library.
        // If you later want the size to grow/shrink dynamically, switch to List<string>.
        string?[] books = new string?[Capacity];
        bool[] checkedOut = new bool[Capacity];
        string?[] borrowedBooks = new string?[BorrowLimit];

        while (true)
        {
            Console.WriteLine("What would you like to do? (add/remove/search/borrow/return/exit)");
            string action = (Console.ReadLine() ?? string.Empty).Trim();

            if (action.Equals("add", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter the title of the book to add:");
                string title = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Book title can't be empty.");
                }
                else if (TryAddBook(books, title))
                {
                    Console.WriteLine($"Added: {title}");
                }
                else
                {
                    Console.WriteLine("The library is full. No more books can be added.");
                }
            }
            else if (action.Equals("remove", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter the title of the book to remove:");
                string title = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Book title can't be empty.");
                }
                else if (TryRemoveBook(books, title))
                {
                    Console.WriteLine($"Removed: {title}");
                }
                else
                {
                    Console.WriteLine("Book not found.");
                }
            }
            else if (action.Equals("search", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter a title (or part of a title) to search for:");
                string query = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(query))
                {
                    Console.WriteLine("Search text can't be empty.");
                }
                else
                {
                    TrySearchBook(books, query);
                }
            }
            else if (action.Equals("borrow", StringComparison.OrdinalIgnoreCase))
            {
                if (!LimitBorrowing(borrowedBooks))
                {
                    // Already at limit; message printed inside LimitBorrowing.
                }
                else
                {
                    Console.WriteLine("Enter the title of the book to borrow:");
                    string title = (Console.ReadLine() ?? string.Empty).Trim();

                    if (string.IsNullOrWhiteSpace(title))
                    {
                        Console.WriteLine("Book title can't be empty.");
                    }
                    else if (TryBorrowBook(books, checkedOut, borrowedBooks, title))
                    {
                        Console.WriteLine($"Borrowed: {title}");
                    }
                    else
                    {
                        Console.WriteLine("That book isn't available to borrow.");
                    }
                }
            }
            else if (action.Equals("checkin", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter the title of the book to check in:");
                string title = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Book title can't be empty.");
                }
                else if (TryCheckInBook(books, checkedOut, borrowedBooks, title))
                {
                    Console.WriteLine($"Checked in: {title}");
                }
                else
                {
                    Console.WriteLine("That book isn't currently checked out.");
                }
            }
            else if (action.Equals("return", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter the title of the book to return:");
                string title = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Book title can't be empty.");
                }
                else if (TryReturnBook(books, checkedOut, borrowedBooks, title))
                {
                    Console.WriteLine($"Returned: {title}");
                }
                else
                {
                    Console.WriteLine("That book isn't currently borrowed.");
                }
            }
            else if (action.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid action. Please type 'add', 'remove', 'search', 'borrow', 'checkin', 'return', or 'exit'.");
            }

            PrintBooks(books, checkedOut);
            PrintBorrowedBooks(borrowedBooks);
        }
    }

    // Returns true if user is allowed to borrow more books.
    private static bool LimitBorrowing(string?[] borrowedBooks)
    {
        int borrowedCount = 0;
        for (int i = 0; i < borrowedBooks.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(borrowedBooks[i]))
            {
                borrowedCount++;
            }
        }

        if (borrowedCount >= BorrowLimit)
        {
            Console.WriteLine($"Borrowing limit reached. You can only borrow {BorrowLimit} books at a time.");
            return false;
        }

        return true;
    }

    private static bool TryAddBook(string?[] books, string title)
    {
        for (int i = 0; i < books.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(books[i]))
            {
                books[i] = title;
                return true;
            }
        }

        return false;
    }

    private static bool TryRemoveBook(string?[] books, string title)
    {
        for (int i = 0; i < books.Length; i++)
        {
            string? existing = books[i];
            if (!string.IsNullOrWhiteSpace(existing) &&
                existing.Equals(title, StringComparison.OrdinalIgnoreCase))
            {
                books[i] = null;
                return true;
            }
        }

        return false;
    }

    private static bool TryBorrowBook(string?[] books, bool[] checkedOut, string?[] borrowedBooks, string title)
    {
        // Must exist in the library and not already checked out.
        int bookIndex = FindBookIndex(books, title);
        if (bookIndex < 0)
        {
            return false;
        }

        if (checkedOut[bookIndex])
        {
            return false;
        }

        // Already in the borrowed list?
        for (int i = 0; i < borrowedBooks.Length; i++)
        {
            string? existingBorrowed = borrowedBooks[i];
            if (!string.IsNullOrWhiteSpace(existingBorrowed) &&
                existingBorrowed.Equals(title, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        // Add to borrowed list.
        for (int i = 0; i < borrowedBooks.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(borrowedBooks[i]))
            {
                borrowedBooks[i] = books[bookIndex];
                checkedOut[bookIndex] = true;
                return true;
            }
        }

        return false;
    }

    // Check-in: only succeeds if the book exists AND is currently checked out.
    // It clears the checkedOut flag and removes it from the borrowedBooks tracking list.
    private static bool TryCheckInBook(string?[] books, bool[] checkedOut, string?[] borrowedBooks, string title)
    {
        int bookIndex = FindBookIndex(books, title);
        if (bookIndex < 0)
        {
            return false;
        }

        if (!checkedOut[bookIndex])
        {
            return false;
        }

        checkedOut[bookIndex] = false;

        // Remove from borrowed list (best-effort).
        for (int i = 0; i < borrowedBooks.Length; i++)
        {
            string? existing = borrowedBooks[i];
            if (!string.IsNullOrWhiteSpace(existing) &&
                existing.Equals(title, StringComparison.OrdinalIgnoreCase))
            {
                borrowedBooks[i] = null;
                break;
            }
        }

        return true;
    }

    private static bool TryReturnBook(string?[] books, bool[] checkedOut, string?[] borrowedBooks, string title)
    {
        // Keep "return" as an alias of "checkin".
        return TryCheckInBook(books, checkedOut, borrowedBooks, title);
    }

    private static int FindBookIndex(string?[] books, string title)
    {
        for (int i = 0; i < books.Length; i++)
        {
            string? existing = books[i];
            if (!string.IsNullOrWhiteSpace(existing) &&
                existing.Equals(title, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    private static void PrintBooks(string?[] books, bool[] checkedOut)
    {
        Console.WriteLine();
        Console.WriteLine("Available books:");

        bool any = false;
        for (int i = 0; i < books.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(books[i]) && !checkedOut[i])
            {
                any = true;
                Console.WriteLine($"- {books[i]}");
            }
        }

        if (!any)
        {
            Console.WriteLine("(none)");
        }

        Console.WriteLine();
    }

    private static void PrintBorrowedBooks(string?[] borrowedBooks)
    {
        Console.WriteLine("Borrowed books:");

        bool any = false;
        for (int i = 0; i < borrowedBooks.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(borrowedBooks[i]))
            {
                any = true;
                Console.WriteLine($"- {borrowedBooks[i]}");
            }
        }

        if (!any)
        {
            Console.WriteLine("(none)");
        }

        Console.WriteLine();
    }

    private static void TrySearchBook(string?[] books, string title)
    {
        Console.WriteLine();
        //string interpolation  {title}
        Console.WriteLine($"Search results for: {title}");

        bool found = false;
        for (int i = 0; i < books.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(books[i]) &&
                books[i]!.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                found = true;
                Console.WriteLine($"- {books[i]}");
            }
        }   

        if (found)
        {
            Console.WriteLine("This book is available in the collection.");
        }
        else
        {
            Console.WriteLine("This book is not in the collection.");
        }
    }
}