# BudgetBlitz 

BudgetBlitz Lite is a budget management application developed using ASP.NET Core Web API. This application allows users to effectively track and manage their income, expenses, and savings.

## Features

- **Real-Time Notifications**: Integrated with Firebase to send real-time notifications to users, enhancing engagement and timely updates.
- **Distributed Caching**: Utilized Redis for distributed caching to improve application performance and scalability.
- **Secure Authentication**: Implemented JWT authentication using ASP.NET Identity framework for secure user login and data protection.
- **Comprehensive Budget Tracking**: Allows users to track and categorize income and expenses.
- **Reporting and Analytics**: Generates detailed reports and analytics for better financial insights.
- **User Management**: Supports user registration, login, and profile management.
- **Role-Based Access Control**: Implements role-based access to different features and functionalities.
- **Data Export**: Provides options to export financial data in various formats (CSV, Excel).

## Technologies Used

- ASP.NET Core
- SQL Server
- Firebase
- Redis
- ASP.NET Identity
- Entity Framework

## Getting Started

### Prerequisites

- .NET Core SDK
- SQL Server
- Redis
- Firebase account

### Installation

1. **Clone the repository:**
   ```sh
   git clone https://github.com/EyasWannous/BudgetBlitz.git
   cd BudgetBlitzLite
   ```

2. **Setup the Database:**
   - Configure SQL Server with the required database settings.
   - Apply migrations to set up the database schema:
     ```sh
     dotnet ef database update
     ```

3. **Configure Redis:**
   - Ensure Redis is installed and running on your machine or server.

4. **Setup Firebase:**
   - Create a Firebase project and obtain the configuration file.
   - Update the application settings with your Firebase configuration.

5. **Update appsettings.json:**
   - Add your SQL Server, Redis, and Firebase configuration settings in the `appsettings.json` file.

6. **Run the Application:**
   ```sh
   dotnet run
   ```

## Usage

- Access the API endpoints to manage budget entries, track income and expenses, and receive real-time notifications.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a pull request

## Contact

For any questions or inquiries, please contact [eyaswannous@gmail.com].
