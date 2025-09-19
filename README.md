# Team Task Manager by Vedmitskyi Stanislav

The goal of this project is to create **spaces (groups)** where tasks can be created and completed by different users.  
Each user can create a new group, where they become the **Admin**. The group admin can invite new users and assign them to specific tasks.

---

## 🏗️ Project Structure

### 🔧 Backend
- **ASP.NET Core Web API** – main backend technology  
- **Swagger** – for API documentation  
- **SQLite** – database engine  
- **Entity Framework Core** – ORM  
- **NUnit** – testing framework  

### 🎨 Frontend
- **Blazor WebAssembly (WASM)** – client-side UI

- **SignalR** - for notification system

---

## 🏛️ Architecture

The project follows **Onion Architecture** principles:

### 🧩 Domain Layer
- Domain objects  
- Interfaces for additional services  

### ⚙️ Application Layer
- Application logic  
- Data Transfer Objects (DTOs) and mappers (DTO ↔ Domain)  
- Services used by controllers to handle requests  

### 🗄️ Presentation & Infrastructure
- Database models and mappers (Domain ↔ Model)  
- Repository implementations for ORM communication  
- Controllers  
- Authentication services with custom **Authorization Handler** and **Authorization Filter**  

### 🖥️ UI
- All services are combined into a single project for simplicity

---

## Design Trade-offs

- **Separation of Concerns:**  
  To simplify UI development, the same domain objects and mappers from the backend were reused, which caused issues with strict separation of concerns.  

- **Testing:**  
  Since the project is heavily data-driven, there was not much business logic suitable for unit tests.  
  As a result, all tests are **integration tests** using a temporary SQLite database.

- **Frontend rawnes:**
  Only the most basic functionality was implemented and I hope that simplicity of UI part will not spoil general impression 
