# Kompanion-Case-Study

I'm having a painful illness called 'executing a query without proper WHERE condition' so I had to stay after work for cleanup. That's why my schedule is shifted and I cannot foresee how long it will take so I'll surrender and deliver this half-cooked project for your roasting session. Please enjoy laughing at my attempt! (And apologize if my DBService gives you a anger induced seizure.)

# Features I haven't able to finish by the time and my insight on how would I approach these things.

- Listing with filters.

    Technically, it should work. However, I did not have time to test/debug it to confirm.
    Very sloppy DB layer I built is receiving Query Parameters as Dictionary<string, object>,
    so technically it should be good 'as long as' a seperate SP is created for filtered listing.

    I'd go for an extra nullcheck in controller to keep the SP more... generic and... simple.

    Also, another SP (SP_ReadMoveSetWithMoves) that Joins MoveSet, MoveSetMove and Move entities together for detailed listing or GET /{id} endpoints so no second requests would be required. (nobody likes second requests)

- Authorization/Authentication

    I created an IdentityService which processes UserData for Register/Login and returns a JWT Bearer code.
    However, I didn't have time to test Authorized endpoints. Basically any PUT/POST request should have [[Authorize]] attribute in their respective controller in order to provide JWT Auth checking.

    Potential To-Do List for Authorization:
    - Make it actually work. :D
    - Adding a Policy to the Authorization middleware to control UserTypes (Admin, User) would spice up things such as preventing random users being able to remove others' Movesets/Moves etc.
    - Adding a UserId check to the PUT/Update endpoints in order to prevent users updating other users' Moves/MoveSets.

- SQL Injection Prevention

    - Using an SP and .AddWithParameter() is mostly fine for SQL Injection, however, adding a sanitization
    layer to escape some special characters ( '*', '(', ')' and single quote) can improve the security. We can also overengineer this and write a Regex to prevent that happening.

- Validation

    - Business rules mentioned in the case study are kind of vague, so I avoided any sort of validation.

    But;
        - A user should only be able to Edit/Delete their own Moves/Movesets
        - Move & Moveset Names should be unique
        - Profanity / Personal Info filter in Names & Descriptions (I've seen people giving their phone number for personal training courses)
        - Password & Password Confirm matching and strong password requirements (Length, Alphanumeric, Special Character etc)


# Potential Improvements (Probably overengineering for this project but...)

- Architecture
    - CQRS (DTO's don't fit well for controller-level data transfer, every endpoint should have their own Command/Query objects) or Vertical Slices with MediatR (Or a hybrid of both - famous last words)
    - Move the ConnectionStrings to somewhere safe. (Azure KeyVault or AWS equilavent of it.)

- Entity Improvements:
    - Image Url fields for Move & MoveSets
        - MoveSet Table can have a single varchar field for keeping ImageUrl (CDN url) for cosmetic usage.
        - Move Table can have a pivot (MoveImage) for having multiple images/videos on how a move can be executed.

    - Using the cute little thing called Entity Framework (I know. I know. But I hated every second of writing that DB Layer.)


# What I learned?

    - MySQL Workbench is not as clunky as I remember. But MS is centuries ahead in terms of UX.
    - Stored Procedures are awesome. But forgetting to alter it after I altered the table is caused me a painful 5 minutes of debugging. But that's a sacrifice I'm willing to make.
    - I do not appreciate ORM's enough. What a time saver!
    - I should refresh my knowledge on Dependency Injection & computational complexity.
    - You can't type check an object on runtime and dynamically create an instance for it if it's a List<T>
        (You probably can but I couldn't.)
    - Dynamic programming is easy to implement but not every time is the best solution. Can cause more headache than it solves.
    - Swagger is still awesome but I'm holding my dear Postman closer because I'm addicted with automation.


Thanks for this project, it was fun and useful to my weakpoints. Regardless of this hiring process, I took some notes and will continue my road on constant improvements.


# Time Log

- Friday, 2th Dec, 22:00 - ~02:00
    - Pen & Paper Entity Design
    - MySQL Installation and creating the tables
    - Inserting dummy data and creating SP's (with the worst naming)
- Saturday, 3th Dec, 11:00 - ~19:00
    - Boilerplate (Empty Controllers, Enums, Entities)
    - DBService (Old one. Sorry about that one, didn't remove it so I'll be ashamed for an eternity.)
    - User Happy Path
    - Move Happy Path
    - MoveSet Happy Path
    - Realization of forgotten pivot tables and rage quit.
- Sunday, 4th Dec, 10:00 - ~22:00
    - DBService rewrite
    - Pivot Table creation
    - Refactoring
    - IdentityService
    - 30 minutes of YouTube & StackOverflow & Medium surf on Singleton, Scoped, Transient dilemma.
    - ExceptionHandler middleware
    - 3 hours of Googling on how to get MySQL logging (still don't know. No MacOS info on it)
    - Back to caveman debugging
    - Basic CRUD complete. (with the exception of things I mentioned above)
- Monday, 12:00 (Lunch Break)
    - Bugfix & Refactoring
    - My SQL Oopsie on the job and here we are.

:qw (Just kidding, I wrote this file on VSCode)