# 🛒 WebShop

## 📌 Description
WebShop is a web-based e-commerce application that allows users to browse products, manage a shopping cart, and place orders. The system also includes an admin panel for managing products and categories.

---

## 🚀 Features
- Browse products by categories  
- View product details  
- Add/remove items to/from cart  
- Checkout and create orders  
- View order history  
- User authentication and authorization (Admin/User roles)  
- Admin management of products and categories  

---

## 🛠️ Technologies Used
- ASP.NET Core MVC  
- C#  
- Entity Framework Core  
- PostgreSQL  
- Razor Views  
- ASP.NET Identity  

---

## ⚙️ Installation

1. Clone the repository:
git clone https://github.com/your-username/webshop.git
cd webshop

2. Open the project in Visual Studio or VS Code  

3. Configure PostgreSQL connection in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=webshop;Username=postgres;Password=yourpassword"
}

4. Apply migrations:
dotnet ef database update

5. Run the application:
dotnet run

6. Open in browser:
https://localhost:5001
