# Hotel Booking API - Clean Architecture

## 📌 Overview
This is a **Itineray Generator API** built using **ASP.NET Core** with **Entity Framework Core (EF Core)** following **Clean Architecture** and **RESTful** principles. It allows users to:
- Generate a travel plan based on destination, date of travel, budget, and interests
- The follow up Answers are in ```FollowUp_Answers.md```

## 🏗️ Solution Structure
```
/AITravel
 ├── /Presentation (Startup project, contains `Program.cs`, controllers)
   ├── /AUTravekAPI
 ├── /Application (Contains interfaces, CQRS handlers, business logic)
 ├── /Infrastructure (Contains `DbContext`, database-related logic)
 ├── /Domaint (Contains entity models)
```

The API is use to generate a travel plan based on destination, date of travel, budget, and interests. It builds a request to send to OpenAI API which then returns a structred json that we can use to deserialise into a dto or map it to an entity to save to the database. This returned dto then is used to show the user a list of activities they can do every day with an estimated cost.
Please note that OpenAI API is a bit slow and it might take up to 7 seconds to get a response back.

---

## 🚀 Getting Started

### **1️⃣ Prerequisites**
Make sure you have:
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Entity Framework Core CLI](https://learn.microsoft.com/en-us/ef/core/)

### **2️⃣ Setup Database Connection**
Modify `appsettings.json` in `YourAPIProject` if you want to set up an actual database:
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
Run the following EF Core commands from the **solution root directory** if you are using actual database:
```sh
dotnet ef database update --project Infrastructure --startup-project WebApplication1
```

### **5️⃣ Run the API**
To start the API, run:
```sh
dotnet run --project WebApplication1
```
The API will be available at `https://localhost:7260`.

### **6️⃣ Test the API**

You can access swaggar at `https://localhost:7260/swagger/index.html`
please make sure you replace the port number with your debug port number

you can also use the following cURL:
```sh
curl -X POST "https://localhost:7260/api/itinerary/generate" \
  -H "Content-Type: application/json" \
  -d '{
        "destination": "Tokyo",
        "travelDates": ["2025-06-01", "2025-06-10"],
        "budget": 2000,
        "interests": ["history", "food", "adventure"]
      }'
```

---
## 🛠️ API Endpoints

### **🏨 Itinerary Endpoints**
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/itinerary/generate` | Gets an itineray based on user travel details |

### **📋 Itinerary Request Inputs `POST` | `/api/itinerary/generate` |**

| Field               | Type       | Description                               |
| ------------------- | ---------- | ----------------------------------------- |
| `Destination`       | `string`     | The destination of travel          |
| `TravelDates`     | `DateTime[]`   | This should only contain 2 dates. the first one is the start date and the second one is end date |
| `Budget`          | `decimal`     | The total budget for the travel        |
| `Interests`       | `string[]?`   | The individual intrests such as food, shopping, running etc      |


## ✅ Unit Testing

Unit tests are located in `YourTestsProject` and can be run using:
```sh
dotnet test
```

Mocking is done using **Moq**, and EF Core is tested using an **in-memory database**.

---
## 🛠️ Common Issues & Fixes
### **1️⃣ `dotnet ef` Command Not Found (this is not applicable if you are using in memory DB)**
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
- Add more endpoint, perhaps getting an existing itinerary.
- Add more excpetions handling using controller filter.
- Add CQRS to be able to have request pipeline in the application layer which would allow us to add behaviours more elegantly e.g. fluent validation could be part of the pipeline instead of dependency injection
- Add input sanitization
- Investigate why openAI api is slow

