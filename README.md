# 🎬 Movie Director Management System

This is a full-stack project built with **ASP.NET Core Web API** and **ASP.NET Core MVC** to manage Movies and Directors. It supports assigning multiple directors to movies and allows unassigning them too.

---

## Tech Stack
- ASP.NET Core Web API
- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server
- Bootstrap
- Newtonsoft.Json

---

## Features
- Add/Edit/Delete Movies
- Add/Edit/Delete Directors
- Assign & Unassign Directors to Movies
- View all Movies with assigned Directors
- Web API consumed in MVC using HttpClient

---

## API Routes (Sample)
- `GET /api/movie` → Get all movies
- `POST /api/director` → Add a director
- `POST /api/moviedirector/assign` → Assign directors
- `GET /api/moviedirector` → Get movie-director mappings
- `DELETE /api/moviedirector/unassign` → Unassign director

---

## How to Run
1. Clone this repo
2. Update connection string in `appsettings.json`
3. Run migrations or use pre-created DB
4. Start both API and MVC projects in Visual Studio
5. Test in browser 

---


