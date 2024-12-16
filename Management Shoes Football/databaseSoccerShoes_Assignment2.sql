	CREATE DATABASE soccer_shoes_store_management;

	USE soccer_shoes_store_management;

	-- Bảng Customer
	CREATE TABLE Customer (
		CustomerID INT IDENTITY(1,1) PRIMARY KEY,
		CustomerName VARCHAR(50),
		Email VARCHAR(100) UNIQUE,
		PhoneNumber VARCHAR(20),
		Address VARCHAR(255)
	);

	-- Bảng Employee
	CREATE TABLE Employee (
		EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
		EmployeeName VARCHAR(50),
		Email VARCHAR(100) UNIQUE,
		PhoneNumber VARCHAR(20),
		Position VARCHAR(50)
	);

	-- Bảng TypeFootballBoots (Loại giày)
	CREATE TABLE TypeFootballBoots (
		TypeID INT IDENTITY(1,1) PRIMARY KEY,
		TypeName VARCHAR(50)
	);

	-- Bảng FootballBoots (Giày bóng đá)
	CREATE TABLE FootballBoots (
		BootID INT IDENTITY(1,1) PRIMARY KEY,
		BootName VARCHAR(100),
		TypeID INT,
		Size INT,
		Price DECIMAL(10, 2),
		StockQuantity INT,
		SupplierID INT,
		FOREIGN KEY (TypeID) REFERENCES TypeFootballBoots(TypeID),
		FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)
	);

Select * From FootballBoots;
	-- Bảng Supplier (Nhà cung cấp)
	CREATE TABLE Supplier (
		SupplierID INT IDENTITY(1,1) PRIMARY KEY,
		SupplierName VARCHAR(100),
		ContactName VARCHAR(50),
		ContactPhone VARCHAR(20),
		ContactEmail VARCHAR(100)
	);

	SELECT * FROM Supplier

	-- Bảng Order
	CREATE TABLE Orders (
		OrderID INT IDENTITY(1,1) PRIMARY KEY,
		CustomerID INT,
		EmployeeID INT,
		OrderDate DATETIME,
		TotalAmount DECIMAL(10, 2),
		Status VARCHAR(20),
		FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
		FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
	);

	SELECT * FROM Orders
	-- Bảng OrderDetail (Chi tiết đơn hàng)
	CREATE TABLE OrderDetail (
		OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
		OrderID INT,
		BootID INT,
		Quantity INT,
		Price DECIMAL(10, 2),
		FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
		FOREIGN KEY (BootID) REFERENCES FootballBoots(BootID)
	);



	USE soccer_shoes_store_management;

-- Thêm dữ liệu vào bảng Customer
INSERT INTO Customer (CustomerName, Email, PhoneNumber, Address)
VALUES
('Nguyen Minh Tuan', 'nguyenminhtuan@example.com', '0987654321', '123 Duong 1, Can Tho, Viet Nam'),
('Tran Thi Lan', 'tranthilan@example.com', '0123456789', '456 Duong 2, Can Tho, Viet Nam'),
('Le Hoang Nam', 'lehoangnam@example.com', '0981223344', '789 Duong 3, Can Tho, Viet Nam');

SELECT * FROM Customer;

-- Thêm dữ liệu vào bảng Employee
INSERT INTO Employee (EmployeeName, Email, PhoneNumber, Position)
VALUES
('Nguyen Thi Mai', 'nguyenthimai@store.com', '0912345678', 'Quan Ly Ban Hang'),
('Pham Quang Hieu', 'phamquanghieu@store.com', '0908765432', 'Nhan Vien Ban Hang'),
('Hoang Thanh Son', 'hoangthanhson@store.com', '0911223344', 'Quan Ly Kho');

SELECT * FROM Employee;

-- Thêm dữ liệu vào bảng TypeFootballBoots (Loại giày)
INSERT INTO TypeFootballBoots (TypeName)
VALUES
('Giày sân cỏ'),
('Giày trong nhà'),
('Giày sân cỏ nhân tạo');

SELECT * FROM TypeFootballBoots;

-- Thêm dữ liệu vào bảng Supplier (Nhà cung cấp)
INSERT INTO Supplier (SupplierName, ContactName, ContactPhone, ContactEmail)
VALUES
('SportsGear Inc.', 'Tom Green', '5555555555', 'tom.green@sportsgear.com'),
('Footwear Co.', 'Sarah White', '5556666666', 'sarah.white@footwear.com'),
('KickSports Ltd.', 'James Black', '5557777777', 'james.black@kicksports.com');

SELECT * FROM Supplier;

-- Thêm dữ liệu vào bảng FootballBoots (Giày bóng đá)
INSERT INTO FootballBoots (BootID, BootName, TypeID, Size, Price, StockQuantity, SupplierID)
VALUES
(1, 'Nike Phantom GX', 1, 42, 120.50, 30, 1),
(2, 'Adidas Predator Edge', 1, 44, 140.00, 25, 2),
(3, 'Puma Future Z', 2, 40, 115.75, 50, 3),
(4, 'Nike Vapor Edge', 3, 43, 130.00, 40, 1),
(5, 'New Balance Furon V7', 1, 45, 125.00, 20, 2);

SELECT * FROM FootballBoots;

-- Thêm dữ liệu vào bảng Orders (Đơn hàng)
INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, TotalAmount, Status)
VALUES
(1, 1, '2024-12-01 10:30:00', 240.50, 'Hoan Tat'),
(2, 2, '2024-12-02 15:45:00', 315.75, 'Dang Xy Ly'),
(3, 3, '2024-12-03 09:00:00', 230.00, 'Hoan Tat');

SELECT * FROM Orders;

-- Thêm dữ liệu vào bảng OrderDetail (Chi tiết đơn hàng)
INSERT INTO OrderDetail (OrderID, BootID, Quantity, Price)
VALUES
(1, 1, 1, 120.50),
(1, 3, 1, 120.00),
(2, 2, 2, 140.00),
(2, 4, 1, 130.00),
(3, 5, 2, 125.00); 

SELECT * FROM OrderDetail;

CREATE TABLE LOGINFORM (
    USERNAME NVARCHAR(50) PRIMARY KEY, -- Unique identifier for each user
    PASSWORDS NVARCHAR(255) NOT NULL   -- Store hashed passwords
);

INSERT INTO LOGINFORM (USERNAME, PASSWORDS)
VALUES
('admin', '123'),
('test', '321'),
('demo', '111');
SELECT * FROM LOGINFORM

  DELETE FROM Orders
WHERE OrderID = 3;