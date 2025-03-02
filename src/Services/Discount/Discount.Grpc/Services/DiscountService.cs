using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
	public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
	{
		Coupon? coupon = null;
		if (!string.IsNullOrWhiteSpace(request?.ProductName))
			coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
		
		coupon ??= new Coupon { ProductName = "No discount", Description = "No discount available", Amount = 0 };
		var couponModel = coupon.Adapt<CouponModel>();
		logger.LogInformation("Get discount for : {ProductName}", couponModel.ProductName);
		
		return couponModel;
	}

	public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
	{
		return await base.CreateDiscount(request, context);
	}

	public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
	{
		return await base.UpdateDiscount(request, context);
	}

	public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
	{
		return await base.DeleteDiscount(request, context);
	}
}