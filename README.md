# Cinema Management System (ASP.NET Core MVC)

A cinema web application built with **ASP.NET Core MVC**, **Entity Framework Core**, **ASP.NET Identity** and **SQL Server**.
Admins manage cinemas, films and actors; spectators browse films, view cast details and book tickets with **Stripe** checkout.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Stripe](https://img.shields.io/badge/Payments-Stripe-635BFF?logo=stripe&logoColor=white)

## Features

**Identity**
- Register, login and logout with **ASP.NET Identity**
- Email confirmation, resend email, forget / reset password (OTP-based)
- Role-based access (Admin / Spectator)

**Admin area**
- CRUD for **Cinemas** (with cinema details), **Films** and **Actors** (with image upload / replace / delete)
- Many-to-many **Film ↔ Actor** relationship
- User management: create users, change role, lock / unlock accounts

**Spectator area**
- Browse films, film details and **actor details**
- **Cart** for tickets (add, increase / decrease, remove)
- **Favorites** list
- **Checkout** with **Stripe** (pay, success, cancel)

**Extras**
- `MoviePoster` console tool that fetches film posters from an external movie API
- SQL seed scripts for cinemas, films, actors and images

## Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 MVC (Areas: Admin, Spectator, Identity) |
| ORM | Entity Framework Core (Code First) |
| Database | SQL Server |
| Auth | ASP.NET Identity |
| Payments | Stripe.net |

## Project structure

```
CinemaProjectInEraaSoft/
├── Cinema/
│   ├── Areas/
│   │   ├── Admin/Controllers       # Home, Cinema, Film, Actor, User
│   │   ├── Spectator/Controllers   # Home, Cart, Favorite, CheckOut
│   │   └── Identity/Controllers    # Account
│   ├── Data/                       # ApplicationDbContext
│   ├── Models/                     # Film, Actor, FilmActor, Cinemaa, Cart, Order… + ViewModels
│   ├── Repositories/               # Generic Repository + specific repositories
│   └── Utility/                    # Email sender, Stripe settings
├── MoviePoster/                    # Console tool to download film posters
└── *.sql                           # Seed data scripts
```

## Getting started

1. Clone the repo and open `CinemaProjectInEraaSoft.sln` in Visual Studio 2022.
2. Put your connection string and Stripe test key in **User Secrets** (never commit real keys):
   ```json
   {
     "ConnectionStrings": { "DefualtConnections": "Server=.;Database=CinemaData;Trusted_Connection=True;TrustServerCertificate=True" },
     "Stripe": { "SecretKey": "<your-stripe-test-key>" }
   }
   ```
3. Set the Gmail **App Password** used for sending emails as an environment variable named `EMAIL_APP_PASSWORD`.
4. Create the database (`dotnet ef database update`) and optionally run the `.sql` seed scripts.
5. Run the `Cinema` project.

> Built as part of the .NET Backend training program at **EraaSoft**.

## Author

**Samir Mohamed Hamed** — Backend .NET Developer
[LinkedIn](https://www.linkedin.com/in/samir-mohamed-dotnet) · [GitHub](https://github.com/Samirmohamedhamed)
