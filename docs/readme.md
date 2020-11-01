# Functional UseCases

## An admin can provide a product in the catalog

1. Create a `Product` in the Catalog (Event: NewProductEntered)
    1. Create the product (Event: NewProductCreated)
        1. Create a `Price` for the `Product` in Pricing
        1. Create an `StockEntry` for the `Product` in the Warehouse
1. Modify the `Price` for a `Product` (Event: ProductPriceRequested)
    1. Update Price (Event: ProductPriceUpdated)
        1. Update `Product`'s priceInfo in catalog
1. Modify a `StockEntry` for a `Product` (Event: StockAmountRequested)
   1. Update availability (Event: StockAmountUpdated)
        1. Update `Product`'s availability in catalog

## Customer can buy a product from the shop (saga)

### Steps

1. Browse the `Catalog` viewing products, prices and availability (Query)
1. Manage the content of a  `ShoppingCart`
    1. Add a product and amount to a `ShoppingCart` (Event: ProductAdded)
        1. Create `ShoppingCart` if not exists
        1. Add product
    1. Remove product from a `ShoppingCart` (Event: ProductRemoved)
    1. Change amount of a product in a `ShoppingCart` (Event: ProductAmountChanged)
1. Review `ShoppingCart` details (Query)
    1. Fetch current pricing (optional)
    1. Recheck availability stock (optional)
1. Submit an `SalesTransaction` based of current `ShoppingCart` (Event: ShoppingCartSubmitted)
    1. Start new `SalesTransaction` based on submitted `ShoppingCart`
    1. Update pricing of `SalesTransaction` (Event: PriceUpdateRequested)
    1. Set status of `ShoppingCart` to submitted (Event: SalesTransactionStarted)
        1. Create `Delivery` in stock with status requested (StockProductRequested)
        1. Create `Payment` with status unpayed
    1. Set status of `SalesTransaction` to started
1. Reserve product from stock (Event: StockProductReserved)
    1. Update availability (Event: StockProductAvailabilityChanged)
        1. Update catalog availability
    1. Set `Delivery` to status reserved
1. Provide shipping details for the `Delivery` (Event: ShippingDetailChanged)
    1. Store shipping details for `Delivery`
    1. Set Status of `Delivery` to shippable
        1. Set status of `SalesTransaction` to InProgress(=reserved/unpayed)
1. Pay for an order (Event: PaymentSubmitted)
    1. Receive payment
    1. Set `Payment` status to payed
        1. Set `SalesTransaction` status to purchased (Event `SalesTransactionPurchased`)
            1. Send notification of purchase to customer
1. Start `Delivery` (backend Event based on `SalesTransactionPurchased`)
    1. Checkout from stock and ship `Delivery`
    1. Set `Delivery` status to in progress
    1. Set `SalesTransaction` to status shipped
        1. Send notification of shipment
1. Receive notification of purchase
1. Deliver the purchase
    1. Set `Delivery` to status delivered
        1. Set `SalesTransaction` to status closed
1. Receive notification of delivery
