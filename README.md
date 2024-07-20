Integrated Energy Management System

This repository contains the frontend and backend code for the Integrated Energy Management System. The project involves creating an integrated system by connecting microservices and ensuring security features are in place.

The Integrated Energy Management System provides functionalities for managing users, devices, and real-time communication. It is designed to be scalable, secure, and deployable using Docker containers.

Features

User Registration and Login
User and Device Management
Real-time Notifications using WebSockets
Secure Communication with JWT
Dockerized Microservices

Architecture

The system is composed of multiple microservices, each responsible for specific functionalities:

User Management Microservice: Handles user operations (CRUD) and interacts with the user database.
Device Management Microservice: Manages device operations (CRUD) and interacts with the device database.
Monitoring and Communication Microservice: Provides real-time notifications using WebSocket and interacts with RabbitMQ for asynchronous communication.
