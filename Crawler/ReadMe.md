# Crawler Bot - Web Scraping and E-commerce Data Management

## Description

Crawler Bot is a C# .NET project designed to scrape product information from an e-commerce website and store it in a MySQL database. It utilizes a C# background worker to crawl the target website, extracting details such as product names, prices, discount availability, image URLs, etc. The project follows the Clean Architecture and CQRS design patterns.

### Features

- Web scraping of product details: The Crawler Bot navigates to the e-commerce website and gathers product information, including regular and discounted prices, image URLs, and product names.
- Clean Architecture and CQRS: The project is structured according to the Clean Architecture principles, ensuring separation of concerns and maintainability. The CQRS pattern is implemented to segregate read and write operations effectively.
- User Management: Microsoft Identity is employed for user authentication, allowing users to register and log in through traditional methods or using Google login. JWT tokens are used for secure login/logout procedures.
- Email Notifications: The application sends email notifications to users upon registration and when specific product details are scraped.
- Global Exception Handling: A GlobalException filter is implemented to handle and manage exceptions gracefully throughout the application.
- Front-end with React and TypeScript: The front-end of the application is built using React and TypeScript, providing a responsive and user-friendly interface.
- Tailwind CSS for Styling: Tailwind CSS is used for designing the UI, ensuring a clean and modern appearance.

## Back-end Technologies

- C# .NET
- Clean Architecture
- CQRS (Command Query Responsibility Segregation)
- Microsoft Identity for User Management
- JWT (JSON Web Tokens) for Authentication
- MySQL for Database Management
- Background Worker for Web Scraping
- SignalR for Real-Time Communication

## Front-end Technologies

- React with TypeScript
- Tailwind CSS for Styling

## How the Crawler Bot Works

1. User Authentication: Users can register and log in using traditional methods or through Google login. JWT tokens are issued for secure authentication.
2. Dashboard: Upon successful login, users are directed to the dashboard page, where they can view their previous orders and manage them, including soft and hard deletion options.
3. Creating Orders: Users can create new orders through a modal, specifying details such as the number of products to scrape and the type of products (all, discounted, non-discounted).
4. SignalR Communication: When an order is created, the details are sent to the back-end worker using SignalR hub for web scraping.
5. Web Scraping: The background worker, with the help of a web driver, navigates to the e-commerce website and performs web scraping based on the user's order details.
6. Data Storage: The scraped product information is stored in the "Product" table, and order-related information is stored in the "Order" table. Bot status and order completion details are saved in the "Order Event" table.
7. Live Tracking: Users can track the bot's progress and the scraped products in real-time using the "Live Track" page. SignalR facilitates the transfer of logs from the back-end to the front-end for live updates.
8. Protected Routes: The application uses protected routes to ensure user session security. If a token expires, the user is automatically logged out for enhanced security.

## Email and Toaster Notifications Management

After user registration, users can manage their email or toaster notification preferences. The application provides options to enable or disable various types of notifications, ensuring a personalized experience for each user.

## Export to Excel

Users have the option to export their order details to an Excel table directly from the dashboard. This feature enables users to conveniently analyze, store, and share their scraped data with ease.

## Installation and Setup

- Clone the repository from GitHub.
- Set up the necessary environment for C# .NET and React with TypeScript.
- Install the required C# and JavaScript dependencies.
- Configure the MySQL database connection settings.
- Build and run the C# .NET back-end.
- Start the React front-end to access the Crawler Bot application.

## Sneak Peak to the project

- Landing Page
<br/>

![HomePage](https://github.com/1neslihan/UpSchool-FullStack-Development-Bootcamp/assets/30401423/9ff818c9-e451-4f81-943a-be215f84f698)
<hr/>

- Sign Up Page
<br/>

![RegisterPage](https://github.com/1neslihan/UpSchool-FullStack-Development-Bootcamp/assets/30401423/3f2647f9-01f7-431b-b8cb-981c56d83f5f)
<hr/>

- Login Page
<br/>

![LoginPage](https://github.com/1neslihan/UpSchool-FullStack-Development-Bootcamp/assets/30401423/24e8c4a9-fd64-4463-96e0-726ceabf5116)
<hr/>

- Records (Dashboard) Page
<br/>

![RecordsPage](https://github.com/1neslihan/UpSchool-FullStack-Development-Bootcamp/assets/30401423/b262f14b-7c5c-4127-b171-9aaca2b9b596)
<hr/>

- Modal
<br/>

![Modal](https://github.com/1neslihan/UpSchool-FullStack-Development-Bootcamp/assets/30401423/230ec365-1c16-49ac-bf2e-e41e7911850f)
<hr/>


- Live Track Page 
<br/>

![LiveTrackPage](https://github.com/1neslihan/UpSchool-FullStack-Development-Bootcamp/assets/30401423/57ca9094-2537-4d0a-b769-8169418ce7e0)
<hr/>

- Users Page
<br/>

![UsersPage](https://github.com/1neslihan/UpSchool-FullStack-Development-Bootcamp/assets/30401423/0dda2707-864a-496b-b670-70aa05c42786)
<hr/>

- Settings Page
<br/>

![SettingsPage](https://github.com/1neslihan/UpSchool-FullStack-Development-Bootcamp/assets/30401423/59c7655c-e28d-4591-9b3d-03b5f4f12c1a)
<hr/>








