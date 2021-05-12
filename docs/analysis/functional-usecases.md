# Functional UseCases

## Manage Products

1. `Shopowner` creates a `Product` in the `Catalog` (Event: NewProductEntered)  in Products (=MT-Courier Example -> transactional)
    1. Create a new `Product` (Event: NewProductCreated) in `Catalog`
        1. Create a `Price` for the `Product` in Pricing
            1. (Event: ProductPriceUpdated) -> Update priceinfo back to `Catalog`.
        1. Create an `StockEntry` for the `Product` in the `Warehouse`
            1. (Event: StockAmountUpdated) -> updates stockinfo in `Catalog`.
1. `Shopowner` modifies the `Price` for a `Product` (Event: ProductPriceRequested) in Pricing
    1. Update Price (Event: ProductPriceUpdated)
        1. Update `Product`'s priceInfo in catalog
1. `Shopowner` modifies a `StockEntry` for a `Product` (Event: StockAmountRequested) in WareHouse
   1. Update stockamount (Event: StockAmountUpdated)
        1. (Event: StockAmountUpdated) -> Update `Product`'s availability in catalog
1. `Customer` browses the `Catalog` viewing products, prices and availability (Query in Products)

## Manage ShoppingCart

1. `Customer` manages the content of a `ShoppingCart`
    1. Add a product and amount to a `ShoppingCart` (Event: ProductAdded)
        1. Create `ShoppingCart` if not exists
        1. Add a product to a `ShoppingCart`
    1. Remove a product from a `ShoppingCart` (Event: ProductRemoved)
    1. Change amount of a product in a `ShoppingCart` (Event: ProductAmountChanged)
1. `Customer` reviews `ShoppingCart` details (Query)
    1. Fetch current pricing (optional) (Query in Pricing)
    1. Recheck availability stock (optional) (Query in WareHouse)

## Manage Order

1. `Customer` submits an `SalesTransaction` based of current `ShoppingCart` (Event: ShoppingCartSubmitted) -> MT-Courier
    1. Start new `SalesTransaction` based on submitted `ShoppingCart`
    1. Update pricing of `SalesTransaction` (Event: PriceUpdateRequested) -> IRequest
    1. Set status of `ShoppingCart` to submitted (Event: SalesTransactionStarted)
        1. Create `Delivery` in stock with status requested (StockProductRequested)
        1. Create `Payment` with status unpayed
    1. Set status of `SalesTransaction` to started
1. `Shopowner` reserves the products for a `Delivery`
    1. Reserve products from `WareHouse` (Event: StockProductReserved) they still remain in the warehouse. They are marked as sold (linked deliveryId) and are thus available not for purchase.
        1. Update availability amount (Event: ProductStockChanged)
            1. Update catalog availability
    1. Set `Delivery` to status reserved
1. `Customer` provides shipping details for the `Delivery` (Event: ShippingDetailChanged)
    1. Store shipping details for `Delivery`
    1. Set Status of `Delivery` to shippable
        1. Set status of `SalesTransaction` to InProgress(=reserved/unpayed)
1. `Customer` pays for an order (Event: PaymentSubmitted)
    1. Receive payment
    1. Set `Payment` status to payed
        1. Set `SalesTransaction` status to purchased (Event `SalesTransactionPurchased`)
            1. Send notification of purchase to `Customer`
1. `Customer` cancels pending `SalesTransaction` (Event: SalesTransactionCancelRequested ) -> MT-Courier
    1. Activity Refund in Pricing
    1. Activity Cancel Delivery in Delivery
    1. Set `SalesTransaction` to status to cancelled
1. `Shopowner` starts `Delivery` (backend Event based on `SalesTransactionPurchased`)
    1. Checkout from stock and ship `Delivery`
    1. Set `Delivery` status to in progress
    1. Set `SalesTransaction` to status shipped
        1. Send notification of shipment
1. `Customer` receives notification of purchase
1. `Shopowner` delivers the delivery
    1. Set `Delivery` to status delivered
        1. Set `SalesTransaction` to status closed
1. `Customer` receives notification of delivery

## Services

### Catalog Service

### Checkout Service/ Sales

### Delivery Service

### Notifications

### Ordering Service

The ordering service keeps track of the shoppingcart, before they are converted into an order.

### Pricing Service

### WareHouse
