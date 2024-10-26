# Delivery Notification API

## Project Overview

We are developing an application to inform customers about when they should expect the delivery of their most recent order. This project consists of a RESTful API that communicates with a SQL Server database to retrieve order information based on customer details.

## Technologies Used

- **.NET 8**: The application is built on the .NET framework, utilizing its robust capabilities for web development.
- **ADO.NET**: Data access is handled using ADO.NET, allowing for efficient interaction with the SQL Server database.
- **SQL Server**: The database used to store customer and order information.

## Features

- **Retrieve Most Recent Order**: The API accepts customer email and ID to fetch the details of the most recent order.
- **Handle Edge Cases**: The API handles scenarios such as:
  - Invalid customer information (mismatched email and customer ID).
  - Customers without any orders.
  - Products marked as gifts are displayed as "Gift" to maintain privacy.
  
## API Endpoints

- **POST /api/orders**: Retrieve the most recent order details for a customer.

### Request Format

The API accepts a JSON request body in the following format:

```json
{
    "user": "bob@aol.com",
    "customerId": "1"
}

