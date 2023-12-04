USE [testDataWindow]
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
	[perCoupon] [float] NOT NULL,
	[CustomerId] [int] NULL,
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
	[MoneyForPromotion] [float] NOT NULL,
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
	[Quantity] [int] NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
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
	[Name] [nvarchar](max) NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231125025546_Store', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231202084359_testdb', N'7.0.14')
GO
ALTER TABLE [dbo].[Coupon]  WITH CHECK ADD  CONSTRAINT [FK_Coupon_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Coupon] CHECK CONSTRAINT [FK_Coupon_Customer_CustomerId]
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

CREATE TRIGGER TG_AfterInsertUpdate 
ON OrderDetail 
for INSERT,UPDATE
AS
BEGIN
	-- Update thanh tien
	declare @orderId int = 0;
	declare @productId int = 0;
	declare @quantity int = 0;
	select @orderId = i.OrderId, @productId = i.ProductId, @quantity = i.Quantity from inserted i;
	update [dbo].[Order] set TotalPrice = (select sum( p.Price * od.Quantity)
	from OrderDetail od join Product p on od.ProductId = p.Id where od.OrderId = @orderId
	group by od.OrderId ) where Id = @orderId
	-- update so luong con trong kho
	if exists(select * from deleted d where d.OrderId = @orderId and d.ProductId = @productId) 
		begin
			declare @oldQuantity int = 0;
			select @oldQuantity = d.Quantity from deleted d where d.OrderId = @orderId and d.ProductId = @productId;
			update Product set Quantity = Quantity + @oldQuantity - @quantity where Id = @productId;
		end
	else 
		begin
			update Product set Quantity = Quantity - @quantity where Id = @productId;
		end
END
GO

CREATE TRIGGER TG_AfterDelete
ON OrderDetail 
FOR DELETE
AS
BEGIN
	-- Update thanh tien
	declare @orderId int = 0;
	declare @productId int = 0;
	declare @quantity int = 0;
	select @orderId = d.OrderId, @productId = d.ProductId, @quantity = d.Quantity from deleted d
	update [dbo].[Order] set TotalPrice = (select sum( p.Price * od.Quantity)
	from OrderDetail od join Product p on od.ProductId = p.Id where od.OrderId = @orderId
	group by od.OrderId ) where Id = @orderId
	-- update so luong con trong kho
	update Product set Quantity = Quantity + @quantity where Id = @productId;
END
GO

CREATE TRIGGER CheckValidQuantityBeforeInsert ON OrderDetail INSTEAD OF INSERT
AS
BEGIN
  IF EXISTS (
    SELECT *
    FROM INSERTED i INNER JOIN Product p ON i.ProductId = p.Id
    WHERE i.Quantity > p.Quantity
  )
  BEGIN
    RAISERROR ('Quantity inserted in OrderDetail cannot be greater than Quantity in Product.', 16, 1)
    ROLLBACK TRANSACTION
    RETURN
  END

  INSERT INTO OrderDetail
  SELECT OrderId, ProductId, Quantity
  FROM INSERTED
END
GO

INSERT INTO Customer(Name, MoneyForPromotion) VALUES (N'Nguyễn Văn A', 100000)
INSERT INTO Customer(Name, MoneyForPromotion) VALUES (N'Nguyễn Văn B', 200000)
INSERT INTO Customer(Name, MoneyForPromotion) VALUES (N'Nguyễn Văn C', 300000)
INSERT INTO Customer(Name, MoneyForPromotion) VALUES (N'Nguyễn Văn D', 400000)
INSERT INTO Customer(Name, MoneyForPromotion) VALUES (N'Nguyễn Văn E', 500000)

INSERT INTO Coupon(ThresHold, perCoupon, CustomerId) VALUES (100000, 20000, 1)
INSERT INTO Coupon(ThresHold, perCoupon, CustomerId) VALUES (100000, 20000, 2)
INSERT INTO Coupon(ThresHold, perCoupon, CustomerId) VALUES (100000, 20000, 3)
INSERT INTO Coupon(ThresHold, perCoupon, CustomerId) VALUES (100000, 20000, 4)
INSERT INTO Coupon(ThresHold, perCoupon, CustomerId) VALUES (100000, 20000, 5)

INSERT INTO ProductType(Name, Image) VALUES (N'Đồ ăn vặt', N'link.image.com')
INSERT INTO ProductType(Name, Image) VALUES (N'Nước', N'link.image.com')
INSERT INTO ProductType(Name, Image) VALUES (N'Mì gói', N'link.image.com')
INSERT INTO ProductType(Name, Image) VALUES (N'Kem', N'link.image.com')
INSERT INTO ProductType(Name, Image) VALUES (N'Sữa', N'link.image.com')

INSERT INTO Product(Name, TypeId, Price, Quantity, Image) VALUES (N'Khoai tây lắc rong biển', 1, 20000, 50, N'link.image.com')
INSERT INTO Product(Name, TypeId, Price, Quantity, Image) VALUES (N'Đùi gà chiên', 1, 32000, 50, N'link.image.com')
INSERT INTO Product(Name, TypeId, Price, Quantity, Image) VALUES (N'Trà olong vị đào', 2, 19000, 50, N'link.image.com')
INSERT INTO Product(Name, TypeId, Price, Quantity, Image) VALUES (N'Trà tắc', 2, 17000, 50, N'link.image.com')
INSERT INTO Product(Name, TypeId, Price, Quantity, Image) VALUES (N'Mì Hảo Hảo', 3, 5000, 50, N'link.image.com')

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