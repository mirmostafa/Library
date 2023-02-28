module FuncTest

type CheckNumber = int
type CardNumber = int
type CardType = Visa | Mastercard
type CreditCardInfo = CardType * CardNumber


type PaymentMethod = 
    | Cash
    | Check of CheckNumber
    | Card of CreditCardInfo

type PaymentAmount = decimal
type Currency = EUR | USD

type Payment = {
    Amount: PaymentAmount
    Currency: Currency
    Method: PaymentMethod }
