﻿using LinqExercises.Infrastructure;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace LinqExercises.Controllers
{
    public class OrdersController : ApiController
    {
        private NORTHWNDEntities _db;

        public OrdersController()
        {
            _db = new NORTHWNDEntities();
        }

        //GET: api/orders/between/01.01.1997/12.31.1997
        [HttpGet, Route("api/orders/between/{startDate}/{endDate}"), ResponseType(typeof(IQueryable<Order>))]
        public IHttpActionResult GetOrdersBetween(DateTime startDate, DateTime endDate)
        {
            var resultSet = _db.Orders.Where(o => o.RequiredDate >= startDate).Where(o => o.RequiredDate <= endDate).Where(o => o.Freight < 100);
            return Ok(resultSet);
            throw new NotImplementedException("Write a query to return all orders with required dates between the given start date and the given end date with freight under 100 units.");
        }

        //GET: api/orders/reports/purchase
        [HttpGet, Route("api/orders/reports/purchase"), ResponseType(typeof(IQueryable<object>))]
        public IHttpActionResult PurchaseReport()
        {
            // See this blog post for more information about projecting to anonymous objects. https://blogs.msdn.microsoft.com/swiss_dpe_team/2008/01/25/using-your-own-defined-type-in-a-linq-query-expression/

            var resultSet = _db.Products.Select(p => new
            {
                Product = p,
                QuantityPurchased = p.Order_Details.Sum(od => od.Quantity)
            }).OrderByDescending(p => p.QuantityPurchased);

            return Ok(resultSet);

            throw new NotImplementedException(@"
                Write a query to return an array of anonymous objects that have two properties. 

                1. A Product property containing that particular product
                2. A QuantityPurchased property containing the number of times that product was purchased.

                This array should be ordered by QuantityPurchased in descending order.
            ");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}
