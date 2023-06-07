using EasMe.Result;

namespace NewWorldMarket.Core;

public static class DomainResult
{
    public static Result Unauthorized => Result.Fatal("ErrUnauthorized");
    public static Result InvalidOperation => Result.Error("ErrInvalidOperation");
    public static Result NotFound => Result.Error("ErrNotFound");
    public static Result InvalidRequest => Result.Error("ErrInvalidRequest");

    public static class Order
    {
        public static Result ErrNotFound => Result.Error("Order not found");
        public static Result ErrNotExpired => Result.Error("Order must be expired");
        public static Result ErrInvalid => Result.Error("Order is invalid");
        public static Result ErrCompleted => Result.Error("Order is completed");
        public static Result ErrExpired => Result.Error("Order is expired");
        public static Result ErrCancelled => Result.Error("Order is cancelled");
        public static Result Cancelled => Result.Error("Order is cancelled successfully");
    }
    public static class User
    {
        public static Result ErrNotFound => Result.Error("User not found");

        
    }
}