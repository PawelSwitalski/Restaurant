﻿namespace Restaurant.Business.Validation
{
    public class RestaurantException : Exception
    {
        public RestaurantException() { }

        public RestaurantException(string message) : base(message) { }

        public RestaurantException(string message, Exception innerException) : base(message, innerException) { }
    }
}
