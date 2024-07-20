# Integrated Energy Management System

This repository contains the frontend and backend code for the Integrated Energy Management System. The project involves creating an integrated system by connecting microservices and ensuring robust security features.

The Integrated Energy Management System provides functionalities for managing users, devices, and real-time communication. It is designed to be scalable, secure, and deployable using Docker containers.

## Features

- **User Registration and Login:** Secure user authentication and registration process.
- **User and Device Management:** Manage user and device operations through CRUD functionalities.
- **Real-time Notifications:** Utilizes WebSockets for real-time notifications.
- **Secure Communication:** Ensures secure communication using JWT (JSON Web Tokens).
- **Dockerized Microservices:** Deployable using Docker containers for easy scalability and management.

## Architecture

The system is composed of multiple microservices, each responsible for specific functionalities:

- **User Management Microservice:** Handles user operations (CRUD) and interacts with the user database.
- **Device Management Microservice:** Manages device operations (CRUD) and interacts with the device database.
- **Monitoring and Communication Microservice:** Provides real-time notifications using WebSocket and interacts with RabbitMQ for asynchronous communication.

## Technologies Used

- **Backend:** .NET
- **Frontend:** Angular
- **Database:** MS SQL Server for user and device data
- **Real-time Communication:** WebSockets, RabbitMQ
- **Security:** JWT for secure communication
- **Containerization:** Docker
