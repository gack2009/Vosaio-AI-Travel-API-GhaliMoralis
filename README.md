# Hotel Booking API - Clean Architecture

## 📌 Overview
This is a **Hotel Booking API** built using **ASP.NET Core** with **Entity Framework Core (EF Core)** following **Clean Architecture** and **RESTful** principles. It allows users to:
- Find available hotels and rooms
- Book a hotel room
- Retrieve booking details

## 🏗️ Solution Structure
```
/YourSolution
 ├── /Presentation (Startup project, contains `Program.cs`, controllers)
   ├── /BookingAPI
 ├── /Application (Contains interfaces, CQRS handlers, business logic)
 ├── /Infrastructure (Contains `DbContext`, database-related logic)
 ├── /Domaint (Contains entity models)
```
---
## 🚀 Getting Started

### **1️⃣ Prerequisites**
Make sure you have:
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Entity Framework Core CLI](https://learn.microsoft.com/en-us/ef/core/)

### **2️⃣ Setup Database Connection**
Modify `appsettings.json` in `YourAPIProject`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HotelBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
For testing purposes you can use in memory DB feature so you dont have to run migrations and won't need a Database:
```json
{
  "UseInMemoryDatabase": true
}
```

### **3️⃣ Install Dependencies**
Run the following commands:
```sh
dotnet restore
```

### **4️⃣ Run Migrations or use in memory DB setting**
Run the following EF Core commands from the **solution root directory**:
```sh
dotnet ef database update --project Infrastructure --startup-project WebApplication1
```

### **5️⃣ Run the API**
To start the API, run:
```sh
dotnet run --project WebApplication1
```
The API will be available at `https://localhost:5001`.

---
## 🛠️ API Endpoints

### **🏨 Hotel Endpoints**
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/hotels` | Get all hotels |
| `GET` | `/api/hotels/{name}` | Get hotel details by name |

### **🛏️ Room Endpoints**
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/rooms/available?CheckInDate={date}&CheckOutDate={date}&Guests={num}` | Get available rooms |

### **📅 Booking Endpoints**
| Method | Endpoint | Description |
|--------|---------|-------------|
| `POST` | `/api/bookings` | Create a new booking |
| `GET` | `/api/bookings/{id}` | Get booking details by ID |
| `DELETE` | `/api/bookings/{id}` | Cancel a booking |

### **📋 Create Booking Form Inputs `POST` | `/api/bookings` |**

| Field               | Type       | Description                               |
| ------------------- | ---------- | ----------------------------------------- |
| `RoomId`            | `Guid`     | The ID of the room being booked.          |
| `MainGuestFullName` | `string`   | The full name of the main guest.          |
| `CheckInDate`       | `DateTime` | The check-in date for the booking.        |
| `CheckOutDate`      | `DateTime` | The check-out date for the booking.       |
| `Guests`            | `int`      | The number of guests staying in the room. |
---
## ✅ Unit Testing

Unit tests are located in `YourTestsProject` and can be run using:
```sh
dotnet test
```

Mocking is done using **Moq**, and EF Core is tested using an **in-memory database**.

---
## 🛠️ Common Issues & Fixes
### **1️⃣ `dotnet ef` Command Not Found**
Run:
```sh
dotnet tool install --global dotnet-ef
```
### **2️⃣ Cannot Connect to SQL Server**
- Ensure SQL Server is **running**.
- Check that the **connection string is correct**.

### **3️⃣ Reset Migrations**
If needed, reset migrations:
```sh
dotnet ef migrations remove --project Infrastructure --startup-project WebApplication1
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project WebApplication1
dotnet ef database update --project Infrastructure --startup-project WebApplication1
```

---
## 💡 Need Help?
If you have any issues, feel free to open an **issue** or reach out for support! 🚀

---
# Future Improvements
- Improve dateTime properties to account for time zones
- Add more excpetions handling using controller filter
- Improve the delete booking functionality
- Improve the booking number uniquness
- Add CQRS to be able to have request pipeline in the application layer which would allow us to add behaviours more elegantly e.g. fluent validation could be part of the pipeline instead of dependency injection
- Add input sanitization

