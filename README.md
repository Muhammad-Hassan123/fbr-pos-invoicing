# FBR POS and Invoicing

A point of sale system for Pakistani retailers. It manages products, customers and tax rates, creates invoices, and connects to FBR's digital invoicing service.

## What it does

- Sign-in with JWT authentication
- Products, categories, customers and tax rates
- Invoice creation, with FBR submission through `FbrApiService`
- Dashboard and reports

## Stack

- **Backend:** ASP.NET Core Web API, Entity Framework Core, SQL Server
- **Frontend:** React 18, Vite, Tailwind CSS, React Router, Axios

## Run it locally

Backend (set your connection string and `Jwt:Secret` in `appsettings.json` first):

```
cd backend/fbr_full.Server
dotnet ef database update
dotnet run
```

Frontend:

```
cd frontend
cp .env.example .env
npm install
npm run dev
```
