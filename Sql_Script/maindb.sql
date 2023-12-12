use master
go
                   
IF EXISTS(SELECT * FROM sys.databases WHERE name = 'newestDataBase')
BEGIN
    DROP DATABASE newestDataBase;
END

create database newestDataBase
go

USE newestDataBase
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 12/2/2023 3:46:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coupon]    Script Date: 12/2/2023 3:46:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupon](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ThresHold] [float] NOT NULL,
	[perCoupon] [float] NOT NULL
 CONSTRAINT [PK_Coupon] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 12/2/2023 3:46:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max),
	[Tel] [nvarchar](max),
	[MoneyForPromotion] [float] NOT NULL,
	[CouponCount] [int] DEFAULT 0 NOT NULL
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 12/2/2023 3:46:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[OrderDate] [datetime2](7) NOT NULL,
	[TotalPrice] [float] NOT NULL,
	[TotalDiscount] [float] DEFAULT 0 NOT NULL
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 12/2/2023 3:46:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 12/2/2023 3:46:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[TypeId] [int] NOT NULL,
	[Price] [float] NOT NULL,
	[Quantity] [int] NOT NULL
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductType]    Script Date: 12/2/2023 3:46:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL
 CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231125025546_Store', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231202084359_testdb', N'7.0.14')
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer_CustomerId]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductType_TypeId] FOREIGN KEY([TypeId])
REFERENCES [dbo].[ProductType] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductType_TypeId]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Order_OrderId]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Product_ProductId]
GO

INSERT INTO Customer(Name, Address, Tel, MoneyForPromotion, CouponCount) VALUES (N'Nguyễn Văn A', N'TP HCM', '0123456789', 100000, 5)
INSERT INTO Customer(Name, Address, Tel, MoneyForPromotion, CouponCount) VALUES (N'Nguyễn Văn B', N'TP HCM', '0123456788', 200000, 5)
INSERT INTO Customer(Name, Address, Tel, MoneyForPromotion, CouponCount) VALUES (N'Nguyễn Văn C', N'TP HCM', '0123456787', 300000, 5)
INSERT INTO Customer(Name, Address, Tel, MoneyForPromotion, CouponCount) VALUES (N'Nguyễn Văn D', N'TP HCM', '0123456786', 400000, 5)
INSERT INTO Customer(Name, Address, Tel, MoneyForPromotion, CouponCount) VALUES (N'Nguyễn Văn E', N'TP HCM', '0123456785', 500000, 5)

INSERT INTO Coupon(ThresHold, perCoupon) VALUES (100000, 20000)

INSERT INTO ProductType(Name) VALUES (N'Đồ ăn vặt')
INSERT INTO ProductType(Name) VALUES (N'Nước')
INSERT INTO ProductType(Name) VALUES (N'Mì gói')
INSERT INTO ProductType(Name) VALUES (N'Kem')
INSERT INTO ProductType(Name) VALUES (N'Sữa')

INSERT INTO Product(Name, TypeId, Price, Quantity) VALUES (N'Khoai tây lắc rong biển', 1, 20000, 50)
INSERT INTO Product(Name, TypeId, Price, Quantity) VALUES (N'Đùi gà chiên', 1, 32000, 50)
INSERT INTO Product(Name, TypeId, Price, Quantity) VALUES (N'Trà olong vị đào', 2, 19000, 50)
INSERT INTO Product(Name, TypeId, Price, Quantity) VALUES (N'Trà tắc', 2, 17000, 50)
INSERT INTO Product(Name, TypeId, Price, Quantity) VALUES (N'Mì Hảo Hảo', 3, 5000, 50)

INSERT INTO [dbo].[Order](CustomerId, OrderDate, TotalPrice) VALUES (1, '2023/12/02', 0)
INSERT INTO [dbo].[Order](CustomerId, OrderDate, TotalPrice) VALUES (2, '2023/12/02', 0)
INSERT INTO [dbo].[Order](CustomerId, OrderDate, TotalPrice) VALUES (3, '2023/12/02', 0)
INSERT INTO [dbo].[Order](CustomerId, OrderDate, TotalPrice) VALUES (4, '2023/12/02', 0)
INSERT INTO [dbo].[Order](CustomerId, OrderDate, TotalPrice) VALUES (5, '2023/12/02', 0)

INSERT INTO OrderDetail(OrderId, ProductId, Quantity) VALUES (1, 1, 2)
INSERT INTO OrderDetail(OrderId, ProductId, Quantity) VALUES (1, 2, 1)
INSERT INTO OrderDetail(OrderId, ProductId, Quantity) VALUES (2, 3, 1)
INSERT INTO OrderDetail(OrderId, ProductId, Quantity) VALUES (3, 5, 5)
INSERT INTO OrderDetail(OrderId, ProductId, Quantity) VALUES (4, 4, 2)