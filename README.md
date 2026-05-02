# Just Food – Restaurant Management System API

![Project Status](https://img.shields.io/badge/Status-Active-brightgreen)
![.NET Version](https://img.shields.io/badge/.NET-Core-blue)
![Architecture](https://img.shields.io/badge/Architecture-Clean_Architecture-purple)

## 📖 Overview
**Just Food** is a comprehensive, scalable, and secure backend RESTful API built using **ASP.NET Core**. It is designed using Clean Architecture principles to manage the complete lifecycle of a restaurant's operations, from ordering and kitchen management to inventory tracking, delivery, and real-time notifications.

This project was developed as an ITI Graduation Project and implements advanced integrations including AI-based nutrition analysis (via Groq/LLaMA), real-time messaging (SignalR), caching (Redis), cloud storage (Cloudinary), and automated payment gateways (Paymob).

---

## ✨ Key Features
- **Authentication & Authorization:** Secure JWT-based authentication with role-based access control (RBAC), supporting both native Identity and external OAuth (Google and Facebook).
- **Ordering System:** Supports multiple order types including Dine-In, Pickup, and Delivery, with centralized shopping baskets managed in Redis.
- **Kitchen Display System (KDS):** Real-time ticket management for chefs using SignalR WebSockets for zero-latency updates.
- **Inventory & Stock Management:** Comprehensive tracking of multi-branch stock, recipes, and raw ingredients with automatic reconciliation.
- **AI Integrations:** Intelligent meal recommendations and automated nutrition tracking (Calories, Protein, Carbohydrates) powered by Groq API and LLaMA models.
- **Delivery Management:** Real-time driver assignment and live status tracking for customers.
- **Payment Gateway:** Seamless integration with Paymob for secure credit card and digital wallet transactions.
- **Multi-language Support:** Full localization for Arabic and English (LTR/RTL support).

---

## 🏗️ Architecture
The system is built on **Clean Architecture**, ensuring a strict separation of concerns and high testability.

- **`RMS.Domain`**: Core business entities and contracts.
- **`RMS.Persistence`**: EF Core context, repositories, and specifications.
- **`RMS.Services`**: Business logic, mapping, and external service orchestrations.
- **`RMS.Presentation`**: RESTful Controllers and SignalR Hubs.
- **`RMS.Web`**: Composition root, DI registration, and middleware configuration.

---

## 💻 Tech Stack
* **Framework:** ASP.NET Core 8
* **Database:** Microsoft SQL Server
* **Caching:** Redis
* **Real-time:** SignalR
* **AI:** Groq API (LLaMA Models)
* **Frontend:** Angular 20 (Responsive, Role-based)
* **Auth:** ASP.NET Core Identity, JWT, Google/Facebook OAuth
* **Third-Party:** Cloudinary (Images), Paymob (Payments)

---

## 🧩 Core Modules

### 1. Identity & Role Management
- Granular RBAC for Admins, Chefs, Waiters, Cashiers, Drivers, and Customers.
- Secure token handling with HTTP-only cookie support.

### 2. Menu & AI Nutrition
- Dynamic menu management with AI-powered nutritional analysis and DRV comparisons.

### 3. Order Lifecycle
- Automated workflow from creation to kitchen, preparation, and final delivery/payment.

### 4. Real-Time Operations
- Instant communication between the kitchen, staff, and customers via SignalR.

### 5. Multi-Branch Inventory
- Isolated branch stock management and cross-branch administrative reporting.

---

## 🚀 Getting Started

### Prerequisites
- .NET SDK 8.0+
- SQL Server & Redis
- API Keys: Groq, Cloudinary, Paymob, Google/Facebook

### Configuration
Update `appsettings.json` in the `RMS.Web` project with your connection strings and API credentials.

### Running the Application
1. Apply migrations: `dotnet ef database update --project RMS.Persistence --startup-project RMS.Web`
2. Run the `RMS.Web` project.
3. Access Swagger at `/swagger`.

---

## 👥 Contributors

* [Mahmoud Ali](https://github.com/mahmoudali2429)
* [Amr Alaraby](https://github.com/AmrAlaraby)
* [Mustafa Saad](https://github.com/mustafas3aad)
* [Hossam Abouelenien](https://github.com/HossamAbouelenien)
* [Arwa Hassan](https://github.com/arwainme)
* [Areej Kammoush](https://github.com/areejkammoush10)


---
*Just Food – Empowering Modern Restaurant Excellence.*
