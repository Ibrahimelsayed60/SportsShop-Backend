using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsShop.Repository.Data;

namespace SportsShop.API.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ShopContext shopContext)
        {
            var method = context.Request.Method.ToUpper();

            if (method == "POST" || method == "PUT" || method == "DELETE")
            {
                var transaction = await shopContext.Database.BeginTransactionAsync();

                try
                {
                    await _next(context);
                    await shopContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
            else
            {
                await _next(context);
            }

        }

    }
}
